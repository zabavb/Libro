import { 
    DropdownMenu, 
    DropdownMenuTrigger, 
    DropdownMenuContent, 
    DropdownMenuItem, 
    DropdownMenuSub, 
    DropdownMenuSubTrigger, 
    DropdownMenuSubContent 
  } from "@/components/ui/dropdown-menu";
  import hamburgerIcon from '@/assets/icons/hamburgerIcon.svg'
  
  
  export default function CatalogMenu() {
    return (
        <DropdownMenu>
            <DropdownMenuTrigger className="bg-[rgb(26,29,35)] shadow-md w-full text-left flex items-center gap-3.5 whitespace-nowrap">
                <img src={hamburgerIcon}/>
                <span className="text-white text-lg" style={{lineHeight:"20px"}}>
                    Book Catalog
                </span>
            </DropdownMenuTrigger>
  
            <DropdownMenuContent className="w-80 h-[400px] bg-white rounded-lg shadow-xl border border-gray-200 mt-2 overflow-y-auto">
                <div className="flex items-center justify-between px-4 py-3 border-b border-gray-200">
                    <span className="font-semibold text-gray-800">
                        Paper
                    </span>
                    <div className="flex gap-2">
                        <button
                            className="text-gray-600 hover:bg-gray-100 text-sm border border-gray-300 rounded-md px-2 py-1"
                        >
                            E-books
                        </button>
                        <button     
                            className="text-gray-600 hover:bg-gray-100"
                        >
                            Audio
                        </button>
                    </div>
                </div>
  
                <DropdownMenuSub>
                    <DropdownMenuSubTrigger className="px-4 py-2 text-sm text-orange-500 font-medium hover:bg-gray-100 focus:bg-gray-100 focus:outline-none transition-colors">
                        Fiction
                    </DropdownMenuSubTrigger>
                    <DropdownMenuSubContent className="w-80 bg-gray-900 rounded-lg shadow-xl border border-gray-200 overflow-y-auto">
                        <DropdownMenuItem className="px-4 py-2 text-sm text-gray-200 hover:bg-gray-700 hover:text-orange-500 focus:bg-gray-700 focus:text-orange-500 focus:outline-none transition-colors">
                            Books of aphorisms and quotes
                        </DropdownMenuItem>
                        <DropdownMenuItem className="px-4 py-2 text-sm text-gray-200 hover:bg-gray-700 hover:text-orange-500 focus:bg-gray-700 focus:text-orange-500 focus:outline-none transition-colors">
                            Detective books
                        </DropdownMenuItem>
                        <DropdownMenuItem className="px-4 py-2 text-sm text-gray-200 hover:bg-gray-700 hover:text-orange-500 focus:bg-gray-700 focus:text-orange-500 focus:outline-none transition-colors">
                            Thriller books
                        </DropdownMenuItem>
                        <DropdownMenuItem className="px-4 py-2 text-sm text-gray-200 hover:bg-gray-700 hover:text-orange-500 focus:bg-gray-700 focus:text-orange-500 focus:outline-none transition-colors">
                            Action books
                        </DropdownMenuItem>
                        <DropdownMenuItem className="px-4 py-2 text-sm text-gray-200 hover:bg-gray-700 hover:text-orange-500 focus:bg-gray-700 focus:text-orange-500 focus:outline-none transition-colors">
                            Medieval books
                        </DropdownMenuItem>
                        <DropdownMenuItem className="px-4 py-2 text-sm text-gray-200 hover:bg-gray-700 hover:text-orange-500 focus:bg-gray-700 focus:text-orange-500 focus:outline-none transition-colors">
                            Romantic prose
                        </DropdownMenuItem>
                        <DropdownMenuItem className="px-4 py-2 text-sm text-gray-200 hover:bg-gray-700 hover:text-orange-500 focus:bg-gray-700 focus:text-orange-500 focus:outline-none transition-colors">
                            Classic prose
                        </DropdownMenuItem>
                    </DropdownMenuSubContent>
                </DropdownMenuSub>
  

                <DropdownMenuItem className="px-4 py-2 text-sm text-gray-800 hover:bg-gray-100 hover:text-orange-500 focus:bg-gray-100 focus:text-orange-500 focus:outline-none transition-colors">
                  Children's literature
              </DropdownMenuItem>
              <DropdownMenuItem className="px-4 py-2 text-sm text-gray-800 hover:bg-gray-100 hover:text-orange-500 focus:bg-gray-100 focus:text-orange-500 focus:outline-none transition-colors">
                  Business, money, economy
              </DropdownMenuItem>
          </DropdownMenuContent>
      </DropdownMenu>
  );
}
