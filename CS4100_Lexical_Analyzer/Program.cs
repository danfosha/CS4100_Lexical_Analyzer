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
                //PrintToken(TokenizerClass.nextToken, TokenizerClass.tokenCode);
            }

            Console.ReadLine();

        }

        public static void InitializeStructures()
        {
            int MaxQuad = 100;
            FileHandler FileGetter = new FileHandler();
            SymbolClass SymbolTable = new SymbolClass(MaxQuad);
            TokenizerClass Tokenizer = new TokenizerClass();


        }

        public static void PrintToken(string token, int tokenCode)
        {
            if (token.Length > 0)
            {
                Console.WriteLine(token + "\t" + tokenCode);
            }
        }

        public static void PrintHeader()
        {
            Console.WriteLine("Lexeme \tToken Code \t \tMnemonic \tSymbolTable Index");
            Console.WriteLine("*********************************************************");
        }
    }

}
