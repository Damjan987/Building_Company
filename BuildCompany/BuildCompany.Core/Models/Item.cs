using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildCompany.Core.Models
{
    // Items in our own storage
    public class Item : BaseEntity
    {
        public string Name { get; set; }
        public float Price { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public string Category { get; set; }
    }
}
