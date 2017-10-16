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
        public static char nextChar;
        public static int caseGroup = -1;
        public static int tokenCode = -1;

        public static bool tokenizerFinished = false;
        public static bool tokenComplete = false;
        public static bool tokenTooLong = false;
        public static bool stringComplete = true;
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
                        tokenComplete = true;
                        lineComplete = true;
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
                        caseGroup = 1;
                    }
                    else if (Char.IsDigit(nextChar))
                    {
                        caseGroup = 2;
                    }
                    else if ('"'.Equals(nextChar))
                    {
                        caseGroup = 3;
                    }

                    else if ('#'.Equals(nextChar))
                    {
                        caseGroup = 4;
                    }
                    else if (OtherTokenFirst(nextChar))
                    {
                        caseGroup = 5;
                    }
                    else if (OtherTokenSecond(nextChar))
                    {
                        caseGroup = 6;
                    }
                    else
                    {
                        caseGroup = 0;
                    }


                    switch (caseGroup)
                    {
                        case 0:
                            workingToken.Append(nextChar);
                            nextChar = GetNextChar();
                            tokenComplete = true;                            
                            // return unidentified
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
                                    nextChar = GetNextChar();
                                    tokenTooLong = true;
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
                            }
                            tokenComplete = true;
                            break;

                        case 4:
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
                        case 5:
                            // other symbol 1 / + -  ) ; , [ ] .
                            workingToken.Append(nextChar);
                            tokenComplete = true;
                            nextChar = GetNextChar();
                            break;
                        case 6:
                            // other symbol 2 : < (
                            workingToken.Append(nextChar);
                            nextChar = GetNextChar();
                            if ((':'.Equals(workingToken[0])) && ('='.Equals(nextChar)))
                            {
                                workingToken.Append(nextChar);
                                tokenComplete = true;
                                nextChar = GetNextChar();
                            }
                            else if (('<'.Equals(workingToken[0])) && (('='.Equals(nextChar)) || ('>'.Equals(nextChar))))
                            {
                                workingToken.Append(nextChar);
                                tokenComplete = true;
                                nextChar = GetNextChar();
                            }

                            else if (('>'.Equals(workingToken[0])) && ('='.Equals(nextChar)))
                            {
                                workingToken.Append(nextChar);
                                tokenComplete = true;
                                nextChar = GetNextChar();

                            }
                            else if (('('.Equals(workingToken[0])) && (('*'.Equals(nextChar))))
                            {
                                workingToken.Append(nextChar);
                                nextChar = GetNextChar();
                                while (!('*'.Equals(nextChar)))
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
                                while (nextChar != ')')
                                {
                                    workingToken.Append(nextChar);
                                    nextChar = GetNextChar();
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
                                }
                                workingToken.Append(nextChar);
                                nextChar = GetNextChar();
                                tokenComplete = true;
                                break;
                            }
                            else
                            {
                                // workingToken.Append(nextChar);
                                // nextChar = GetNextChar();
                                tokenComplete = true;
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

                    nextToken = workingToken.ToString();
                    workingToken.Clear();
                    tokenTooLong = false;
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
            if ((':'.Equals(x)) || ('<'.Equals(x)) || ('('.Equals(x)) || ('>'.Equals(x)) || ('*'.Equals(x)) || (('>'.Equals(x)) || ('='.Equals(x))))
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
    }
}




