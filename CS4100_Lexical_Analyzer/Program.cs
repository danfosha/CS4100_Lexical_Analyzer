using System;
using System.Collections.Generic;
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
            InitializeInputFile(GetFileName);
            while (!EOF)
            {
                GetNextToken(echoOn);
                PrintToken(nextToken, tokenCode);
            }
            SymbolTable.Print();
            Terminate;
            
        }

        public void InitializeStructures()
        {
            int MaxQuad = 100;            
            SymbolTable Symbols1 = new SymbolTable(MaxQuad);
        }

    }

  
}
