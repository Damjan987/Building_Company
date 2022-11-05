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
    public class ItemController : Controller
    {
        IRepository<Item> context;
        IRepository<Category> categoryContext;

        public ItemController(IRepository<Item> itemContext, IRepository<Category> categoryCtx)
        {
            context = itemContext;
            categoryContext = categoryCtx;
        }

        public ActionResult Index(string category)
        {
            CategoryItemsViewModel viewModel = new CategoryItemsViewModel();
            viewModel.Categories = categoryContext.Collection().ToList();
            if (category != null)
            {
                List<Item> filteredItems = new List<Item>();
                foreach (var i in context.Collection().ToList())
                {
                    if (i.Category == category)
                    {
                        filteredItems.Add(i);
                    }
                }
                viewModel.Items = filteredItems;
            }
            else
            {
                viewModel.Items = context.Collection().ToList();
            }

            return View(viewModel);
        }

        public ActionResult Create()
        {
            CategoryItemViewModel viewModel = new CategoryItemViewModel();
            viewModel.Item = new Item();
            viewModel.Categories = categoryContext.Collection();
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Create(Item item)
        {
            if (!ModelState.IsValid)
            {
                return View(item);
            }
            else
            {
                context.Insert(item);
                context.Commit();
                return RedirectToAction("Index");
            }
        }

        public ActionResult Edit(string Id)
        {
            Item item = context.Find(Id);
            if (item == null)
            {
                return HttpNotFound();
            }
            else
            {
                return View(item);
            }
        }

        [HttpPost]
        public ActionResult Edit(Item item, string Id)
        {
            Item itemToEdit = context.Find(Id);
            if (itemToEdit == null)
            {
                return HttpNotFound();
            }
            else
            {
                if (!ModelState.IsValid)
                {
                    return View(item);
                }
                itemToEdit.Name = item.Name;
                itemToEdit.Price = item.Price;
                itemToEdit.Description = item.Description;
                itemToEdit.Image = item.Image;
                itemToEdit.Category = item.Category;
                context.Commit();
                return RedirectToAction("Index");
            }
        }

        public ActionResult Delete(string Id)
        {
            Item itemToDelete = context.Find(Id);
            if (itemToDelete == null)
            {
                return HttpNotFound();
            }
            else
            {
                return View(itemToDelete);
            }
        }

        [HttpPost]
        [ActionName("Delete")]
        public ActionResult ConfirmDelete(string Id)
        {
            Item itemToDelete = context.Find(Id);
            if (itemToDelete == null)
            {
                return HttpNotFound();
            }
            else
            {
                context.Delete(Id);
                context.Commit();
                return View(itemToDelete);
            }
        }
    }
}