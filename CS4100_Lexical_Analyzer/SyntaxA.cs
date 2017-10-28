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
        
        public void Analyze(int tokenCode)
        {
            program(tokenCode);
        }


        // Methods
        public void program(int tokenCode)
        {
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
        }

        public void prog_identifier(int tokenCode)
        {
            identifier(tokenCode);
        }

        public void block(int tokenCode)
        {

        }

        public void statement(int tokenCode)
        {

        }

        public void identifier(int tokenCode)
        {

        }

        public void simple_expression(int tokenCode)
        {

        }

        public void addop(int tokenCode)
        {

        }

        public void sign(int tokenCode)
        {

        }

        public void term(int tokenCode)
        {

        }

        public void mulop(int tokenCode)
        {

        }

        public void factor(int tokenCode)
        {

        }

        public void unsigned_constant(int tokenCode)
        {

        }

        public void unsigned_number(int tokenCode)
        {

        }
        


    }

}