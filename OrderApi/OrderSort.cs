﻿using Library.Sorts;

namespace OrderAPI
{
    public class OrderSort
    {
        public Bool OrderDate { get; set; } // OrderDate Variable
        public Bool BooksAmount { get; set; } // Count of BookIds list
        public Bool OrderPrice { get; set; }
        public Bool DeliveryPrice { get; set; }
        public Bool DeliveryDate { get; set; }
        public Bool StatusSort { get; set; } 
    }
}
