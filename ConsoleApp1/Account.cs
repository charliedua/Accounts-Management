using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Bank
{
	public class Account
	{
		// Core Account Thingies
		public int AccountID { get; set; }

		private string _Name;
		public string Name
		{
			get => _Name;

			set
			{
				Regex regex = new Regex(@"^[a-zA-Z ]+$");
				if (regex.IsMatch(value))
					_Name = value;
				else
				{
					Console.WriteLine("Invalid data enterd \nPress Enter to continue...");
					Console.ReadLine();
				}
			}
		}

		private decimal _Balance;
		public decimal Balance
		{
			get => _Balance;

			set
			{
				if (value >= 0.0m)
				{
					_Balance = value;
				}
				else
				{
					Console.WriteLine("Invalid data enterd \nPress Enter to continue...");
					Console.ReadLine();
				}
			}
		}

		// Loan Thingies
		public bool HasLoan { get; set; }
		public Loan AccLoan { get; set; }

		// Genrates the instance which is empty
		public Account()
		{
			;
		}

		// Creator
		public virtual void Create(int account_id)
		{
			Console.WriteLine("Please enter your name");
			Name = Console.ReadLine();
			Balance = 0.0m;
			AccountID = account_id;
			HasLoan = false;
			Console.WriteLine("Account has been created.\nPress Enter to continue...");
			Console.ReadLine();
			Console.Clear();
		}

		// Core Functionality of account
		public void Deposit(decimal depositAmount)
		{
			if (depositAmount > 0.0m)
			{
				Balance += depositAmount;
			}
			else
			{
				Console.WriteLine("Invalid data enterd \nPress Enter to continue...");
				Console.ReadLine();
				Console.Clear();
			}
		}
		public void Widraw(decimal amount)
		{
			if (Balance > amount)
			{
				Balance -= amount;
			}
			else
			{
				Console.WriteLine("Check if you have sufficient ballance \nPress Enter to continue...");
				Console.ReadLine();
				Console.Clear();
			}
		}

		// Save to a File
		public static void Save(List<Account> accounts, string path)
		{
			using (FileStream stream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None))
			{
				StringBuilder data = new StringBuilder("{");
				StringBuilder SavingsData = new StringBuilder("\"Savings\": [");
				StringBuilder CheckingData = new StringBuilder("\"Checking\": [");

				foreach (var account in accounts)
				{
					if (account is SavingsAccount savingsAccount)
					{
						SavingsData.Append(JsonConvert.SerializeObject(savingsAccount));
						SavingsData.Append(",");
					}
					else if (account is CheckingAccount checkingAccount)
					{
						CheckingData.Append(JsonConvert.SerializeObject(checkingAccount));
						CheckingData.Append(",");
					}
				}
				SavingsData.Append("],");
				CheckingData.Append("],");
				data.Append($"{SavingsData} {CheckingData} }}");
				stream.Write(Encoding.ASCII.GetBytes(data.ToString()), 0, data.Length);
				stream.Flush();
			}
		}
		public static void Save(List<Account> accounts)
		{
			var path = Environment.GetFolderPath(
				Environment.SpecialFolder.MyDocuments) +
				"//accountinfo.dat";

			using (FileStream stream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None))
			{
				StringBuilder data = new StringBuilder("{");
				StringBuilder SavingsData = new StringBuilder("\"Savings\": [");
				StringBuilder CheckingData = new StringBuilder("\"Checking\": [");

				foreach (var account in accounts)
				{
					if (account is SavingsAccount savingsAccount)
					{
						SavingsData.Append(JsonConvert.SerializeObject(savingsAccount));

						int elementCount = (from _account in accounts
											where (_account is SavingsAccount)
											select account).Count();
						if (elementCount > savingsAccount.AccountID)
						{
							SavingsData.Append(",");
						}
					}
					else if (account is CheckingAccount checkingAccount)
					{
						CheckingData.Append(JsonConvert.SerializeObject(checkingAccount));
						int elementCount = (from _account in accounts
											where (_account is CheckingAccount)
											select account).Count();
						if (elementCount > checkingAccount.AccountID)
						{
							CheckingData.Append(",");
						}
					}
				}
				SavingsData.Append("],");
				CheckingData.Append("]");
				data.Append($"{SavingsData} {CheckingData} }}");
				stream.Write(Encoding.ASCII.GetBytes(data.ToString()), 0, data.Length);
				stream.Flush();
			}
		}

		// Load From a file
		public static List<Account> Load()
		{
			List<Account> accounts = new List<Account>();

			var path = Environment.GetFolderPath(
				Environment.SpecialFolder.MyDocuments) +
				"//accountinfo.dat";

			using (StreamReader r = new StreamReader(path))
			{
				string data = r.ReadToEnd();
				if (data.Length > 0)
				{
					JObject obj = JObject.Parse(data);
					JArray SavingsArray = (JArray)obj["Savings"];
					JArray CheckingArray = (JArray)obj["Checking"];
					for (int i = 0; i < SavingsArray.Count; i++)
					{
						SavingsAccount account = JsonConvert.DeserializeObject<SavingsAccount>(SavingsArray[i].ToString());
						accounts.Add(account);
					}
					for (int i = 0; i < CheckingArray.Count; i++)
					{
						CheckingAccount account = JsonConvert.DeserializeObject<CheckingAccount>(CheckingArray[i].ToString());
						accounts.Add(account);
					}
				}
			}
			accounts = (from account in accounts orderby account.AccountID select account).ToList();

			return accounts;
		}
		public static List<Account> Load(string path)
		{
			List<Account> accounts = new List<Account>();

			using (StreamReader r = new StreamReader(path))
			{
				JObject obj = JObject.Parse(r.ReadToEnd());
				JArray SavingsArray = (JArray)obj["Savings"];
				JArray CheckingArray = (JArray)obj["Checking"];
				for (int i = 0; i < SavingsArray.Count; i++)
				{
					SavingsAccount account = JsonConvert.DeserializeObject<SavingsAccount>(SavingsArray[i].ToString());
					accounts.Add(account);
				}
				for (int i = 0; i < CheckingArray.Count; i++)
				{
					CheckingAccount account = JsonConvert.DeserializeObject<CheckingAccount>(CheckingArray[i].ToString());
					accounts.Add(account);
				}
			}

			return accounts;
		}

		// Basic Overrides
		public override string ToString()
		{
			StringBuilder str = new StringBuilder();
			str.AppendLine($"Name : {Name}");
			str.AppendLine($"Balance : {Balance.ToString():C}");
			str.AppendLine($"Account ID : {AccountID.ToString()}");
			return str.ToString();
		}

		// Core Loan functionality
		public void InitiateLoan()
		{
			AccLoan = new Loan();
			AccLoan.Process(this);
		}

		public void PayInstallments()
		{
			AccLoan.PayInstallments(this);
		}
	}
}
