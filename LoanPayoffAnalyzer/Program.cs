using System;
using System.Collections.Generic;
using LoanPayoffAnalyzerLibrary;
using LoanPayoffAnalyzerLibrary.Loans;

namespace LoanPayoffAnalyzer
{
    class Program
    {
        static void Main(string[] args)
        {
            Loan landLoan = new Loan("Land", 64000, 15 * 12, CompoundType.Monthly, 5.875, 12);
            landLoan.MakePayment(DateTime.Parse("06/08/2021"));
            landLoan.MakePayment(DateTime.Parse("06/29/2021"));
            landLoan.MakePayment(DateTime.Parse("07/26/2021"));
            landLoan.MakePayment(DateTime.Parse("08/16/2021"));
            landLoan.MakePayment(DateTime.Parse("10/20/2021"));
            landLoan.MakePrinciplePayment(DateTime.Parse("10/21/2021"), 1);
            landLoan.MakePayment(DateTime.Parse("11/30/2021"));
            landLoan.MakePayment(DateTime.Parse("12/03/2021"));
            landLoan.MakePrinciplePayment(DateTime.Parse("12/03/2021"), 100);
            landLoan.MakePayment(DateTime.Parse("12/06/2021"));
            landLoan.MakePrinciplePayment(DateTime.Parse("12/06/2021"), 100);

            Loan truckLoan = new Loan("Truck", 35076.56m, 7 * 12, CompoundType.Monthly, 2.24, 12);
            Loan rvLoan = new Loan("RV", 15972.00m, 15 * 12, CompoundType.Monthly, 1.99, 12);
            Loan rzrLoan = new Loan("Rzr", 64000, 15 * 12, CompoundType.Monthly, 5.825, 12);
            Loan subaruLoan = new Loan("Subaru", 64000, 15 * 12, CompoundType.Monthly, 5.825, 12);

            List<Loan> prioritizedLoans = LoanAnalyzer.PrioritizeLoans(new List<Loan>()
            {
                landLoan,
                truckLoan,
                rvLoan,
                rzrLoan,
                subaruLoan
            }, 100);

            Console.WriteLine("***********************");
            Console.WriteLine("Prioritized Loans:");
            foreach(Loan l in prioritizedLoans)
            {
                Console.WriteLine(l.Name);
            }
            Console.WriteLine("***********************");
        }
    }
}
