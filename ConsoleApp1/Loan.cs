﻿using System;
using TerminalUserInput;

namespace Bank
{
	public class Loan
	{
		public decimal Amount { get; set; }
		public int Days { get; set; }
		public int Rate { get; set; }
		public DateTime DateIssued { get; set; }
		public DateTime DateExpire => DateIssued.AddDays(Days);

		// returns the status after processsing
		public void Process(Account account)
		{
			{ 
				// TODO: add a system for time/duration
				// TODO: ask for ID proof before loan
				bool result = false;
				string reason = "";
				if (!account.HasLoan)
				{
					Amount = TUI.ReadDecimal("Please Enter What amount of loan would you like : ");
					Days = TUI.ReadInteger("For How many Days: ");
					Rate = TUI.ReadInteger("The intrest rate from the chart: ");
					bool verified = TUI.ReadInteger("Please Enter Your unique accountID: ") == account.AccountID;
					if (!verified)
					{
						reason = "Verification Error";
					}
					else
					{
						if (account.Balance > Amount)
						{
							account.Balance += Amount;
							account.HasLoan = true;
							DateIssued = DateTime.Now;
							Console.WriteLine("Loan Successfully Initiated!");
							Console.WriteLine($"This is your balance now {account.Balance:C2}");
							result = true;
						}
						else
						{
							reason = "Low Balance";
							Console.WriteLine($"This is your balance {account.Balance:C2}");
							Console.WriteLine($"This is your amount {Amount:C2}");
						}
					}
				}
				else
					reason = "Loan Already Issued";
				if (!result)
						Console.WriteLine($"Loan Couldn't be processed please try again, reason: {reason}.");
			}
		}

		public void UnProcess(Account account)
		{
			string reason = "";
			if (account.HasLoan)
			{
				if ((account.AccLoan.Days * account.AccLoan.Rate) + account.AccLoan.Amount > account.Balance)
				{
					account.HasLoan = false;
					account.AccLoan = null;
					Console.WriteLine("Your loan has been unprocessed");
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
			Console.WriteLine($"The actions could not be performed due to the following reasons: {reason}");
		}
	}
}
