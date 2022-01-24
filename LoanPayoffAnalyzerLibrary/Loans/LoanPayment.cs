using System;
namespace LoanPayoffAnalyzerLibrary.Loans
{
    public class LoanPayment
    {
        public DateTime Date { get; private set; }
        public decimal Principle { get; private set; }
        public decimal Interest { get; private set; }
        public decimal Fee { get; private set; }
        public decimal Total { get; private set; }

        public LoanPayment(DateTime date, decimal principle, decimal interest, decimal fee = 0)
        {
            Date = date;
            Principle = principle;
            Interest = interest;
            Fee = fee;

            Total = principle + interest + fee;
        }
    }
}
