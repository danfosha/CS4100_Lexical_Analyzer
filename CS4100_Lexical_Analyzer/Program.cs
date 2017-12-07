using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// CS4100 Fall 2017 Complier Project - Fosha
namespace CS4100_Code_Generator
{

    // CFG
    // <program>            -> $UNIT <prog-identifier> $SEMICOLON <block> $PERIOD
    // <block>              -> [<label-declaration>] {<variable-dec-sec>]* <block-body>
    // <block-body>>        -> $BEGIN <statement> {$SEMICOLON <statement>} $END
    // <variable-dec-sc>    -> $VAR <variable-declaration>
    // <variable-declaration> -> {<identifier> {$COMMA <identifier>}* $COLON <type> $SEMICOLON}+
    // <prog-identifier>    -> <identifier>
    // <statement>          -> {<label> $COLON]}* [<variable> $ASSIGN (<simple expression> | <string literal>) } |
    //                          <block-body> | $IF <relexpression> $THEN <statement> [$ELSE <statement>]  |
    //                          $WHILE <relexpression> $DO <statement> |
    //                          $REPEAT <statement> $UNTIL <relexpression> |
    //                          $FOR <variable> $ASSIGN <simple expression> $TO <simple expression> $DO <statement> |
    //                          $GOTO <label> |
    //                          $WRITELN $LPAR (<simple expression> | <stringconst>) $RPAR ] +
    // <variable>           -> <identifier> [$LBRACK <simple expression> $RBRACK]
    // <label>              -> <identifier>
    // <relexpression>      -> <simple expression> <relop> <simple expression>
    // <relop>              -> $EQ | $LSS | $GTR | $NEQ | $LEQ | $GEQ
    // <simple expression>  -> [<sign>] <term> {<addop> <term>}*
    // <addop>              -> $PLUS | $MINUS
    // <sign>               -> $PLUS | $MINUS
    // <term>               -> <factor> {<mulop> <factor> }*
    // <mulop>              -> $MULTIPLY | $DIVIDE
    // <factor>             -> <unsigned constant> | <variable> | $LPAR <simple expression> $RPAR
    // <type>               -> <simple type> | $ARRAY $LBRACK $INTTYPE $RBRACK $OF $INTEGER
    // <simple type>        -> $INTEGER | $FLOAT | $STRING
    // <constant>           -> [<sign>] <unsigned constant>
    // <unsigned constant>  -> <unsigned number>
    // <unsigned number>    -> $FLOAT | $INTTYPE // as defined for lexical
    // <identifier>         -> $IDENTIFIER
    // <stringconst>        -> $STRINGTYPE

    class Program
    {
        public static void Main(string[] args)
        {
            Run_Program();
            
        }

        public static void Run_Program()
        {
            bool echoOn = true;
            // string fileName = "3BGoodTestfile1.txt";
            // string fileName = "3BBadTestfile1.txt";
            // string fileName = "GoodtreeA.txt";
            // string fileName = "BadProg1.txt";
            // string fileName = "BadProg2B.txt";
            // string fileName = "BadProg3B.txt";
            // string fileName = "working.txt";
            string fileName = "GoodProg4.txt";
            int MaxQuad = 1000;
            FileHandler FileGetter = new FileHandler();
            ReserveWordClass ReserveTable = new ReserveWordClass();
            OpCodeTableClass OpCodeTable = new OpCodeTableClass();
            ReserveTable.InitializeReserveWords();
            OpCodeTable.InitializeOpCodes();
            SymbolTable SymbolTable = new SymbolTable(MaxQuad);
            
            QuadTable Quads = new QuadTable();
            QuadTable.Initialize();
            TokenizerClass Tokenizer = new TokenizerClass();
            SyntaxAndCodeGen SyntaxAndCodeGen = new SyntaxAndCodeGen();
            FileHandler.InitializeInputFile(fileName);
            SymbolTable.Initialize();
            PrintHeader();
            
            while (!TokenizerClass.tokenizerFinished)
            {
                echoOn = false;
                GetNextToken(echoOn);
                echoOn = true;
                SyntaxAndCodeGen.Analyze(echoOn);
            
            }

            // Console.WriteLine("Tokenizer Finished");
            Console.ReadLine();
            if (!SyntaxAndCodeGen.error)
            {
                SymbolTable.PrintSymbolTable();
                QuadTable.PrintQuadTable();
            }
            
            Interpreter.IntrepretQuads(Quads, SymbolTable, true);
            SymbolTable.PrintSymbolTable();
            Console.WriteLine("End of Program - Press any key to exit.");
            Console.ReadLine();
        }

        public static void GetNextToken(bool echoOn)
        {
            TokenizerClass.GetNextToken(echoOn);
            TokentoSymTable(TokenizerClass.nextToken, TokenizerClass.tokenCode);
            if (SyntaxAndCodeGen.verbose)
            {
                PrintToken(TokenizerClass.nextToken, TokenizerClass.tokenCode);
            }

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


}
