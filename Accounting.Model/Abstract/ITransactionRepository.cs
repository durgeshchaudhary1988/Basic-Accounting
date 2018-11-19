using Accounting.Model.Model.Transaction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accounting.Model.Abstract
{
    public interface ITransactionRepository
    {

        IEnumerable<TransactionSummary> TransactionSummaries { get; }
        IEnumerable<TransactionAccountDetail> TransactionAccountDetail { get; }

        #region Transaction Summary
        int SaveTransactionSummary(TransactionSummary transactionSummary);
        void SaveTransactionDetail(List<TransactionAccountDetail> transactionAccountDetails, int transactionSummaryId);
        int[] GetTransactionsIdsForLedgerAccount(int ledgerAccountId);
        List<TransactionSummary> GetTransactionSummaryForTransactionIds(IEnumerable<int> transactionIds);
        List<TransactionAccountDetail> GetTransactionAccountDetailForTransactionIds(IEnumerable<int> transactionIds);
        int[] GetTransactionsIdsForLedgerAccount(int[] ledgerAccountIds);
        Dictionary<int, double> GetOpeningBalance(string asOfDate);
        Dictionary<int, double> GetNetBalance(string startDate, string endDate);
        #endregion
    }
}
