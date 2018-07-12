using System.Text;
using TerminalUserInput;

namespace Bank
{
	class SavingsAccount : Account
	{
		public decimal IntrestRate { get; set; }
		public SavingsAccount() : base()
		{
			;
		}

		// overrides
		public override void Create(int account_id)
		{
			Balance = TUI.ReadDecimal("Please Enter the initial Balance.");
			IntrestRate = TUI.ReadDecimal("Please Enter the intrest Rate");
			base.Create(account_id);
		}

		// core functionality
		public decimal CalculateIntrest()
		{
			return Balance * IntrestRate;
		}

		// gernal overrides
		public override string ToString()
		{
			StringBuilder str = new StringBuilder();
			str.Append(base.ToString());
			str.Append($"Intrest : {IntrestRate:C}");
			return str.ToString();
		}
	}
}
