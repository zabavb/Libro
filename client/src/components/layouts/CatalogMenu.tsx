import { 
    DropdownMenu, 
    DropdownMenuTrigger, 
    DropdownMenuContent, 
    DropdownMenuItem, 
    DropdownMenuSub, 
    DropdownMenuSubTrigger, 
    DropdownMenuSubContent 
  } from "@/components/ui/dropdown-menu";
  
  export default function CatalogMenu() {
    return (       
        <DropdownMenu>
          <DropdownMenuTrigger className="p-2 border rounded bg-white shadow-md w-full text-left">
            Book Catalog
          </DropdownMenuTrigger>
          <DropdownMenuContent className="w-80">
            <DropdownMenuSub>
              <DropdownMenuSubTrigger>Fiction</DropdownMenuSubTrigger>
              <DropdownMenuSubContent>
                <DropdownMenuItem>Books of aphorisms and quotes</DropdownMenuItem>
                <DropdownMenuItem>Detective books</DropdownMenuItem>
                <DropdownMenuItem>Thriller books</DropdownMenuItem>
                <DropdownMenuItem>Action books</DropdownMenuItem>
                <DropdownMenuItem>Medieval books</DropdownMenuItem>
                <DropdownMenuItem>Romantic prose</DropdownMenuItem>
                <DropdownMenuItem>Classic prose</DropdownMenuItem>
              </DropdownMenuSubContent>
            </DropdownMenuSub>
  
            <DropdownMenuItem>Children's literature</DropdownMenuItem>
            <DropdownMenuItem>Business, money, economy</DropdownMenuItem>
          </DropdownMenuContent>
        </DropdownMenu>  
    );
  }
  