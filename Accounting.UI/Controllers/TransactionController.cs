using Accounting.Model.Abstract;
using Accounting.Model.Concrete;
using Accounting.Model.Model.Transaction;
using Accounting.Model.Utility;
using Accounting.UI.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Accounting.UI.Controllers
{
    public class TransactionController : Controller
    {
        private ITransactionRepository transactionRepository;
        private ILedgerRepository ledgerRepository;
        public TransactionController(ITransactionRepository repoTran,ILedgerRepository repoLedg)
        {
            transactionRepository = repoTran;
            ledgerRepository = repoLedg;
        }
        public ViewResult Create()
        {
            SetMetaDataForForm();
            return View("Edit", new CreateTransactionViewModel { TransactionAccounts = new List<TransactionAccountDetail> { new TransactionAccountDetail { TransactionSide = TransactionSide.Credit }, new TransactionAccountDetail { TransactionSide = TransactionSide.Debit } } });
        }
        public ViewResult Edit(int transactionSummaryId)
        {
            SetMetaDataForForm();
            return View(new CreateTransactionViewModel { TransactionAccounts = new List<TransactionAccountDetail> { new TransactionAccountDetail { } } });
        }
        [HttpPost]
        public ActionResult Edit(CreateTransactionViewModel transactionDetail)
        {
            if (Request.Form["Add"] != null)
            {
                transactionDetail.TransactionAccounts.Add(GetPendingTransactionAccountDetail(transactionDetail));
            }
            else if (ValidateTransaction(transactionDetail) && ModelState.IsValid)
            {
                var id = transactionRepository.SaveTransactionSummary(new TransactionSummary { TransactionDate = transactionDetail.TransactionDate, TransactionNarration = transactionDetail.TransactionNarration, TransactionSummaryId = transactionDetail.TransactionSummaryId });
                transactionRepository.SaveTransactionDetail(transactionDetail.TransactionAccounts, id);
                TempData["message"] = string.Format("Transaction saved successfully [{0}]", id);
                return RedirectToAction("Create");
            }
            SetMetaDataForForm();
            return View(transactionDetail);
        }
        public ViewResult ListLedgerAccount(int ledgerAccountId)
        {
            var ledgerAccount = ledgerAccountId.LedgerAccount();
            return View(PopulateTransactionDetailsForAccounts("Account", ledgerAccount.LedgerAccountName, new int[] { ledgerAccountId }, ledgerAccount.OpeningBalance, true));
        }
        public ViewResult ListLedgerHead(int ledgerHeadId)
        {
            var ledgerHead = ledgerHeadId.LedgerHead();
            var accountIds = ledgerRepository.GetLedgerAccountsForHead(ledgerHeadId, false).Select(x => x.LedgerAccountId).ToArray();
            return View("ListLedgerAccount", PopulateTransactionDetailsForAccounts("Head", ledgerHead.LedgerHeadName, accountIds, 0, false));
        }

        public ViewResult ViewTransactionReport(int transactionId)
        {
            var tranIds = new List<int> { transactionId };
            var summary  = transactionRepository.GetTransactionSummaryForTransactionIds(tranIds);
            var detail = transactionRepository.GetTransactionAccountDetailForTransactionIds(tranIds);
            return View(new ViewTransactionDetailViewModel
            {
                TransactionDate = summary[0].TransactionDate,
                TransactionNarration = summary[0].TransactionNarration,
                TransactionSummaryId = summary[0].TransactionSummaryId,
                CreditDetails = detail.Where(a => a.TransactionSide == TransactionSide.Credit).Select(a => new ViewTransactionDetailViewModel.TransactionLedgerAccount { AccountId = a.LedgerAccountId, AccountName = a.LedgerAccountId.LedgerAccount().LedgerAccountName, Amount = a.Amount }).ToList(),
                DebitDetails = detail.Where(a => a.TransactionSide == TransactionSide.Debit).Select(a => new ViewTransactionDetailViewModel.TransactionLedgerAccount { AccountId = a.LedgerAccountId, AccountName = a.LedgerAccountId.LedgerAccount().LedgerAccountName, Amount = a.Amount }).ToList()
            });
        }

        private void SetMetaDataForForm()
        {
            ViewBag.LedgerAccounts = CacheRepository.LedgerAccounts.OrderBy(x => x.LedgerAccountName).ToList();
        }
        private bool ValidateTransaction(CreateTransactionViewModel transactionDetail)
        {
            return GetBalance(transactionDetail) == 0;
        }
        private TransactionAccountDetail GetPendingTransactionAccountDetail(CreateTransactionViewModel transactionDetail)
        {
            var balance = GetBalance(transactionDetail);
            return new TransactionAccountDetail
            {
                TransactionSide = balance > 0 ? TransactionSide.Debit : TransactionSide.Credit,
                Amount = balance * (balance < 0 ? -1 : 1)
            };
        }
        private double GetBalance(CreateTransactionViewModel transactionDetail)
        {
            return transactionDetail.TransactionAccounts.Sum(x => x.Amount * (x.TransactionSide == TransactionSide.Credit ? 1 : -1));
        }
        private List<DisplayTransactionViewModel> PopulateTransactionDetailsForAccounts(string listType, string listTypeValue, int[] accountIds, double openingBalance, bool removeCurrent)
        {
            ViewBag.ListType = listType;
            ViewBag.ListTypeValue = listTypeValue;
            var transactionIds = transactionRepository.GetTransactionsIdsForLedgerAccount(accountIds);
            var transactionSummary = transactionRepository.GetTransactionSummaryForTransactionIds(transactionIds);
            var transactionAccountDetail = transactionRepository.GetTransactionAccountDetailForTransactionIds(transactionIds);
            var displayTransactionDetail = (from summary in transactionSummary.OrderBy(x => x.TransactionDate)
                                            select new DisplayTransactionViewModel
                                            {
                                                TransactionSummaryId = summary.TransactionSummaryId,
                                                TransactionDate = summary.TransactionDate,
                                                TransactionNarration = summary.TransactionNarration,
                                                TransactionAccounts = (from acc in transactionAccountDetail where acc.TransactionSummaryId == summary.TransactionSummaryId select acc).ToList(),
                                            }).ToList();
            double closingBal = openingBalance;
            foreach (var tran in displayTransactionDetail)
            {
                var ledgerDetail = (from l in tran.TransactionAccounts where accountIds.Contains(l.LedgerAccountId) select l).ToList();
                double creditAmount = 0;
                double debitAmount = 0;
                foreach (var l in ledgerDetail)
                {
                    if (removeCurrent)
                    {
                        tran.TransactionAccounts.Remove(l);
                    }
                    if (l.TransactionSide == TransactionSide.Credit)
                    {
                        creditAmount += l.Amount;
                    }
                    else
                    {
                        debitAmount += l.Amount;
                    }
                }
                closingBal += creditAmount - debitAmount;
                tran.CreditAmount = creditAmount;
                tran.DebitAmount = debitAmount;
                tran.Balance = closingBal;
            }
            ViewBag.OpeningBalance = openingBalance;
            ViewBag.ClosingBalance = closingBal;
            return displayTransactionDetail;
        }
    }
}