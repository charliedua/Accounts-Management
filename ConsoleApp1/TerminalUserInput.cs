using System;

namespace TerminalUserInput
{
	public class TUI
	{
		public static int ReadInteger(string prompt)
		{
			int inputInt;
			string input;
			do
			{
				Console.Write(prompt);
				input = Console.ReadLine();
			} while (!int.TryParse(input, out inputInt));

			return inputInt;
		}

		public static int ReadInteger()
		{
			int inputInt;
			string input;
			do
			{
				Console.Write("> ");
				input = Console.ReadLine();
			} while (!int.TryParse(input, out inputInt));

			return inputInt;
		}

		public static int ReadIntegerRange(int min, int max, string prompt = "")
		{
			int input;
			string errMsg = "";
			bool condition;
			do
			{
				input = ReadInteger(prompt + errMsg);
				condition = (min < input) && (input < max);
				if (!condition)
				{
					errMsg += $"\nPlease enter a number between {min} and {max}.";
				}
			} while (condition);
			return input;
		}

		public static decimal ReadDecimal(string prompt)
		{
			decimal inputDec;
			string input;
			do
			{
				Console.Write(prompt);
				input = Console.ReadLine();
			} while (!decimal.TryParse(input, out inputDec));
			return inputDec;
		}

		public static decimal ReadDecimal()
		{
			decimal inputDec;
			string input;
			do
			{
				Console.Write("> ");
				input = Console.ReadLine();
			} while (!decimal.TryParse(input, out inputDec));

			return inputDec;
		}

		public static bool ReadBool()
		{
			string input = "";
			do
			{
				Console.Write("Enter Y/N> ");
				input = Console.ReadLine();
			} while (input == "Y" || input == "N" || input == "y" || input == "n");

			if (input == "Y" || input == "y")
			{
				return true;
			}
			else
			{
				return false;
			}
		}
		public static bool ReadBool(string prompt)
		{
			string input = "";
			do
			{
				Console.Write(prompt + "Enter Y/N> ");
				input = Console.ReadLine();
			} while (!(input == "Y" || input == "N" || input == "y" || input == "n"));

			if (input == "Y" || input == "y")
			{
				return true;
			}
			else
			{
				return false;
			}
		}
	}
}
