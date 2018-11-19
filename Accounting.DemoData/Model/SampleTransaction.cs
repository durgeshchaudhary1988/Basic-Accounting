namespace Accounting.DemoData.Model
{
    /// <summary>
    /// Transaction Information
    /// </summary>
    public class SampleTransaction
    {
        /// <summary>
        /// Transaction date in yyyy-mm-dd format
        /// </summary>
        public string TransactionDate { get; set; }
        /// <summary>
        /// Transaction Description
        /// </summary>
        public string TransactionNarration { get; set; }
        /// <summary>
        /// Transaction Amount
        /// </summary>
        public double Amount { get; set; }
        /// <summary>
        /// Account name to be credited
        /// </summary>
        public string CreditAccount { get; set; }
        /// <summary>
        /// Account name to be debited
        /// </summary>
        public string DebitAccount { get; set; }
        /// <summary>
        /// System Account ID: Will be auto populated
        /// </summary>
        public int CreditAccountId { get; set; }
        /// <summary>
        /// System Account ID: Will be auto populated
        /// </summary>
        public int DebitAccountId { get; set; }
    }
}
