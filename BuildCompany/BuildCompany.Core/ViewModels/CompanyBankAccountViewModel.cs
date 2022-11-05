using BuildCompany.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildCompany.Core.ViewModels
{
    public class CompanyBankAccountViewModel
    {
        public BankAccount BankAccount { get; set; }
        public IEnumerable<Partner> Partners { get; set; }
    }
}
