using Accounting.Model.Abstract;
using Accounting.Model.Concrete;
using Accounting.UI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Accounting.UI.Controllers
{
    public class ReportController : Controller
    {
        private ITransactionRepository transactionRepository;
        private ILedgerRepository ledgerRepository;
        public ReportController(ITransactionRepository repoTran, ILedgerRepository repoLedg)
        {
            transactionRepository = repoTran;
            ledgerRepository = repoLedg;
        }
        public void IncomeStatement()
        {

        }

        private ReportViewModel CreateReport(string startDate, string endDate, int leftSideId, int rightSideId, string leftHeader, string rightHeader, string reportName, bool includeOpeningBalance, string leftBalance,string rightBalance)
        {
            var report = new ReportViewModel
            {
                ReportName = reportName,
                LeftHeader = leftHeader,
                RightHeader = rightHeader,
                LeftBalance = leftBalance,
                RightBalance = rightBalance,
                StartDate = startDate,
                EndDate = endDate,
                LeftEntries = new List<ReportViewModel.ReportEntry> { },
                RightEntries = new List<ReportViewModel.ReportEntry> { }
            };
            Dictionary<int, ReportViewModel.ReportEntry> accounts = new Dictionary<int, ReportViewModel.ReportEntry>();
            Dictionary<int, ReportViewModel.ReportEntry> heads = new Dictionary<int, ReportViewModel.ReportEntry>();

            Dictionary<int, double> openingBalance = includeOpeningBalance ? transactionRepository.GetOpeningBalance(startDate) : new Dictionary<int, double>();

            var netBalance = transactionRepository.GetNetBalance(startDate, endDate);

            foreach (var head in ledgerRepository.LedgerHeads)
            {
                heads.Add(head.LedgerHeadId, new ReportViewModel.ReportEntry
                {
                    CreditAmount = 0,
                    EntryType = ReportViewModel.ReportEntryType.Head,
                    Id = head.LedgerHeadId,
                    Name = head.LedgerHeadName,
                    OpeningBalance = 0,
                    SubEntry = new List<ReportViewModel.ReportEntry> { }
                });
            }

            foreach (var head in ledgerRepository.LedgerHeads)
            {
                if (head.ParentLedgerHeadId != 0)
                {
                    heads[head.ParentLedgerHeadId].SubEntry.Add(heads[head.LedgerHeadId]);
                }
            }

            foreach (var account in ledgerRepository.LedgerAccounts)
            {
                accounts.Add(account.LedgerAccountId, new ReportViewModel.ReportEntry
                {
                    OpeningBalance = account.OpeningBalance,
                    Id = account.LedgerAccountId,
                    CreditAmount = account.OpeningBalance,
                    EntryType = ReportViewModel.ReportEntryType.Account,
                    Name = account.LedgerAccountName,
                    SubEntry = null
                });
                heads[account.ParentLedgerHeadId].SubEntry.Add(accounts[account.LedgerAccountId]);
            }

            foreach (var key in openingBalance.Keys)
            {
                accounts[key].OpeningBalance += openingBalance[key];
                accounts[key].CreditAmount += openingBalance[key];
            }

            foreach (var key in netBalance.Keys)
            {
                accounts[key].CreditAmount += netBalance[key];
            }

            foreach (var head in ledgerRepository.LedgerHeads)
            {
                if (head.ParentLedgerHeadId == 0)
                {
                    if (head.ParentLedgerTypeId == leftSideId)
                    {
                        report.LeftEntries.Add(heads[head.LedgerHeadId]);
                    }
                    else if (head.ParentLedgerTypeId == rightSideId)
                    {
                        report.RightEntries.Add(heads[head.LedgerHeadId]);
                    }
                    CalculateBalanceSheet(heads, head.LedgerHeadId);
                }
            }

            return report;
        }

        public ViewResult BalanceSheet()
        {
            var reportYear = DateTime.Now.Month > 3 ? (DateTime.Now.Year + 1) : DateTime.Now.Year;
            var EndDate = reportYear.ToString() + "-03-31";
            var StartDate = (reportYear - 1).ToString() + "-04-01";

            // var EndDate = "2018-10-31";
            // var StartDate = "2018-10-01";
            int rightSideId = ledgerRepository.LedgerTypes.Where(x => x.LedgerTypeName.ToLower().StartsWith("asset")).Select(x => x.LedgerTypeId).FirstOrDefault();
            int leftSideId = ledgerRepository.LedgerTypes.Where(x => x.LedgerTypeName.ToLower().StartsWith("lia")).Select(x => x.LedgerTypeId).FirstOrDefault();

            var report = CreateReport(StartDate, EndDate, leftSideId, rightSideId, "Liabilities", "Assets", "Balance Sheet",true,"Credit","Debit");

            var PnLAmount = report.LeftEntries.Sum(x => x.CreditAmount) + report.RightEntries.Sum(x => x.CreditAmount);

            if (PnLAmount <= 0)
            {
                report.LeftEntries.Add(new ReportViewModel.ReportEntry { CreditAmount = PnLAmount * -1, EntryType = ReportViewModel.ReportEntryType.Head, Name = "Profit", OpeningBalance = 0 });
            }
            else
            {
                report.RightEntries.Add(new ReportViewModel.ReportEntry { CreditAmount = PnLAmount, EntryType = ReportViewModel.ReportEntryType.Head, Name = "Loss", OpeningBalance = 0 });
            }

            return View("Report",report);
        }
        public ViewResult PnL()
        {
            var reportYear = DateTime.Now.Month > 3 ? (DateTime.Now.Year + 1) : DateTime.Now.Year;
            var EndDate = reportYear.ToString() + "-03-31";
            var StartDate = (reportYear - 1).ToString() + "-04-01";

            // var EndDate = "2018-10-31";
            // var StartDate = "2018-10-01";
            int rightSideId = ledgerRepository.LedgerTypes.Where(x => x.LedgerTypeName.ToLower().StartsWith("revenue")).Select(x => x.LedgerTypeId).FirstOrDefault();
            int leftSideId = ledgerRepository.LedgerTypes.Where(x => x.LedgerTypeName.ToLower().StartsWith("expense")).Select(x => x.LedgerTypeId).FirstOrDefault();

            var report = CreateReport(StartDate, EndDate, leftSideId, rightSideId, "Expense", "Revenue", "Profit n Loss",false,"Debit","Credit");

            var PnLAmount = report.LeftEntries.Sum(x => x.CreditAmount) + report.RightEntries.Sum(x => x.CreditAmount);

            if (PnLAmount >= 0)
            {
                report.LeftEntries.Add(new ReportViewModel.ReportEntry { CreditAmount = PnLAmount * -1, EntryType = ReportViewModel.ReportEntryType.Head, Name = "Profit", OpeningBalance = 0 });
            }
            else
            {
                report.RightEntries.Add(new ReportViewModel.ReportEntry { CreditAmount = PnLAmount, EntryType = ReportViewModel.ReportEntryType.Head, Name = "Loss", OpeningBalance = 0 });
            }

            return View("Report",report);
        }

        private void CalculateBalanceSheet(Dictionary<int, ReportViewModel.ReportEntry> entries, int id)
        {
            if (entries[id].SubEntry != null && entries[id].SubEntry.Count > 0)
            {
                foreach (var entry in entries[id].SubEntry)
                {
                    if (entry.EntryType == ReportViewModel.ReportEntryType.Head)
                    {
                        CalculateBalanceSheet(entries, entry.Id);
                    }
                    entries[id].CreditAmount += entry.CreditAmount;
                }
            }
        }

        public void CashFlowStatement()
        {

        }

        public ActionResult GenerateSQLScripts()
        {

            // "SET IDENTITY_INSERT LedgerTypes ON";

            var query = PrependQuery("LedgerTypes", string.Join("," + Environment.NewLine, CacheRepository.LedgerTypes.Select(x => string.Format("({0},'{1}',{2})", x.LedgerTypeId, x.LedgerTypeName, (x.CanParticipateInPnL ? 1 : 0)))));
            query += PrependQuery("LedgerHeads", string.Join("," + Environment.NewLine, CacheRepository.LedgerHeads.Select(x => string.Format("({0},'{1}','{2}',{3},{4},{5})", x.LedgerHeadId, x.LedgerHeadName, x.LedgerHeadDescription, x.ParentLedgerTypeId, x.ParentLedgerTypeId, (x.AffectsGrossPnL ? 1 : 0)))));
            query += PrependQuery("LedgerAccounts", string.Join("," + Environment.NewLine, CacheRepository.LedgerAccounts.Select(x => string.Format("({0},'{1}',{2},{3},{4})", x.LedgerAccountId, x.LedgerAccountName, x.ParentLedgerHeadId, x.OpeningBalance, (x.AffectsInventory ? 1 : 0)))));

            query += PrependQuery("TransactionSummaries", string.Join("," + Environment.NewLine, transactionRepository.TransactionSummaries.Select(x => string.Format("('{0}','{1}','{2}')", x.TransactionSummaryId, x.TransactionDate, x.TransactionNarration))));
            query += PrependQuery("TransactionAccountDetails", string.Join("," + Environment.NewLine, transactionRepository.TransactionAccountDetail.Select(x => string.Format("({0},{1},{2},{3},{4})", x.TransactionAccountDetailId, x.LedgerAccountId, (int)x.TransactionSide, x.Amount, x.TransactionSummaryId))));


            return View((object)query);
        }
        private string PrependQuery(string tableName, string query)
        {
            return string.Format("SET IDENTITY_INSERT {0} ON{1}GO{1}INSERT INTO {0} VALUES{1}{2}{1}GO{1}SET IDENTITY_INSERT {0} OFF{1}GO{1}", tableName, Environment.NewLine, query);
        }
    }
}