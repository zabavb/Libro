﻿namespace Library.UserEntities
{
    public class Basket
    {
        public List<BasketItem> Items { get; set; } = new List<BasketItem>();
        public decimal TotalPrice => Items.Sum(item => item.TotalPrice);
    }
}
