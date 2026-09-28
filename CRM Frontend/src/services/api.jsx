import axios from "axios";

const api = axios.create({
  baseURL: import.meta.env.VITE_API_URL,
  withCredentials:true,
  headers: {
    "Content-Type": "application/json",
  },
});

// ---------------------------------------------------------
// REFRESH TOKEN STATE
// ---------------------------------------------------------

// This tells us whether a refresh request is already running.
//
// Example:
//
// Request A → 401
// Request B → 401
// Request C → 401
//
// We only want ONE refresh request:
//
// Request A → 401 ─┐
// Request B → 401 ─┼──> ONE /auth/refresh
// Request C → 401 ─┘
//
// Without this variable, all three requests
// could try to refresh the token at the same time.
let isRefreshing = false;


// Requests that receive a 401 while another refresh
// request is already running are stored here.
//
// Example:
//
// Request A → 401 → starts refresh
//
// Request B → 401 → waits
// Request C → 401 → waits
//
// After the refresh succeeds, B and C will be retried.
let refreshSubscribers = [];


// Adds a request to the waiting list.
//
// The callback contains the logic needed to retry
// the original request after the token has been refreshed.
const subscribeToRefresh = (callback) => {
    refreshSubscribers.push(callback);
};


// Runs all requests that were waiting for the refresh.
//
// Example:
//
// refreshSubscribers = [
//     retry Request B,
//     retry Request C
// ]
//
// Once the refresh succeeds, both requests are allowed
// to continue.
//
// After that, we clear the list because those requests
// have already been handled.
const notifyRefreshSubscribers = () => {
    refreshSubscribers.forEach((callback) => callback());

    refreshSubscribers = [];
};


// ---------------------------------------------------------
// AXIOS RESPONSE INTERCEPTOR
// ---------------------------------------------------------

// An Axios interceptor allows us to inspect every response
// before it reaches the component/service that made the request.
//
// There are two possibilities:
//
// 1. Request succeeds
// 2. Request fails
//
// The first function handles successful responses.
// The second function handles errors.
api.interceptors.response.use(

    // -----------------------------------------------------
    // SUCCESS
    // -----------------------------------------------------

    // If the API returns something like:
    //
    // 200 OK
    // 201 Created
    // 204 No Content
    //
    // simply return the response normally.
    (response) => {
        return response;
    },


    // -----------------------------------------------------
    // ERROR
    // -----------------------------------------------------

    async (error) => {

        // Axios keeps the original request that caused
        // the error inside error.config.
        //
        // We save it because, if the access token expired,
        // we need to send this exact request again after
        // refreshing the token.
        //
        // Example:
        //
        // GET /api/leads
        //       ↓
        //      401
        //       ↓
        // refresh token
        //       ↓
        // GET /api/leads again
        const originalRequest = error.config;


        // -------------------------------------------------
        // CHECK WHETHER WE SHOULD REFRESH
        // -------------------------------------------------

        // We only want to refresh when the response is 401.
        //
        // 401 usually means that the current access token
        // is missing, invalid, or expired.
        //
        // We also check:
        //
        // originalRequest._retry
        //
        // This prevents an infinite loop.
        //
        // Example of what we DON'T want:
        //
        // /api/leads → 401
        //      ↓
        // refresh
        //      ↓
        // /api/leads → 401
        //      ↓
        // refresh
        //      ↓
        // /api/leads → 401
        //      ↓
        // forever...
        //
        // Once _retry is true, we won't refresh again
        // for that particular request.
        //
        // Finally, we don't want the refresh endpoint
        // itself to trigger another refresh.
        //
        // Otherwise:
        //
        // /auth/refresh → 401
        //       ↓
        // /auth/refresh
        //       ↓
        // /auth/refresh
        //       ↓
        // infinite loop
        if (
            error.response?.status !== 401 ||
            originalRequest._retry ||
            originalRequest.url?.includes("/api/auth/refresh")
        ) {
            return Promise.reject(error);
        }


        // -------------------------------------------------
        // MARK THIS REQUEST AS RETRIED
        // -------------------------------------------------

        // This tells the interceptor:
        //
        // "We have already attempted to refresh the token
        // for this request."
        //
        // If the retried request fails with 401 again,
        // it won't start another refresh.
        originalRequest._retry = true;


        // -------------------------------------------------
        // CHECK IF REFRESH IS ALREADY RUNNING
        // -------------------------------------------------

        // Another API request may have already detected
        // that the access token expired.
        //
        // Example:
        //
        // Request A → 401 → starts refreshing
        //
        // Request B → 401 → refresh is already running
        //
        // Request C → 401 → refresh is already running
        //
        // We don't start another refresh.
        //
        // Instead, B and C wait until A finishes.
        if (isRefreshing) {

            // Return a Promise that will remain pending
            // until notifyRefreshSubscribers() is called.
            //
            // Once the refresh succeeds, this callback
            // retries the original request.
            return new Promise((resolve, reject) => {

                subscribeToRefresh(() => {

                    // Retry the original request.
                    //
                    // The browser will automatically send
                    // the newly refreshed access_token cookie.
                    api(originalRequest)
                        .then(resolve)
                        .catch(reject);
                });
            });
        }


        // -------------------------------------------------
        // START REFRESH
        // -------------------------------------------------

        // No other refresh request is running,
        // so this request becomes responsible for
        // refreshing the access token.
        isRefreshing = true;


        try {

            // Call your backend refresh endpoint.
            //
            // POST /api/auth/refresh
            //
            // The browser automatically sends the
            // HttpOnly refresh_token cookie because:
            //
            // withCredentials: true
            //
            // Your backend then:
            //
            // 1. Reads refresh_token
            // 2. Validates it
            // 3. Creates a new access token
            // 4. Sends the new access_token cookie
            await api.post("/api/auth/refresh");


            // The refresh request succeeded.
            //
            // We can now allow other requests that were
            // waiting for the refresh to continue.
            isRefreshing = false;


            // Tell all waiting requests:
            //
            // "The new access token is ready.
            // You can retry your requests now."
            notifyRefreshSubscribers();


            // Retry the original request that caused
            // the refresh.
            //
            // Example:
            //
            // GET /api/leads → 401
            //       ↓
            // refresh
            //       ↓
            // GET /api/leads → 200
            return api(originalRequest);

        } catch (refreshError) {

            // The refresh request failed.
            //
            // This normally means the refresh token is:
            //
            // - expired
            // - invalid
            // - revoked
            // - missing
            //
            // At this point the user really needs
            // to authenticate again.
            isRefreshing = false;


            // Clear any requests that were waiting
            // for the refresh.
            //
            // We don't want them to remain stuck waiting.
            refreshSubscribers = [];


            // Return the refresh error.
            //
            // Your application can handle this and
            // redirect the user to /login if needed.
            return Promise.reject(refreshError);
        }
    }
);


// Export the configured Axios instance.
//
// Every API service in your CRM can now use:
//
// import api from "../api";
//
// api.get(...)
// api.post(...)
// api.put(...)
// api.patch(...)
// api.delete(...)

export default api;