using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using TerminalUserInput;

namespace Bank
{
	internal class Account
	{
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

		public Account()
		{
			;
		}

		public virtual void Create(int account_id)
		{
			Console.WriteLine("Please enter your name");
			Name = Console.ReadLine();
			Balance = 0.0m;
			AccountID = account_id;
			Console.WriteLine("Account has been created.\nPress Enter to continue...");
			Console.ReadLine();
			Console.Clear();
		}

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
						// TODO: Fix linq query.
						//var a = from accounts where (account is SavingsAccount) select account;
						SavingsData.Append(",");
					}
					else if (account is CheckingAccount checkingAccount)
					{
						CheckingData.Append(JsonConvert.SerializeObject(checkingAccount));
						CheckingData.Append(",");
					}
				}
				SavingsData.Append("],");
				CheckingData.Append("]");
				data.Append($"{SavingsData} {CheckingData} }}");
				stream.Write(Encoding.ASCII.GetBytes(data.ToString()), 0, data.Length);
				stream.Flush();
			}
		}

		public static List<Account> Load()
		{
			List<Account> accounts = new List<Account>();

			var path = Environment.GetFolderPath(
				Environment.SpecialFolder.MyDocuments) +
				"//accountinfo.dat";

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

		public override string ToString()
		{
			StringBuilder str = new StringBuilder();
			str.AppendLine($"Name : {Name}");
			str.AppendLine($"Balance : {Balance.ToString():C2}");
			str.AppendLine($"Account ID : {AccountID.ToString()}");
			return str.ToString();
		}
	}

	class SavingsAccount: Account
	{
		public decimal IntrestRate { get; set; }
		public SavingsAccount() : base()
		{
			;
		}

		public override void Create(int account_id)
		{
			Balance = TUI.ReadDecimal("Please Enter the initial Balance.");
			IntrestRate = TUI.ReadDecimal("Please Enter the intrest Rate");
			base.Create(account_id);
		}

		public decimal CalculateIntrest()
		{
			return Balance * IntrestRate;
		}

		public override string ToString()
		{
			StringBuilder str = new StringBuilder();
			str.Append(base.ToString());
			str.Append($"Intrest : {IntrestRate:C2}");
			return str.ToString();
		}
	}

	class CheckingAccount : Account
	{
		public decimal FeeChargedPerTxn { get; set; }
		public CheckingAccount() : base()
		{
			;
		}

		public override void Create(int account_id)
		{
			Balance = TUI.ReadDecimal("Please Enter the initial Balance.");
			FeeChargedPerTxn = TUI.ReadDecimal("Please Enter the Fee Charged Per Transaction");
			base.Create(account_id);
		}

		public new void Deposit(decimal depositAmount)
		{
			base.Deposit(depositAmount - FeeChargedPerTxn);
		}

		public new void Widraw(decimal amount)
		{
			if (amount > Balance)
			{
				base.Widraw(amount + FeeChargedPerTxn);
			}
			else
			{
				base.Widraw(amount);
			}
		}

		public override string ToString()
		{
			StringBuilder str = new StringBuilder();
			str.Append(base.ToString());
			str.Append($"FeeCharge : {FeeChargedPerTxn:C2}");
			return str.ToString();
		}

	}
}
