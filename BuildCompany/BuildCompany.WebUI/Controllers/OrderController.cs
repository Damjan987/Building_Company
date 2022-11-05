using BuildCompany.Core.Contracts;
using BuildCompany.Core.Models;
using BuildCompany.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BuildCompany.WebUI.Controllers
{
    // problem je sta itemi unutar ordera ne dobiju id ordera koji tribaju dobit nego nekog drugog nepoznatog
    public class OrderController : Controller
    {
        IRepository<Order>       context;
        IRepository<Partner>     partnerContext;
        IRepository<CompanyItem> companyItemContext;
        IRepository<CompanyInfo> companyInfoContext;
        IRepository<ItemInOrder> orderItemContext;
        IRepository<Bill>        billContext;
        IRepository<BillItem>    billItemContext;
        IRepository<BankAccount> bankAccountContext;

        ItemInOrder orderItem = new ItemInOrder();
        String currentCompanyNameForOrder;

        public OrderController(IRepository<Order> orderContext, IRepository<Partner> partnerCtx,
            IRepository<CompanyItem> companyItemCtx, IRepository<CompanyInfo> companyInfoCtx,
            IRepository<ItemInOrder> orderItemCtx, IRepository<Bill> billCtx,
            IRepository<BillItem> billItemCtx, IRepository<BankAccount> bankAccountCtx)
        {
            context            = orderContext;
            partnerContext     = partnerCtx;
            companyItemContext = companyItemCtx;
            companyInfoContext = companyInfoCtx;
            orderItemContext   = orderItemCtx;
            billContext        = billCtx;
            billItemContext    = billItemCtx;
            bankAccountContext = bankAccountCtx;
        }
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult PartnersList()
        {
            return View(partnerContext.Collection());
        }

        // GOTOVO POPRAVLJANJE
        public ActionResult ItemsFromPartner(string companyName)
        {
            // ako ne postoji niti jedan aktivan order onda
            // postavi sve podatke na njega i stvori ga u bazi
            if (context.Collection().ToList().Count == 0)
            {
                Order order = new Order();
                // minjano
                order.Buyer = companyInfoContext.Collection().ToList()[0].Id;
                order.Seller = companyName;
                order.Amount = 0;
                context.Insert(order);
            }
            // ako smo klikli na ime druge firme
            if (context.Collection().ToList().Count > 0)
            {
                if (context.Collection().ToList()[0].Seller != companyName)
                {
                    // izbrisi sve iteme vezane za taj order
                    foreach (var i in orderItemContext.Collection().ToList())
                    {
                        orderItemContext.Delete(i.Id);
                        orderItemContext.Commit();
                    }
                    // izbrisi sami order
                    context.Delete(context.Collection().ToList()[0].Id);
                }
            }

            context.Commit();

            // amount will be incremented later depending on the order items
            List<CompanyItem> companyItems = companyItemContext.Collection().ToList();
            List<CompanyItem> companyItemsFiltered = new List<CompanyItem>();
            if (companyName == null)
            {
                companyItems = companyItemContext.Collection().ToList();
            }
            else
            {
                foreach (var item in companyItems)
                {
                    if (item.Company == companyName)
                    {
                        companyItemsFiltered.Add(item);
                    }
                }
                companyItems = companyItemsFiltered;
            }
            return View(companyItems);
        }

        // GOTOVO POPRAVLJANJE
        public ActionResult SelectQuantityForItem(string quantity, string itemName)
        {
            CompanyItem companyItem = new CompanyItem();
            foreach (var i in companyItemContext.Collection().ToList())
            {
                // itemname ce sada bit itemId
                // ode netriba nista minjat nego samo u htmlu
                if (i.Item == itemName)
                {
                    companyItem = i;
                }
            }
            orderItem.Name = companyItem.Name;
            orderItem.Item = companyItem.Item;
            orderItem.Price = companyItem.Price;
            orderItem.Quantity = Int16.Parse(quantity);
            if (context.Collection().ToList().Count > 0)
            {
                orderItem.Order = context.Collection().ToList()[0].Id;
                context.Collection().ToList()[0].Amount += orderItem.Price * orderItem.Quantity;
                context.Commit();
            }
            orderItemContext.Insert(orderItem);
            orderItemContext.Commit();

            return RedirectToAction("OrderInfo");
        }

        // GOTOVO POPRAVLJANJE
        public ActionResult OrderInfo()
        {
            OrderOrderItemViewModel viewModel = new OrderOrderItemViewModel();
            if (context.Collection().ToList().Count > 0) {
                viewModel.Order = context.Collection().ToList()[0];
            }
            List<ItemInOrder> orderItems = new List<ItemInOrder>();
            if (context.Collection().ToList().Count > 0)
            {
                foreach (var i in orderItemContext.Collection().ToList())
                {
                    if (i.Order == context.Collection().ToList()[0].Id)
                    {
                        orderItems.Add(i);
                    }
                }
            }
            viewModel.OrderItems = orderItems;
            return View(viewModel);
        }

        public ActionResult OrderProcessing()
        {
            List<ItemInOrder> visitedOrderItems = new List<ItemInOrder>();
            // pretrazi koji sve proizvodi postoje na skladistu
            foreach (var oi in orderItemContext.Collection().ToList())
            {
                foreach (var ci in companyItemContext.Collection().ToList())
                {
                    if (oi.Item == ci.Item && ci.Company == companyInfoContext.Collection().ToList()[0].Id)
                    {
                        ci.Quantity += oi.Quantity;
                        visitedOrderItems.Add(oi);
                        break;
                    }
                }
            }
            companyItemContext.Commit();


            // pretvori sve orderIteme u companyIteme i spremi ih u bazu
            foreach (var oi in orderItemContext.Collection().ToList())
            {
                if (visitedOrderItems.Find(i => i.Id == oi.Id) == null)
                {
                    CompanyItem companyItem = new CompanyItem();
                    companyItem.Company = companyInfoContext.Collection().ToList()[0].Id;
                    companyItem.Name = oi.Name;
                    companyItem.Item = oi.Item;
                    companyItem.Quantity = oi.Quantity;
                    companyItem.Price = oi.Price;
                    companyItemContext.Insert(companyItem);
                }
            }
            companyItemContext.Commit();

            visitedOrderItems.Clear();





            // napravi Bill object i spremi ga u bazu
            // te mu dodaj sve orderIteme kao billIteme
            // te izracunaj ukupnu cijenu racuna
            Bill bill = new Bill();
            bill.Buyer = context.Collection().ToList()[0].Buyer;
            bill.Seller = context.Collection().ToList()[0].Seller;
            billContext.Insert(bill);
            billContext.Commit();
            float billAmount = context.Collection().ToList()[0].Amount;
            foreach (var i in orderItemContext.Collection().ToList())
            {
                BillItem billItem = new BillItem();
                billItem.Name = i.Name;
                billItem.Bill = bill.Id;
                billItem.Item = i.Item;
                billItem.Quantity = i.Quantity;
                billItem.Price = i.Price;
                billItemContext.Insert(billItem);
                billItemContext.Commit();
            }
            // najnovijem računu dodaj pravu cijenu
            billContext.Collection().ToList().Last().Amount = billAmount;
            billContext.Commit();

            // smanjiti balans računa naše firme za cijenu računa
            bankAccountContext.Collection().ToList()[0].Amount -= (int) billAmount;

            // nije stavljeno u word
            // povećaj bank account dobavljača
            BankAccount partnerBankAccount = new BankAccount();
            foreach (var ba in bankAccountContext.Collection().ToList())
            {
                if (ba.Company == bill.Seller)
                {
                    partnerBankAccount = ba;
                }
            }
            partnerBankAccount.Amount += (int) billAmount;
            bankAccountContext.Commit();

            // oduzmi quantity proizvoda sa skladista dobavljača
            foreach (var ci in companyItemContext.Collection().ToList())
            {
                if (ci.Company == bill.Seller)
                {
                    foreach (var oi in orderItemContext.Collection().ToList())
                    {
                        if (oi.Item == ci.Item)
                        {
                            ci.Quantity -= oi.Quantity;
                        }
                    }
                }
            }
            companyItemContext.Commit();

            // ispod je sve OK
            // izbrisi sve order iteme iz baze
            foreach (var i in orderItemContext.Collection().ToList())
            {
                orderItemContext.Delete(i.Id);
            }
            orderItemContext.Commit();

            // izbrisi order object iz baze
            foreach (var o in context.Collection().ToList())
            {
                context.Delete(o.Id);
            }
            context.Commit();

            return RedirectToAction("HomePage", "CompanyInfo");
        }

        public ActionResult PayWorkerSelectAmount(string worker)
        {
            ViewBag.worker = worker;
            return View();
        }

        public ActionResult PayWorker(string worker, string amount)
        {
            // smanji balans naše kompanije za amount
            bankAccountContext.Collection().ToList()[0].Amount -= Int16.Parse(amount);

            // povećaj bank account amount workera
            BankAccount workerBankAccount = new BankAccount();
            foreach (var ba in bankAccountContext.Collection().ToList())
            {
                if (ba.Company == worker)
                {
                    workerBankAccount = ba;
                }
            }
            workerBankAccount.Amount += Int16.Parse(amount);
            bankAccountContext.Commit();

            // stvori novi racun
            Bill bill = new Bill();
            bill.Buyer = bankAccountContext.Collection().ToList()[0].Company;
            bill.Seller = worker;
            bill.Amount = Int16.Parse(amount);
            billContext.Insert(bill);
            billContext.Commit();

            return RedirectToAction("Index", "Partner");
        }
    }
}