﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Shared
{

    public class BasketItemDto
    {
        [JsonPropertyName("id")] 
        public int ProductId { get; set; }

        public string ProductName { get; set; }
        public string PictureUrl { get; set; }

        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }

        [Range(1, 99)]
        public int Quantity { get; set; }
    }

}
