using BuildCompany.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildCompany.Core.ViewModels
{
    public class CompanyItemViewModel
    {
        public CompanyItem CompanyItem { get; set; }
        public IEnumerable<Item> Items { get; set; }
    }
}
