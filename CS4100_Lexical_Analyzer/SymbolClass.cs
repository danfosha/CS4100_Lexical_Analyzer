using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// CS4100 Fall 2017 Lexical Analyzer Project - Fosha
namespace CS4100_Lexical_Analyzer
{
    class SymbolClass
    {
        // Properties    
        public enum Data_Kind { label, variable, constant }
        public int numUsed = 0;
        static int MaxSymbols = 100;

        public SymbolClass(int maxSymbols)
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

        public Symbol[] SymbolTableArray = new Symbol[MaxSymbols];

        //Methods

        // Adds symbol with given kind and value to the symbol table, automatically setting the correct data_type,
        // and returns the index where the symbol was located. If the symbol is already in the table, no change is made, and this just returns
        // the index where the symbol was found.
        // These three could be combind, but keeping all three per assignment instructions

        public int AddSymbol(String symbol, Data_Kind Kind, int value)
        {

            int index = LookupSymbol(symbol);
            if (index < 0)
            {
                SymbolTableArray[numUsed] = new Symbol(symbol, Kind, value);
                numUsed++;
            }

            return LookupSymbol(symbol);
        }
        public int AddSymbol(String symbol, Data_Kind Kind, double value)
        {
            int index = LookupSymbol(symbol);
            if (index < 0)
            {
                SymbolTableArray[numUsed] = new Symbol(symbol, Kind, value);
                numUsed++;
            }

            return LookupSymbol(symbol);
        }

        public int AddSymbol(String symbol, Data_Kind Kind, String value)
        {
            int index = LookupSymbol(symbol);
            if (index < 0)
            {
                SymbolTableArray[numUsed] = new Symbol(symbol, Kind, value);
                numUsed++;
            }

            return LookupSymbol(symbol);
        }

        int LookupSymbol(string symbol)
        //// Returns the index where symbol is found, or -1 if not in the table 
        {
            int i = 0;
            foreach (Symbol test_symbol in SymbolTableArray)
            {
                if (test_symbol != null)
                {
                    if (test_symbol.Name.Equals(symbol))
                        return i;
                    i++;
                }
            }
            return -1;

        }


        Symbol GetSymbol(int index)
        //// Return kind, data type, and value fields stored at index
        {
            // Don't return name? I'll return entire symbol object 
            return SymbolTableArray[index];
        }

        // Methods below set appropriate fields at slot indicated by index
        void UpdateSymbol(int index, Data_Kind Kind, int value)
        {
            Symbol updated = new Symbol("", Data_Kind.label, 0);
            updated = SymbolTableArray[index];
            updated.Kind = Kind;
            updated.Value = value;
            // updated.Data_type = value.GetType();
            SymbolTableArray[index] = updated;

        }

        void UpdateSymbol(int index, Data_Kind Kind, double value)
        {
            Symbol updated = new Symbol("", Data_Kind.label, 0);
            updated = SymbolTableArray[index];
            updated.Kind = Kind;
            updated.Value = value;
            // updated.Data_type = value.GetType();
            SymbolTableArray[index] = updated;
        }

        void UpdateSymbol(int index, Data_Kind Kind, String value)
        {
            Symbol updated = new Symbol("", Data_Kind.label, 0);
            updated = SymbolTableArray[index];
            updated.Kind = Kind;
            updated.Value = value;
            // updated.Data_type = value.GetType();
            SymbolTableArray[index] = updated;
        }

        public void PrintSymbolTable()
        //// Prints the utilized rows of the symbol table in neat tabular format, showing only
        //// the value field which is active for that row
        {
            Console.WriteLine("Name" + "\t\t\t" + "Kind" + "\t\t" + "Type" + "\t\t" + "Value");
            Console.WriteLine("***************************************************************");
            foreach (Symbol test_symbol in SymbolTableArray)
            {
                if (test_symbol != null)
                {
                    Console.WriteLine(test_symbol.Name + "\t\t" + test_symbol.Kind + "\t" + test_symbol.Data_type + "\t" + test_symbol.Value);
                }
            }
            Console.WriteLine("\n");
        }
    }
}
