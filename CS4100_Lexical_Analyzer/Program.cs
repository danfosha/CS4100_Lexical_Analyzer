using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS4100_Lexical_Analyzer
{
    class Program
    {
        static void Main(string[] args)
        {
            InitializeStructures();
            string fileName = GetFileName();            
            string fileText = InitializeInputFile(fileName);
            
            string nextToken = "";
            int tokenCode= -1;
            bool echoOn = true;

            while (tokenCode != 100)
            {
                nextToken = GetNextToken(echoOn,fileText);
                //PrintToken(nextToken, tokenCode);                
            }
            //SymbolTable.Print();
            //Terminate;
            
        }

        public static void InitializeStructures()
        {
            int MaxQuad = 100;            
            SymbolClass SymbolTable = new SymbolClass(MaxQuad);
        }

        public static string GetFileName()
        {
            string filename;
            Console.Write("Enter a filename: ");
            filename = Convert.ToString(Console.ReadLine());
            return filename;
        }
        
        public static string InitializeInputFile(string fileName)
        {
            try
            {   // Open the text file using a stream reader.
                using (StreamReader sr = new StreamReader(fileName))
                {
                    // Read the stream to a string, and write the string to the console.
                    String text = sr.ReadToEnd();
                    // Console.WriteLine(text);
                    return text;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
                return "";
            }
        }

        public static string GetNextToken(bool echoOn, string fileText)
        {
            int textLength = fileText.Length;
            int caseGroup = -1;
            
            while (fileText.Length > 0)
            {
                char x = fileText[fileText.Length - textLength];
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


                textLength--;

            }

            return "";
        }

    }

  
}
