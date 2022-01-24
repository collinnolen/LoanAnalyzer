using System;
using System.Collections.Generic;
using System.Linq;

namespace LoanPayoffAnalyzerLibrary.Loans
{
    public class Loan
    {
        public string Name { get; private set; }
        public decimal CurrentAmount { get; private set; }
        public decimal InitialAmount { get; private set; }
        public decimal TotalAmountByMaturityWithoutExtraPayments { get; private set; }
        public decimal TotalAmountByMaturity { get; private set; }

        public int LoanTerm { get; private set; }
        public CompoundType Compound { get; private set; }

        public decimal PaymentAmount { get; set; }
        public double IntrestRate { get; private set; }

        public int RemainingPayments { get; private set; }
        public int PaymentsPerYear { get; private set; }

        public List<LoanPayment> PaymentHistory { get; private set; }

        public Loan(string name, decimal initialAmount, int loanTerm, CompoundType compound, double intrestRate, int paymentsPerYear)
        {
            Name = name;
            InitialAmount = initialAmount;
            CurrentAmount = initialAmount;
            LoanTerm = loanTerm;
            Compound = compound;
            IntrestRate = intrestRate / 100.0;
            PaymentsPerYear = paymentsPerYear;
            PaymentHistory = new List<LoanPayment>();

            PaymentAmount = LoanEquations.LoanPayment(InitialAmount, LoanTerm / 12 * PaymentsPerYear, IntrestRate, PaymentsPerYear);
            TotalAmountByMaturityWithoutExtraPayments = LoanEquations.AmountTotalLoanRepayementAmount(CurrentAmount, PaymentAmount, IntrestRate, -1, PaymentsPerYear);
            CalculateTotalLoanRepaymentAmount();
        }

        public void MakePayment(DateTime dateTime, decimal extraPrinciple = 0, decimal fee = 0)
        {
            LoanPayment payment;
            decimal intrestDue = LoanEquations.InterestDue(CurrentAmount, IntrestRate, PaymentsPerYear);
            if(CurrentAmount + intrestDue < PaymentAmount + extraPrinciple)
                payment = new LoanPayment(dateTime, CurrentAmount, intrestDue, fee);
            else
                payment = new LoanPayment(dateTime, PaymentAmount - intrestDue + extraPrinciple, intrestDue, fee);

            CurrentAmount -= payment.Principle;

            PaymentHistory.Add(payment);

            CalculateTotalLoanRepaymentAmount();
            CalculateRemainingPayments();
        }

        public void MakePrinciplePayment(DateTime dateTime, decimal principlePayment)
        {
            LoanPayment payment = new LoanPayment(dateTime, principlePayment, 0, 0);
            CurrentAmount -= payment.Principle;

            PaymentHistory.Add(payment);

            CalculateTotalLoanRepaymentAmount();
            CalculateRemainingPayments();
        }

        public Loan Clone()
        {
            return new Loan(Name, InitialAmount, LoanTerm, Compound, IntrestRate * 100, PaymentsPerYear);
        }

        private void CalculateRemainingPayments()
        {
            RemainingPayments = (int) Math.Ceiling(LoanEquations.AmountTotalLoanRepayementAmount(CurrentAmount, PaymentAmount, IntrestRate, -1, PaymentsPerYear) / PaymentAmount);
        }

        private void CalculateTotalLoanRepaymentAmount()
        {
            decimal totalPaidSoFar = PaymentHistory.Sum(p => p.Total);
            TotalAmountByMaturity = totalPaidSoFar + LoanEquations.AmountTotalLoanRepayementAmount(CurrentAmount, PaymentAmount, IntrestRate, -1, PaymentsPerYear);
        }
    }
}