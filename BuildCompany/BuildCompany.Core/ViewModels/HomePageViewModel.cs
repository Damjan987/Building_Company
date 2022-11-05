using BuildCompany.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildCompany.Core.ViewModels
{
    public class HomePageViewModel
    {
        public CompanyInfo CompanyInfo { get; set; }
        public IEnumerable<BankAccount> BankAccounts { get; set; }
        public IEnumerable<CompanyItem> CompanyItems { get; set; }
    }
}
