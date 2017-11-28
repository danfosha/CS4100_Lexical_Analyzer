using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// CS4100 Fall 2017 Lexical Analyzer Project - Fosha
namespace CS4100_Code_Generator
{
    class ReserveWordClass
    {
        protected int codeUsed = 0;
        protected int numUsed = 0;

        public class ReserveWords
        {
            public ReserveWords(int code, string word, string menm)
            {
                Word = word;
                Code = code;
                Mnemonic = menm;
            }

            public string Word { get; set; }
            public int Code { get; set; }
            public string Mnemonic { get; set; }

        }

        public static ReserveWords[] ReserveWordTable = new ReserveWords[49];

        public void Initialize()
        // Constructor
        {

            Add("GOTO","GOTO");
            Add("INTEGER","INTG");
            Add("TO","TO__");
            Add("DO","DO__");
            Add("IF","IF__");
            Add("THEN","THEN");
            Add("ELSE", "ELSE");
            Add("FOR","FOR_");
            Add("OF","OF__");
            Add("WRITELN","WRLN");
            Add("BEGIN","BEGI");
            Add("END","END_");
            Add("ARRAY","ARRY");
            Add("VAR","VAR_");
            Add("WHILE","WHIL");
            Add("UNIT","UNIT");
            Add("LABEL","LABL");
            Add("REPEAT","RPET");
            Add("UNTIL","UNTL");
            Add("PROCEDURE","PROC");
            Add("DOWNTO","DOWN");
            Add("READLN","RDLN");
            Add("RETURN","RTRN");
            Add("FLOAT","FLOT");
            Add("STRING","STRG");

            codeUsed = 30;

            Add("/","DIVI");
            Add("*","MULT");
            Add("+","PLUS");
            Add("-","MINU");
            Add("(","LPAR");
            Add(")","RPAR");
            Add(";","SEMI");
            Add(":=","ASSN");
            Add(">","GRTR");
            Add("<","LSSR");
            Add(">=","GREQ");
            Add("<=","LSEQ");
            Add("=","EQUA");
            Add("<>","NTEQ");
            Add(",","COMM");
            Add("[","LBRA");
            Add("]","RBRA");
            Add(":","COLN");
            Add(".","PERD");
            Add("", "NULL");
            Add("IDENTIFIER", "IDEN");
            Add("INTEGER", "INT");
            Add("FLOAT", "FLOA");
            Add("STRING", "STRN");



        }

        public void Add(string word, string menm)
        // Returns the index of the row where the data was placed; just adds to end of list.
        {
            ReserveWordTable[numUsed] = new ReserveWords(codeUsed, word, menm);
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
                    if (Data.Word.Equals(word, StringComparison.OrdinalIgnoreCase))
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

        public static string LookupMnem(int code)
        // Returns the associated name if code is there, else an empty string
        {
            foreach (ReserveWords Data in ReserveWordTable)
            {

                if (Data.Code == code)
                {
                    return Data.Mnemonic;
                }
            }
            return "";
        }

    }

}
