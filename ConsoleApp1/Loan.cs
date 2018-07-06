using System;

namespace Bank
{
	public class Loan
	{
		// returns the status after processsing
		public static bool Process(Account account)
		{
			decimal amount = TerminalUserInput.TUI.ReadDecimal("Please Enter What amount of loan would you like : ");
			if (account.Balance > amount)
			{
				account.Balance += amount;
				Console.WriteLine("Loan Successfully Initiated!");
				Console.WriteLine($"This is your balance now {account.Balance:C2}");
				return true;
			}
			Console.WriteLine("Please Check your balance");
			Console.WriteLine($"This is your balance {account.Balance:C2}");
			Console.WriteLine($"This is your amount {amount:C2}");
			return false;
		}
	}
}
