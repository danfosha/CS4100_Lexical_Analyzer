using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS4100_Lexical_Analyzer
{
    class Program
    {
        public static void Main(string[] args)
        {
            bool echoOn = true;
            InitializeStructures();
            string filename = FileHandler.GetFileName();
            FileHandler.InitializeInputFile(filename);
            PrintHeader();

            while (Tokenizer.tokenizerFinished)
            {
                Tokenizer.GetNextToken(echoOn);
                PrintToken(Tokenizer.nextToken, Tokenizer.tokenCode);
            }
            

        }

        public static void InitializeStructures()
        {
            int MaxQuad = 100;
            SymbolClass SymbolTable = new SymbolClass(MaxQuad);
            Tokenizer Tokenizer1 = new Tokenizer();
            FileHandler FileGetter = new FileHandler();
            
        }

        public static void PrintToken(string token, int tokenCode)
        {

            Console.WriteLine();
        }

        public static void PrintHeader()
        {
            Console.WriteLine("Lexeme \tToken Code \t \tMnemonic \tSymbolTable Index");
            Console.WriteLine("*********************************************************");
        }
    }

}
