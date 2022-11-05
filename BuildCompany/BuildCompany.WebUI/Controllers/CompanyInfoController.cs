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
    public class CompanyInfoController : Controller
    {
        IRepository<CompanyInfo> context;
        IRepository<BankAccount> bankAccountContext;
        IRepository<CompanyItem> companyItemContext;

        public CompanyInfoController(IRepository<CompanyInfo> companyInfoContext, IRepository<BankAccount> bankAccountCtx, IRepository<CompanyItem> companyItemCtx)
        {
            context = companyInfoContext;
            bankAccountContext = bankAccountCtx;
            companyItemContext = companyItemCtx;
        }

        public ActionResult Index()
        {
            List<CompanyInfo> companyInfos = context.Collection().ToList();
            return View(companyInfos);
        }

        public ActionResult Create()
        {
            CompanyInfo companyInfo = new CompanyInfo();
            return View(companyInfo);
        }

        [HttpPost]
        public ActionResult Create(CompanyInfo companyInfo)
        {
            if (!ModelState.IsValid)
            {
                return View(companyInfo);
            }
            else
            {
                context.Insert(companyInfo);
                context.Commit();
                return RedirectToAction("Index");
            }
        }

        public ActionResult Edit(string Id)
        {
            CompanyInfo companyInfo = context.Find(Id);
            if (companyInfo == null)
            {
                return HttpNotFound();
            }
            else
            {
                return View(companyInfo);
            }
        }

        [HttpPost]
        public ActionResult Edit(CompanyInfo companyInfo, string Id)
        {
            CompanyInfo companyInfoToEdit = context.Find(Id);
            if (companyInfoToEdit == null)
            {
                return HttpNotFound();
            }
            else
            {
                if (!ModelState.IsValid)
                {
                    return View(companyInfo);
                }
                companyInfoToEdit.Name = companyInfo.Name;
                context.Commit();
                return RedirectToAction("Index");
            }
        }

        public ActionResult Delete(string Id)
        {
            CompanyInfo companyInfoToDelete = context.Find(Id);
            if (companyInfoToDelete == null)
            {
                return HttpNotFound();
            }
            else
            {
                return View(companyInfoToDelete);
            }
        }

        [HttpPost]
        [ActionName("Delete")]
        public ActionResult ConfirmDelete(string Id)
        {
            CompanyInfo companyInfoToDelete = context.Find(Id);
            if (companyInfoToDelete == null)
            {
                return HttpNotFound();
            }
            else
            {
                context.Delete(Id);
                context.Commit();
                return View(companyInfoToDelete);
            }
        }

        public ActionResult HomePage()
        {
            IEnumerable<BankAccount> bankAccounts = bankAccountContext.Collection();
            List<BankAccount> bankAccountsFiltered = new List<BankAccount>();
            foreach (var account in bankAccounts)
            {
                if (account.Company == context.Collection().ToList()[0].Name)
                {
                    bankAccountsFiltered.Add(account);
                }
            }
            IEnumerable<CompanyItem> companyItems = companyItemContext.Collection();
            List<CompanyItem> companyItemsFiltered = new List<CompanyItem>();
            foreach (var item in companyItems)
            {
                if (item.Company == context.Collection().ToList()[0].Id)
                {
                    companyItemsFiltered.Add(item);
                }
            }
            HomePageViewModel viewModel = new HomePageViewModel();
            viewModel.CompanyInfo = context.Collection().ToList()[0];
            viewModel.BankAccounts = bankAccountsFiltered;
            viewModel.CompanyItems = companyItemsFiltered;

            return View(viewModel);
        }

        public ActionResult RemoveItemFromStorage(string item)
        {
            companyItemContext.Delete(item);
            companyItemContext.Commit();
            return RedirectToAction("HomePage");
        }
    }
}