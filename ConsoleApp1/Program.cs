using System;
using System.Collections.Generic;
using System.Linq;
using Bank;
using TerminalUserInput;

namespace Accounts
{
    class Program
    {
		private static int RunningAccountNumber = 0;

		static void Main()
        {
			var accounts = new List<Account>();
			bool accountsCreated = false;
			accounts = ReadAccounts();
			if (accounts.Count > 0)
			{
				RunningAccountNumber = accounts.Last().AccountID;
				accountsCreated = true;
			}
			HandleAccounts(accounts, accountsCreated);
			WriteAccounts(accounts);
		}

		private static List<Account> ReadAccounts() => Account.Load();

		private static void WriteAccounts(List<Account> accounts) => Account.Save(accounts);

		// TODO: Add a system for loan
		private static void HandleAccounts(List<Account> accounts, bool accountsCreated = false)
		{
			int input;
			do
			{
				DisplayAccountsMenu();
				input = TUI.ReadInteger();
				switch (input)
				{
					case 1:
						Createaccounts(accounts);
						Console.WriteLine("Accounts have been created!");
						accountsCreated = true;
						break;
					case 2:
						if (accountsCreated)
							HandleAccount(accounts);
						break;
					case 3:
						break;
					default:
						Console.WriteLine("Woops! That didn't work.");
						break;
				}
			} while (input != 3);
		}

		private static void DisplayAccountsMenu()
		{
			Console.WriteLine("Please enter a number as the menu");
			Console.WriteLine("1. Create accounts");
			Console.WriteLine("2. Account actions");
			Console.WriteLine("3. Quit");
		}

		private static void Createaccounts(List<Account> accounts)
		{
			int NumberOfAccounts  = TUI.ReadInteger("How many Accounts would you like to create? ");

			for (int i = 0; i < NumberOfAccounts; i++)
			{
				Console.WriteLine($"Account number - {++RunningAccountNumber}");
				Console.WriteLine("Which account Would You Like to create?: ");
				Console.WriteLine("1. Savings Account");
				Console.WriteLine("2. Checking Account");
				int input = TUI.ReadIntegerRange(1, 2);
				Account account = null;
				switch (input)
				{
					case 1:
						account = new SavingsAccount();
						break;
					case 2:
						account = new CheckingAccount();
						break;
				}

				account.Create(RunningAccountNumber);
				accounts.Add(account);
			}
		}

		private static void HandleAccount(List<Account> accounts)
		{
			bool accountSelected = false;
			Account account = new Account();
			int input;
			do
			{
				if (accountSelected)
					DisplayMenu(account);
				else
					DisplayMenu();
				Console.Title = "Account";
				input = TUI.ReadInteger("write your input here: ");
				Console.Clear();
				const string errormsg = "Please select a account first";
				switch (input)
				{
					case 1:
						int account_id = TUI.ReadInteger("please enter your account id you want: ");
						if (account_id <= RunningAccountNumber)
						{
							var query = from _account in accounts
										where _account.AccountID == account_id
										select _account;

							foreach (Account _account in query)
							{
								account = _account;
							}
							accountSelected = true;
						}
						else
							Console.WriteLine("Please check your account number again.");
						break;

					case 2:
						if (accountSelected)
						{
							decimal balance = TUI.ReadDecimal("How much you want to deposit: ");
							account.Deposit(balance);
						}
						else
							Console.WriteLine(errormsg);
						break;

					case 3:
						if (accountSelected)
							Console.WriteLine("This account is the property of " + account.Name + ".");
						else
							Console.WriteLine(errormsg);
						break;

					case 4:
						if (accountSelected)
							Console.WriteLine("This account has the balance " + account.Balance.ToString() + " .");
						else
							Console.WriteLine(errormsg);
						break;

					case 5:
						if (accountSelected)
						{
							decimal amount = TUI.ReadDecimal("How much you want to widraw");
							account.Widraw(amount);
						}
						else
							Console.WriteLine(errormsg);
						break;
					case 6:
						if (accountSelected)
							Console.WriteLine(account.ToString());
						else
							Console.WriteLine(errormsg);
						break;
					case 7:
						if (account is SavingsAccount savingsAccount)
						{
							Console.WriteLine($"{savingsAccount.CalculateIntrest():C2}");
						}
						break;
					case 8:
						break;
					default:
						Console.WriteLine("Woops! That didn't work.");
						break;
				}
			} while (input != 8);
		}

        private static void DisplayMenu()
        {
            Console.WriteLine("Please enter a number as the menu");
            Console.WriteLine("1. Select a account");
            Console.WriteLine("2. add deposit");
            Console.WriteLine("3. check name");
            Console.WriteLine("4. check balance");
            Console.WriteLine("5. Widraw");
            Console.WriteLine("6. View Account");
            Console.WriteLine("8. Quit");
        }

		// TODO: Add a system for loan
		private static void DisplayMenu(Account account)
		{
			Console.WriteLine("Please enter a number as the menu");
			Console.WriteLine("1. Select a account");
			Console.WriteLine("2. add deposit");
			Console.WriteLine("3. check name");
			Console.WriteLine("4. check balance");
			Console.WriteLine("5. Widraw");
			Console.WriteLine("6. View Account");
			if (account is SavingsAccount)
			{
				Console.WriteLine("7. Calculate intrest");
			}
			Console.WriteLine("8. Quit");
		}
	}
}
