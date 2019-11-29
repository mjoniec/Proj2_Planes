using System;
using System.Collections.Generic;

namespace MetalPrices.Model
{
    public class MetalPrices
    {
        public List<MetalPriceDay> Prices { get; set; }
    }

    public class MetalPriceDay
    {
        public DateTime Date { get; set; }
        public double Price { get; set; }
    }
}
