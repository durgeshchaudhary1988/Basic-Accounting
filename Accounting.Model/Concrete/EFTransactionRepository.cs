using Accounting.Model.Abstract;
using Accounting.Model.Model.Transaction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accounting.Model.Concrete
{
    public class EFTransactionRepository : ITransactionRepository
    {
        private EFDbContext context = new EFDbContext();

        public IEnumerable<TransactionSummary> TransactionSummaries
        {
            get
            {
                return context.TransactionSummaries;
            }
        }

        public IEnumerable<TransactionAccountDetail> TransactionAccountDetail
        {
            get
            {
                return context.TransactionAccountDetails;
            }
        }

        public int[] GetTransactionsIdsForLedgerAccount(int ledgerAccountId)
        {
            return context.TransactionAccountDetails.Where(x => x.LedgerAccountId == ledgerAccountId).Select(x => x.TransactionSummaryId).ToArray();
        }

        public List<TransactionSummary> GetTransactionSummaryForTransactionIds(IEnumerable<int> transactionIds)
        {
            return context.TransactionSummaries.Where(x => transactionIds.Contains(x.TransactionSummaryId)).ToList();
        }

        public List<TransactionAccountDetail> GetTransactionAccountDetailForTransactionIds(IEnumerable<int> transactionIds)
        {
            return context.TransactionAccountDetails.Where(x => transactionIds.Contains(x.TransactionSummaryId)).ToList();
        }

        public void SaveTransactionDetail(List<TransactionAccountDetail> transactionAccountDetails, int transactionSummaryId)
        {
            var existingDetail = context.TransactionAccountDetails.Where(x => x.TransactionSummaryId == transactionSummaryId);
            foreach (var e in existingDetail)
            {
                context.TransactionAccountDetails.Remove(e);
            }
            foreach (var t in transactionAccountDetails)
            {
                t.TransactionSummaryId = transactionSummaryId;
                t.TransactionAccountDetailId = 0;
                context.TransactionAccountDetails.Add(t);
            }
            context.SaveChanges();
        }

        public int SaveTransactionSummary(TransactionSummary transactionSummary)
        {
            if (transactionSummary.TransactionSummaryId == 0)
            {
                context.TransactionSummaries.Add(transactionSummary);
            }
            var dbEntry = context.TransactionSummaries.Find(transactionSummary.TransactionSummaryId);
            if (dbEntry != null)
            {
                dbEntry.TransactionDate = transactionSummary.TransactionDate;
                dbEntry.TransactionNarration = transactionSummary.TransactionNarration;
            }
            context.SaveChanges();
            return transactionSummary.TransactionSummaryId;
        }

        public int[] GetTransactionsIdsForLedgerAccount(int[] ledgerAccountIds)
        {
            return context.TransactionAccountDetails.Where(x => ledgerAccountIds.Contains(x.LedgerAccountId)).Select(x => x.TransactionSummaryId).ToArray();
        }

        public Dictionary<int, double> GetOpeningBalance(string asOfDate)
        {
            return (from s in context.TransactionSummaries.Where(x => x.TransactionDate.CompareTo(asOfDate) < 0)
                    join a in context.TransactionAccountDetails
                    on s.TransactionSummaryId equals a.TransactionSummaryId
                    group new { Amount = a.Amount, TransactionSide = a.TransactionSide } by a.LedgerAccountId into g
                    select g).ToDictionary(x => x.Key, x => x.Sum(y => y.Amount * (y.TransactionSide == TransactionSide.Credit ? 1 : -1)));
        }

        public Dictionary<int, double> GetNetBalance(string startDate, string endDate)
        {
            return (from s in context.TransactionSummaries.Where(x => x.TransactionDate.CompareTo(startDate) >= 0 && x.TransactionDate.CompareTo(endDate) <= 0)
                    join a in context.TransactionAccountDetails
                    on s.TransactionSummaryId equals a.TransactionSummaryId
                    group new { Amount = a.Amount, TransactionSide = a.TransactionSide } by a.LedgerAccountId into g
                    select g).ToDictionary(x => x.Key, x => x.Sum(y => y.Amount * (y.TransactionSide == TransactionSide.Credit ? 1 : -1)));
        }
    }
}
