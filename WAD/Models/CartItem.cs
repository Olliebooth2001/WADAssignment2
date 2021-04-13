using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WAD.Models
{
    public class CartItem
    {
        public int GigID { get; set; }
        public string GigTitle { get; set; }
        public decimal GigPrice { get; set; }
        public int OrderQuantity { get; set; }
        public DateTime OrderDate { get; set; }
    }
}
