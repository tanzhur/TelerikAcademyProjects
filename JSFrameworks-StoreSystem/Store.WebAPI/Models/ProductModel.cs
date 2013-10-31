﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Store.WebAPI.Models
{
    public class ProductModel
    {
        public string Name { get; set; }

        public int Id { get; set; }

        public string Description { get; set; }

        public string ImageSource { get; set; }

        public decimal Price { get; set; }

        public int CategoryId { get; set; }

        public string CategoryName { get; set; }

        public int Quantity { get; set; }
    }
}