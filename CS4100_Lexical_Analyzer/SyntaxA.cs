﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS4100_Lexical_Analyzer
{
    class SyntaxA
    {
        public SyntaxA()
        {
        }

        public static bool echoOn = true;
        public static bool trace = true;
        public static bool verbose = true;
        public static bool error = false;
        public static int paddingIndent = 0;
        public static bool uniqueProgIdent = false;
        public static string ProgIdent;
       
        // public static int tokenCode = TokenizerClass.tokenCode;

        public static void Analyze(bool echoon)
        {
            echoOn = echoon;
            if ((TokenizerClass.tokenCode >= 0) && (TokenizerClass.tokenCode < 100)) // skip -1
            {
                program();
            }
        }

        // Methods
        public static int program()
        {
            if (!error)
            {
                Debug(true, "program");

                if (TokenizerClass.tokenCode == 15)// $UNIT
                {
                    GetNextToken(echoOn);
                    // need to advance tokencode
                    prog_identifier();
                    if (TokenizerClass.tokenCode == 36) // $;
                    {
                        GetNextToken(echoOn);
                        block();
                        if (TokenizerClass.tokenCode == 48) // $.
                        {
                            Console.WriteLine("No syntax errors found.");
                            TokenizerClass.tokenizerFinished = true;
                            // will ignore rest of file
                        }
                        else
                        {

                            ErrorMessage(48, TokenizerClass.tokenCode);
                        }
                    }
                    else
                    {

                        ErrorMessage(36, TokenizerClass.tokenCode);
                    }
                }
                else
                {

                    ErrorMessage(18, TokenizerClass.tokenCode);
                }
                Debug(false, "program");
            }
            return 0;
        }

        public static int prog_identifier()
        {
            if (!error)
            {
                Debug(true, "prog_identifier");
                identifier();
                Debug(false, "prog_identifier");
            }
            return 0;
        }

        public static int block()
        {
            if (!error)
            {
                Debug(true, "block");
                if (TokenizerClass.tokenCode == 10) // $BEGIN
                {
                    GetNextToken(echoOn);
                    statement();
                    while ((TokenizerClass.tokenCode == 36) && !error) // $;
                    {
                        GetNextToken(echoOn);
                        statement();
                    }
                    if (TokenizerClass.tokenCode == 11) // $END
                    {
                        GetNextToken(echoOn);
                    }
                    else
                    {

                        ErrorMessage(11, TokenizerClass.tokenCode);
                    }
                }
                else
                {

                    ErrorMessage(10, TokenizerClass.tokenCode);
                }
                Debug(false, "block");
            }
            return 0;
        }

        public static int statement()
        {
            if (!error)
            {
                Debug(true, "statement");
                variable();
                if (TokenizerClass.tokenCode == 37) // $:=
                {
                    GetNextToken(echoOn);
                    simple_expression();
                }
                else
                {

                    ErrorMessage(37, TokenizerClass.tokenCode);
                }
                Debug(false, "statement");
            }
            return 0;
        }

        public static int variable()
        {
            if (!error)
            {
                Debug(true, "variable");
                identifier();
                Debug(false, "variable");
            }
            return 0;
        }

        public static int simple_expression()
        {
            if (!error)
            {
                Debug(true, "simple_expression");
                if ((TokenizerClass.tokenCode == 32) || (TokenizerClass.tokenCode == 33))
                {
                    sign();
                }
                term();
                while (((TokenizerClass.tokenCode == 32) || (TokenizerClass.tokenCode == 33)) && !error)
                {
                    GetNextToken(echoOn);
                    term();
                }

                Debug(false, "simple_expression");
            }
            return 0;
        }

        public static int addop()
        {
            if (!error)
            {
                Debug(true, "addop");
                if ((TokenizerClass.tokenCode != 32) && (TokenizerClass.tokenCode != 33))
                {

                    ErrorMessage(31, 32, TokenizerClass.tokenCode);
                    Debug(false, "addop"); // check this!
                    return 0;
                }
                GetNextToken(echoOn);
                Debug(false, "addop");
            }
            return 1;
        }

        public static int sign()
        {
            if (!error)
            {
                Debug(true, "sign");
                if ((TokenizerClass.tokenCode != 32) && (TokenizerClass.tokenCode != 33))
                {

                    ErrorMessage(32, 33, TokenizerClass.tokenCode);
                    return 0;
                }
                GetNextToken(echoOn);
                Debug(false, "sign");
            }
            return 0;
        }

        public static int term()
        {
            if (!error)
            {
                Debug(true, "term");
                factor();
                while (((TokenizerClass.tokenCode == 31) || (TokenizerClass.tokenCode == 30)) && !error)
                {
                    mulop();
                    factor();
                    // GetNextToken(echoOn);
                }
                Debug(false, "term");
            }
            return 0;
        }

        public static int mulop()
        {
            if (!error)
            {
                Debug(true, "mulop");
                if ((TokenizerClass.tokenCode != 30) && (TokenizerClass.tokenCode != 31))
                {
                    // error = true;
                    ErrorMessage(30, 31, TokenizerClass.tokenCode);

                    return 0;
                }
                GetNextToken(echoOn);
                Debug(false, "mulop");
            }
            return 1;
        }

        public static int factor()
        {
            if (!error)
            {
                Debug(true, "factor");
                if ((TokenizerClass.tokenCode == 51) || (TokenizerClass.tokenCode == 52)) // int or float
                {
                    unsigned_constant();
                }
                else if (TokenizerClass.tokenCode == 50) // identifier
                {
                    variable();
                }
                else if (TokenizerClass.tokenCode == 34) // $(
                {
                    GetNextToken(echoOn);
                    simple_expression();
                    if (TokenizerClass.tokenCode == 35) //$)
                    {
                        GetNextToken(echoOn);
                    }
                    else
                    {

                        ErrorMessage(35, TokenizerClass.tokenCode);
                    }

                }
                else
                {

                    ErrorMessage(50, 51, 52, 34, TokenizerClass.tokenCode);
                }
                Debug(false, "factor");
            }
            return 0;
        }

        public static int unsigned_constant()
        {
            if (!error)
            {
                Debug(true, "unsigned_constant");
                unsigned_number();
                Debug(false, "unsigned_constant");
            }
            return 0;
        }

        public static int unsigned_number()
        {
            if (!error)
            {
                Debug(true, "unsigned_number");
                if ((TokenizerClass.tokenCode != 51) && (TokenizerClass.tokenCode != 52))
                {

                    ErrorMessage(51, 52, TokenizerClass.tokenCode);
                    return 0;
                }
                GetNextToken(echoOn);
                Debug(false, "unsigned_number");
            }
            return 1;
        }

        public static int identifier()
        {
            if (!error)
            {
                Debug(true, "identifier");
                if (TokenizerClass.tokenCode != 50)
                {

                    ErrorMessage(50, TokenizerClass.tokenCode);
                    return 0;
                }
                // first pass sets the program identifier
                if (uniqueProgIdent == false)
                {
                    uniqueProgIdent = true;
                    ProgIdent = TokenizerClass.nextToken;
                }
                else
                {
                    // if -1 is returned, program identifier is already in symbol table
                    if (SymbolTable.LookupSymbol(ProgIdent) >= 0)
                    {
                        ProgIdentErrorMessage(TokenizerClass.nextToken);
                        return 0;
                    }
                }
                GetNextToken(echoOn);
                Debug(false, "identifier");
            }
            return 0;
        }

        public static void Debug(bool entering, string name)
        {
            if (trace)
            {
                if (entering)
                {
                    for (int i = 0; i <= paddingIndent; i++)
                    {
                        Console.Write(" ");
                    }
                    Console.WriteLine(("Entering " + name)); 
                    paddingIndent += 5;
                }
                else
                {
                    paddingIndent -= 5;
                    for (int i = 0; i <= paddingIndent; i++)
                    {
                        Console.Write(" ");
                    }
                    Console.WriteLine(("Exiting " + name)); 

                }
            }
        }


        public static void ErrorMessage(int rightTokenCode, int wrongTokenCode)
        {
            if (!error)
            {
                Error();
                Console.WriteLine(ReserveWordClass.LookupMnem(rightTokenCode) + " expected, but " + ReserveWordClass.LookupMnem(wrongTokenCode) + " found, in line number " + TokenizerClass.lineNumber);
            }
        }

        public static void ErrorMessage(int rightTokenCode1, int rightTokenCode2, int wrongTokenCode)
        {
            if (!error)
            {
                Error();
                Console.WriteLine(ReserveWordClass.LookupMnem(rightTokenCode1) + " or " + ReserveWordClass.LookupMnem(rightTokenCode2) + " expected, but " + ReserveWordClass.LookupMnem(wrongTokenCode) + " found, in line number " + TokenizerClass.lineNumber);
            }
        }

        public static void ErrorMessage(int rightTokenCode1, int rightTokenCode2, int rightTokenCode3, int wrongTokenCode)
        {
            if (!error)
            {
                Error();
                Console.WriteLine(ReserveWordClass.LookupMnem(rightTokenCode1) + ", " + ReserveWordClass.LookupMnem(rightTokenCode2) + ", or" + ReserveWordClass.LookupMnem(rightTokenCode3) + " expected, but " + ReserveWordClass.LookupMnem(wrongTokenCode) + " found, in line number " + TokenizerClass.lineNumber);
            }
        }

        public static void ErrorMessage(int rightTokenCode1, int rightTokenCode2, int rightTokenCode3, int rightTokenCode4, int wrongTokenCode)
        {
            if (!error)
            {
                Error();
                Console.WriteLine(ReserveWordClass.LookupMnem(rightTokenCode1) + ", " + ReserveWordClass.LookupMnem(rightTokenCode2) + ", " + ReserveWordClass.LookupMnem(rightTokenCode3) + ", or " + ReserveWordClass.LookupMnem(rightTokenCode4) + " expected, but " + ReserveWordClass.LookupMnem(wrongTokenCode) + " found, in line number " + TokenizerClass.lineNumber);
            }
        }

        public static void ProgIdentErrorMessage(string token)
        {
            Error();
            Console.WriteLine("Error: " + token + " has already been declared.");
        }

        public static void Error()
        {
            error = true;
            TokenizerClass.tokenizerFinished = true;
        }

        public static void GetNextToken(bool echoOn)
        {
            TokenizerClass.GetNextToken(echoOn);
            while (TokenizerClass.tokenCode == -1)
            {
                TokenizerClass.GetNextToken(echoOn);
            }
            //TokentoSymTable(TokenizerClass.nextToken, TokenizerClass.tokenCode);
            if (verbose)
            {
                PrintToken(TokenizerClass.nextToken, TokenizerClass.tokenCode);
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
    }
}
