using System;
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
        public static bool error = false;
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
                            Console.WriteLine("You did it!");
                        }
                        else
                        {
                            error = true;
                            ErrorMessage(48, TokenizerClass.tokenCode);
                        }
                    }
                    else
                    {
                        error = true;
                        ErrorMessage(36, TokenizerClass.tokenCode);
                    }
                }
                else
                {
                    error = true;
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
                    while (TokenizerClass.tokenCode == 36) // $;
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
                        error = true;
                        ErrorMessage(11, TokenizerClass.tokenCode);
                    }
                }
                else
                {
                    error = true;
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
                    error = true;
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
                while ((TokenizerClass.tokenCode == 32) || (TokenizerClass.tokenCode == 33))
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
                    error = true;
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
                    error = true;
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
                while ((TokenizerClass.tokenCode == 31) || (TokenizerClass.tokenCode == 30))
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
                    Console.WriteLine("Not a mulop. Going on.");
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
                    if (TokenizerClass.tokenCode != 35) //$)
                    {
                        error = true;
                        ErrorMessage(35, TokenizerClass.tokenCode);
                    }
                    GetNextToken(echoOn);
                }
                else
                {
                    error = true;
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
                    error = true;
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
                    error = true;
                    ErrorMessage(50, TokenizerClass.tokenCode);
                    return 0;
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
                    Console.WriteLine("Entering " + name);
                }
                else
                {
                    Console.WriteLine("Exiting " + name);
                }
            }
        }

        public static void ErrorMessage(int rightTokenCode, int wrongTokenCode)
        {
            Console.WriteLine(rightTokenCode + " expected, but " + wrongTokenCode + " found");
        }

        public static void ErrorMessage(int rightTokenCode1, int rightTokenCode2, int wrongTokenCode)
        {
            Console.WriteLine(rightTokenCode1 + " or " + rightTokenCode2 + " expected, but " + wrongTokenCode + " found.");
        }

        public static void ErrorMessage(int rightTokenCode1, int rightTokenCode2, int rightTokenCode3, int wrongTokenCode)
        {
            Console.WriteLine(rightTokenCode1 + ", " + rightTokenCode2 + ", or" + rightTokenCode3 + " expected, but " + wrongTokenCode + " found.");
        }

        public static void ErrorMessage(int rightTokenCode1, int rightTokenCode2, int rightTokenCode3, int rightTokenCode4, int wrongTokenCode)
        {
            Console.WriteLine(rightTokenCode1 + ", " + rightTokenCode2 + ", " + rightTokenCode3 + ", or" + rightTokenCode4 + " expected, but " + wrongTokenCode + " found.");
        }


        public static void GetNextToken(bool echoOn)
        {
            TokenizerClass.GetNextToken(echoOn);
            while (TokenizerClass.tokenCode == -1)
            {
                TokenizerClass.GetNextToken(echoOn);
            }
            //TokentoSymTable(TokenizerClass.nextToken, TokenizerClass.tokenCode);
            PrintToken(TokenizerClass.nextToken, TokenizerClass.tokenCode);
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
