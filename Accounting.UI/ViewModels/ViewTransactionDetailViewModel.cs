using Accounting.Model.Model.Transaction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Accounting.UI.ViewModels
{
    public class ViewTransactionDetailViewModel: TransactionSummary
    {
        public class TransactionLedgerAccount
        {
            public int AccountId { get; set; }
            public string AccountName { get; set; }
            public double Amount { get; set; }
        }
        public List<TransactionLedgerAccount> CreditDetails { get; set; }
        public List<TransactionLedgerAccount> DebitDetails { get; set; }
    }
}