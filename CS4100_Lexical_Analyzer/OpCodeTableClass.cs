using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// CS4100 Fall 2017 Code Generator Project - Fosha
namespace CS4100_Code_Generator
{
    public class OpCodeTableClass
    {       

        protected int codeUsed = 0;
        protected int numUsed = 0;

        public class OpCodes
        {
            public OpCodes(string name, int code)
            {
                Name = name;
                Code = code;
            }

            public string Name { get; set; }
            public int Code { get; set; }

        }

        public static OpCodes[] OpCodeTable = new OpCodes[17];


        public void InitializeOpCodes()
        // Constructor, as needed.
        {

            OpCodeTable[0] = new OpCodes("STOP", 0);
            OpCodeTable[1] = new OpCodes("DIV", 1);
            OpCodeTable[2] = new OpCodes("MUL", 2);
            OpCodeTable[3] = new OpCodes("SUB", 3);
            OpCodeTable[4] = new OpCodes("ADD", 4);
            OpCodeTable[5] = new OpCodes("MOV", 5);
            OpCodeTable[6] = new OpCodes("STI", 6);
            OpCodeTable[7] = new OpCodes("LDI", 7);
            OpCodeTable[8] = new OpCodes("BNZ", 8);
            OpCodeTable[9] = new OpCodes("BNP", 9);
            OpCodeTable[10] = new OpCodes("BNN", 10);
            OpCodeTable[11] = new OpCodes("BZ", 11);
            OpCodeTable[12] = new OpCodes("BP", 12);
            OpCodeTable[13] = new OpCodes("BN", 13);
            OpCodeTable[14] = new OpCodes("BR", 14);
            OpCodeTable[15] = new OpCodes("BINDR", 15);
            OpCodeTable[16] = new OpCodes("PRINT", 16);



        }

        public int Add(string name, int code)
        // Returns the index of the row where the data was placed; just adds to end of list.
        {
            int notUsed = 0;
            
            foreach (OpCodes Data in OpCodeTable)
            {
                // check this conditional
                if (OpCodeTable[notUsed] == null)
                {
                    OpCodeTable[notUsed] = new OpCodes(name, code);
                    
                }
            }
            notUsed++;
            return notUsed;
        }


        public static int LookupName(string name)
        // Returns the code associated with name if name is in the table, else returns -1
        {
            foreach (OpCodes Data in OpCodeTable)
            {
                //if (Data.Word != null)
                //{
                if (Data.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
                {
                    return Data.Code;
                }
                //}
            }
            return -1;
        }


        public static string LookupCode(int code)
        // Returns the associated name if code is there, else an empty string
        {
            foreach (OpCodes Data in OpCodeTable)
            {

                if (Data.Code == code)
                {
                    return Data.Name;
                }
            }
            return "";
        }

        public static void PrintOpCodeTable()
        // Prints the currently used contents of the Reserve table in neat tabular format
        {
            Console.WriteLine("Name" + "\t" + "Code");
            Console.WriteLine("***************************");
            foreach (OpCodes Data in OpCodeTable)
            {
                Console.WriteLine(Data.Name + "\t" + Data.Code);
            }
        }

    }

}


