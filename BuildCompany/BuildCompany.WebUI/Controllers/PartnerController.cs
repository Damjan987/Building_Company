using BuildCompany.Core.Contracts;
using BuildCompany.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BuildCompany.WebUI.Controllers
{
    public class PartnerController : Controller
    {
        IRepository<Partner> context;

        public PartnerController(IRepository<Partner> partnerContext)
        {
            context = partnerContext;
        }

        public ActionResult Index()
        {
            List<Partner> partners = context.Collection().ToList();
            return View(partners);
        }

        public ActionResult Create()
        {
            Partner partner = new Partner();
            return View(partner);
        }

        [HttpPost]
        public ActionResult Create(Partner partner)
        {
            if (!ModelState.IsValid)
            {
                return View(partner);
            }
            else
            {
                context.Insert(partner);
                context.Commit();
                return RedirectToAction("Index");
            }
        }

        public ActionResult Edit(string Id)
        {
            Partner partner = context.Find(Id);
            if (partner == null)
            {
                return HttpNotFound();
            }
            else
            {
                return View(partner);
            }
        }

        [HttpPost]
        public ActionResult Edit(Partner partner, string Id)
        {
            Partner partnerToEdit = context.Find(Id);
            if (partnerToEdit == null)
            {
                return HttpNotFound();
            }
            else
            {
                if (!ModelState.IsValid)
                {
                    return View(partner);
                }
                partnerToEdit.Name = partner.Name;
                partnerToEdit.Status = partner.Status;
                context.Commit();
                return RedirectToAction("Index");
            }
        }

        public ActionResult Delete(string Id)
        {
            Partner partnerToDelete = context.Find(Id);
            if (partnerToDelete == null)
            {
                return HttpNotFound();
            }
            else
            {
                return View(partnerToDelete);
            }
        }

        [HttpPost]
        [ActionName("Delete")]
        public ActionResult ConfirmDelete(string Id)
        {
            Partner partnerToDelete = context.Find(Id);
            if (partnerToDelete == null)
            {
                return HttpNotFound();
            }
            else
            {
                context.Delete(Id);
                context.Commit();
                return View(partnerToDelete);
            }
        }
    }
}