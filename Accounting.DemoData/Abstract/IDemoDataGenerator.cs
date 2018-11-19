using Accounting.DemoData.Model;
using Accounting.Model.Abstract;
using Accounting.Model.Model.Transaction;
using System.Collections.Generic;

namespace Accounting.DemoData.Abstract
{
    /// <summary>
    /// Base class (abstract) which needs to be inherited in order to create a Dummy Data Generator.
    /// Inherited class can also be used to load production data at the time of installation.
    /// </summary>
    public abstract class IDemoDataGenerator
    {
        #region Constructor
        /// <summary>
        /// Constructor for Demo Data Generator
        /// </summary>
        /// <param name="ledgerRepo">Repository provider for Ledger</param>
        /// <param name="tranRepo">Repository provider for Transaction</param>
        public IDemoDataGenerator(ILedgerRepository ledgerRepo, ITransactionRepository tranRepo)
        {
            LedgerRepository = ledgerRepo;
            TransactionRepository = tranRepo;
        }
        #endregion

        #region DataRepositories
        private ILedgerRepository LedgerRepository = null;
        private ITransactionRepository TransactionRepository = null;
        #endregion

        #region Internal Account Map
        private Dictionary<string, int> _ledgerHeadMap = new Dictionary<string, int>();
        private Dictionary<string, int> _ledgerAccountMap = new Dictionary<string, int>();
        #endregion

        #region Abstract Members
        /// <summary>
        /// Override this method and provide Chart of Accounts
        /// </summary>
        /// <returns>Chart of Accounts</returns>
        public abstract List<SampleLedgerType> GetAccountingStructure();
        /// <summary>
        /// Override this method and provide Transactions to be loaded in system
        /// </summary>
        /// <returns>List of Transactions</returns>
        public abstract List<SampleTransaction> GetTransactions();
        #endregion

        #region Public Members to Load Data
        /// <summary>
        /// Loads Chart of Accounts in system
        /// </summary>
        public void CreateAccountingStructure()
        {
            var accountingStructure = GetAccountingStructure();
            if (accountingStructure != null && accountingStructure.Count > 0)
            {
                foreach (var ledgerTypeStructure in accountingStructure)
                {
                    LedgerRepository.SaveLedgerType(ledgerTypeStructure.ledgerType);
                    if (ledgerTypeStructure.SampleLedgerHeads != null && ledgerTypeStructure.SampleLedgerHeads.Count > 0)
                    {
                        foreach (var ledgerHeadStructure in ledgerTypeStructure.SampleLedgerHeads)
                        {
                            ledgerHeadStructure.ledgerHead.ParentLedgerTypeId = ledgerTypeStructure.ledgerType.LedgerTypeId;
                            ledgerHeadStructure.ledgerHead.ParentLedgerHeadId = 0;
                            CreateLedgerHead(ledgerHeadStructure);
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Loads Transactions in system
        /// </summary>
        /// <returns>Accounts not found while loading transactions</returns>
        public List<string> CreateTransactions()
        {
            var errors = new List<string>();
            var transactions = GetTransactions();
            foreach (var tran in transactions)
            {
                if (!_ledgerAccountMap.ContainsKey(tran.CreditAccount.ToLower()))
                {
                    errors.Add(tran.CreditAccount);
                }
                else
                {
                    tran.CreditAccountId = _ledgerAccountMap[tran.CreditAccount.ToLower()];
                }
                if (!_ledgerAccountMap.ContainsKey(tran.DebitAccount.ToLower()))
                {
                    errors.Add(tran.DebitAccount);
                }
                else
                {
                    tran.DebitAccountId = _ledgerAccountMap[tran.DebitAccount.ToLower()];
                }
                if (tran.CreditAccountId > 0 && tran.DebitAccountId > 0)
                {
                    var tranId = TransactionRepository.SaveTransactionSummary(new TransactionSummary { TransactionNarration = tran.TransactionNarration, TransactionDate = tran.TransactionDate });
                    TransactionRepository.SaveTransactionDetail(new List<TransactionAccountDetail>
                    {
                        new TransactionAccountDetail
                        {
                            Amount = tran.Amount,
                            TransactionSide = TransactionSide.Credit,
                            LedgerAccountId = tran.CreditAccountId
                        },
                        new TransactionAccountDetail
                        {
                            Amount = tran.Amount,
                            TransactionSide = TransactionSide.Debit,
                            LedgerAccountId = tran.DebitAccountId
                        }
                    }, tranId);
                }
            }
            return errors;
        }
        #endregion

        #region Private Members
        private void CreateLedgerHead(SampleLedgerHead ledgerHeadStructure)
        {
            LedgerRepository.SaveLedgerHead(ledgerHeadStructure.ledgerHead);
            _ledgerHeadMap.Add(ledgerHeadStructure.ledgerHead.LedgerHeadName.ToLower(), ledgerHeadStructure.ledgerHead.LedgerHeadId);
            if (ledgerHeadStructure.SampleLedgerHeads != null && ledgerHeadStructure.SampleLedgerHeads.Count > 0)
            {
                foreach (var headStructure in ledgerHeadStructure.SampleLedgerHeads)
                {
                    headStructure.ledgerHead.ParentLedgerTypeId = ledgerHeadStructure.ledgerHead.ParentLedgerTypeId;
                    headStructure.ledgerHead.ParentLedgerHeadId = ledgerHeadStructure.ledgerHead.LedgerHeadId;
                    CreateLedgerHead(headStructure);
                }
            }
            if (ledgerHeadStructure.SampleLedgerAccounts != null && ledgerHeadStructure.SampleLedgerAccounts.Count > 0)
            {
                foreach (var ledgerAccount in ledgerHeadStructure.SampleLedgerAccounts)
                {
                    ledgerAccount.ParentLedgerHeadId = ledgerHeadStructure.ledgerHead.LedgerHeadId;
                    LedgerRepository.SaveLedgerAccount(ledgerAccount);
                    _ledgerAccountMap.Add(ledgerAccount.LedgerAccountName.ToLower(), ledgerAccount.LedgerAccountId);
                }
            }
        }
        #endregion
    }
}
