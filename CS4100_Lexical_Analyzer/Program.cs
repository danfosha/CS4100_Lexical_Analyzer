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
            InitializeInputFile(fileName);
            while (!EOF)
            {
                GetNextToken(echoOn);
                PrintToken(nextToken, tokenCode);
            }
            SymbolTable.Print();
            Terminate;
            
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
        
        public static void InitializeInputFile(string fileName)
        {
            try
            {   // Open the text file using a stream reader.
                using (StreamReader sr = new StreamReader(fileName))
                {
                    // Read the stream to a string, and write the string to the console.
                    String line = sr.ReadToEnd();
                    Console.WriteLine(line);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
            }
        }

    }

  
}
