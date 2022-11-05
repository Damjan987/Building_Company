using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildCompany.Core.Models
{
    public class BankAccount : BaseEntity
    {
        // company that is owner of the account
        public string Company { get; set; }
        public string AccountNo { get; set; }
        public int Amount { get; set; }
    }
}
