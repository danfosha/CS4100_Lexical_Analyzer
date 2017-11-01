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
            // string globalToken;
            // int globalTokenCode;
            bool echoOn = true;
            // string fileName = "GoodtreeA.txt";
            // string fileName = "BadProg1.txt";
            string fileName = "BadProg2B.txt";
            // string fileName = "BadProg3B.txt";
            InitializeStructures();
            FileHandler.InitializeInputFile(fileName);
            PrintHeader();

            while (!TokenizerClass.tokenizerFinished)
            {
                GetNextToken(echoOn);
                SyntaxA.Analyze(echoOn);
                //globalToken = TokenizerClass.nextToken;
                //globalTokenCode = TokenizerClass.tokenCode;
                
            }

            Console.WriteLine("Tokenizer Finished");
            Console.ReadLine();
            SymbolTable.PrintSymbolTable();
            Console.ReadLine();
        }

        public static void InitializeStructures()
        {
            int MaxQuad = 100;
            FileHandler FileGetter = new FileHandler();
            ReserveWordClass ReserveTable = new ReserveWordClass();
            ReserveTable.Initialize();
            SymbolTable SymbolTable = new SymbolTable(MaxQuad);
            TokenizerClass Tokenizer = new TokenizerClass();
            SyntaxA SyntaxAnalyzer = new SyntaxA();

        }

        public static void GetNextToken(bool echoOn)
        {
            TokenizerClass.GetNextToken(echoOn);
            TokentoSymTable(TokenizerClass.nextToken, TokenizerClass.tokenCode);
            PrintToken(TokenizerClass.nextToken, TokenizerClass.tokenCode);
        }

        public static void TokentoSymTable(string token, int tokenCode)
        {
            if (tokenCode == 50)
            {
                SymbolTable.AddSymbol(token, SymbolTable.Data_Kind.variable, 0);
            }
            else if (tokenCode == 51)
            {
                SymbolTable.AddSymbol(token, SymbolTable.Data_Kind.constant, Convert.ToDouble(token));
            }
            else if (tokenCode == 52)
            {
                SymbolTable.AddSymbol(token, SymbolTable.Data_Kind.constant, Convert.ToDouble(token));
            }
        }

        public static void PrintToken(string token, int tokenCode)
        {
            if (token.Length > 0)
            {
                string mnem = ReserveWordClass.LookupMnem(tokenCode);
                int symIndex = SymbolTable.LookupSymbol(token);
                // anything with a symbol table value
                if (symIndex != -1)
                {
                    Console.WriteLine(SymbolTable.Truncate(token, 16).PadRight(20) + tokenCode.ToString().PadRight(22) + symIndex);
                }
                else
                {
                    Console.WriteLine(SymbolTable.Truncate(token, 16).PadRight(20) + tokenCode.ToString().PadRight(10) + mnem.PadRight(10));
                }
            }
        }

        public static void PrintHeader()
        {
            Console.WriteLine("Lexeme".PadRight(16) + "Token Code".PadRight(12) + "Mnemonic".PadRight(10) + "SymbolTable Index".PadRight(17));
            Console.WriteLine("*".PadRight(55, '*'));
        }
    }

    // CFG
    // <program>            -> $UNIT <prog-identifier> $SEMICOLON <block> $PERIOD
    // <block>              -> $BEGIN <statement> {$SEMICOLON <statement>} $END
    // <prog-identifier>    -> <identifier>
    // <statement>          -> <variable> $COLON-EQUALS <simple expression>
    // <variable>           -> <identifier>
    // <simple expression>  -> [<sign>] <term> {<addop> <term>}*
    // <addop>              -> $PLUS | $MINUS
    // <sign>               -> $PLUS | $MINUS
    // <term>               -> <factor> {<mulop> <factor> }*
    // <mulop>              -> $MULTIPLY | $DIVIDE
    // <factor>             -> <unsigned constant> | <variable> | $LPAR <simple expression> $RPAR
    // <unsigned constant>  -> <unsigned number>
    // <unsigned number>    -> $FLOAT | $INTTYPE // as defined for lexical
    // <identifier>         -> $IDENTIFIER

}
