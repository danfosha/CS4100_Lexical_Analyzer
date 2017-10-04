using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS4100_Lexical_Analyzer
{
    class Tokenizer
    {

        public static char GetNextToken(bool echoOn, char nextChar)
        {
           // int textLength = fileText.Length;
            int caseGroup = -1;
            bool tokenComplete = false;

            while (!tokenComplete)
            {
                char x = nextChar;
                if (Char.IsWhiteSpace(x))
                {
                    caseGroup = 0;
                }
                else if (Char.IsDigit(x))
                {
                    caseGroup = 1;
                }
                else if (Char.IsLetter(x))
                {
                    caseGroup = 2;
                }
                else if (Char.IsSymbol(x))
                {
                    caseGroup = 3;
                }

                switch (caseGroup)
                {
                    case 0:
                        break;
                    case 1:


                    default:
                        break;

                }


                

            }

            return nextChar;
        }
    }
}
