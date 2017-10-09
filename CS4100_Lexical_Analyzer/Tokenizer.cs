using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


// CS4100 Fall 2017 Lexical Analyzer Project - Fosha
namespace CS4100_Lexical_Analyzer
{
    public class TokenizerClass
    {

        public TokenizerClass()
        {
        }

        public static StringBuilder workingToken = new StringBuilder();
        public static StringBuilder workingLine = new StringBuilder();

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
        public static bool comment1 = false;
        public static bool comment2 = false;
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
        public static bool lineComplete = true;
        //public static String fileText; 
        public static string textLine;


        // getNextChar
        public static int charIndex = 0;
        public static int lineIndex = 0;


        public static char GetNextChar()
        {
            if (charIndex >= textLine.Length)
            {
                lineComplete = true;
                return ('\n');
            }
            else
            {
                return textLine[charIndex++];
            }

        }

        public static string GetNextLine()
        {

            if (lineIndex >= FileHandler.FileText.Length)
            {
                tokenComplete = true;
                tokenizerFinished = true;
                return ("\a");
            }

            while ((lineIndex < FileHandler.FileText.Length) && (!FileHandler.FileText[lineIndex].Equals('\n')))
            {
                workingLine.Append(FileHandler.FileText[lineIndex++]);

            }

            if (FileHandler.FileText[lineIndex].Equals('\n'))
            {
                workingLine.Append('\n');
                lineIndex++;
            }

            lineComplete = false;
            textLine = workingLine.ToString();
            workingLine.Clear();
            return textLine;
        }

        // could make getNextChar and call it inside below to match specs of assignment
        public static void GetNextToken(bool echoOn)
        {
            if (lineComplete)
            {
                string nextLine = GetNextLine();
                
                if (echoOn)
                {
                    Console.Write(nextLine);
                }
                charIndex = 0;
            }

            tokenComplete = false;

            do
            {
                if (!tempUsed)
                {
                    nextChar = GetNextChar();
                    x = nextChar;
                }
                else
                {
                    nextChar = tempChar;
                }

                // end token if new line
                if (nextChar.Equals('\a'))
                {
                    workingToken.Append("");
                    tokenComplete = true;
                    tokenizerFinished = true;

                }

                else if ((nextChar.Equals('\n')) || (nextChar.Equals('\r')))
                {
                    caseGroup = 0;
                    if ((nextChar.Equals('\n')) && (!commentComplete))
                    {
                        tokenComplete = true;
                        lineComplete = true;
                        ResetFlags();
                    }

                }
                else
                {

                    // check to see if already in a state
                    if (workingToken.Length > 1)
                    {
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
                        else if (comment1) // (**)
                        {
                            caseGroup = 4;
                        }
                        else if (comment2) // ##
                        {
                            caseGroup = 5;
                        }
                        else if (other1)
                        {
                            caseGroup = 6;
                        }
                        else if (other2)
                        {
                            caseGroup = 7;
                        }
                        else if (other3)
                        {
                            caseGroup = 8;
                        }
                    }

                    if (workingToken.Length < 1) // first character
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

                        else if ('#'.Equals(x))
                        {
                            comment2 = true;
                            caseGroup = 5;
                        }
                        else if (OtherTokenFirst(x))
                        {
                            other1 = true;
                            caseGroup = 6;
                        }
                        else if (OtherTokenSecond(x))
                        {
                            other2 = true;
                            caseGroup = 7;
                        }
                        else if (OtherTokenThird(x))
                        {
                            other3 = true;
                            caseGroup = 8;
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
                            tempUsed = false;
                            break;

                        case 1:
                            // append to identifier, first character is a letter
                            if (IdentifierChar(x))
                            {
                                if (workingToken.Length > 30)
                                {
                                    //tokenComplete = true;
                                    tokenTooLong = true;
                                    break;
                                }
                                else
                                {
                                    workingToken.Append(x);
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
                            if (NumericChar(x, test))
                            {
                                if (test.Length < 29)
                                {
                                    workingToken.Append(x);
                                }
                                else
                                {
                                    tokenTooLong = true;
                                }

                            }
                            else
                            {
                                tempChar = x;
                                tempUsed = true;
                                tokenComplete = true;
                            }

                            break;

                        case 3: // stringConstant
                            if ((workingToken.Length > 0) && ('"'.Equals(x)) && ('"'.Equals(workingToken[0])))
                            {
                                commentComplete = true;
                                tokenComplete = true;
                            }
                            workingToken.Append(x);
                            break;

                        case 4:
                            // comments (* *)
                            if ('*'.Equals(workingToken[workingToken.Length - 1]) && (')'.Equals(x)))
                            {
                                commentComplete = true;
                                tokenComplete = true;
                            }
                            workingToken.Append(x);
                            break;
                        case 5:

                            // comment handling ##
                            // append next char
                            if ((workingToken.Length > 0) && ('#'.Equals(x)) && ('#'.Equals(workingToken[0])))
                            {
                                commentComplete = true;
                                tokenComplete = true;
                            }

                            workingToken.Append(x);
                            //tempChar = '\0';
                            break;
                        case 6:
                            // other symbol 1 / + -  ) ; , [ ] .
                            workingToken.Append(x);
                            tokenComplete = true;
                            break;
                        case 7:
                            // other symbol 2 : < (
                            if ((':'.Equals(tempChar)) && ('='.Equals(nextChar)))
                            {
                                //workingToken.Append(x);
                                workingToken.Append(nextChar);
                                tempChar = '\0';
                                tempUsed = false;
                                tokenComplete = true;

                            }
                            else if (('<'.Equals(tempChar)) && (('='.Equals(nextChar)) || ('>'.Equals(nextChar))))
                            {
                                //workingToken.Append(x);
                                workingToken.Append(nextChar);
                                tempChar = '\0';
                                tempUsed = false;
                                tokenComplete = true;
                            }

                            else if (('>'.Equals(tempChar)) && ('='.Equals(nextChar)))
                            {
                                //workingToken.Append(x);
                                workingToken.Append(nextChar);
                                tempChar = '\0';
                                tempUsed = false;
                                tokenComplete = true;

                            }
                            else if (('('.Equals(tempChar)) && (('*'.Equals(nextChar))))
                            {
                                //workingToken.Append(x);
                                workingToken.Append(nextChar);
                                tempChar = '\0';
                                tempUsed = false;
                                comment1 = true;
                            }
                            else if (('*'.Equals(nextChar)) || ('>'.Equals(nextChar)) || ('='.Equals(nextChar)))
                            {
                                workingToken.Append(x);
                                tokenComplete = true;
                            }
                            else
                            {
                                workingToken.Append(x);
                                tempChar = x;
                            }

                            break;
                        case 8:
                            //// othersymbol3 > = *
                            //if (('>'.Equals(tempChar)) && ('='.Equals(nextChar)))
                            //{
                            //    workingToken.Append(x);
                            //    workingToken.Append(nextChar);
                            //    tokenComplete = true;
                            //}
                            //else
                            //{
                            //    workingToken.Append(x);
                            //    tokenComplete = true;
                            //    tempChar = nextChar;
                            //}
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

                    ResetFlags();

                    nextToken = workingToken.ToString();
                    workingToken.Clear();
                    if (tempUsed)
                    {
                        tempChar = x;
                    }
                }

            }
            while (!tokenComplete);
        }


        // methods
        public static bool IdentifierChar(char x)
        {
            if (Char.IsLetter(x) || '$'.Equals(x) || '_'.Equals(x) || Char.IsNumber(x))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool NumericChar(char x, string test)
        {
            if (Char.IsNumber(x))
            {
                return true;
            }
            else if (('.'.Equals(x)) && !(test.Contains(".")))
            {
                return true;
            }
            else if ((('E'.Equals(x)) || 'e'.Equals(x)) && (!(test.Contains("E")) || !(test.Contains("e"))) && (test.Contains(".")))
            {
                return true;
            }
            else if ((('+'.Equals(x)) || '-'.Equals(x)) && ('E'.Equals(test[test.Length - 1])) || ('e'.Equals(test[test.Length - 1])))
            {
                return true;
            }
            return false;
        }




        public static bool OtherTokenFirst(char x)
        {
            if (('/'.Equals(x)) || ('*'.Equals(x)) || ('+'.Equals(x)) || ('-'.Equals(x)) || (')'.Equals(x)) || (';'.Equals(x)) || (','.Equals(x)) || ('['.Equals(x)) || (']'.Equals(x)) || ('.'.Equals(x)))
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
            if ((':'.Equals(x)) || ('<'.Equals(x)) || ('('.Equals(x)) || ('>'.Equals(x)) || ('='.Equals(x)) || ('*'.Equals(x)))
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
            return true;
        }

        public static void ResetFlags()
        {
            identifier = false;
            numeric = false;
            stringConstant = false;
            comment1 = false;
            comment2 = false;
            other1 = false;
            other2 = false;
            unidentified = false;

            tooLong = false;
            tokenTooLong = false;
            stringComplete = false;
            commentComplete = false;
        }
    }
}




