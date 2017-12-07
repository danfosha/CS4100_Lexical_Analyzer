using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS4100_Code_Generator
{
    class SyntaxAndCodeGen
    {
        public SyntaxAndCodeGen()
        {
        }

        public static bool echoOn = true;
        public static bool trace = false;
        public static bool verbose = false;
        public static bool error = false;
        public static int paddingIndent = 0;
        public static bool uniqueProgIdent = false;
        public static string ProgIdent;
        public static bool declaration_section = true;
        public static bool declare_label = false;
        public static bool declare_var = false;

        public const int GOTO = 0;
        public const int INTEGER = 1;
        public const int TO = 2;
        public const int DO = 3;
        public const int IF = 4;
        public const int THEN = 5;
        public const int ELSE = 6;
        public const int FOR = 7;
        public const int OF = 8;
        public const int WRITELN = 9;
        public const int BEGIN = 10;
        public const int END = 11;
        public const int ARRAY = 12;
        public const int VAR = 13;
        public const int WHILE = 14;
        public const int UNIT = 15;
        public const int LABEL = 16;
        public const int REPEAT = 17;
        public const int UNTIL = 18;
        public const int PROCEDURE = 19;
        public const int DOWNTO = 20;
        public const int READLN = 21;
        public const int RETURN = 22;
        public const int FLOAT = 23;
        public const int STRING = 24;
        public const int DIVI = 30;
        public const int MULT = 31;
        public const int PLUS = 32;
        public const int MINUS = 33;
        public const int LPAREN = 34;
        public const int RPAREN = 35;
        public const int SEMI = 36;
        public const int ASSN = 37;
        public const int GRTR = 38;
        public const int LSSR = 39;
        public const int GREQ = 40;
        public const int LSEQ = 41;
        public const int EQUA = 42;
        public const int NTEQ = 43;
        public const int COMM = 44;
        public const int LBRA = 45;
        public const int RBRA = 46;
        public const int COLN = 47;
        public const int PERD = 48;
        public const int NULL = 99;
        public const int IDENTIFIER = 50;
        public const int INTEGERTYPE = 51;
        public const int FLOATTYPE = 52;
        public const int STRINGTYPE = 53;

        public const int STOP_OP = 0;
        public const int DIV_OP = 1;
        public const int MUL_OP = 2;
        public const int SUB_OP = 3;
        public const int ADD_OP = 4;
        public const int MOV_OP = 5;
        public const int STI_OP = 6;
        public const int LDI_OP = 7;
        public const int BNZ_OP = 8;
        public const int BNP_OP = 9;
        public const int BNN_OP = 10;
        public const int BZ_OP = 11;
        public const int BP_OP = 12;
        public const int BN_OP = 13;
        public const int BR_OP = 14;
        public const int BINDR_OP = 15;
        public const int PRINT_OP = 16;


        public static void Analyze(bool echo)
        {
            echoOn = echo;
            // skip -1, only go forward with valid tokencodes
            if ((TokenizerClass.tokenCode >= 0) && (TokenizerClass.tokenCode < 100))
            {
                program();
                if (!error)
                {
                    Console.WriteLine("No syntax errors found.");
                }
                SymbolTable.ValidateLabelUsed();
            }
        }

        // Methods
        public static int program()
        {
            if (!error)
            {
                Debug(true, "program");

                if (TokenizerClass.tokenCode == UNIT)// $UNIT
                {
                    GetNextToken(echoOn);
                    // need to advance tokencode
                    prog_identifier();
                    if (TokenizerClass.tokenCode == SEMI) // $;
                    {
                        GetNextToken(echoOn);
                        block();
                        if (TokenizerClass.tokenCode == PERD) // $.
                        {
                            QuadTable.AddQuad(STOP_OP, 0, 0, 0);
                            TokenizerClass.tokenizerFinished = true;
                            // will ignore rest of file
                        }
                        else
                        {

                            ErrorMessage(PERD, TokenizerClass.tokenCode);
                        }
                    }
                    else
                    {

                        ErrorMessage(SEMI, TokenizerClass.tokenCode);
                    }
                }
                else
                {

                    ErrorMessage(UNTIL, TokenizerClass.tokenCode);
                }
                Debug(false, "program");
            }
            return 0;
        }

        public static int prog_identifier()
        {
            if (!error)
            {
                Debug(true, "prog_identifier");
                identifier();
                Debug(false, "prog_identifier");
            }
            return 0;
        }

        public static int block()
        {
            if (!error)
            {
                Debug(true, "block");
                if (TokenizerClass.tokenCode == LABEL) // $LABEL
                {
                    label_declaration();
                }
                while ((TokenizerClass.tokenCode == VAR) && !error) // $VAR
                {
                    variable_dec_sec();
                }
                block_body();
                Debug(false, "block");

            }
            return 0;
        }

        public static int block_body()
        {
            if (!error)
            {
                Debug(true, "block_body");
                declaration_section = false; // declarations are complete
                if (TokenizerClass.tokenCode == BEGIN) // $BEGIN
                {
                    GetNextToken(echoOn);
                    statement();
                    while ((TokenizerClass.tokenCode == SEMI) && !error) // $;
                    {
                        GetNextToken(echoOn);
                        statement();
                    }
                    if (TokenizerClass.tokenCode == END) // $END
                    {
                        GetNextToken(echoOn);
                    }
                    else
                    {
                        ErrorMessage(END, TokenizerClass.tokenCode);
                    }
                }
                else
                {

                    ErrorMessage(BEGIN, TokenizerClass.tokenCode);
                }
                Debug(false, "block_body");
            }
            return 0;
        }

        public static int label_declaration()
        {
            if (!error)
            {
                Debug(true, "label_declaration");
                if (TokenizerClass.tokenCode == LABEL) // $LABEL
                {
                    declare_label = true;
                    GetNextToken(echoOn);
                    identifier();
                    while ((TokenizerClass.tokenCode == COMM) && !error)
                    {
                        declare_label = true;
                        GetNextToken(echoOn);
                        identifier();
                    }
                    if (TokenizerClass.tokenCode == SEMI)
                    {
                        GetNextToken(echoOn);
                    }
                    else
                    {
                        ErrorMessage(ASSN, TokenizerClass.tokenCode);
                    }
                }
                else
                {
                    ErrorMessage(LABEL, TokenizerClass.tokenCode);
                }
                Debug(false, "label_declaration");
            }
            return 0;
        }

        public static int variable_dec_sec()
        {
            if (!error)
            {
                Debug(true, "variable_dec_sec");
                if (TokenizerClass.tokenCode == VAR) // $VAR
                {
                    GetNextToken(echoOn);
                    variable_declaration();
                }
                else
                {
                    ErrorMessage(VAR, TokenizerClass.tokenCode);
                }
                Debug(false, "variable_dec_sec");
            }
            return 0;
        }

        public static int variable_declaration()
        {
            if (!error)
            {
                Debug(true, "variable_declaration");
                while ((TokenizerClass.tokenCode == IDENTIFIER) && !error) // $IDENT
                {
                    declare_var = true;
                    identifier();
                    while ((TokenizerClass.tokenCode == COMM) && !error) // $COMMA
                    {
                        declare_var = true;
                        GetNextToken(echoOn);
                        identifier();
                    }
                    if (TokenizerClass.tokenCode == COLN) // $COLON
                    {
                        GetNextToken(echoOn);
                        type();
                        if (TokenizerClass.tokenCode == SEMI) // $SEMI
                        {
                            GetNextToken(echoOn);
                        }
                        else
                        {
                            ErrorMessage(SEMI, TokenizerClass.tokenCode);
                        }
                    }
                    else
                    {
                        ErrorMessage(COLN, TokenizerClass.tokenCode);
                    }

                }
                Debug(false, "variable_declaration");
            }
            return 0;
        }

        public static int statement()
        {
            int left, right;

            if (!error)
            {
                int location = 0;
                Debug(true, "statement");
                if (TokenizerClass.tokenCode == IDENTIFIER)
                {

                    while (!error && (SymbolTable.LookupSymbol(TokenizerClass.nextToken)) >= 0 && (SymbolTable.GetSymbol(SymbolTable.LookupSymbol(TokenizerClass.nextToken)).Kind.ToString()) == "label")
                    {
                        location = label(); // should return nextQuad
                        if (TokenizerClass.tokenCode == COLN) // $:
                        {
                            GetNextToken(echoOn);
                        }
                        else
                        {
                            ErrorMessage(47, TokenizerClass.tokenCode);
                        }
                    }
                }

                int branchTarget, branchQuad, saveTop, patchElse;

                // must be one and only one of the below
                switch (TokenizerClass.tokenCode)
                {

                    case IDENTIFIER: // variable  - check for enum type here? does it matter if variable or constant?

                        left = variable();
                        if (TokenizerClass.tokenCode == ASSN) // $assign
                        {
                            GetNextToken(echoOn);
                            if (TokenizerClass.tokenCode == STRINGTYPE) // $string literal?
                            {
                                right = stringconst();
                            }
                            else
                            {
                                right = simple_expression();
                                QuadTable.AddQuad(OpCodeTableClass.LookupName("MOV"), right, 0, left);
                            }

                        }
                        else
                        {
                            ErrorMessage(ASSN, TokenizerClass.tokenCode);
                        }

                        break;
                    case BEGIN: // $begin
                        block_body();
                        break;
                    case IF: // $if
                        GetNextToken(echoOn);
                        branchQuad = relexpression();
                        if (TokenizerClass.tokenCode == THEN) // $then
                        {
                            GetNextToken(echoOn);
                            statement();
                            if (TokenizerClass.tokenCode == ELSE) // $else
                            {
                                GetNextToken(echoOn);
                                patchElse = QuadTable.NextQuad();
                                QuadTable.AddQuad(BR_OP, 0, 0, 0);
                                QuadTable.SetQuadOp3(branchQuad, QuadTable.NextQuad());
                                statement();
                                QuadTable.SetQuadOp3(patchElse, QuadTable.NextQuad());
                            }
                            else
                            {
                                QuadTable.SetQuadOp3(branchQuad, QuadTable.NextQuad());
                            }
                        }
                        else
                        {
                            ErrorMessage(5, TokenizerClass.tokenCode);
                        }
                        break;
                    case WHILE: // $while
                        GetNextToken(echoOn);
                        saveTop = QuadTable.NextQuad();
                        branchQuad = relexpression();
                        if (TokenizerClass.tokenCode == DO) // $do
                        {
                            GetNextToken(echoOn);
                            statement();
                            QuadTable.AddQuad(BR_OP, 0, 0, saveTop);
                            QuadTable.SetQuadOp3(branchQuad, QuadTable.NextQuad());
                        }
                        else
                        {
                            ErrorMessage(3, TokenizerClass.tokenCode);
                        }
                        break;
                    case REPEAT: // $repeat
                        GetNextToken(echoOn);
                        branchTarget = QuadTable.NextQuad();
                        statement();
                        if (TokenizerClass.tokenCode == UNTIL) // $until
                        {
                            GetNextToken(echoOn);
                            branchQuad = relexpression();
                            QuadTable.SetQuadOp3(branchQuad, branchTarget);
                        }
                        else
                        {
                            ErrorMessage(18, TokenizerClass.tokenCode);
                        }
                        break;
                    case FOR: // $FOR

                        int branchTarg = 0, branchQd = 0, temp = 0, topBound = 0, temp2 = 0;
                        GetNextToken(echoOn);
                        left = variable();
                        if (TokenizerClass.tokenCode == ASSN) // $ASSIGN
                        {
                            GetNextToken(echoOn);
                            right = simple_expression();
                            QuadTable.AddQuad(OpCodeTableClass.LookupName("MOV"), right, 0, left);
                            saveTop = QuadTable.NextQuad();
                            if (TokenizerClass.tokenCode == TO) // $TO
                            {
                                GetNextToken(echoOn);
                                topBound = simple_expression();
                                temp = SymbolTable.GenSymbol();
                                QuadTable.AddQuad(SUB_OP, left, topBound, temp);
                                if (TokenizerClass.tokenCode == DO) // $DO
                                {
                                    GetNextToken(echoOn);
                                    branchTarg = QuadTable.NextQuad();
                                    QuadTable.AddQuad(BP_OP, temp, 0, topBound);
                                    statement();
                                    temp2 = SymbolTable.GenSymbol();
                                    QuadTable.AddQuad(ADD_OP, left, right, temp2);
                                    QuadTable.AddQuad(MOV_OP, temp2, right, left);
                                    QuadTable.AddQuad(BR_OP, 0, 0, saveTop);
                                    QuadTable.SetQuadOp3(branchTarg, QuadTable.NextQuad());
                                }
                                else
                                {
                                    ErrorMessage(DO, TokenizerClass.tokenCode);

                                }
                            }
                            else
                            {
                                ErrorMessage(TO, TokenizerClass.tokenCode);

                            }
                        }
                        else
                        {
                            ErrorMessage(ASSN, TokenizerClass.tokenCode);

                        }

                        break;
                    case GOTO: // $GOTO
                        GetNextToken(echoOn);
                        location = label(); ; // returns nextQuad from label()
                        QuadTable.AddQuad(BINDR_OP, 0, 0, location);
                        break;
                    case WRITELN: // $WRITELN
                        GetNextToken(echoOn);
                        if (TokenizerClass.tokenCode == LPAREN) // $LPAR
                        {
                            GetNextToken(echoOn);
                            if (TokenizerClass.tokenCode == IDENTIFIER) // $IDENT
                            {
                                location = identifier();
                            }
                            else if (TokenizerClass.tokenCode == STRINGTYPE) // $STRING
                            {
                                location = stringconst();
                            }
                            else
                            {
                                location = simple_expression();
                            }
                            QuadTable.AddQuad(PRINT_OP, location, 0, 0);
                            if (TokenizerClass.tokenCode == RPAREN) // $RPAR
                            {
                                GetNextToken(echoOn);
                            }
                            else
                            {
                                ErrorMessage(RPAREN, TokenizerClass.tokenCode);
                            }
                        }
                        else
                        {
                            ErrorMessage(LPAREN, TokenizerClass.tokenCode);
                        }
                        break;
                    default: // must hit one of the above
                        ErrorMessage();
                        break;
                }
                Debug(false, "statement");
            }
            return 0;
        }

        public static int variable() // non-declaration section
        {
            int result = -1;
            if (!error)
            {
                Debug(true, "variable");
                // check if variable has been declared
                if ((SymbolTable.GetSymbol(SymbolTable.LookupSymbol(TokenizerClass.nextToken)).Value.ToString()) == "undeclared")
                {
                    // fix this
                    SymbolTable.UpdateSymbol(SymbolTable.LookupSymbol(TokenizerClass.nextToken), SymbolTable.Data_Kind.variable, 0);
                    Console.WriteLine("Warning: Variable " + TokenizerClass.nextToken + " has not been declared");
                }
                if (((SymbolTable.GetSymbol(SymbolTable.LookupSymbol(TokenizerClass.nextToken)).Kind.ToString()) == "variable")) // && (!TokenizerClass.nextToken.Equals(ProgIdent)))
                {
                    result = identifier();   // GNT happens in identifier
                }
                else
                {
                    ErrorMessage("variable", SymbolTable.GetSymbol(SymbolTable.LookupSymbol(TokenizerClass.nextToken)).Kind.ToString());
                }

                if (TokenizerClass.tokenCode == LBRA) // $[
                {
                    GetNextToken(echoOn);
                    simple_expression();
                    if (TokenizerClass.tokenCode == RBRA) // $]
                    {
                        GetNextToken(echoOn);
                    }
                    else
                    {
                        ErrorMessage(RBRA, TokenizerClass.tokenCode);
                    }
                }
                Debug(false, "variable");
            }
            return result;
        }

        public static int label() // non-declaration section
        {
            int location = 0;
            if (!error)
            {
                Debug(true, "label");
                if ((SymbolTable.GetSymbol(SymbolTable.LookupSymbol(TokenizerClass.nextToken)).Kind.ToString()) == "label")
                {
                    // if label has been referenced, update value, set index line reference to 0.
                    SymbolTable.UpdateSymbol(SymbolTable.LookupSymbol(TokenizerClass.nextToken), SymbolTable.Data_Kind.label, QuadTable.NextQuad());
                    location = SymbolTable.LookupSymbol(TokenizerClass.nextToken);
                    identifier();
                }
                else
                {
                    ErrorMessage("Label", (SymbolTable.GetSymbol(SymbolTable.LookupSymbol(TokenizerClass.nextToken)).Data_type.ToString()));
                }
                Debug(false, "label");
            }
            return location;
        }

        public static int relexpression()
        {
            int left, right, saveRelop, result, temp;
            result = 0;
            if (!error)
            {
                Debug(true, "relexpression");
                left = simple_expression();
                saveRelop = relop();
                right = simple_expression();
                temp = SymbolTable.GenSymbol();
                QuadTable.AddQuad(SUB_OP, left, right, temp);
                result = QuadTable.NextQuad();
                QuadTable.AddQuad(relopToOpcode(saveRelop), temp, 0, 0);
                Debug(false, "relexpression");
            }
            return result;
        }

        public static int relop()
        {
            int result = 0;
            if (!error)
            {
                Debug(true, "relop");
                switch (TokenizerClass.tokenCode)
                {
                    case EQUA: //$EQ
                        result = EQUA;
                        GetNextToken(echoOn);
                        break;
                    case LSSR: //$LSS
                        result = LSSR;
                        GetNextToken(echoOn);
                        break;
                    case GRTR: // $GTR
                        result = GRTR;
                        GetNextToken(echoOn);
                        break;
                    case NTEQ: //$NEQ
                        result = NTEQ;
                        GetNextToken(echoOn);
                        break;
                    case LSEQ: // $LEQ
                        result = LSEQ;
                        GetNextToken(echoOn);
                        break;
                    case GREQ:  // $GEQ
                        result = GREQ;
                        GetNextToken(echoOn);
                        break;
                    default:
                        ErrorMessage("Relative Operator", TokenizerClass.tokenCode);
                        break;
                }
                Debug(false, "relexpression");
            }
            return result;
        }

        public static int simple_expression()
        {
            bool changeSign;
            int left, right, signval, temp, mulTemp, opcode;
            mulTemp = 0;
            left = 0;
            signval = 1;
            changeSign = false;

            if (!error)
            {
                Debug(true, "simple_expression");
                // change sign of symbol
                if ((TokenizerClass.tokenCode == PLUS) || (TokenizerClass.tokenCode == MINUS))
                {
                    signval = sign();
                    changeSign = true;
                    mulTemp = SymbolTable.GenSymbol();
                }
                left = term();
                if (changeSign == true)
                {
                    QuadTable.AddQuad(MUL_OP, left, signval, mulTemp); // multiply value in op1 by op2 and store in op3- changes sign 
                    left = mulTemp;
                }

                while (((TokenizerClass.tokenCode == PLUS) || (TokenizerClass.tokenCode == MINUS)) && !error)
                {
                    opcode = addop();
                    // GetNextToken(echoOn);
                    right = term();
                    temp = SymbolTable.GenSymbol();
                    QuadTable.AddQuad(opcode, left, right, temp); // this should be an add or sub, result stored in temp
                    left = temp;
                }

                Debug(false, "simple_expression");
            }
            return (left);
        }

        public static int addop()
        {
            int return_op = 0;
            if (!error)
            {
                Debug(true, "addop");
                if ((TokenizerClass.tokenCode != PLUS) && (TokenizerClass.tokenCode != MINUS))
                {

                    ErrorMessage(PLUS, MINUS, TokenizerClass.tokenCode);
                    Debug(false, "addop"); // check this!
                    return 0;
                }
                if (TokenizerClass.tokenCode == PLUS)
                {
                    return_op = ADD_OP;
                }
                else if (TokenizerClass.tokenCode == MINUS)
                {
                    return_op = SUB_OP;
                }
                GetNextToken(echoOn);
                Debug(false, "addop");
            }
            return return_op;
        }

        public static int sign()
        {
            int result = 1;
            if (!error)
            {
                Debug(true, "sign");
                if (TokenizerClass.tokenCode == MINUS)
                {
                    // add -1 to symbol table?
                    result = SymbolTable.LookupSymbol("-1");
                    GetNextToken(echoOn);

                }
                else if (TokenizerClass.tokenCode == PLUS)
                {
                    result = SymbolTable.LookupSymbol("1");
                    GetNextToken(echoOn);
                }
                else
                {
                    ErrorMessage(PLUS, MINUS, TokenizerClass.tokenCode);
                    return 0;
                }
                Debug(false, "sign");
            }
            return result;
        }

        public static int term()
        {
            int right, left, temp;
            left = 0;
            int opcode = 0;
            if (!error)
            {
                Debug(true, "term");
                left = factor();
                while (((TokenizerClass.tokenCode == MULT) || (TokenizerClass.tokenCode == DIVI)) && !error)
                {
                    opcode = mulop();
                    right = factor();
                    temp = SymbolTable.GenSymbol();
                    QuadTable.AddQuad(opcode, left, right, temp); // 
                    left = temp;

                }
                Debug(false, "term");
            }
            return left;
        }

        public static int mulop()
        {
            int return_op = 0;
            if (!error)
            {
                Debug(true, "mulop");
                if ((TokenizerClass.tokenCode != DIVI) && (TokenizerClass.tokenCode != MULT))
                {
                    // error = true;
                    ErrorMessage(DIVI, MULT, TokenizerClass.tokenCode);

                    return 0;
                }
                if (TokenizerClass.tokenCode == MULT)
                {
                    return_op = MUL_OP;
                }
                else if (TokenizerClass.tokenCode == DIVI)
                {
                    return_op = DIV_OP;
                }
                GetNextToken(echoOn);
                Debug(false, "mulop");
            }
            return return_op;
        }

        public static int factor()
        {
            int result = 0;
            if (!error)
            {
                Debug(true, "factor");
                if ((TokenizerClass.tokenCode == INTEGERTYPE) || (TokenizerClass.tokenCode == FLOATTYPE)) // int or float
                {
                    result = unsigned_constant();
                }
                else if (TokenizerClass.tokenCode == IDENTIFIER) // identifier
                {
                    result = variable();
                }
                else if (TokenizerClass.tokenCode == LPAREN) // $(
                {
                    GetNextToken(echoOn);
                    result = simple_expression();
                    if (TokenizerClass.tokenCode == RPAREN) //$)
                    {
                        GetNextToken(echoOn);
                    }
                    else
                    {
                        ErrorMessage(RPAREN, TokenizerClass.tokenCode);
                    }
                }
                else
                {
                    ErrorMessage(IDENTIFIER, INTEGERTYPE, FLOATTYPE, LPAREN, TokenizerClass.tokenCode);
                }
                Debug(false, "factor");
            }
            return result;
        }

        public static int type()
        {
            if (!error)
            {
                Debug(true, "type");
                if ((TokenizerClass.tokenCode == INTEGER) || (TokenizerClass.tokenCode == FLOAT) || (TokenizerClass.tokenCode == STRING))
                {
                    simple_type();
                }
                else if (TokenizerClass.tokenCode == ARRAY) // $ARRAY
                {
                    GetNextToken(echoOn);
                    if (TokenizerClass.tokenCode == LPAREN) // $LBRACK
                    {
                        GetNextToken(echoOn);
                        if (TokenizerClass.tokenCode == INTEGERTYPE) // $INTTYPE?
                        {
                            GetNextToken(echoOn);
                            if (TokenizerClass.tokenCode == RBRA) // $RBRACK
                            {
                                GetNextToken(echoOn);
                                if (TokenizerClass.tokenCode == OF) //$OF
                                {
                                    GetNextToken(echoOn);
                                    if (TokenizerClass.tokenCode == INTEGER) // $INTEGER
                                    {
                                        GetNextToken(echoOn);
                                    }
                                    else
                                    {
                                        ErrorMessage(IDENTIFIER, TokenizerClass.tokenCode);
                                    }
                                }
                                ErrorMessage(OF, TokenizerClass.tokenCode);
                            }
                            else
                            {
                                ErrorMessage(RPAREN, TokenizerClass.tokenCode);
                            }
                        }
                        else
                        {
                            ErrorMessage(INTEGERTYPE, TokenizerClass.tokenCode);
                        }

                    }
                    else
                    {
                        ErrorMessage(LBRA, TokenizerClass.tokenCode);
                    }
                }
                else
                {
                    ErrorMessage(ARRAY, TokenizerClass.tokenCode);
                }
                Debug(false, "type");
            }
            return 0;
        }

        public static int simple_type()
        {
            if (!error)
            {
                Debug(true, "simple_type");
                if ((TokenizerClass.tokenCode == INTEGER) || (TokenizerClass.tokenCode == FLOAT) || (TokenizerClass.tokenCode == STRING))
                {
                    GetNextToken(echoOn);
                }
                else
                {
                    ErrorMessage(IDENTIFIER, FLOAT, STRING, TokenizerClass.tokenCode);
                }
                Debug(false, "simple_type");
            }
            return 0;
        }

        public static int constant()
        {
            if (!error)
            {
                Debug(true, "constant");
                if ((TokenizerClass.tokenCode == PLUS) || (TokenizerClass.tokenCode == MINUS))
                {
                    sign();
                }
                else
                {
                    unsigned_constant();
                }
                Debug(false, "constant");
            }
            return 0;
        }


        public static int unsigned_constant()
        {
            int result = 0;
            if (!error)
            {
                Debug(true, "unsigned_constant");
                result = unsigned_number();
                Debug(false, "unsigned_constant");
            }
            return result;
        }

        public static int unsigned_number()
        {
            int result = 0;
            if (!error)
            {
                Debug(true, "unsigned_number");
                if ((TokenizerClass.tokenCode != INTEGERTYPE) && (TokenizerClass.tokenCode != FLOATTYPE))
                {
                    ErrorMessage(INTEGERTYPE, FLOATTYPE, TokenizerClass.tokenCode);
                    return 0;
                }
                result = SymbolTable.LookupSymbol(TokenizerClass.nextToken);
                GetNextToken(echoOn);
                Debug(false, "unsigned_number");
            }
            return result;
        }

        public static int identifier()
        {
            int result = 0;
            if (!error)
            {
                Debug(true, "identifier");
                if (TokenizerClass.tokenCode != IDENTIFIER)
                {
                    ErrorMessage(IDENTIFIER, TokenizerClass.tokenCode);
                    return 0;
                }
                // first pass sets the program identifier
                if (uniqueProgIdent == false)
                {
                    uniqueProgIdent = true;
                    ProgIdent = TokenizerClass.nextToken;
                    SymbolTable.UpdateSymbol(SymbolTable.LookupSymbol(TokenizerClass.nextToken), SymbolTable.Data_Kind.label, 0);
                }
                else
                {
                    if (TokenizerClass.nextToken.Equals(ProgIdent))
                    {
                        // if -1 is returned, program identifier is already in symbol table
                        if (SymbolTable.LookupSymbol(ProgIdent) >= 0)
                        {
                            AlreadyDeclaredError(TokenizerClass.nextToken);
                        }
                    }
                }
                if (declaration_section == true)
                {
                    if (declare_label == true)
                    {
                        if ((SymbolTable.GetSymbol(SymbolTable.LookupSymbol(TokenizerClass.nextToken)).Kind.ToString()) == "undeclared")
                        {
                            SymbolTable.UpdateSymbol(SymbolTable.LookupSymbol(TokenizerClass.nextToken), SymbolTable.Data_Kind.label, -1); // -1 means label is declared but not used
                            declare_label = false;
                        }
                        else
                        {
                            AlreadyDeclaredError(TokenizerClass.nextToken);
                        }

                    }
                    if (declare_var == true)
                    {
                        if ((SymbolTable.GetSymbol(SymbolTable.LookupSymbol(TokenizerClass.nextToken)).Kind.ToString()) == "undeclared")
                        {
                            SymbolTable.UpdateSymbol(SymbolTable.LookupSymbol(TokenizerClass.nextToken), SymbolTable.Data_Kind.variable, 0); // 0 means variable is declared
                            declare_var = false;
                        }
                        else
                        {
                            AlreadyDeclaredError(TokenizerClass.nextToken);
                        }
                    }
                }
                else
                {
                    if ((SymbolTable.GetSymbol(SymbolTable.LookupSymbol(TokenizerClass.nextToken)).Kind.ToString()) == "undeclared")
                    {
                        UnDeclaredError(TokenizerClass.nextToken);
                    }

                }
                result = SymbolTable.LookupSymbol(TokenizerClass.nextToken); // should return index of variable
                GetNextToken(echoOn);
                Debug(false, "identifier");
            }
            return result;
        }

        public static int stringconst()
        {
            int result = 0;
            if (!error)
            {
                Debug(true, "stringconst");
                if (TokenizerClass.tokenCode == STRINGTYPE)
                {
                    result = SymbolTable.LookupSymbol(TokenizerClass.nextToken);
                    GetNextToken(echoOn);
                }
                else
                {
                    ErrorMessage(STRING, TokenizerClass.tokenCode);
                    return 0;
                }
                Debug(false, "stringconst");
            }
            return result;
        }

        public static int relopToOpcode(int relop)
        {
            int result = 0;
            switch (relop)
            {
                case EQUA:
                    result = BNZ_OP;
                    break;
                case NTEQ:
                    result = BZ_OP;
                    break;
                case LSSR:
                    result = BNN_OP;
                    break;
                case GRTR:
                    result = BNP_OP;
                    break;
                case GREQ:
                    result = BN_OP;
                    break;
                case LSEQ:
                    result = BP_OP;
                    break;
                default:
                    break;
            }
            return result;
        }

        public static void Debug(bool entering, string name)
        {
            if (trace)
            {
                if (entering)
                {
                    for (int i = 0; i <= paddingIndent; i++)
                    {
                        Console.Write(" ");
                    }
                    Console.WriteLine(("Entering " + name));
                    paddingIndent += 5;
                }
                else
                {
                    paddingIndent -= 5;
                    for (int i = 0; i <= paddingIndent; i++)
                    {
                        Console.Write(" ");
                    }
                    Console.WriteLine(("Exiting " + name));

                }
            }
        }

        public static void ErrorMessage()
        {
            if (!error)
            {
                Console.WriteLine("Many possible tokens expected, but not " + (TokenizerClass.nextToken) + ", error in line number " + TokenizerClass.lineNumber);
                Error();
            }
        }


        public static void ErrorMessage(string rightTokenType, string wrongTokenType)
        {
            if (!error)
            {
                Console.WriteLine(rightTokenType + " expected, but " + (wrongTokenType) + " found, in line number " + TokenizerClass.lineNumber);
                Error();
            }
        }

        public static void ErrorMessage(string rightTokenType, int wrongTokenCode)
        {
            if (!error)
            {
                Console.WriteLine(rightTokenType + " expected, but " + ReserveWordClass.LookupMnem(wrongTokenCode) + " found, in line number " + TokenizerClass.lineNumber);
                Error();
            }
        }


        public static void ErrorMessage(int rightTokenCode, int wrongTokenCode)
        {
            if (!error)
            {
                Console.WriteLine(ReserveWordClass.LookupMnem(rightTokenCode) + " expected, but " + ReserveWordClass.LookupMnem(wrongTokenCode) + " found, in line number " + TokenizerClass.lineNumber);
                Error();
            }
        }

        public static void ErrorMessage(int rightTokenCode1, int rightTokenCode2, int wrongTokenCode)
        {
            if (!error)
            {
                Console.WriteLine(ReserveWordClass.LookupMnem(rightTokenCode1) + " or " + ReserveWordClass.LookupMnem(rightTokenCode2) + " expected, but " + ReserveWordClass.LookupMnem(wrongTokenCode) + " found, in line number " + TokenizerClass.lineNumber);
                Error();
            }
        }

        public static void ErrorMessage(int rightTokenCode1, int rightTokenCode2, int rightTokenCode3, int wrongTokenCode)
        {
            if (!error)
            {
                Console.WriteLine(ReserveWordClass.LookupMnem(rightTokenCode1) + ", " + ReserveWordClass.LookupMnem(rightTokenCode2) + ", or" + ReserveWordClass.LookupMnem(rightTokenCode3) + " expected, but " + ReserveWordClass.LookupMnem(wrongTokenCode) + " found, in line number " + TokenizerClass.lineNumber);
                Error();
            }
        }

        public static void ErrorMessage(int rightTokenCode1, int rightTokenCode2, int rightTokenCode3, int rightTokenCode4, int wrongTokenCode)
        {
            if (!error)
            {
                Console.WriteLine(ReserveWordClass.LookupMnem(rightTokenCode1) + ", " + ReserveWordClass.LookupMnem(rightTokenCode2) + ", " + ReserveWordClass.LookupMnem(rightTokenCode3) + ", or " + ReserveWordClass.LookupMnem(rightTokenCode4) + " expected, but " + ReserveWordClass.LookupMnem(wrongTokenCode) + " found, in line number " + TokenizerClass.lineNumber);
                Error();
            }
        }

        public static void AlreadyDeclaredError(string token)
        {
            Console.WriteLine("Error: " + token + " has already been declared.");
            //Error();
        }

        public static void UnDeclaredError(string token)
        {
            Console.WriteLine("Error: " + token + " has not been declared.");
            //Error();
        }

        public static void Error()
        {
            // error = true;
            while (!TokenizerClass.tokenizerFinished)
            {
                error = false;
                Console.WriteLine("Error found. Moving to start of next statement");
                FindStatementStart();
                statement();
            }
        }

        public static void FindStatementStart()
        {
            while ((!TokenizerClass.tokenizerFinished) && (!isStatement(TokenizerClass.tokenCode)))
            {
                GetNextToken(echoOn);
                if (TokenizerClass.tokenCode == 48)
                {
                    break;
                }
            }
        }

        public static bool isStatement(int tokencode)
        {
            bool isStatement = false;
            switch (tokencode)
            {

                case 4:
                    isStatement = true;
                    break;
                case 10:
                    isStatement = true;
                    break;
                case 14:
                    isStatement = true;
                    break;
                case 17:
                    isStatement = true;
                    break;
                case 7:
                    isStatement = true;
                    break;
                case 0:
                    isStatement = true;
                    break;
                case 9:
                    isStatement = true;
                    break;
                case 50:
                    isStatement = true;
                    break;
            }
            return isStatement;
        }


        public static void GetNextToken(bool echoOn)
        {
            TokenizerClass.GetNextToken(echoOn);
            while ((TokenizerClass.tokenCode == -1) && (!TokenizerClass.tokenizerFinished))
            {
                TokenizerClass.GetNextToken(echoOn);
            }
            //TokentoSymTable(TokenizerClass.nextToken, TokenizerClass.tokenCode);
            if (verbose)
            {
                PrintToken(TokenizerClass.nextToken, TokenizerClass.tokenCode);
            }
        }


        public static void PrintToken(string token, int tokenCode)
        {
            if (token.Length > 0)
            {
                string mnem = ReserveWordClass.LookupMnem(tokenCode);
                int symIndex = SymbolTable.LookupSymbol(token);
                // anything with a symbol table value
                if (symIndex != -1)
                {
                    Console.WriteLine(SymbolTable.Truncate(token, 16).PadRight(20) + tokenCode.ToString().PadRight(22) + symIndex);
                }
                else
                {
                    Console.WriteLine(SymbolTable.Truncate(token, 16).PadRight(20) + tokenCode.ToString().PadRight(10) + mnem.PadRight(10));
                }
            }
        }
    }
}
