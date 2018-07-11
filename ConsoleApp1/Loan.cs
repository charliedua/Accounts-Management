using System;
using System.Text;
using TerminalUserInput;

namespace Bank
{
	public class Loan
	{
		// Core Properties
		public decimal Amount { get; set; }
		public int Days { get; set; }
		public decimal Rate { get; set; }
		public DateTime DateIssued { get; set; }
		public DateTime DateExpire => DateIssued.AddDays(Days);
		public int Installments { get; set; }
		public decimal AmountPerInstallment { get; set; }
		public int InstallmentsLeft { get; set; }
		
		// per day
		public decimal LateCharge { get; set; }

		// Processes The loan
		// returns the status after processsing
		public void Process(Account account)
		{
			{
				bool result = false;
				string reason = "";
				
				// Checks if the account already has a loan.
				if (!account.HasLoan)
				{
					// Verification Check
					if (!(TUI.ReadInteger("Please Enter Your unique accountID for verification: ") == account.AccountID))
					{
						reason = "Verification Error";
					}
					else
					{
						// Checks the initial Balance Before Giving Loan.
						if (account.Balance > Amount)
						{
							// Asks For all the details
							Amount = TUI.ReadDecimal("Please Enter What amount of loan would you like : ");
							Days = TUI.ReadInteger("For How many Days: ");
							Rate = TUI.ReadInteger("The intrest rate from the chart: ");
							Installments = TUI.ReadIntegerRange(3, 10, "How many installments would you like? (3-10): ");
							AmountPerInstallment = ((Rate * Days) + Amount) / Installments;
							account.Balance += Amount;
							account.HasLoan = true;
							DateIssued = DateTime.Now;
							InstallmentsLeft = Installments;
							LateCharge = TUI.ReadDecimal("Please Enter the Late Charge displayed on the chart.");
							Console.WriteLine("Loan Successfully Initiated!");
							Console.WriteLine($"This is your balance now {account.Balance:C}");
							Console.WriteLine($"The Loan is due on: {DateExpire.ToShortDateString()}");
							result = true;
						}
						else
						{
							reason = "Low Balance";
							Console.WriteLine($"This is your balance {account.Balance:C}");
							Console.WriteLine($"This is your amount {Amount:C}");
						}
					}
				}
				else
					reason = "Loan Already Issued";
				if (!(result && reason == ""))
					Console.WriteLine($"Loan Couldn't be processed please try again, reason: {reason}.");
			}
		}

		// Pays of the loan (No Questions Asked, Errors can be caused)
		public string Complete(Account account)
		{
			string reason = "";

			// Checks if the account has loan or not
			if (account.HasLoan)
			{
				// Checks if the owner is capable of paying the loan
				if ((Days * Rate) + Amount > account.Balance)
				{
					account.HasLoan = false;
					account.AccLoan = null;
					Console.WriteLine("Your loan has been Completed");
				}
				else
				{
					reason = "Insufficient Funds";
				}
			}
			else
			{
				reason = "Don't have a loan";
			}
			return reason;
		}

		// Pays Specific number of installments
		public void PayInstallments(Account account)
		{
			int InstallmentsToPay = TUI.ReadInteger("How many installments would you like to pay? ");
			string reason = "";
			
			// Checking if you pay more than installments present
			if (InstallmentsToPay > InstallmentsLeft && InstallmentsToPay > Installments)
				reason = "Installments out of Bound. (More to pay than available)";
			else
			{
				decimal AmountToPay = InstallmentsToPay * AmountPerInstallment;

				// Checks for the expiry and adds extra ammount to initial
				if (DateTime.Compare(DateExpire, DateTime.Now) < 0)
				{
					TimeSpan TimePeriod = DateExpire - DateTime.Now;
					decimal Extra = (LateCharge * (int)TimePeriod.TotalDays);
					Console.WriteLine($"The Date of Loan has been Expired! You will have to pay {Extra.ToString():C} extra." +
						$" For late by {(int)TimePeriod.TotalDays} days.");
					AmountToPay += Extra;
				}

				// Checks if the owner is capable of paying the amount
				if (AmountToPay > account.Balance)
				{
					reason = "Insufficient Balance.";
				}
				else
				{
					// Verification Check
					if (!(TUI.ReadInteger("Please Enter Your unique accountID for verification: ") == account.AccountID))
					{
						reason = "Verification Error";
					}
					else
					{
						// Gives a Chance to quit
						if (!TUI.ReadBool("Do You want to Continue?"))
						{
							reason = "Terminated by User";
						}
						else
						{
							account.Balance -= AmountToPay;
							InstallmentsLeft -= InstallmentsToPay;
							if (InstallmentsLeft == 0)
							{
								reason = Complete(account);
							}
						}
					}
				}
			}
			if (reason != "")
			{
				Console.WriteLine($"The action was cancelled, Reason: {reason}");
			}
			else
			{
				Console.WriteLine("Installments Successfully paid!");
			}
		}

		// Gernal Override
		public override string ToString() => $"Amount: {Amount} \n" +
				$"Rate: {Rate} \n" +
				$"Installments Left: {InstallmentsLeft} \n" +
				$"DateIssued: {DateIssued.ToLongDateString()} \n" +
				$"DateExpire: {DateExpire.ToLongDateString()} \n" +
				$"InstallmentsLeft: {InstallmentsLeft} \n" +
				$"Days: {Days}\n";
	}
}
