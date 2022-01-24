using System;
namespace LoanPayoffAnalyzerLibrary.Loans
{
    public class LoanEquations
    {
        public static decimal LoanPayment(decimal initialLoanAmount, int remainingPayments, double interestRate, int paymentsPerYear = 12)
        {
            double interestPerPayment = interestRate / paymentsPerYear;

            decimal first = initialLoanAmount * Convert.ToDecimal(interestPerPayment);
            decimal second = Convert.ToDecimal(Math.Pow(1 + interestPerPayment, remainingPayments));
            decimal third = second - 1;

            return Math.Round((first * second) / third, 2);
        }

        public static decimal InterestDue(decimal loanAmount, double interestRate, int paymentsPerYear = 12)
        {
            return Math.Round(loanAmount * Convert.ToDecimal(interestRate / paymentsPerYear),2);
        }

        public static decimal AmountTotalLoanRepayementAmount(decimal initialLoanAmount, decimal paymentPerInterval, double interestRate, int payementsToCalculate, int paymentsPerYear = 12)
        {
            decimal loanAmount = initialLoanAmount;
            decimal amountPaid = 0;
            int counter = 0;

            while (loanAmount > 0 && counter != payementsToCalculate)
            {
                if (loanAmount > paymentPerInterval)
                {
                    amountPaid += paymentPerInterval;
                    loanAmount -= paymentPerInterval - InterestDue(loanAmount, interestRate, paymentsPerYear);
                }
                else
                {
                    amountPaid += loanAmount + InterestDue(loanAmount, interestRate, paymentsPerYear);
                    break;
                }

                counter++;
            }

            return amountPaid;
        }
    }
}
