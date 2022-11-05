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
    public class CompanyItemController : Controller
    {
        IRepository<CompanyItem> context;
        IRepository<Item> itemContext;

        public CompanyItemController(IRepository<CompanyItem> companyItemContext, IRepository<Item> itemCtx)
        {
            context = companyItemContext;
            itemContext = itemCtx;
        }

        public ActionResult Index(string companyName)
        {
            List<CompanyItem> companyItems = context.Collection().ToList();
            List<CompanyItem> companyItemsFiltered = new List<CompanyItem>();
            if (companyName == null)
            {
                companyItems = context.Collection().ToList();
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

        public ActionResult Create(string companyName)
        {
            CompanyItemViewModel viewModel = new CompanyItemViewModel();
            viewModel.CompanyItem = new CompanyItem();
            viewModel.CompanyItem.Company = companyName;
            viewModel.Items = itemContext.Collection();
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Create(CompanyItem companyItem)
        {
            if (!ModelState.IsValid)
            {
                return View(companyItem);
            }
            else
            {
                // NAPRAVLJENO ovde triba proc kroz sve iteme i vidit koji ima
                // isti id ka i companyItem i onda setirat
                // cijenu tog companyItema na cijenu to itema
                foreach (var item in itemContext.Collection().ToList())
                {
                    if (item.Name == companyItem.Item)
                    {
                        companyItem.Name = item.Name;
                        companyItem.Price = (int) item.Price;
                        companyItem.Item = item.Id;
                    }
                }
                context.Insert(companyItem);
                context.Commit();
                return RedirectToAction("Index");
            }
        }

        public ActionResult Edit(string Id)
        {
            CompanyItem companyItem = context.Find(Id);
            if (companyItem == null)
            {
                return HttpNotFound();
            }
            else
            {
                return View(companyItem);
            }
        }

        [HttpPost]
        public ActionResult Edit(CompanyItem companyItem, string Id)
        {
            CompanyItem companyItemToEdit = context.Find(Id);
            if (companyItemToEdit == null)
            {
                return HttpNotFound();
            }
            else
            {
                if (!ModelState.IsValid)
                {
                    return View(companyItem);
                }
                companyItemToEdit.Company = companyItem.Company;
                companyItemToEdit.Item = companyItem.Item;
                companyItemToEdit.Quantity = companyItem.Quantity;
                context.Commit();
                return RedirectToAction("Index");
            }
        }

        public ActionResult Delete(string Id)
        {
            CompanyItem companyItemToDelete = context.Find(Id);
            if (companyItemToDelete == null)
            {
                return HttpNotFound();
            }
            else
            {
                return View(companyItemToDelete);
            }
        }

        [HttpPost]
        [ActionName("Delete")]
        public ActionResult ConfirmDelete(string Id)
        {
            CompanyItem companyItemToDelete = context.Find(Id);
            if (companyItemToDelete == null)
            {
                return HttpNotFound();
            }
            else
            {
                context.Delete(Id);
                context.Commit();
                return View(companyItemToDelete);
            }
        }
    }
}