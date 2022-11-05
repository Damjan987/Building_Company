using BuildCompany.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildCompany.Core.ViewModels
{
    public class CategoryItemViewModel
    {
        public Item Item { get; set; }
        public IEnumerable<Category> Categories { get; set; }
    }
}
