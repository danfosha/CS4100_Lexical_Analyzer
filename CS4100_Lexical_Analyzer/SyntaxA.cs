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

        public void Analyze(int tokenCode)
        {
            program(tokenCode);
        }


        // Methods
        public void program(int tokenCode)
        {
            if (!error)
            {
                Debug(true, "program");

                if (tokenCode == 18)
                {
                    TokenizerClass.GetNextToken(echoOn);
                    // need to advance tokencode
                    prog_identifier(tokenCode);
                    TokenizerClass.GetNextToken(echoOn);
                    if (tokenCode == 36)
                    {
                        block(tokenCode);
                        if (tokenCode == 48)
                        {
                            Console.WriteLine("You did it!");
                        }
                    }
                }
                Debug(false, "program");
            }
        }

        public void prog_identifier(int tokenCode)
        {
            if (!error)
            {
                Debug(true, "prog_identifier");
                identifier(tokenCode);
                Debug(false, "prog_identifier");
            }
        }

        public void block(int tokenCode)
        {
            if (!error)
            {
                Debug(true, "block");
                Debug(false, "block");
            }
        }

        public void statement(int tokenCode)
        {
            if (!error)
            {
                Debug(true, "statement");
                Debug(false, "statement");
            }
        }

        public void identifier(int tokenCode)
        {
            if (!error)
            {
                Debug(true, "identifier");
                Debug(false, "identifier");
            }
        }

        public void simple_expression(int tokenCode)
        {
            if (!error)
            {
                Debug(true, "simple_expression");
                Debug(false, "simple_expression");
            }
        }

        public void addop(int tokenCode)
        {
            if (!error)
            {
                Debug(true, "addop");
                Debug(false, "addop");
            }
        }

        public void sign(int tokenCode)
        {
            if (!error)
            {
                Debug(true, "sign");
                Debug(false, "sign");
            }
        }

        public void term(int tokenCode)
        {
            if (!error)
            {
                Debug(true, "term");
                Debug(false, "term");
            }
        }

        public void mulop(int tokenCode)
        {
            if (!error)
            {
                Debug(true, "mulop");
                Debug(false, "mulop");
            }
        }

        public void factor(int tokenCode)
        {
            if (!error)
            {
                Debug(true, "factor");
                Debug(false, "factor");
            }
        }

        public void unsigned_constant(int tokenCode)
        {
            if (!error)
            {
                Debug(true, "unsigned_constant");
                Debug(false, "unsigned_constant");
            }
        }

        public void unsigned_number(int tokenCode)
        {
            if (!error)
            {
                Debug(true, "unsigned_number");
                Debug(false, "unsigned_number");
            }
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
    }
}
