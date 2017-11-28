using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// CS4100 Fall 2017 Lexical Analyzer Project - Fosha
namespace CS4100_Code_Generator
{
    class SymbolTable
    {
        // Properties    
        public enum Data_Kind {undeclared, label, variable, constant,  }
        public static int numUsed = 0;
        static int MaxSymbols = 100;
        public static int Minus1Index = -1;
        public static int Plus1Index = 1;

        public SymbolTable(int maxSymbols)
        {
            MaxSymbols = maxSymbols;
        }

        public class Symbol
        {
            //public Symbol() { }
            public Symbol(string name, Data_Kind kind, object value)
            {

                Name = name;
                Kind = kind;
                Data_type = value.GetType();
                Value = value;
            }

            public string Name { get; set; }
            public Data_Kind Kind { get; set; }
            public Type Data_type { get; set; }
            public object Value { get; set; }

        }

        public static Symbol[] SymbolTableArray = new Symbol[MaxSymbols];

        //Methods

        // Adds symbol with given kind and value to the symbol table, automatically setting the correct data_type,
        // and returns the index where the symbol was located. If the symbol is already in the table, no change is made, and this just returns
        // the index where the symbol was found.
        // These three could be combined, but keeping all three per assignment instructions

        public static int AddSymbol(string symbol, Data_Kind Kind, int value)
        {

            int index = LookupSymbol(symbol);
            if (index < 0)
            {
                SymbolTableArray[numUsed] = new Symbol(symbol, Kind, value);
                numUsed++;
            }

            return LookupSymbol(symbol);
        }
        public static int AddSymbol(string symbol, Data_Kind Kind, double value)
        {
            int index = LookupSymbol(symbol);
            if (index < 0)
            {
                SymbolTableArray[numUsed] = new Symbol(symbol, Kind, value);
                numUsed++;
            }

            return LookupSymbol(symbol);
        }

        public static int AddSymbol(string symbol, Data_Kind Kind, string value)
        {
            int index = LookupSymbol(symbol);
            if (index < 0)
            {
                SymbolTableArray[numUsed] = new Symbol(symbol, Kind, value);
                numUsed++;
            }

            return LookupSymbol(symbol);
        }

        public static int LookupSymbol(string symbol)
        //// Returns the index where symbol is found, or -1 if not in the table 
        {
            int i = 0;
            foreach (Symbol test_symbol in SymbolTableArray)
            {
                if (test_symbol != null)
                {
                    if (String.Equals(test_symbol.Name, symbol, StringComparison.OrdinalIgnoreCase))
                    {
                        return i;
                    }
                    i++;
                }
            }
            return -1;

        }

        public static Symbol GetSymbol(int index)
        //// Return kind, data type, and value fields stored at index
        {
            // Don't return name? I'll return entire symbol object 
            return SymbolTableArray[index];
        }

        // Methods below set appropriate fields at slot indicated by index
        //public static void UpdateSymbol(int index, Data_Kind Kind, int value)
        //{
        //    Symbol updated = new Symbol("", Data_Kind.label, 0);
        //    updated = SymbolTableArray[index];
        //    updated.Kind = Kind;
        //    updated.Value = value;
        //    // updated.Data_type = value.GetType();
        //    SymbolTableArray[index] = updated;
        //}

        public static void UpdateSymbol(int index, Data_Kind Kind, double value)
        {
            Symbol updated = new Symbol("", Data_Kind.label, 0);
            updated = SymbolTableArray[index];
            updated.Kind = Kind;
            updated.Value = value;
            // updated.Data_type = value.GetType();
            SymbolTableArray[index] = updated;
        }

        public static void UpdateSymbol(int index, Data_Kind Kind, String value)
        {
            Symbol updated = new Symbol("", Data_Kind.label, value);
            updated = SymbolTableArray[index];
            updated.Kind = Kind;
            updated.Value = value;
            // updated.Data_type = value.GetType();
            SymbolTableArray[index] = updated;
        }

        public static void ValidateLabelUsed()
        {
            foreach (Symbol test_symbol in SymbolTableArray)
            {
                if (test_symbol != null)
                {
                    if ((test_symbol.Kind.ToString() == "label")) //&& (test_symbol.Value is int)
                    {
                        int symbol_value = Convert.ToInt32(test_symbol.Value);
                        if (symbol_value < 0)

                        {
                            Console.WriteLine("Warning: Label " + test_symbol.Name + " is not used.");
                        }
                    }
                }
            }
        }


        public static void ValidateDeclaration()
        {
            foreach (Symbol test_symbol in SymbolTableArray)
            {
                if (test_symbol != null)
                {
                    if ((test_symbol.Kind.ToString() == "label")) //&& (test_symbol.Value is int)
                    {
                        int symbol_value = Convert.ToInt32(test_symbol.Value);
                        if (symbol_value < 0)

                        {
                            Console.WriteLine("Warning: Label " + test_symbol.Name + " is not used.");
                        }
                    }
                }
            }
        }


        public static void PrintSymbolTable()
        //// Prints the utilized rows of the symbol table in neat tabular format, showing only
        //// the value field which is active for that row
        {
            Console.WriteLine("Name".PadRight(20) + "Kind".PadRight(12) + "Type" + "\t\t" + "Value");
            Console.WriteLine("***************************************************************");
            foreach (Symbol test_symbol in SymbolTableArray)
            {
                if (test_symbol != null)
                {
                    Console.WriteLine(Truncate(test_symbol.Name, 16).PadRight(20) + test_symbol.Kind + "\t" + test_symbol.Data_type + "\t" + test_symbol.Value);
                }
            }
            Console.WriteLine("\n");
        }

        public static string Truncate(string value, int maxChars)
        {
            // thanks to StackOverFlow!
            return value.Length <= maxChars ? value : value.Substring(0, maxChars) + "...";
        }
    }
}
