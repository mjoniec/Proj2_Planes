using System;
using System.Collections.Generic;

namespace MetalPrices.Model
{
    public class MetalPrices
    {
        public List<MetalPriceDateTime> Prices { get; set; }
    }

    public class MetalPriceDateTime
    {
        public DateTime DateTime { get; set; }
        public double Price { get; set; }
    }
}
