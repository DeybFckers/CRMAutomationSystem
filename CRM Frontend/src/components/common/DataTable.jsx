import {
    flexRender,
    getCoreRowModel,
    useReactTable
} from "@tanstack/react-table";

// Reusable table component.
//
// "data" contains the actual records we want to display.
// "columns" contains the configuration for what columns to display.
//
// We give data a default value of [] so the table does not crash
// if the API data has not loaded yet.
export const DataTable = ({ data = [], columns }) => {

    console.log("DataTable data:", data);
    console.log("DataTable columns:", columns);
    console.log("data is array:", Array.isArray(data));
    console.log("columns is array:", Array.isArray(columns));

    // useReactTable creates and manages the table instance.
    //
    // We pass "data" and "columns" from the component using DataTable.
    // This is what makes this table reusable.
    //
    // For example:
    //
    // <DataTable data={leads} columns={leadColumns} />
    //
    // or:
    //
    // <DataTable data={customers} columns={customerColumns} />
    const table = useReactTable({
        data,
        columns,

        // This tells TanStack Table to create the basic row structure.
        //
        // We need this because later we use:
        // table.getRowModel().rows
        //
        // to get the rows that should be rendered.
        getCoreRowModel: getCoreRowModel()
    });

    return (
        <div className="h-165 overflow-x-auto rounded-lg border border-border bg-surface shadow-sm">
            <table className="w-full text-left">

            {/* 
                THEAD = table header.

                getHeaderGroups() gives us the column headers
                that we defined inside our "columns" configuration.
            */}
            <thead className="bg-primary text-table-header-text">

                {table.getHeaderGroups().map((headerGroup) => (

                    // Every header group needs a unique key.
                    <tr key={headerGroup.id}>

                        {headerGroup.headers.map((header) => (

                            // Each <th> represents one column header.
                            <th key={header.id} className="px-4 py-3 text-base font-semibold tracking-wide whitespace-nowrap">

                                {/*
                                    flexRender() renders the header.

                                    Why do we use flexRender instead of simply:

                                    {header.column.columnDef.header}

                                    ?

                                    Because TanStack allows "header" to be
                                    either a normal value or a function/component.

                                    For example:

                                    header: "Name"

                                    or:

                                    header: () => <span>Name</span>

                                    flexRender() handles both cases.
                                */}
                                {flexRender(
                                    header.column.columnDef.header,
                                    header.getContext()
                                )}

                            </th>
                        ))}

                    </tr>
                ))}

            </thead>

            {/*
                TBODY = actual table data.

                getRowModel().rows gives us the rows created by
                TanStack Table from the "data" we provided.
            */}
            <tbody className="divide-y divide-gray-200">

                {table.getRowModel().rows.map((row) => (

                    // Each row needs a unique key.
                    <tr key={row.id} className="odd:bg-surface even:bg-table-row-alt hover:bg-table-row-hover/50 transition-colors ">

                        {/*
                            getVisibleCells() gives us the cells that
                            should currently be displayed for this row.

                            This is useful because later, if you add
                            column visibility, the table can automatically
                            render only the visible columns.
                        */}
                        {row.getVisibleCells().map((cell) => (

                            // Each cell needs a unique key.
                            <td key={cell.id} className="px-4 py-3 text-text-secondary text-sm">

                                {/*
                                    Render the actual cell value.

                                    We use flexRender() for the same reason
                                    we used it for the header.

                                    A column can simply be:

                                    {
                                        accessorKey: "email",
                                        header: "Email"
                                    }

                                    Or it can have custom rendering:

                                    {
                                        accessorKey: "status",
                                        header: "Status",
                                        cell: ({ row }) => {
                                            return row.original.status.name;
                                        }
                                    }

                                    flexRender() supports both.
                                */}
                                {flexRender(
                                    cell.column.columnDef.cell,
                                    cell.getContext()
                                )}

                            </td>
                        ))}

                    </tr>
                ))}

            </tbody>

        </table>
        </div>
       
    );
};