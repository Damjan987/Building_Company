using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildCompany.Core.Models
{
    public class Order : BaseEntity
    {
        public float Amount { get; set; }
        public string Buyer { get; set; }
        public string Seller { get; set; }
    }
}
