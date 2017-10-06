using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


// CS4100 Fall 2017 Lexical Analyzer Project - Fosha
namespace CS4100_Lexical_Analyzer
{
    public class Tokenizer
    {
        public static StringBuilder workingToken = new StringBuilder();

        public static string nextToken;
        public static char tempChar;
        public static char x;
        public static char nextChar;
        public static int caseGroup = -1;
        public static int tokenCode = -1;

        public static bool tokenizerFinished = false;
        public static bool identifier = false;
        public static bool numeric = false;
        public static bool stringConstant = false;
        public static bool comment = false;
        public static bool other1 = false;
        public static bool other2 = false;
        public static bool other3 = false;
        public static bool unidentified = false;
        public static bool tempUsed = false;

        public static bool tooLong = false;
        public static bool tokenComplete = false;
        public static bool tokenTooLong = false;
        public static bool stringComplete = false;
        public static bool commentComplete = false;
        public static string fileText = FileHandler.FileText;

        // getNextChar
        public static int charIndex = 0;
        public static char GetNextChar()
        {
            if (charIndex >= textLine.Length - 1)
            {
                return '\a';
            }
            else
            {
                return textLine[charIndex++];
            }
        }


        // nextline stuff
        public static string textLine = null;
        public static StringReader strReader = new StringReader(fileText);
        public static string GetNextLine(string text, bool echoOn)
        {
            textLine = strReader.ReadLine();
            if (textLine != null)
            {
                if (echoOn == true)
                {
                    Console.WriteLine(textLine);
                }
                return textLine;
            }
            else
            {
                return ('\n'.ToString());
            }
        }

        // could make getNextChar and call it inside below to match specs of assignment
        public static void GetNextToken(bool echoOn)
        {

            string textLine = GetNextLine(fileText, echoOn);

            do
            {
                nextChar = GetNextChar();

                // end token if new line
                if (nextChar.Equals('\a'))
                {
                    tokenizerFinished = true;
                    break;
                }
                else if ((nextChar.Equals('\n')) || (nextChar.Equals('\r')))
                {
                    caseGroup = 0;
                    ResetFlags();
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
                //else
                //{
                //caseGroup = 0;
                //}

                // set state with first character 
                if (tempChar != '\0')
                {
                    x = tempChar;
                }
                else
                {
                    x = nextChar;
                }


                if (Tokenizer.workingToken.Length < 1) // first character
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
                    else if (Tokenizer.OtherTokenThird(x))
                    {
                        other3 = true;
                        caseGroup = 7;
                    }
                    else
                    { // put undefined here
                        caseGroup = 0;
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
                            if (Tokenizer.workingToken.Length > 30)
                            {
                                tokenComplete = true;
                                tokenTooLong = true;
                                break;
                            }
                            else
                            {
                                Tokenizer.workingToken.Append(x);
                            }
                        }
                        else
                        {
                            tokenComplete = true;
                            tempChar = x;
                            tempUsed = true;
                        }
                        break;

                    case 2:
                        // append to numeric
                        string test = workingToken.ToString();
                        if (Char.IsNumber(x))
                        {
                            Tokenizer.workingToken.Append(x);
                        }
                        else if (('.'.Equals(x)) && !(test.Contains(".")))
                        {
                            Tokenizer.workingToken.Append(x);
                        }
                        else if ((('E'.Equals(x)) || 'e'.Equals(x)) && (!(test.Contains("E")) || !(test.Contains("e"))) && (test.Contains(".")))
                        {
                            Tokenizer.workingToken.Append(x);
                        }
                        else if ((('+'.Equals(x)) || '-'.Equals(x)) && ('E'.Equals(test[test.Length - 1])) || ('e'.Equals(test[test.Length - 1])))
                        {
                            Tokenizer.workingToken.Append(x);
                        }
                        else
                        {
                            tempChar = x;
                            tempUsed = true;
                            tokenComplete = true;
                        }

                        if (Tokenizer.workingToken.Length > 29)
                        {
                            tokenComplete = true;
                            tokenTooLong = true;
                            break;
                        }

                        break;

                    case 3:
                        Tokenizer.workingToken.Append(x);
                        break;
                    case 4:
                        // comment handling
                        // append next char
                        if ((Tokenizer.workingToken.Length > 0) && ('#'.Equals(x)) && ('#'.Equals(Tokenizer.workingToken[0])))
                        {
                            commentComplete = true;
                            tokenComplete = true;
                        }
                        Tokenizer.workingToken.Append(x);
                        //tempChar = '\0';
                        break;
                    case 5:
                        // other symbols 1
                        Tokenizer.workingToken.Append(x);
                        tempUsed = false;
                        tempChar = '\0';
                        tokenComplete = true;
                        break;
                    case 6:
                        // other symbol 2             
                        if ((':'.Equals(x)) && ('='.Equals(nextChar)))
                        {
                            Tokenizer.workingToken.Append(x);
                            Tokenizer.workingToken.Append(nextChar);
                            tempChar = '\0';
                            tempUsed = false;
                            tokenComplete = true;

                        }
                        else if (('<'.Equals(x)) && ('='.Equals(nextChar)) || ('>'.Equals(nextChar)))
                        {
                            Tokenizer.workingToken.Append(x);
                            Tokenizer.workingToken.Append(nextChar);
                            tempChar = '\0';
                            tempUsed = false;
                            tokenComplete = true;
                        }
                        else
                        {
                            Tokenizer.workingToken.Append(x);
                            tokenComplete = true;
                            tempChar = nextChar;
                        }
                        break;

                    default:
                        break;
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
                        // not sure yet
                    }
                    Tokenizer.ResetFlags();
                    //tempChar = '\0';
                    nextToken = workingToken.ToString();
                    workingToken.Clear();
                }

            }
            while ((!tokenComplete));
        }


        // methods
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
            if (('/'.Equals(x)) || ('*'.Equals(x)) || ('+'.Equals(x)) || ('-'.Equals(x)) || ('('.Equals(x)) || (')'.Equals(x)) || (';'.Equals(x)) || (','.Equals(x)) || ('['.Equals(x)) || (']'.Equals(x)) || ('.'.Equals(x)))
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
            if ((':'.Equals(x)) || ('<'.Equals(x)))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool OtherTokenThird(char x)
        {
            if (('>'.Equals(x)) || ('='.Equals(x)))
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
