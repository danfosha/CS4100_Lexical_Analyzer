using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS4100_Lexical_Analyzer
{
    class SyntaxA
    {

        public bool echoOn = true;
        public bool trace = true;
        public bool error = false;
        public static int tokenCode = TokenizerClass.tokenCode;

        public void Analyze(int tokenCode)
        {
            program();
        }


        // Methods
        public int program()
        {
            if (!error)
            {
                Debug(true, "program");

                if (tokenCode == 18) // $UNIT
                {
                    TokenizerClass.GetNextToken(echoOn);
                    // need to advance tokencode
                    prog_identifier();
                    TokenizerClass.GetNextToken(echoOn);
                    if (tokenCode == 36) // $;
                    {
                        block();
                        if (tokenCode == 48) // $.
                        {
                            Console.WriteLine("You did it!");
                        }
                        else
                        {
                            error = true;
                            ErrorMessage(48, tokenCode);
                        }
                    }
                    else
                    {
                        error = true;
                        ErrorMessage(36, tokenCode);
                    }
                }
                else
                {
                    error = true;
                    ErrorMessage(18, tokenCode);
                }
                Debug(false, "program");
            }
            return 0;
        }

        public int prog_identifier()
        {
            if (!error)
            {
                Debug(true, "prog_identifier");
                identifier();
                Debug(false, "prog_identifier");
            }
            return 0;
        }

        public int block()
        {
            if (!error)
            {
                Debug(true, "block");
                if (tokenCode == 10) // $BEGIN
                {
                    statement();
                    if (tokenCode == 36) // $;
                    {
                        statement();
                        if (tokenCode != 11) // $END
                        {
                            error = true;
                            ErrorMessage(11, tokenCode);
                        }
                    }
                    else
                    {
                        error = true;
                        ErrorMessage(36, tokenCode);
                    }
                }
                else
                {
                    error = true;
                    ErrorMessage(10, tokenCode);
                }
                Debug(false, "block");
            }
            return 0;
        }

        public int statement()
        {
            if (!error)
            {
                Debug(true, "statement");
                variable();
                if (tokenCode == 37) // $:=
                {
                    simple_expression();
                }
                else
                {
                    error = true;
                    ErrorMessage(37, tokenCode);
                }
                Debug(false, "statement");
            }
            return 0;
        }

        public int variable()
        {
            if (!error)
            {
                Debug(true, "variable");
                identifier();
                Debug(false, "variable");
            }
            return 0;
        }

        public int simple_expression()
        {
            if (!error)
            {
                Debug(true, "simple_expression");
                sign(); // optional
                term();
                addop();
                term();

                Debug(false, "simple_expression");
            }
            return 0;
        }

        public int addop()
        {
            if (!error)
            {
                Debug(true, "addop");
                if ((tokenCode != 32) || (tokenCode != 33)) 
                {
                    error = true;
                    ErrorMessage(31, 32, tokenCode);
                }
                Debug(false, "addop");
            }
            return 0;
        }

        public int sign()
        {
            if (!error)
            {
                Debug(true, "sign");
                if ((tokenCode != 32) || (tokenCode != 33))
                {
                    error = true;
                    ErrorMessage(32, 33, tokenCode);
                }
                Debug(false, "sign");
            }
            return 0;
        }

        public int term()
        {
            if (!error)
            {
                Debug(true, "term");
                factor();
                mulop();
                factor();
                Debug(false, "term");
            }
            return 0;
        }

        public int mulop()
        {
            if (!error)
            {
                Debug(true, "mulop");
                if ((tokenCode != 30) || (tokenCode != 31))
                {
                    error = true;
                }
                Debug(false, "mulop");
            }
            return 0;
        }

        public int factor()
        {
            if (!error)
            {
                Debug(true, "factor");
                unsigned_constant();
                variable();
                if (tokenCode == 34)
                {
                    simple_expression();
                    if (tokenCode != 35)
                    {
                        error = true;
                    }
                }
                else
                {
                    error = true;
                }
                Debug(false, "factor");
            }
            return 0;
        }

        public int unsigned_constant()
        {
            if (!error)
            {
                Debug(true, "unsigned_constant");
                unsigned_number();
                Debug(false, "unsigned_constant");
            }
            return 0;
        }

        public int unsigned_number()
        {
            if (!error)
            {
                Debug(true, "unsigned_number");
                if ((tokenCode != 51) || (tokenCode != 52))
                {
                    error = true;
                }
                Debug(false, "unsigned_number");
            }
            return 0;
        }

        public int identifier()
        {
            if (!error)
            {
                Debug(true, "identifier");
                Debug(false, "identifier");
            }
            return 0;
        }

        public void Debug(bool entering, string name)
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

        public void ErrorMessage(int rightTokenCode, int wrongTokenCode)
        {
            Console.WriteLine(rightTokenCode + " expected, but " + wrongTokenCode + " found");
        }

        public void ErrorMessage(int rightTokenCode1, int rightTokenCode2, int wrongTokenCode)
        {
            Console.WriteLine(rightTokenCode1 + " or " + rightTokenCode2 + " expected, but " + wrongTokenCode + " found.");
        }
    }
}
