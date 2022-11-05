using BuildCompany.Core.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildCompany.DataAccess.SQL
{
    public class DataContext : DbContext
    {
        public DataContext()
            : base("DefaultConnection")
        {

        }

        public DbSet<BankAccount> BankAccounts { get; set; }
        public DbSet<Bill>        Bills        { get; set; }
        public DbSet<Category>    Categories   { get; set; }
        public DbSet<CompanyInfo> CompanyInfos { get; set; }
        public DbSet<Item>        Items        { get; set; }
        public DbSet<Order>       Orders       { get; set; }
        public DbSet<Partner>     Partners     { get; set; }
        public DbSet<BillItem>    BillItems    { get; set; }
        public DbSet<CompanyItem> CompanyItems { get; set; }
        public DbSet<Orderitem>   OrderItems   { get; set; }
        public DbSet<ItemInOrder> ItemsInOrder { get; set; }
    }
}
