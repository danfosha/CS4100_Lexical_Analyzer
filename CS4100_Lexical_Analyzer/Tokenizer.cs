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
        public static bool tokenComplete = false;
        public static bool tokenTooLong = false;
        public static bool stringComplete = true;
        public static bool commentComplete = false;
        public static bool lineComplete = true;
        public static string textLine;
        public static string nextLine;


        // getNextChar
        public static int charIndex = 0;
        public static int lineIndex = 0;


        public static char GetNextChar()
        {
            if (charIndex >= textLine.Length)
            {
                lineComplete = true;
                return ('\0');
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
                return ("");
            }

            while ((lineIndex < FileHandler.FileText.Length) && (!FileHandler.FileText[lineIndex].Equals('\n')))
            {
                workingLine.Append(FileHandler.FileText[lineIndex++]);

            }

            if (lineIndex < FileHandler.FileText.Length)
            {
                workingLine.Append('\n');
                lineIndex++;
            }
            else
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
            if ((lineComplete) && (!tokenizerFinished))
            {
                nextLine = GetNextLine();
                if (nextLine == "")
                {
                    tokenizerFinished = true;
                }
                if (echoOn)
                {
                    Console.Write(nextLine);
                }
                charIndex = 0;
            }

            tokenComplete = false;
            if (nextChar == '\0')
            {
                nextChar = GetNextChar();
            }

            do
            {   // end token if new line
                if (nextChar.Equals('\0'))
                {

                    tokenComplete = true;
                    tokenizerFinished = true;
                }

                else if ((nextChar.Equals('\n')) || (nextChar.Equals('\r')))
                {
                    caseGroup = 0;
                    nextChar = GetNextChar();

                    if (nextChar.Equals('\n'))
                    {

                        if ((comment1 || comment2) && (!commentComplete))
                        {
                            tokenComplete = false;
                            //refactor this into a method
                            nextLine = GetNextLine();
                            if (nextLine == "")
                            {
                                tokenizerFinished = true;

                            }
                            if (echoOn)
                            {
                                Console.Write(nextLine);
                            }
                            charIndex = 0;
                        }
                        else
                        {
                            tokenComplete = true;
                            lineComplete = true;
                            ResetFlags();
                        }
                    }
                }
                else if (!tokenComplete)
                {

                    while (Char.IsWhiteSpace(nextChar))
                    {
                        nextChar = GetNextChar();
                    }

                    if (Char.IsLetter(nextChar) || '$'.Equals(nextChar) || '_'.Equals(nextChar))
                    {
                        identifier = true;
                        caseGroup = 1;
                    }
                    else if (Char.IsDigit(nextChar))
                    {
                        numeric = true;
                        caseGroup = 2;
                    }
                    else if ('"'.Equals(nextChar))
                    {
                        stringConstant = true;
                        caseGroup = 3;
                    }

                    else if ('#'.Equals(nextChar))
                    {
                        comment2 = true;
                        caseGroup = 5;
                    }
                    else if (OtherTokenFirst(nextChar))
                    {
                        other1 = true;
                        caseGroup = 6;
                    }
                    else if (OtherTokenSecond(nextChar))
                    {
                        other2 = true;
                        caseGroup = 7;
                    }
                    else if (OtherTokenThird(nextChar))
                    {
                        other3 = true;
                        caseGroup = 8;
                    }
                    else
                    { // put undefined here
                        caseGroup = 0;
                    }

                    //}


                    switch (caseGroup)
                    {
                        case 0:
                            // ignore, don't append
                            // tempUsed = false;
                            break;

                        case 1:
                            // append to identifier, first character is a letter
                            workingToken.Append(nextChar);
                            nextChar = GetNextChar();
                            while (IdentifierChar(nextChar))
                            {
                                if (workingToken.Length < 30)
                                {
                                    workingToken.Append(nextChar);
                                    nextChar = GetNextChar();
                                }
                                else
                                {
                                    tokenTooLong = true;
                                    while (IdentifierChar(nextChar))
                                    {
                                        nextChar = GetNextChar();
                                    }
                                }
                            }
                            tokenComplete = true;
                            break;

                        case 2:
                            // append to numeric
                            workingToken.Append(nextChar);
                            nextChar = GetNextChar();
                            while (NumericChar(nextChar, workingToken.ToString()))
                            {
                                if (workingToken.Length > 29)
                                {
                                    //tokenComplete = true;
                                    tokenTooLong = true;
                                    break;
                                }
                                else
                                {
                                    workingToken.Append(nextChar);
                                }
                                nextChar = GetNextChar();
                            }
                            tokenComplete = true;
                            break;

                        case 3: // stringConstant
                            stringComplete = false;
                            workingToken.Append(nextChar);
                            nextChar = GetNextChar();
                            while (!('"'.Equals(nextChar)))
                            {
                                workingToken.Append(nextChar);
                                nextChar = GetNextChar();
                                if (LineEnd(nextChar) || Formatting(nextChar))
                                {
                                    lineComplete = true;
                                    tokenComplete = true;
                                    workingToken.Clear();
                                    break;
                                }
                            }
                            if ('"'.Equals(nextChar))
                            {
                                workingToken.Append(nextChar);
                                nextChar = GetNextChar();
                                stringComplete = true; // may not need
                            }
                            tokenComplete = true;
                            break;

                        case 4:
                            // comments (* *)
                            if ('*'.Equals(workingToken[workingToken.Length - 1]) && (')'.Equals(nextChar)))
                            {
                                commentComplete = true;
                                tokenComplete = true;
                            }
                            workingToken.Append(nextChar);
                            break;
                        case 5:
                            // comment handling ##
                            workingToken.Append(nextChar);
                            nextChar = GetNextChar();
                            while (!('#'.Equals(nextChar)))
                            {
                                if (nextChar == '\0')
                                {
                                    workingToken.Clear();
                                    tokenComplete = true;
                                    tokenizerFinished = true;
                                    break;
                                }
                                else if (nextChar == '\n')
                                {
                                    nextLine = GetNextLine();
                                    // below refactored into GetNextLine method?
                                    if (nextLine == "")
                                    {
                                        tokenizerFinished = true;
                                    }
                                    if (echoOn)
                                    {
                                        Console.Write(nextLine);
                                    }
                                    charIndex = 0;

                                }
                                workingToken.Append(nextChar);
                                nextChar = GetNextChar();
                            }
                            workingToken.Append(nextChar);
                            nextChar = GetNextChar();
                            tokenComplete = true;
                            break;
                        case 6:
                            // other symbol 1 / + -  ) ; , [ ] .
                            workingToken.Append(nextChar);
                            tokenComplete = true;
                            nextChar = GetNextChar();
                            break;
                        case 7:
                            // other symbol 2 : < (
                            if ((':'.Equals(tempChar)) && ('='.Equals(nextChar)))
                            {
                                //workingToken.Append(x);
                                workingToken.Append(nextChar);
                                tempChar = '\0';
                                // tempUsed = false;
                                tokenComplete = true;

                            }
                            else if (('<'.Equals(tempChar)) && (('='.Equals(nextChar)) || ('>'.Equals(nextChar))))
                            {
                                //workingToken.Append(x);
                                workingToken.Append(nextChar);
                                tempChar = '\0';
                                // tempUsed = false;
                                tokenComplete = true;
                            }

                            else if (('>'.Equals(tempChar)) && ('='.Equals(nextChar)))
                            {
                                //workingToken.Append(x);
                                workingToken.Append(nextChar);
                                tempChar = '\0';
                                // tempUsed = false;
                                tokenComplete = true;

                            }
                            else if (('('.Equals(tempChar)) && (('*'.Equals(nextChar))))
                            {
                                //workingToken.Append(x);
                                workingToken.Append(nextChar);
                                tempChar = '\0';
                                // tempUsed = false;
                                comment1 = true;
                            }
                            else if (('*'.Equals(nextChar)) || ('='.Equals(nextChar)))
                            {
                                workingToken.Append(nextChar);
                                tokenComplete = true;

                            }
                            else if (('>'.Equals(tempChar) && (!OtherTokenThird(nextChar))))
                            {
                                workingToken.Append(nextChar);
                                tokenComplete = true;
                            }
                            else
                            {
                                workingToken.Append(nextChar);
                                tempChar = nextChar;
                            }

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
                    //if (tempUsed)
                    //{
                    //    tempChar = nextChar;
                    //}
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
            else if (
                (('E'.Equals(x)) || ('e'.Equals(x)))
                    && (!(test.Contains("E")) || !(test.Contains("e")))
                        && (test.Contains("."))
                )
            {
                return true;
            }
            else if (('+'.Equals(x)) || '-'.Equals(x)) // && ('E'.Equals(test[test.Length - 1])) || ('e'.Equals(test[test.Length - 1])))
            {
                return true;
            }
            return false;
        }




        public static bool OtherTokenFirst(char x)
        { //
            if (('/'.Equals(x)) || ('+'.Equals(x)) || ('*'.Equals(x)) || ('-'.Equals(x)) || (')'.Equals(x)) || (';'.Equals(x)) || (','.Equals(x)) || ('['.Equals(x)) || (']'.Equals(x)) || ('.'.Equals(x)))
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
            if ((':'.Equals(x)) || ('<'.Equals(x)) || ('('.Equals(x)) || ('>'.Equals(x)) || ('='.Equals(x))) // || ('*'.Equals(x))
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
            if (('>'.Equals(x)) || ('='.Equals(x)) || ('('.Equals(x)))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool LineEnd(char x)
        {
            if ('\n'.Equals(x))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool Formatting(char x)
        {
            if ('\r'.Equals(x))
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
            //identifier = false;
            //numeric = false;
            // tokenComplete = false;
            stringConstant = false;
            comment1 = false;
            comment2 = false;
            other1 = false;
            other2 = false;
            unidentified = false;
            tokenTooLong = false;
            stringComplete = true;
            commentComplete = false;
        }
    }
}




