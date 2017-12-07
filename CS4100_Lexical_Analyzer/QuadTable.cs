using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS4100_Code_Generator
{
    class QuadTable
    {
        public static int numUsed = -1;

        public class QuadStruct
        {

            public QuadStruct(int opcode, int op1, int op2, int op3)
            {

                OpCode = opcode;
                Op1 = op1;
                Op2 = op2;
                Op3 = op3;
            }

            public int OpCode { get; set; }
            public int Op1 { get; set; }
            public int Op2 { get; set; }
            public int Op3 { get; set; }

        }

        public static QuadStruct[] QuadTableArray = new QuadStruct[100];
        // The QuadTable is different from the SymbolTable in its access and contents.Each indexed entry row
        //  consists of four int values representing an opcode and three operands.  The methods needed are:

        public static void Initialize()  // size and other parameters as needed
        // Create a new, empty QuadTable ready for data to be added, with the specified number of rows(size).
        {
            QuadStruct[] QuadTableArray = new QuadStruct[100];

        }

        public static int NextQuad()
        // Returns the int index of the next open slot in the QuadTable. Very important during code generation, this must be implemented exactly as described.
        {
            return numUsed + 1;
        }


        public static void AddQuad(int opcode, int op1, int op2, int op3)
        // Expands the active length of the quad table by adding a new row at the NextQuad slot, with the parameters sent as the new contents, 
        // and increments the NextQuad counter to the next available(empty) index.}
        {

            QuadTableArray[NextQuad()] = new QuadStruct(opcode, op1, op2, op3);
            numUsed++;

        }

        public static QuadStruct GetQuad(int index)
        // Returns the data for the opcode and three operands located at index
        {
            return QuadTableArray[index];
        }

        public static void SetQuad(int index, int opcode, int op1, int op2, int op3)
        // Changes the contents of the existing quad at index. Used only when backfilling
        // jump addresses later, during code generation, and very important
        {
            QuadTableArray[index].OpCode = opcode;
            QuadTableArray[index].Op1 = op1;
            QuadTableArray[index].Op2 = op2;
            QuadTableArray[index].Op3 = op3;
        }

        public static void SetQuadOp3(int index, int op3)
        // Changes the contents of the existing quad at index. Used only when backfilling
        // jump addresses later, during code generation, and very important
        {
            QuadTableArray[index].Op3 = op3;
        }

        public static string GetMnemonic(int opcode)
        // Returns the mnemonic string (‘ADD’, ‘PRINT’, etc.) associated with the opcode parameter. Used during interpreter
        // ‘TRACE’ mode to print out the stored opcodes in readable format. Use the ReserveTable ADT to implement this.
        {
            string code = OpCodeTableClass.LookupCode(opcode);
            return code;
        }

        public static void PrintQuadTable()
        //Prints the currently used contents of the Quad table in neat tabular format
        {
            Console.WriteLine("******************     QUADTABLE     ******************\n");
            Console.WriteLine("OpCode".PadRight(10)  + "Op1".PadRight(30)  + "Op2".PadRight(10) + "Op3".PadRight(10));
            Console.WriteLine("*************************************************************");
            foreach (QuadStruct Quad in QuadTableArray)
            {
                if (Quad != null)
                {
                    switch (Quad.OpCode)
                    {

                        case SyntaxAndCodeGen.STOP_OP: //0
                            Console.WriteLine(GetMnemonic(Quad.OpCode).PadRight(10)  + "-----".PadRight(30) + "-----".PadRight(10) + "-----");
                            break;
                        case SyntaxAndCodeGen.DIV_OP: //1
                            Console.WriteLine(GetMnemonic(Quad.OpCode).PadRight(10)  + (SymbolTable.GetSymbol(Quad.Op1).Name + " <" + Quad.Op1 + ">").PadRight(30) +  (SymbolTable.GetSymbol(Quad.Op2).Name + " <" + Quad.Op2 + ">").PadRight(10) + SymbolTable.GetSymbol(Quad.Op3).Name + " <" + Quad.Op3 + ">");
                            break;
                        case SyntaxAndCodeGen.MUL_OP: //2
                            Console.WriteLine(GetMnemonic(Quad.OpCode).PadRight(10)  + (SymbolTable.GetSymbol(Quad.Op1).Name + " <" + Quad.Op1 + ">").PadRight(30)  + (SymbolTable.GetSymbol(Quad.Op2).Name + " <" + Quad.Op2 + ">").PadRight(10) + SymbolTable.GetSymbol(Quad.Op3).Name + " <" + Quad.Op3 + ">");
                            break;
                        case SyntaxAndCodeGen.SUB_OP: //3
                            Console.WriteLine(GetMnemonic(Quad.OpCode).PadRight(10)  + (SymbolTable.GetSymbol(Quad.Op1).Name + " <" + Quad.Op1 + ">").PadRight(30)  + (SymbolTable.GetSymbol(Quad.Op2).Name + " <" + Quad.Op2 + ">").PadRight(10) + SymbolTable.GetSymbol(Quad.Op3).Name + " <" + Quad.Op3 + ">");
                            break;
                        case SyntaxAndCodeGen.ADD_OP: //4
                            Console.WriteLine(GetMnemonic(Quad.OpCode).PadRight(10)  + (SymbolTable.GetSymbol(Quad.Op1).Name + " <" + Quad.Op1 + ">").PadRight(30)  + (SymbolTable.GetSymbol(Quad.Op2).Name + " <" + Quad.Op2 + ">").PadRight(10) + SymbolTable.GetSymbol(Quad.Op3).Name + " <" + Quad.Op3 + ">");
                            break;
                        case SyntaxAndCodeGen.MOV_OP: //5
                            Console.WriteLine(GetMnemonic(Quad.OpCode).PadRight(10)  + (SymbolTable.GetSymbol(Quad.Op1).Name + " <" + Quad.Op1 + ">").PadRight(30)  + "-----".PadRight(10) + SymbolTable.GetSymbol(Quad.Op3).Name + " <" + Quad.Op3 + ">");
                            break;
                        case SyntaxAndCodeGen.STI_OP: //6
                            Console.WriteLine(GetMnemonic(Quad.OpCode).PadRight(10)  + SymbolTable.GetSymbol(Quad.Op1).Name.PadRight(30)+ SymbolTable.GetSymbol(Quad.Op2).Name.PadRight(10) + "-----");
                            break;
                        case SyntaxAndCodeGen.LDI_OP: //7
                            Console.WriteLine(GetMnemonic(Quad.OpCode).PadRight(10)  + SymbolTable.GetSymbol(Quad.Op1).Name.PadRight(30) + "-----".PadRight(10) + SymbolTable.GetSymbol(Quad.Op3).Name);
                            break;
                        case SyntaxAndCodeGen.BNZ_OP: //8
                            Console.WriteLine(GetMnemonic(Quad.OpCode).PadRight(10)  + (SymbolTable.GetSymbol(Quad.Op1).Name + " <" + Quad.Op1 + ">").PadRight(30) + "-----".PadRight(10) + "PC: " + Quad.Op3);
                            break;
                        case SyntaxAndCodeGen.BNP_OP: //9
                            Console.WriteLine(GetMnemonic(Quad.OpCode).PadRight(10)  + (SymbolTable.GetSymbol(Quad.Op1).Name + " <" + Quad.Op1 + ">").PadRight(30) + "-----".PadRight(10) + "PC: " + Quad.Op3);
                            break;
                        case SyntaxAndCodeGen.BNN_OP: //10
                            Console.WriteLine(GetMnemonic(Quad.OpCode).PadRight(10)  + (SymbolTable.GetSymbol(Quad.Op1).Name + " <" + Quad.Op1 + ">").PadRight(30) + "-----".PadRight(10) + "PC: " + Quad.Op3);
                            break;
                        case SyntaxAndCodeGen.BZ_OP: //11
                            Console.WriteLine(GetMnemonic(Quad.OpCode).PadRight(10)  + (SymbolTable.GetSymbol(Quad.Op1).Name + " <" + Quad.Op1 + ">").PadRight(30) + "-----".PadRight(10) + "PC: " + Quad.Op3);
                            break;
                        case SyntaxAndCodeGen.BP_OP: //12
                            Console.WriteLine(GetMnemonic(Quad.OpCode).PadRight(10)  + (SymbolTable.GetSymbol(Quad.Op1).Name + " <" + Quad.Op1 + ">").PadRight(30) + "-----".PadRight(10) + "PC: " + Quad.Op3);
                            break;
                        case SyntaxAndCodeGen.BN_OP: // 13
                            Console.WriteLine(GetMnemonic(Quad.OpCode).PadRight(10)  + (SymbolTable.GetSymbol(Quad.Op1).Name + " <" + Quad.Op1 + ">").PadRight(30) + "-----".PadRight(10) + "PC: " + Quad.Op3);
                            break;
                        case SyntaxAndCodeGen.BR_OP: //14
                            Console.WriteLine(GetMnemonic(Quad.OpCode).PadRight(10)  + "-----".PadRight(30) + "-----".PadRight(10) + "PC: " + Quad.Op3);
                            break;
                        case SyntaxAndCodeGen.BINDR_OP: //15
                            Console.WriteLine(GetMnemonic(Quad.OpCode).PadRight(10)  + "-----".PadRight(30) + "-----".PadRight(10) + "PC: " + Quad.Op3);
                            break;
                        case SyntaxAndCodeGen.PRINT_OP: //16
                            Console.WriteLine(GetMnemonic(Quad.OpCode).PadRight(10)  + (SymbolTable.GetSymbol(Quad.Op1).Name + " <" + Quad.Op1 + ">").PadRight(30)  + "-----".PadRight(10)  + "-----".PadRight(10));
                            break;
                        default:
                            break;
                    }
                }
            }
        }
    }
}
