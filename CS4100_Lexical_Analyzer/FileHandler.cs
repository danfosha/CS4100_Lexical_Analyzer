using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS4100_Lexical_Analyzer
{
    public class FileHandler
    {
        public static string FileText
        {
            get; set;
        }

        public static string fileName
        {
            get; set;
        }

        public static string GetFileName()
        {
            
            //Console.Write("Enter a filename: ");
            // filename = Convert.ToString(Console.ReadLine());
            string filename = "lexical_test.txt";
            return filename;
            
        }

        public static void InitializeInputFile(string fileName)
        {
            try
            {   // Open the text file using a stream reader.
                using (StreamReader sr = new StreamReader(fileName))
                {
                    // Read the stream to a string, and write the string to the console.
                    String text = sr.ReadToEnd();
                    Console.WriteLine(text);
                    Console.ReadLine();
                    FileText = text;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
                Console.ReadLine();
                
            }
        }
    }

    
}

