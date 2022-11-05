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
    public class BankAccountController : Controller
    {
        IRepository<BankAccount> context;
        IRepository<Partner> partnerContext;

        public BankAccountController(IRepository<BankAccount> bankAccountContext, IRepository<Partner> partnerCtx)
        {
            context = bankAccountContext;
            partnerContext = partnerCtx;
        }

        public ActionResult Index()
        {
            List<BankAccount> bankAccounts = context.Collection().ToList();
            return View(bankAccounts);
        }

        public ActionResult Create()
        {
            CompanyBankAccountViewModel viewModel = new CompanyBankAccountViewModel();
            viewModel.BankAccount = new BankAccount();
            viewModel.Partners = partnerContext.Collection();
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Create(BankAccount bankAccount)
        {
            if (!ModelState.IsValid)
            {
                return View(bankAccount);
            }
            else
            {
                context.Insert(bankAccount);
                context.Commit();
                return RedirectToAction("Index");
            }
        }

        public ActionResult Edit(string Id)
        {
            BankAccount bankAccount = context.Find(Id);
            if (bankAccount == null)
            {
                return HttpNotFound();
            }
            else
            {
                return View(bankAccount);
            }
        }

        [HttpPost]
        public ActionResult Edit(BankAccount bankAccount, string Id)
        {
            BankAccount bankAccountToEdit = context.Find(Id);
            if (bankAccountToEdit == null)
            {
                return HttpNotFound();
            }
            else
            {
                if (!ModelState.IsValid)
                {
                    return View(bankAccount);
                }
                bankAccountToEdit.Company = bankAccount.Company;
                bankAccountToEdit.AccountNo = bankAccount.AccountNo;
                bankAccountToEdit.Amount = bankAccount.Amount;
                context.Commit();
                return RedirectToAction("Index");
            }
        }

        public ActionResult Delete(string Id)
        {
            BankAccount bankAccountToDelete = context.Find(Id);
            if (bankAccountToDelete == null)
            {
                return HttpNotFound();
            }
            else
            {
                return View(bankAccountToDelete);
            }
        }

        [HttpPost]
        [ActionName("Delete")]
        public ActionResult ConfirmDelete(string Id)
        {
            BankAccount bankAccountToDelete = context.Find(Id);
            if (bankAccountToDelete == null)
            {
                return HttpNotFound();
            }
            else
            {
                context.Delete(Id);
                context.Commit();
                return View(bankAccountToDelete);
            }
        }
    }
}