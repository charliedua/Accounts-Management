using System;
using System.Collections.Generic;
using System.Linq;
using Bank;
using TerminalUserInput;

namespace Accounts
{
    class Program
    {
		// The last account number
		private static int RunningAccountNumber = 0;

		static void Main()
        {
			// The main accounts list
			var accounts = new List<Account>();

			// The maintainer that if accounts have been created
			// TODO: Replace with accounts.Length
			bool accountsCreated = false;

			// Reads accounts from a file
			// TODO: Make it more accessible
			accounts = ReadAccounts();
			
			// if there were any accounts found
			// set running account number to the last
			// NOTE: accounts have been sorted by AccountID
			if (accounts.Count > 0)
			{
				RunningAccountNumber = accounts.Last().AccountID;
				accountsCreated = true;
			}

			// Displays main Menu
			// Then handles the rest
			HandleAccounts(accounts, accountsCreated);

			// Finally writes the accounts to the file
			// TODO: implement a user action to save or not
			WriteAccounts(accounts);
		}

		// Helper Method to Load accounts 
		// TODO: Make it more accessible
		private static List<Account> ReadAccounts() => Account.Load();

		// Helper Method to Save accounts 
		// TODO: Make it more accessible
		private static void WriteAccounts(List<Account> accounts) => Account.Save(accounts);

		// Handles the accounts menu
		private static void HandleAccounts(List<Account> accounts, bool accountsCreated = false)
		{
			int input;	// User input
			do
			{
				// Displays Account Menu
				DisplayAccountsMenu();
				input = TUI.ReadInteger();
				switch (input)
				{
					//
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

		// The Displays accounts menu
		// NOTE: This is the main menu
		private static void DisplayAccountsMenu()
		{
			Console.WriteLine("Please enter a number as the menu");
			Console.WriteLine("1. Create accounts");
			Console.WriteLine("2. Account actions");
			Console.WriteLine("3. Quit");
		}

		// Creates Accounts
		private static void Createaccounts(List<Account> accounts)
		{
			// The total number of accounts to be created
			int NumberOfAccounts  = TUI.ReadInteger("How many Accounts would you like to create? ");

			// the creation Loop
			for (int i = 0; i < NumberOfAccounts; i++)
			{
				Console.WriteLine($"Account number - {++RunningAccountNumber}");
				Console.WriteLine("Which account Would You Like to create?: ");
				Console.WriteLine("1. Savings Account");
				Console.WriteLine("2. Checking Account");
				int input = TUI.ReadIntegerRange(1, 2);
				Account account = null;
				// creates account according to the type
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

		// handles account menu, selecting one account
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
				
				// TODO: make functions for each of them
				switch (input)
				{
					// Opens Selection Menu
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
					
					// Add Deposit
					case 2:
						if (accountSelected)
						{
							decimal balance = TUI.ReadDecimal("How much you want to deposit: ");
							account.Deposit(balance);
						}
						else
							Console.WriteLine(errormsg);
						break;
					
					// Widrawl Menu 
					case 3:
						if (accountSelected)
						{
							decimal amount = TUI.ReadDecimal("How much you want to widraw");
							account.Widraw(amount);
						}
						else
							Console.WriteLine(errormsg);
						break;
					
					// Account Details
					case 4:
						if (accountSelected)
							Console.WriteLine(account.ToString());
						else
							Console.WriteLine(errormsg);
						break;

					// Shows the intrest
					// NOTE: For Savings Account Only
					case 5:
						if (accountSelected)
						{
							if (account is SavingsAccount savingsAccount)
								Console.WriteLine($"{savingsAccount.CalculateIntrest():C}");
						}
						else
							Console.WriteLine(errormsg);
						break;

					// Displays Loan Menu
					case 6:
						if (accountSelected)
							HandleLoan(account);
						else
							Console.WriteLine(errormsg);
						break;

					// The exit
					case 7:
						break;
					
					// If nothing works
					default:
						Console.WriteLine("Woops! That didn't work.");
						break;
				}
			} while (input != 7);
		}

		// Displays the menu for the account (Single)
        private static void DisplayMenu()
        {
            Console.WriteLine("Please enter a number as the menu");
            Console.WriteLine("1. Select a account");
            Console.WriteLine("2. add deposit");
            Console.WriteLine("3. Widraw");
            Console.WriteLine("4. View Account");
            Console.WriteLine("6. Loan Actions");
            Console.WriteLine("7. Quit");
        }

		// Displays the menu for the account (Single) + overload
		private static void DisplayMenu(Account account)
		{
			Console.WriteLine("Please enter a number as the menu");
			Console.WriteLine("1. Select a account");
			Console.WriteLine("2. add deposit");
			Console.WriteLine("3. Widraw");
			Console.WriteLine("4. View Account");
			if (account is SavingsAccount)
			{
				Console.WriteLine("5. Calculate intrest");
			}
			Console.WriteLine("6. Loan Actions");
			Console.WriteLine("7. Quit");
		}

		// Displays the loan Menu
		private static void DisplayLoanMenu(Account account)
		{
			Console.WriteLine("Please enter a number as the menu");
			Console.WriteLine("1. Take Loan");
			if (account.HasLoan)
			{
				Console.WriteLine("2. Pay Installments");
				Console.WriteLine("3. Check Loan Status");
			}
			Console.WriteLine("4. Back");
		}

		// Handles loans For a Given Account
		private static void HandleLoan(Account account)
		{
			int input;  // User input
			do
			{
				// Displays Loan Menu
				DisplayLoanMenu(account);
				input = TUI.ReadInteger();
				switch (input)
				{
					// Initiate Loan
					case 1:
						account.InitiateLoan();
						break;
					
					// Initiate Payments of installments
					case 2:
						if (account.HasLoan)
							account.PayInstallments();
						break;
					
					// Loan Status; Or overview
					case 3:
						if (account.HasLoan)
							Console.WriteLine(account.AccLoan.ToString());
						break;

					// Exit
					case 4:
						break;

					// Failsafe
					default:
						break;
				}
			} while (input != 4);
		}
	}
}
