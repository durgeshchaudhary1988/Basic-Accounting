using Accounting.DemoData.Abstract;
using Accounting.Model.Model.Ledger;
using System;
using System.Collections.Generic;
using System.Linq;
using Accounting.DemoData.Model;
using Accounting.Model.Abstract;

namespace Accounting.DemoData.Concrete
{
    /// <summary>
    /// Sample data for sake of study
    /// </summary>
    public class SampleData : IDemoDataGenerator
    {
        #region Constructor
        public SampleData(ILedgerRepository ledgerRepo, ITransactionRepository tranRepo) : base(ledgerRepo, tranRepo)
        {

        }
        #endregion

        #region IDemoDataGenerator members
        public override List<SampleLedgerType> GetAccountingStructure()
        {
            var response = new List<SampleLedgerType>();
            return AddExpense(AddRevenue(AddLiabilities(AddAssets(new List<SampleLedgerType> { }))));
        }
        public override List<SampleTransaction> GetTransactions()
        {
            var transactions = new List<SampleTransaction> { };

            #region Expense
            transactions.AddRange(FixedTransactions("HDFC A/c No. 0001", "SBI RD A/c No. RD0001", "Scheduled Transaction for RD", 5000, new string[] { "2018-01-01", "2018-02-01", "2018-03-01", "2018-04-01", "2018-05-01", "2018-06-01", "2018-07-01", "2018-08-01", "2018-09-01", "2018-10-01", "2018-11-01", "2018-12-01" }));
            transactions.AddRange(FixedTransactions("ICICI A/c No. 0001", "SBI RD A/c No. RD0002", "Scheduled Transaction for RD", 10000, new string[] { "2018-01-05", "2018-02-05", "2018-03-05", "2018-04-05", "2018-05-05", "2018-06-05", "2018-07-05", "2018-08-05", "2018-09-05", "2018-10-05", "2018-11-05", "2018-12-05" }));
            transactions.AddRange(FixedTransactions("HDFC A/c No. 0001", "Rent", "Rent", 8200, new string[] { "2018-01-01", "2018-02-01", "2018-03-01", "2018-04-01", "2018-05-01", "2018-06-01", "2018-07-01", "2018-08-01", "2018-09-01", "2018-10-01", "2018-11-01", "2018-12-01" }));
            transactions.AddRange(FixedTransactions("HDFC A/c No. 0001", "Society Maintenance Charge", "Rent", 2600, new string[] { "2018-01-01", "2018-02-01", "2018-03-01", "2018-04-01", "2018-05-01", "2018-06-01", "2018-07-01", "2018-08-01", "2018-09-01", "2018-10-01", "2018-11-01", "2018-12-01" }));
            transactions.AddRange(RangeTransactions("CITI A/c No. 0001", "Electricity", "Electricity Bill", 3000, 7000, new string[] { "2018-01-05", "2018-02-04", "2018-03-05", "2018-04-02", "2018-05-01", "2018-06-08", "2018-07-07", "2018-08-05", "2018-09-04", "2018-10-02", "2018-11-01", "2018-12-10" }));
            transactions.AddRange(RangeTransactions("CITI A/c No. 0001", "Mobile Bills", "Mobile Bill for number 98xxxxxxxx", 1200, 2200, new string[] { "2018-01-10", "2018-02-10", "2018-03-10", "2018-04-10", "2018-05-10", "2018-06-10", "2018-07-10", "2018-08-10", "2018-09-10", "2018-10-10", "2018-11-10", "2018-12-10" }));
            transactions.AddRange(FixedTransactions("ICICI A/c No. 0001", "Society Events", "Events", 500, new string[] { "2018-01-05", "2018-03-05", "2018-08-05", "2018-10-05", "2018-12-05" }));
            transactions.AddRange(FixedTransactions("ICICI A/c No. 0001", "Doctor Fee", "Doctor Fee", 400, new string[] { "2018-02-15", "2018-07-30" }));
            #endregion


            transactions.AddRange(RangeTransactions("Demo Company", "HDFC A/c No. 0001", "Salary", 200000, 220000, new string[] { "2018-01-01", "2018-02-01", "2018-03-01", "2018-04-01", "2018-05-01", "2018-06-01", "2018-07-01", "2018-08-01", "2018-09-01", "2018-10-01", "2018-11-01", "2018-12-01" }));
            transactions.AddRange(RangeTransactions("Interest on Savings A/c", "HDFC A/c No. 0001", "Savings Interest", 100, 500, new string[] { "2018-03-31", "2018-06-30", "2018-09-30", "2018-12-31" }));
            transactions.AddRange(RangeTransactions("Interest on Savings A/c", "SBI A/c No. 0001", "Savings Interest", 100, 500, new string[] { "2018-03-31", "2018-06-30", "2018-09-30", "2018-12-31" }));
            transactions.AddRange(RangeTransactions("Interest on Savings A/c", "ICICI A/c No. 0001", "Savings Interest", 100, 500, new string[] { "2018-03-31", "2018-06-30", "2018-09-30", "2018-12-31" }));
            transactions.AddRange(RangeTransactions("Interest on Savings A/c", "CITI A/c No. 0001", "Savings Interest", 100, 500, new string[] { "2018-03-31", "2018-06-30", "2018-09-30", "2018-12-31" }));

            transactions.AddRange(RangeTransactions("Interest on Deposit A/c", "SBI RD A/c No. RD0001", "Deposit Interest", 100, 500, new string[] { "2018-03-31", "2018-06-30", "2018-09-30", "2018-12-31" }));
            transactions.AddRange(RangeTransactions("Interest on Deposit A/c", "SBI RD A/c No. RD0002", "Deposit Interest", 100, 500, new string[] { "2018-03-31", "2018-06-30", "2018-09-30", "2018-12-31" }));

            transactions.AddRange(FixedTransactions("TCS", "HDFC A/c No. 0001", "Dividend", 2235, new string[] { "2018-03-22" }));
            transactions.AddRange(FixedTransactions("TCS", "HDFC A/c No. 0001", "Dividend", 2235 * 2, new string[] { "2018-06-22" }));
            transactions.AddRange(FixedTransactions("TCS", "HDFC A/c No. 0001", "Dividend", 2235 * 4, new string[] { "2018-07-22" }));

            transactions.AddRange(FixedTransactions("ICICI", "HDFC A/c No. 0001", "Dividend", 45000, new string[] { "2018-10-18" }));

            transactions.AddRange(FixedTransactions("RECLTD", "HDFC A/c No. 0001", "Dividend", 2000 * .07, new string[] { "2018-09-19" }));

            return transactions;
        }
        #endregion

        #region Ledger Type definition for Sample Chart of Accounts
        private List<SampleLedgerType> AddAssets(List<SampleLedgerType> accountingStructure)
        {
            accountingStructure.Add(new SampleLedgerType
            {
                ledgerType = new LedgerType
                {
                    LedgerTypeName = "Assets",
                    CanParticipateInPnL = false,
                },
                SampleLedgerHeads = new List<SampleLedgerHead>
                {
                    GetSampleLedgerHead("Cash A/c",new string [] { "Cash in Hand" }),
                    GetSampleLedgerHead("Bank A/c",new string[] {"HDFC A/c No. 0001","SBI A/c No. 0001","ICICI A/c No. 0001","CITI A/c No. 0001" }),
                    new SampleLedgerHead
                    {
                        ledgerHead = GetLedgerHead("Investments"),
                        SampleLedgerHeads = new List<SampleLedgerHead>
                        {
                            GetSampleLedgerHead("Fixed Deposits",new string[] {"SBI FD A/c No. 0001","SBI FD A/c No. 0002","SBI FD A/c No. 0003","SBI FD A/c No. 0004" }),
                            GetSampleLedgerHead("Recurring Deposits",new string[] {"SBI RD A/c No. RD0001","SBI RD A/c No. RD0002","SBI RD A/c No. RD0003","SBI RD A/c No. RD0004" })
                        }
                    }
                }
            });
            return accountingStructure;
        }
        private List<SampleLedgerType> AddLiabilities(List<SampleLedgerType> accountingStructure)
        {
            accountingStructure.Add(new SampleLedgerType
            {
                ledgerType = new LedgerType { LedgerTypeName = "Liabilities", CanParticipateInPnL = false },
                SampleLedgerHeads = new List<SampleLedgerHead>
                {
                    GetSampleLedgerHead("Accounts Payable",null),
                    GetSampleLedgerHead("Capital A/c",null),
                    GetSampleLedgerHead("Current Liabilities",null),
                    GetSampleLedgerHead("Loans (Liabilities)",new string[] { "HDFC Credit Card", "SBI Credit Card", "ICICI Credit Card" })
                }
            });
            return accountingStructure;
        }
        private List<SampleLedgerType> AddRevenue(List<SampleLedgerType> accountingStructure)
        {
            accountingStructure.Add(new SampleLedgerType
            {
                ledgerType = new LedgerType { LedgerTypeName = "Revenue", CanParticipateInPnL = false },
                SampleLedgerHeads = new List<SampleLedgerHead>
                {
                    GetSampleLedgerHead("Direct Income",new string[] { "Demo Company"  }),
                    new SampleLedgerHead
                    {
                        ledgerHead = GetLedgerHead("Indirect Income"),
                        SampleLedgerHeads = new List<SampleLedgerHead>
                        {
                            GetSampleLedgerHead("Dividends", new string[]{ "TCS", "RECLTD", "ICICI" }),
                            GetSampleLedgerHead("Prize/Bonus",new string[] { "BHIM Reward", "Google Pay" })
                        },
                        SampleLedgerAccounts = GetLedgerAccounts(new string[] { "Interest on Savings A/c","Interest on Deposit A/c" })
                    }
                }
            });
            return accountingStructure;
        }
        private List<SampleLedgerType> AddExpense(List<SampleLedgerType> accountingStructure)
        {
            accountingStructure.Add(new SampleLedgerType
            {
                ledgerType = new LedgerType { LedgerTypeName = "Expense", CanParticipateInPnL = false },
                SampleLedgerHeads = new List<SampleLedgerHead>
                {
                    GetSampleLedgerHead("Direct Expense", new string[] { "Rent", "Electricity","Society Maintenance Charge","Mobile Bills" }),
                    GetSampleLedgerHead("In Direct Expense", new string[] { "Society Events","Doctor Fee" })
                }
            });
            return accountingStructure;
        }
        #endregion

        #region Helper method for creating Ledger Info
        private LedgerHead GetLedgerHead(string name, bool affectsGrossPnL = false, string description = "")
        {
            return new LedgerHead
            {
                LedgerHeadName = name,
                AffectsGrossPnL = affectsGrossPnL,
                LedgerHeadDescription = description
            };
        }
        private SampleLedgerHead GetSampleLedgerHead(string name, string[] accounts, bool affectsGrossPnL = false, string description = "", bool affectsInventory = false, double openingBalance = 0)
        {
            return new SampleLedgerHead
            {
                ledgerHead = GetLedgerHead(name, affectsGrossPnL, description),
                SampleLedgerAccounts = accounts == null ? null : GetLedgerAccounts(accounts, affectsInventory, openingBalance)
            };
        }
        private LedgerAccount GetLedgerAccount(string name, bool affectsInventory = false, double openingBalance = 0)
        {
            return new LedgerAccount
            {
                LedgerAccountName = name,
                AffectsInventory = affectsInventory,
                OpeningBalance = openingBalance
            };
        }
        private List<LedgerAccount> GetLedgerAccounts(string[] names, bool affectsInventory = false, double openingBalance = 0)
        {
            return (from name in names select GetLedgerAccount(name, affectsInventory, openingBalance)).ToList();
        }
        #endregion

        #region Helper method for creating Transactions
        private List<SampleTransaction> FixedTransactions(string creditAccount, string debitAccount, string narration, double amount, string[] dates)
        {
            return (from d in dates select new SampleTransaction { Amount = amount, CreditAccount = creditAccount, DebitAccount = debitAccount, TransactionDate = d, TransactionNarration = narration }).ToList();
        }
        private List<SampleTransaction> RangeTransactions(string creditAccount, string debitAccount, string narration, double minAmount, double maxAmount, string[] dates)
        {
            Random rand = new Random();
            return (from d in dates select new SampleTransaction { Amount = (minAmount + rand.NextDouble() * (maxAmount - minAmount)), CreditAccount = creditAccount, DebitAccount = debitAccount, TransactionDate = d, TransactionNarration = narration }).ToList();
        }
        #endregion
    }
}
