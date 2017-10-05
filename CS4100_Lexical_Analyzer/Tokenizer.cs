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
        public static char tempChar;
        public static char x;

        public static int caseGroup = -1;


        public static bool identifier = false;
        public static bool numeric = false;
        public static bool stringConstant = false;
        public static bool comment = false;
        public static bool other1 = false;
        public static bool other2 = false;
        public static bool unidentified = false;
        public static bool tempUsed = true;

        public static bool tooLong = false;
        public static bool tokenComplete = false;
        public static bool tokenTooLong = false;
        public static bool stringComplete = false;
        public static bool commentComplete = false;


        // could make getNextChar and call it inside below to match specs of assignment
        public static string GetNextToken(bool echoOn, char nextChar)
        {
            
            while (tempUsed)
            {

                // end token if new line
                if (nextChar.Equals('\n'))
                {
                    tokenComplete = true;
                    caseGroup = 0;
                }

                // check to see if already in a state
                if (identifier)
                {
                    caseGroup = 1;
                }
                else if (numeric)
                {
                    caseGroup = 2;
                }
                else if (stringConstant)
                {
                    caseGroup = 3;
                }
                else if (comment)
                {
                    caseGroup = 4;
                }
                else if (other1)
                {
                    caseGroup = 5;
                }
                else if (other2)
                {
                    caseGroup = 6;
                }
                else
                {
                    caseGroup = 0;
                }

                // set state with first character 
                //if (tempChar != '\0')
                //{
                //    x = tempChar;
                //}
                //else
                //{
                   x = nextChar;
                //}
                

                if (Tokenizer.nextToken.Length < 1) // first character
                {
                    if (Char.IsWhiteSpace(x))
                    {
                        caseGroup = 0;
                    }
                    else if (Char.IsLetter(x) || '$'.Equals(x) || '_'.Equals(x))
                    {
                        identifier = true;
                        caseGroup = 1;
                    }
                    else if (Char.IsDigit(x))
                    {
                        numeric = true;
                        caseGroup = 2;
                    }
                    else if ('"'.Equals(x))
                    {
                        stringConstant = true;
                        caseGroup = 3;
                    }

                    else if ('('.Equals(x))
                    {
                        comment = true;
                        caseGroup = 4;

                    }

                    else if ('#'.Equals(x))
                    {
                        comment = true;
                        caseGroup = 4;
                    }
                    else if (Tokenizer.OtherTokenFirst(x))
                    {
                        other1 = true;
                        caseGroup = 5;
                    }
                    else if (Tokenizer.OtherTokenSecond(x))
                    {
                        other2 = true;
                        caseGroup = 6;
                    }
                    else
                    {
                        caseGroup = 7;
                    }


                }


                switch (caseGroup)
                {
                    case 0:
                        // ignore, don't append
                        break;

                    case 1:
                        // append to identifier, first character is a letter
                        if (IdentifierChar(x))
                        {
                            if (Tokenizer.nextToken.Length > 30)
                            {
                                tokenComplete = true;
                                tokenTooLong = true;
                                break;
                            }
                            else
                            {
                                Tokenizer.nextToken.Append(x);
                            }
                        }
                        else
                        {
                            tokenComplete = true;
                            tempChar = x;
                        }
                        break;

                    case 2:
                        // append to numeric
                        if (Tokenizer.nextToken.Length > 30)
                        {
                            tokenComplete = true;
                            tokenTooLong = true;
                            break;
                        }
                        Tokenizer.nextToken.Append(x);
                        break;

                    case 3:
                        Tokenizer.nextToken.Append(x);
                        break;
                    case 4:
                        // comment handling
                        // append next char
                        if ((Tokenizer.nextToken.Length>0) && ('#'.Equals(x)) && ('#'.Equals(Tokenizer.nextToken[0])))
                        {
                            commentComplete = true;
                            tokenComplete = true;
                        }
                        Tokenizer.nextToken.Append(x);
                        tempChar = '\0';
                        break;
                    case 5:
                        // other symbols
                        Tokenizer.nextToken.Append(x);
                        break;
                    case 6:
                        // other symbol 2             
                        if (('='.Equals(x)) || ('>'.Equals(x)))
                        {
                            Tokenizer.nextToken.Append(x);
                            tempChar = '\0';
                        }
                        else
                        {
                            tempChar = x;
                        }
                         break;
                        
                    default:
                        break;
                }

                //}

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
                    Tokenizer.ResetFlags();
                    tempChar = '\0';
                    return nextToken.ToString();
                }
                
            }
            return "";
        }

        public static bool IdentifierChar(char x)
        {
            if ((Char.IsLetter(x) || '$'.Equals(x) || '_'.Equals(x)))
            {
                return true;
            }
            else
            {
                return false;
            }
        }



        public static bool OtherTokenFirst(char x)
        {
            if (('/'.Equals(x)) || ('*'.Equals(x)) || ('+'.Equals(x)) || ('-'.Equals(x)) || (')'.Equals(x)) || (';'.Equals(x)) || (','.Equals(x)) || ('['.Equals(x)) || (']'.Equals(x)) || ('.'.Equals(x)) || ('='.Equals(x)))
            {

                return true;
            }
            else
            {
                return false;
            }
        }


        public static bool OtherTokenSecond(char x)
        {
            if ((':'.Equals(x)) || ('>'.Equals(x)) || ('<'.Equals(x)))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static void ResetFlags()
        {
            identifier = false;
            numeric = false;
            stringConstant = false;
            comment = false;
            other1 = false;
            other2 = false;
            unidentified = false;

            tooLong = false;
            tokenComplete = false;
            tokenTooLong = false;
            stringComplete = false;
            commentComplete = false;
        }
    }



}
