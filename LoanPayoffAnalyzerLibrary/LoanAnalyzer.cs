using System;
using System.Collections.Generic;
using System.Linq;
using LoanPayoffAnalyzerLibrary.Loans;

namespace LoanPayoffAnalyzerLibrary
{
    public class LoanAnalyzer
    {
        public static List<Loan> PrioritizeLoans(List<Loan> loans, decimal extraMoneyPerMonth)
        {
            List<Loan> intrestRateDescLoans = CloneLoans(loans);
            intrestRateDescLoans = intrestRateDescLoans.OrderByDescending(l => l.IntrestRate).ToList();
            LogResults("Intrest Rate Descending", Calculate(intrestRateDescLoans, extraMoneyPerMonth));

            List<Loan> smallestToLargestLoans = CloneLoans(loans);
            smallestToLargestLoans = smallestToLargestLoans.OrderBy(l => l.CurrentAmount).ToList();
            LogResults("Smallest Amount to Largest", Calculate(smallestToLargestLoans, extraMoneyPerMonth));

            List<Loan> largestToSmallestLoans = CloneLoans(loans);
            largestToSmallestLoans = largestToSmallestLoans.OrderByDescending(l => l.CurrentAmount).ToList();
            LogResults("Largest Amount to Smallest", Calculate(largestToSmallestLoans, extraMoneyPerMonth));

            List<Loan> intrestRateAscLoans = CloneLoans(loans);
            intrestRateAscLoans = intrestRateAscLoans.OrderBy(l => l.IntrestRate).ToList();
            LogResults("Intrest Rate Ascending", Calculate(intrestRateAscLoans, extraMoneyPerMonth));

            return intrestRateDescLoans;
        }

        public static Tuple<int, decimal> Calculate(List<Loan> inLoans, decimal extraMoneyPerMonth)
        {
            List<Loan> prioritizedLoans = CloneLoans(inLoans);

            DateTime dateTime = DateTime.Now;
            int paymentCount = 0;
            decimal totalPayed = 0;
            decimal totalPaymentAmount = prioritizedLoans.Sum(l => l.PaymentAmount) + extraMoneyPerMonth;

            while (prioritizedLoans.Count > 0)
            {
                for (int i = 0; i < prioritizedLoans.Count; i++)
                {
                    if (i == 0 && prioritizedLoans[i].CurrentAmount > 0)
                    {
                        prioritizedLoans[i].MakePayment(dateTime, extraMoneyPerMonth);
                        totalPayed += prioritizedLoans[i].PaymentHistory.Last().Total;
                    }
                    else if (prioritizedLoans[i].CurrentAmount > 0)
                    {
                        prioritizedLoans[i].MakePayment(dateTime);
                        totalPayed += prioritizedLoans[i].PaymentHistory.Last().Total;
                    }

                    if (prioritizedLoans[i].CurrentAmount <= 0)
                    {
                        extraMoneyPerMonth += prioritizedLoans[i].PaymentAmount;
                        prioritizedLoans.RemoveAt(i);
                        i--;
                    }
                }

                paymentCount++;
            }

            return new Tuple<int, decimal>(paymentCount, totalPayed);
        }

        private static List<Loan> CloneLoans(List<Loan> loans)
        {
            List<Loan> clonedLoans = new List<Loan>();

            foreach (Loan originalLoan in loans)
            {
                clonedLoans.Add(originalLoan.Clone());
            }

            return clonedLoans;
        }

        private static void LogResults(string name, Tuple<int,decimal> paymentsAndAmount)
        {
            Console.WriteLine(name);
            Console.WriteLine($"{paymentsAndAmount.Item1}, {paymentsAndAmount.Item2}");
        }
    }
}
