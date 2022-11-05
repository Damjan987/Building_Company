using BuildCompany.Core.Contracts;
using BuildCompany.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BuildCompany.WebUI.Controllers
{
    public class BillController : Controller
    {
        IRepository<Bill> context;
        IRepository<BillItem> billItemContext;

        public BillController(IRepository<Bill> billContext, IRepository<BillItem> billItemCtx)
        {
            context = billContext;
            billItemContext = billItemCtx;
        }

        public ActionResult Index()
        {
            List<Bill> bills = context.Collection().ToList();
            return View(bills);
        }

        public ActionResult Create()
        {
            Bill bill = new Bill();
            return View(bill);
        }

        [HttpPost]
        public ActionResult Create(Bill bill)
        {
            if (!ModelState.IsValid)
            {
                return View(bill);
            }
            else
            {
                context.Insert(bill);
                context.Commit();
                return RedirectToAction("Index");
            }
        }

        public ActionResult Edit(string Id)
        {
            Bill bill = context.Find(Id);
            if (bill == null)
            {
                return HttpNotFound();
            }
            else
            {
                return View(bill);
            }
        }

        [HttpPost]
        public ActionResult Edit(Bill bill, string Id)
        {
            Bill billToEdit = context.Find(Id);
            if (billToEdit == null)
            {
                return HttpNotFound();
            }
            else
            {
                if (!ModelState.IsValid)
                {
                    return View(bill);
                }
                billToEdit.Amount = bill.Amount;
                billToEdit.Buyer = bill.Buyer;
                billToEdit.Seller = bill.Seller;
                context.Commit();
                return RedirectToAction("Index");
            }
        }

        public ActionResult Delete(string Id)
        {
            Bill billToDelete = context.Find(Id);
            if (billToDelete == null)
            {
                return HttpNotFound();
            }
            else
            {
                return View(billToDelete);
            }
        }

        [HttpPost]
        [ActionName("Delete")]
        public ActionResult ConfirmDelete(string Id)
        {
            Bill billToDelete = context.Find(Id);
            if (billToDelete == null)
            {
                return HttpNotFound();
            }
            else
            {
                foreach (var i in billItemContext.Collection().ToList())
                {
                    if (i.Bill == billToDelete.Id)
                    {
                        billItemContext.Delete(i.Id);
                    }
                }
                billItemContext.Commit();
                context.Delete(Id);
                context.Commit();
                return View(billToDelete);
            }
        }

        public ActionResult BillItems(string Id)
        {
            Bill bill = context.Find(Id);
            List<BillItem> billItems = new List<BillItem>();
            foreach (var i in billItemContext.Collection().ToList())
            {
                if (i.Bill == bill.Id)
                {
                    billItems.Add(i);
                }
            }
            return View(billItems);
        }
    }
}