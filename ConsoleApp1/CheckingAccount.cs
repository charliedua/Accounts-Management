using System.Text;
using TerminalUserInput;

namespace Bank
{
	class CheckingAccount : Account
	{
		// Core members
		public decimal FeeChargedPerTxn { get; set; }

		// Main Constructor
		public CheckingAccount() : base()
		{
			;
		}

		// Overrides
		public override void Create(int account_id)
		{
			Balance = TUI.ReadDecimal("Please Enter the initial Balance.");
			FeeChargedPerTxn = TUI.ReadDecimal("Please Enter the Fee Charged Per Transaction");
			base.Create(account_id);
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
		public new void Deposit(decimal depositAmount)
		{
			base.Deposit(depositAmount - FeeChargedPerTxn);
		}

		// gernal overrides
		public override string ToString()
		{
			StringBuilder str = new StringBuilder();
			str.Append(base.ToString());
			str.Append($"FeeCharge : {FeeChargedPerTxn:C}");
			return str.ToString();
		}
	}
}
