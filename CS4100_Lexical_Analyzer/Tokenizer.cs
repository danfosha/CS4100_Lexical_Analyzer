using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS4100_Lexical_Analyzer
{
    class Tokenizer
    {
        
        public static StringBuilder nextToken = new StringBuilder();
        public static bool tooLong = false;

        // could make getNextChar and call it inside below to match specs of assignment
        public static string GetNextToken(bool echoOn, char nextChar)
        {
            // int textLength = fileText.Length;
            int caseGroup = -1;
            bool identifier = false;
            bool tokenComplete = false;

            if (nextChar.Equals('\n'))
            {
                tokenComplete = true;
            }

            while (tokenComplete)
            {
                char x = nextChar;
                if (Char.IsWhiteSpace(x))
                {
                    caseGroup = 0;
                }
                else if (Char.IsLetter(x) || '$'.Equals(x) || '_'.Equals(x))
                {
                    caseGroup = 1;
                    identifier = true;

                }
                else if (Char.IsDigit(x))
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
                        if (Char.IsLetter(Tokenizer.nextToken[0]))
                        {
                            Tokenizer.nextToken.Append(x);
                        }
                        break;
                    case 2:
                        Tokenizer.nextToken.Append(x);
                        break;
                    case 3:
                        //Tokenizer.nextToken.Append(x);
                        break;


                    default:
                        break;

                }                

            }

            // get the token out

            tokenComplete = false;
            return nextToken.ToString();
            



        }
    }
}
