using BuildCompany.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildCompany.Core.ViewModels
{
    public class OrderOrderItemViewModel
    {
        public Order Order { get; set; }
        public IEnumerable<ItemInOrder> OrderItems { get; set; }
    }
}
