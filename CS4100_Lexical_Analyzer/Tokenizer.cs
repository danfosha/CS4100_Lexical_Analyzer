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
        public static int caseGroup = -1;
        public static bool identifier = false;
        public static bool numeric = false;
        public static bool stringConstant = false;
        public static bool comment = false;
        public static bool tokenComplete = false;
        public static bool tokenTooLong = false;
        public static bool stringComplete = false;
        public static bool commentComplete = false;

        // could make getNextChar and call it inside below to match specs of assignment
        public static string GetNextToken(bool echoOn, char nextChar)
        {
            // int textLength = fileText.Length;


            if (nextChar.Equals('\n'))
            {
                tokenComplete = true;
            }

            // prime read?


            while (!tokenComplete)
            {
                char x = nextChar;
                if (Char.IsWhiteSpace(x))
                {
                    caseGroup = 0;
                }
                else if (Char.IsLetter(x) || '$'.Equals(x) || '_'.Equals(x))
                {
                    caseGroup = 1;
                    if (Char.IsLetter(Tokenizer.nextToken[0]))
                    {
                        identifier = true;
                    }
                }
                else if (Char.IsDigit(x))
                {
                    caseGroup = 2;
                    if (Char.IsDigit(Tokenizer.nextToken[0]))
                    {
                        numeric = true;
                    }

                }
                else if ('"'.Equals(x))
                {
                    caseGroup = 3;
                    if ('"'.Equals(Tokenizer.nextToken[0]))
                    {
                        stringConstant = true;
                    }

                    if (('"'.Equals(Tokenizer.nextToken[0])) && ('"'.Equals(Tokenizer.nextToken[Tokenizer.nextToken.Length - 1])))
                    {
                        stringComplete = true;
                        tokenComplete = true;
                        caseGroup = 0;
                    }


                }

                else if ('('.Equals(x))
                {
                    caseGroup = 4;
                    if ('('.Equals(Tokenizer.nextToken[0]) && '*'.Equals(Tokenizer.nextToken[1]))
                    {
                        comment = true;
                    }
                }

                else if ('#'.Equals(x))
                {
                    caseGroup = 4;
                    if ('#'.Equals(Tokenizer.nextToken[0]) && '#'.Equals(Tokenizer.nextToken[1]))
                    {
                        comment = true;
                    }
                }



                switch (caseGroup)
                {
                    case 0:
                        break;

                    case 1:
                        if (Tokenizer.nextToken.Length > 30)
                        {
                            tokenComplete = true;
                            tokenTooLong = true;
                            break;
                        }

                        if (identifier)
                        {
                            Tokenizer.nextToken.Append(x);
                        }
                        break;

                    case 2:
                        if (Tokenizer.nextToken.Length > 30)
                        {
                            tokenComplete = true;
                            tokenTooLong = true;
                            break;
                        }
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


            if (tokenComplete)
            {
                if (tokenTooLong)
                {
                    Console.WriteLine("Warning: Token longer than 30 characters and is truncated.");
                }

                if ((stringConstant) && (!stringComplete))
                {

                }


                tokenComplete = false;
                return nextToken.ToString();

            }

            return "";

        }
    }
}
