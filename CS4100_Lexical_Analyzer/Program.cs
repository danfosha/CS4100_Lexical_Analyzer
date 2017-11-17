﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// CS4100 Fall 2017 Lexical Analyzer Project - Fosha
namespace CS4100_Lexical_Analyzer
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

            bool echoOn = true;
            // string fileName = "3BGoodTestfile1.txt";
            string fileName = "3BBadTestfile1.txt";
            // string fileName = "GoodtreeA.txt";
            // string fileName = "BadProg1.txt";
            // string fileName = "BadProg2B.txt";
            // string fileName = "BadProg3B.txt";
            // string fileName = "working.txt";
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

            // Console.WriteLine("Tokenizer Finished");
            Console.ReadLine();
            if (!SyntaxA.error)
            {
                SymbolTable.PrintSymbolTable();
            }
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
            if (SyntaxA.verbose)
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
