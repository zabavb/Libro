using Newtonsoft.Json;
using UserAPI.Models;

namespace UserAPI.Services
{
    public class BasketService : IBasketService
    {
        private const string BasketCookieKey = "Basket";

        public Basket GetBasket(HttpContext context)
        {
            var basketData = context.Request.Cookies[BasketCookieKey];
            return string.IsNullOrEmpty(basketData)
                ? new Basket()
                : JsonConvert.DeserializeObject<Basket>(basketData);
        }

        public void AddItem(HttpContext context, BasketItemDto item)
        {
            var basket = GetBasket(context);

            var existingItem = basket.Items.FirstOrDefault(x => x.ProductId == item.ProductId);
            if (existingItem != null)
            {
                existingItem.Quantity += item.Quantity;
            }
            else
            {
                basket.Items.Add(new BasketItem
                {
                    ProductId = item.ProductId,
                    ProductName = item.ProductName,
                    Price = item.Price,
                    Quantity = item.Quantity
                });
            }

            SaveBasket(context, basket);
        }

        public void UpdateQuantity(HttpContext context, Guid productId, int quantity)
        {
            var basket = GetBasket(context);

            var item = basket.Items.FirstOrDefault(x => x.ProductId == productId);
            if (item != null)
            {
                if (quantity > 0)
                {
                    item.Quantity = quantity;
                }
                else
                {
                    basket.Items.Remove(item);
                }

                SaveBasket(context, basket);
            }
        }

        public void RemoveItem(HttpContext context, Guid productId)
        {
            var basket = GetBasket(context);

            var item = basket.Items.FirstOrDefault(x => x.ProductId == productId);
            if (item != null)
            {
                basket.Items.Remove(item);
                SaveBasket(context, basket);
            }
        }

        public void ClearBasket(HttpContext context)
        {
            context.Response.Cookies.Delete(BasketCookieKey);
        }

        public bool ItemExists(HttpContext context, Guid productId)
        {
            var basket = GetBasket(context);
            return basket.Items.Any(x => x.ProductId == productId);
        }

        private void SaveBasket(HttpContext context, Basket basket)
        {
            var basketData = JsonConvert.SerializeObject(basket);
            context.Response.Cookies.Append(BasketCookieKey, basketData, new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTimeOffset.Now.AddMinutes(30)
            });
        }
    }
}

