using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS4100_Code_Generator

{
    class Interpreter
    {

        public static int PC;
        public static int OpCode, Op1, Op2, Op3 = 0;
        public static int MaxQuad = 100;
        public static bool TraceOn;

        public Interpreter() { }

        public static void IntrepretQuads(QuadTable QuadTable, SymbolTable SymbolTable, bool traceOn)
        {
            while (PC < MaxQuad)
            {
                // this could be compressed/refactored somehow?

                OpCode = QuadTable.GetQuad(PC).OpCode;
                Op1 = QuadTable.GetQuad(PC).Op1;
                Op2 = QuadTable.GetQuad(PC).Op2;
                Op3 = QuadTable.GetQuad(PC).Op3;

                if (OpCode <= 16)

                    //
                    if ((traceOn) & (PC != MaxQuad))
                    {
                        Console.WriteLine("PC = " + PC + ": " + QuadTable.GetMnemonic(OpCode) + ", " + Op1 + ", " + Op2 + ", " + Op3);
                    }

                switch (OpCode)
                {
                    case 0: // stop
                        Console.WriteLine("Execution terminated by program stop");
                        Console.WriteLine();
                        PC = MaxQuad;
                        break;

                    case 1: // div
                        SymbolTable.SymbolTableArray[Op3].Value = (int)(SymbolTable.SymbolTableArray[Op1].Value) / (int)(SymbolTable.SymbolTableArray[Op2].Value);
                        PC++;
                        break;

                    case 2: // mul
                        SymbolTable.SymbolTableArray[Op3].Value = (int)(SymbolTable.SymbolTableArray[Op1].Value) * (int)(SymbolTable.SymbolTableArray[Op2].Value);
                        PC++;
                        break;

                    case 3: // sub
                        SymbolTable.SymbolTableArray[Op3].Value = (int)(SymbolTable.SymbolTableArray[Op1].Value) - (int)(SymbolTable.SymbolTableArray[Op2].Value);
                        PC++;
                        break;

                    case 4: // sub
                        SymbolTable.SymbolTableArray[Op3].Value = (int)(SymbolTable.SymbolTableArray[Op1].Value) + (int)(SymbolTable.SymbolTableArray[Op2].Value);
                        PC++;
                        break;

                    case 5: // mov
                        SymbolTable.SymbolTableArray[Op3].Value = (int)(SymbolTable.SymbolTableArray[Op1].Value);
                        PC++;
                        break;

                    case 6: // sti
                            // is this the correct way to offset 2 with 3
                        SymbolTable.SymbolTableArray[Op2 + (int)(SymbolTable.SymbolTableArray[Op3].Value)].Value = (int)(SymbolTable.SymbolTableArray[Op2].Value);
                        PC++;
                        break;

                    case 7: // ldi
                            // is this the correct way to offset 1 with 2
                        SymbolTable.SymbolTableArray[Op3].Value = (int)(SymbolTable.SymbolTableArray[Op1 + (int)(SymbolTable.SymbolTableArray[Op2].Value)].Value);
                        PC++;
                        break;


                    case 8: // bnz
                        if ((int)SymbolTable.SymbolTableArray[Op1].Value != 0)
                        {
                            PC = QuadTable.GetQuad(PC).Op3;
                        }
                        else
                        {
                            PC++;
                        }
                        break;

                    case 9: // bnp
                        if ((int)SymbolTable.SymbolTableArray[Op1].Value <= 0)
                        {
                            PC = QuadTable.GetQuad(PC).Op3;
                        }
                        else
                        {
                            PC++;
                        }
                        break;

                    case 10: // bnn
                        if ((int)SymbolTable.SymbolTableArray[Op1].Value >= 0)
                        {
                            PC = QuadTable.GetQuad(PC).Op3;
                        }
                        else
                        {
                            PC++;
                        }
                        break;

                    case 11: // bz
                        if ((int)SymbolTable.SymbolTableArray[Op1].Value == 0)
                        {
                            PC = QuadTable.GetQuad(PC).Op3;
                        }
                        else
                        {
                            PC++;
                        }
                        break;

                    case 12: // bp
                        if ((int)SymbolTable.SymbolTableArray[Op1].Value > 0)
                        {
                            PC = QuadTable.GetQuad(PC).Op3;
                        }
                        else
                        {
                            PC++;
                        }
                        break;

                    case 13: // bn
                        if ((int)SymbolTable.SymbolTableArray[Op1].Value < 0)
                        {
                            PC = QuadTable.GetQuad(PC).Op3;
                        }
                        else
                        {
                            PC++;
                        }
                        break;

                    case 14: // br
                        PC = QuadTable.GetQuad(PC).Op3;
                        break;

                    case 15: // bindr
                        PC = (int)SymbolTable.SymbolTableArray[Op3].Value;
                        break;

                    case 16: // print
                        Console.WriteLine((string)SymbolTable.SymbolTableArray[Op1].Name + "\t" + (int)SymbolTable.SymbolTableArray[Op1].Value);
                        PC++;
                        break;
                }
            }

        }
    }
}
