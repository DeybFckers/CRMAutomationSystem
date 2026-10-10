import { CustomerOverview } from "./CustomerOverview";
import { CustomerContacts } from "./CustomerContacts";
import { CustomerAddresses } from "./CustomerAddresses";
import { CustomerTasks } from "./CustomerTasks";
import { CustomerNotes } from "./CustomerNotes";
import { CustomerActivities } from "./CustomerActivities";
import { CustomerOpportunities } from "./CustomerOpportunities";

export const CustomerTabs = ({
    customer,
    selectedTab,
    setSelectedTab,
    //customer overview
    onEdit,
    onHold,
    onDeactivate,
    onActivate,

    //customer contact
    onContactsChanged,

    //customer address
    onAddressesChanged
}) => {

    const tabs = [
        {
            key: "overview",
            label: "Overview",
            component: CustomerOverview
        },
        {
            key: "contacts",
            label: "Contacts",
            component: CustomerContacts
        },
        {
            key: "addresses",
            label: "Addresses",
            component: CustomerAddresses
        },
        {
            key: "opportunities",
            label: "Opportunity",
            component: CustomerOpportunities
        },
        {
            key: "tasks",
            label: "Tasks",
            component: CustomerTasks
        },
        {
            key: "notes",
            label: "Notes",
            component: CustomerNotes
        },
        {
            key: "activities",
            label: "Activity",
            component: CustomerActivities
        }
    ];

    return (
        <div className="flex flex-col gap-6">

            {/* Tabs */}
            <div className="flex gap-6 border-b border-border">

                {tabs.map((tab) => (
                    <button
                        key={tab.key}
                        type="button"
                        onClick={() => setSelectedTab(tab.key)}
                        className={`pb-3 font-medium transition-colors cursor-pointer ${
                            selectedTab === tab.key
                                ? "text-primary border-b-2 border-primary"
                                : "text-text-secondary hover:text-text"
                        }`}
                    >
                        {tab.label}
                    </button>
                ))}

            </div>

            {/* Tab Content */}
            {tabs.map((tab) => {
                if (selectedTab !== tab.key) {
                    return null;
                }

                const Component = tab.component;

                return (
                    <Component
                        key={tab.key}
                        customer={customer}

                        // Customer actions
                        onEdit={onEdit}
                        onHold={onHold}
                        onDeactivate={onDeactivate}
                        onActivate={onActivate}
                        //Customer Contacts
                        onContactsChanged={onContactsChanged}
                        //Customer Address
                        onAddressesChanged={onAddressesChanged}
                    />
                );
            })}

        </div>
    );
};