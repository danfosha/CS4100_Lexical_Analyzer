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
            string nextToken = "";

            InitializeStructures();
            string fileName = GetFileName();            
            string fileText = InitializeInputFile(fileName);
            int stringLength = fileText.Length;
            
            
            string tempToken;
            int tokenCode= -1;
            int charIndex = 0;
            bool echoOn = true;

            while (stringLength >0)
            {
                tempToken = (Tokenizer.GetNextToken(echoOn, fileText[charIndex]));
                if (tempToken.Length > 0)
                {
                    nextToken = tempToken;
                    Console.Write(tempToken);
                    Console.Write(nextToken);
                    // do all the post processing here
                }
                //PrintToken(nextToken, tokenCode);
               
                charIndex++;
                stringLength--;
            }
            //SymbolTable.Print();
            //Terminate;
            Console.ReadLine();

        }

        public static void InitializeStructures()
        {
            int MaxQuad = 100;            
            SymbolClass SymbolTable = new SymbolClass(MaxQuad);
        }

        public static string GetFileName()
        {
            string filename;
            //Console.Write("Enter a filename: ");
            // filename = Convert.ToString(Console.ReadLine());
            filename = "lexical_test.txt";
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
                    Console.WriteLine(text);
                    Console.ReadLine();
                    return text;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
                Console.ReadLine();
                return "";
            }
        }

        

    }

  
}
