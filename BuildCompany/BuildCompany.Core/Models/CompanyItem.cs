using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildCompany.Core.Models
{
    public class CompanyItem : BaseEntity
    {
        public string Name { get; set; }
        public string Company { get; set; }
        public string Item { get; set; }
        public int Quantity { get; set; }
        public int Price { get; set; }
    }
}
