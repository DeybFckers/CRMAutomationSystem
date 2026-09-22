using CRMSystem.Models.Responses;
using Microsoft.AspNetCore.Mvc;

namespace CRMSystem.Controllers
{
    public abstract class BaseController : ControllerBase
    {
        protected IActionResult Success<T>(string message, T data)
        {
            return Ok(new ApiResponse<T>
            {
                Success = true,
                Message = message,
                Data = data
            });
        }

        protected IActionResult Success(string message)
        {
            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = message,
                Data = null
            });
        }

        protected IActionResult Created<T>(string message, T data)
        {
            return StatusCode(StatusCodes.Status201Created, new ApiResponse<T> {
                Success = true, 
                Message = message,
                Data = data 
            });
        }

        protected IActionResult Created(string message)
        {
            return StatusCode(StatusCodes.Status201Created, new ApiResponse<object> { 
                Success = true, 
                Message = message, 
                Data = null 
            });
        }
    }
}
