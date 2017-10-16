using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS4100_Lexical_Analyzer
{
    class ReserveWordClass
    {
        protected int codeUsed = 0;
        protected int numUsed = 0;

        public class ReserveWords
        {
            public ReserveWords(int code, string word)
            {
                Word = word;
                Code = code;
            }

            public string Word { get; set; }
            public int Code { get; set; }

        }

        public static ReserveWords[] ReserveWordTable = new ReserveWords[44];

        public void Initialize()
        // Constructor
        {

            Add("GOTO");
            Add("INTEGER");
            Add("TO");
            Add("DO");
            Add("IF");
            Add("THEN");
            Add("ELSE");
            Add("FOR");
            Add("OF");
            Add("WRITELN");
            Add("BEGIN");
            Add("END");
            Add("ARRAY");
            Add("VAR");
            Add("WHILE");
            Add("UNIT");
            Add("LABEL");
            Add("REPEAT");
            Add("UNTIL");
            Add("PROCEDURE");
            Add("DOWNTO");
            Add("READLN");
            Add("RETURN");
            Add("FLOAT");
            Add("STRING");

            codeUsed = 30;

            Add("/");
            Add("*");
            Add("+");
            Add("-");
            Add("(");
            Add(")");
            Add(";");
            Add(":=");
            Add(">");
            Add("<");
            Add(">=");
            Add("<=");
            Add("=");
            Add("<>");
            Add(",");
            Add("[");
            Add("]");
            Add(":");
            Add(".");

        }

        public void Add(string word)
        // Returns the index of the row where the data was placed; just adds to end of list.
        {
            ReserveWordTable[numUsed] = new ReserveWords(codeUsed, word);
            numUsed++;
            codeUsed++;
        }


        public static int LookupCode(string word)
        // Returns the code associated with name if name is in the table, else returns -1
        {
            foreach (ReserveWords Data in ReserveWordTable)
            {
                //if (Data.Word != null)
                //{
                    if (Data.Word.Equals(word))
                    {
                        return Data.Code;
                    }
                //}
            }
            return -1;
        }


        public static string LookupWord(int code)
        // Returns the associated name if code is there, else an empty string
        {
            foreach (ReserveWords Data in ReserveWordTable)
            {

                if (Data.Code == code)
                {
                    return Data.Word;
                }
            }
            return "";
        }

    }

}
