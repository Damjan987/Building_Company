using BuildCompany.Core.Contracts;
using BuildCompany.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BuildCompany.WebUI.Controllers
{
    public class CategoryController : Controller
    {
        IRepository<Category> context;

        public CategoryController(IRepository<Category> categoryContext)
        {
            context = categoryContext;
        }

        public ActionResult Index()
        {
            List<Category> categories = context.Collection().ToList();
            return View(categories);
        }

        public ActionResult Create()
        {
            Category category = new Category();
            return View(category);
        }

        [HttpPost]
        public ActionResult Create(Category category)
        {
            if (!ModelState.IsValid)
            {
                return View(category);
            }
            else
            {
                context.Insert(category);
                context.Commit();
                return RedirectToAction("Index");
            }
        }

        public ActionResult Edit(string Id)
        {
            Category category = context.Find(Id);
            if (category == null)
            {
                return HttpNotFound();
            }
            else
            {
                return View(category);
            }
        }

        [HttpPost]
        public ActionResult Edit(Category category, string Id)
        {
            Category categoryToEdit = context.Find(Id);
            if (categoryToEdit == null)
            {
                return HttpNotFound();
            }
            else
            {
                if (!ModelState.IsValid)
                {
                    return View(category);
                }
                categoryToEdit.Name = category.Name;
                context.Commit();
                return RedirectToAction("Index");
            }
        }

        public ActionResult Delete(string Id)
        {
            Category categoryToDelete = context.Find(Id);
            if (categoryToDelete == null)
            {
                return HttpNotFound();
            }
            else
            {
                return View(categoryToDelete);
            }
        }

        [HttpPost]
        [ActionName("Delete")]
        public ActionResult ConfirmDelete(string Id)
        {
            Category categoryToDelete = context.Find(Id);
            if (categoryToDelete == null)
            {
                return HttpNotFound();
            }
            else
            {
                context.Delete(Id);
                context.Commit();
                return View(categoryToDelete);
            }
        }
    }
}