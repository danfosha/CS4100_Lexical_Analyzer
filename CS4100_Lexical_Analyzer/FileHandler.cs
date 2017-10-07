using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// CS4100 Fall 2017 Lexical Analyzer Project - Fosha
namespace CS4100_Lexical_Analyzer
{
    class FileHandler
    {
        
        public FileHandler(string filename = "")
        {
            FileName = filename;
        }
        public static string FileName { get; set; }
              

        public static string FileText
        {
            get; set;
        }

        public string GetFileName()
        {

            //Console.Write("Enter a filename: ");
            // filename = Convert.ToString(Console.ReadLine());
            string filename = "lexical_test.txt";
            return filename;

        }

        public static string InitializeInputFile(string fileName)
        {
            try
            {   // Open the text file using a stream reader.
                using (StreamReader sr = new StreamReader(fileName))
                {
                    // Read the stream to a string, and write the string to the console.
                    string text = sr.ReadToEnd();
                    Console.WriteLine(text);
                    Console.ReadLine();
                    
                    FileText = text;
                    return FileText;
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

