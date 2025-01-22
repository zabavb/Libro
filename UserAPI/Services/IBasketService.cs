using UserAPI.Models;


namespace UserAPI.Services
{
    public interface IBasketService
    {
        Basket GetBasket(HttpContext context);
        void AddItem(HttpContext context, BasketItemDto item);
        void UpdateQuantity(HttpContext context, Guid productId, int quantity);
        void RemoveItem(HttpContext context, Guid productId);
        void ClearBasket(HttpContext context);
        bool ItemExists(HttpContext context, Guid productId);
    }
}
