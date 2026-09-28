export const Modal = ({isOpen, onClose, title, children}) =>{
    
    if(!isOpen){
        return null;
    }

    return (
        <div className=" fixed inset-0  flex items-center justify-center bg-black/50 p-4">
            <div className="w-full max-w-fit rounded-xl border-border bg-surface shadow-2xl">
                <div className="flex items-center justify-between  px-6 py-3">
                    <h2 className="text-xl font-semibold text-text">
                        {title}
                    </h2>

                    <button
                        type="button"
                        onClick={onClose}
                        aria-label="Close modal"
                        className="flex h-8 w-8 cursor-pointer items-center justify-center rounded-full text-text-muted transition bg-red-100 hover:bg-red-400 hover:text-text">
                        <span className="text-xl leading-none">
                            ×
                        </span>
                    </button>
                </div>

                <div className="px-6 py-3">
                    {children}
                </div>
            </div>
        </div>
    )
}