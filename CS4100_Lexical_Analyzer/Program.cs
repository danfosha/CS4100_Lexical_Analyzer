using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// CS4100 Fall 2017 Lexical Analyzer Project - Fosha
namespace CS4100_Lexical_Analyzer
{
    class Program
    {
        public static void Main(string[] args)
        {
            bool echoOn = true;
            string fileName = "lexical_test.txt";
            InitializeStructures();
            FileHandler.InitializeInputFile(fileName);
            PrintHeader();



            while (!TokenizerClass.tokenizerFinished)
            {
                TokenizerClass.GetNextToken(echoOn);
                TokentoSymTable(TokenizerClass.nextToken, TokenizerClass.tokenCode);
                PrintToken(TokenizerClass.nextToken, TokenizerClass.tokenCode);
            }

            Console.WriteLine("Tokenizer Finished");
            Console.ReadLine();
            SymbolTable.PrintSymbolTable();

        }

        public static void InitializeStructures()
        {
            int MaxQuad = 100;
            FileHandler FileGetter = new FileHandler();
            ReserveWordClass ReserveTable = new ReserveWordClass();
            ReserveTable.Initialize();
            SymbolTable SymbolTable = new SymbolTable(MaxQuad);
            TokenizerClass Tokenizer = new TokenizerClass();

        }


        public static void TokentoSymTable(string token, int tokenCode)
        {
            if ((tokenCode ==50) || (tokenCode == 51) || (tokenCode == 52))
            {
                SymbolTable.AddSymbol("token", SymbolTable.Data_Kind.variable, 0);       
            }
            else if (tokenCode == 51)
            {
                SymbolTable.AddSymbol("token", SymbolTable.Data_Kind.constant, Convert.ToInt32(token) );
            }
            else if (tokenCode == 52)
            {
                SymbolTable.AddSymbol("token", SymbolTable.Data_Kind.constant, Convert.ToDouble(token));

            }

        }
            
        public static void PrintToken(string token, int tokenCode)
        {
            if (token.Length > 0)
            {
                string mnem = ReserveWordClass.LookupMnem(tokenCode);
                int symIndex = SymbolTable.LookupSymbol(token);
                if (symIndex != -1)
                {
                    Console.WriteLine(token + "\t" + tokenCode + "\t" + mnem + "\t" + symIndex);
                }
                else if (tokenCode == -1)
                {
                    Console.WriteLine(token + "\t" + tokenCode + "\t" + mnem + "\t" + symIndex);
                }
                else
                {
                    Console.WriteLine(token + "\t" + tokenCode + "\t" + mnem);
                }
            }
        }

        public static void PrintHeader()
        {
            Console.WriteLine("Lexeme \tToken Code Mnemonic SymbolTable Index");
            Console.WriteLine("*********************************************************");
        }
    }

}
