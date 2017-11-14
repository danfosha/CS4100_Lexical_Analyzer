using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS4100_Lexical_Analyzer
{
    class SyntaxA
    {
        public SyntaxA()
        {
        }

        public static bool echoOn = true;
        public static bool trace = true;
        public static bool verbose = true;
        public static bool error = false;
        public static int paddingIndent = 0;
        public static bool uniqueProgIdent = false;
        public static string ProgIdent;
        public static bool declare_label = false;
        public static bool declare_var = false;

        // public static int tokenCode = TokenizerClass.tokenCode;

        public static void Analyze(bool echoon)
        {
            echoOn = echoon;
            if ((TokenizerClass.tokenCode >= 0) && (TokenizerClass.tokenCode < 100)) // skip -1
            {
                program();
            }
        }

        // Methods
        public static int program()
        {
            if (!error)
            {
                Debug(true, "program");

                if (TokenizerClass.tokenCode == 15)// $UNIT
                {
                    GetNextToken(echoOn);
                    // need to advance tokencode
                    prog_identifier();
                    if (TokenizerClass.tokenCode == 36) // $;
                    {
                        GetNextToken(echoOn);
                        block();
                        if (TokenizerClass.tokenCode == 48) // $.
                        {
                            Console.WriteLine("No syntax errors found.");
                            TokenizerClass.tokenizerFinished = true;
                            // will ignore rest of file
                        }
                        else
                        {

                            ErrorMessage(48, TokenizerClass.tokenCode);
                        }
                    }
                    else
                    {

                        ErrorMessage(36, TokenizerClass.tokenCode);
                    }
                }
                else
                {

                    ErrorMessage(18, TokenizerClass.tokenCode);
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
                if (TokenizerClass.tokenCode == 16) // $LABEL
                {
                    label_declaration();
                }
                while ((TokenizerClass.tokenCode == 13) && !error) // $VAR
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
                if (TokenizerClass.tokenCode == 10) // $BEGIN
                {
                    GetNextToken(echoOn);
                    statement();
                    while ((TokenizerClass.tokenCode == 36) && !error) // $;
                    {
                        GetNextToken(echoOn);
                        statement();
                    }
                    if (TokenizerClass.tokenCode == 11) // $END
                    {
                        GetNextToken(echoOn);
                    }
                    else
                    {
                        ErrorMessage(11, TokenizerClass.tokenCode);
                    }
                }
                else
                {

                    ErrorMessage(10, TokenizerClass.tokenCode);
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
                if (TokenizerClass.tokenCode == 16) // $LABEL
                {
                    declare_label = true;
                    GetNextToken(echoOn);
                    identifier();
                    while ((TokenizerClass.tokenCode == 44) && !error)
                    {
                        GetNextToken(echoOn);
                        identifier();
                    }
                    if (TokenizerClass.tokenCode == 36)
                    {
                        GetNextToken(echoOn);
                    }
                    else
                    {
                        ErrorMessage(36, TokenizerClass.tokenCode);
                    }
                }
                else
                {
                    ErrorMessage(16, TokenizerClass.tokenCode);
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
                if (TokenizerClass.tokenCode == 13) // $VAR
                {
                    declare_var = true;
                    GetNextToken(echoOn);
                    variable_declaration();
                }
                else
                {
                    ErrorMessage(13, TokenizerClass.tokenCode);
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
                while ((TokenizerClass.tokenCode == 50) && !error) // $IDENT
                {
                    identifier();
                    while ((TokenizerClass.tokenCode == 44) && !error) // $COMMA
                    {
                        GetNextToken(echoOn);
                        identifier();
                    }
                    if (TokenizerClass.tokenCode == 47) // $COLON
                    {
                        GetNextToken(echoOn);
                        type();
                        if (TokenizerClass.tokenCode == 36) // $SEMI
                        {
                            GetNextToken(echoOn);
                        }
                        else
                        {
                            ErrorMessage(36, TokenizerClass.tokenCode);
                        }
                    }
                    else
                    {
                        ErrorMessage(47, TokenizerClass.tokenCode);
                    }

                }
                Debug(false, "variable_declaration");
            }
            return 0;
        }

        public static int statement()
        {
            if (!error)
            {
                Debug(true, "statement");
                if (TokenizerClass.tokenCode == 50)
                {
                    string test = SymbolTable.GetSymbol(SymbolTable.LookupSymbol(TokenizerClass.nextToken)).Kind.ToString();
                    while ((SymbolTable.GetSymbol(SymbolTable.LookupSymbol(TokenizerClass.nextToken)).Kind == (SymbolTable.Data_Kind.label) && !error))
                    {
                        label();
                        if (TokenizerClass.tokenCode == 47)
                        {
                            GetNextToken(echoOn);
                        }
                        else
                        {
                            ErrorMessage(47, TokenizerClass.tokenCode);
                        }
                    }
                }

                // must be one and only one of the below
                switch (TokenizerClass.tokenCode)
                {
                    case 50: // variable  - check for enum type here? will always be identifier

                        variable();
                        if (TokenizerClass.tokenCode == 37) // $assign
                        {
                            GetNextToken(echoOn);
                            if (TokenizerClass.tokenCode == 53) // $string literal?
                            {
                                stringconst();
                            }
                            else
                            {
                                simple_expression();
                            }

                        }
                        else
                        {
                            ErrorMessage(37, TokenizerClass.tokenCode);
                        }

                        break;
                    case 10: // $begin
                        block_body();
                        break;
                    case 4: // $if
                        GetNextToken(echoOn);
                        relexpression();
                        if (TokenizerClass.tokenCode == 5) // $then
                        {
                            GetNextToken(echoOn);
                            statement();
                            if (TokenizerClass.tokenCode == 6) // $else
                            {
                                GetNextToken(echoOn);
                                statement();
                            }
                        }
                        else
                        {
                            ErrorMessage(5, TokenizerClass.tokenCode);
                        }
                        break;
                    case 14: // $while
                        GetNextToken(echoOn);
                        relexpression();
                        if (TokenizerClass.tokenCode == 3) // $do
                        {
                            GetNextToken(echoOn);
                            statement();
                        }
                        else
                        {
                            ErrorMessage(3, TokenizerClass.tokenCode);
                        }
                        break;
                    case 17: // $repeat
                        GetNextToken(echoOn);
                        statement();
                        if (TokenizerClass.tokenCode == 18) // $until
                        {
                            GetNextToken(echoOn);
                            relexpression();
                        }
                        else
                        {
                            ErrorMessage(18, TokenizerClass.tokenCode);
                        }
                        break;
                    case 7: // $FOR
                        GetNextToken(echoOn);
                        variable();
                        if (TokenizerClass.tokenCode == 37) // $ASSIGN
                        {
                            GetNextToken(echoOn);
                            simple_expression();
                            if (TokenizerClass.tokenCode == 2) // $TO
                            {
                                GetNextToken(echoOn);
                                simple_expression();
                                if (TokenizerClass.tokenCode == 3) // $DO
                                {
                                    GetNextToken(echoOn);
                                    statement();
                                }
                                else
                                {
                                    ErrorMessage(3, TokenizerClass.tokenCode);

                                }
                            }
                            else
                            {
                                ErrorMessage(2, TokenizerClass.tokenCode);

                            }
                        }
                        else
                        {
                            ErrorMessage(37, TokenizerClass.tokenCode);

                        }
                        break;
                    case 0: // $GOTO
                        GetNextToken(echoOn);
                        label();
                        break;
                    case 9: // $WRITELN
                        GetNextToken(echoOn);
                        if (TokenizerClass.tokenCode == 34) // $LPAR
                        {
                            GetNextToken(echoOn);
                            if (TokenizerClass.tokenCode == 50) // $IDENT
                            {
                                identifier();
                            }
                            else if (TokenizerClass.tokenCode == 53) // $STRING
                            {
                                stringconst();
                            }
                            else
                            {
                                simple_expression();
                            }
                            if (TokenizerClass.tokenCode == 35) // $RPAR
                            {
                                GetNextToken(echoOn);
                            }
                            else
                            {
                                ErrorMessage(35, TokenizerClass.tokenCode);
                            }
                        }
                        else
                        {
                            ErrorMessage(34, TokenizerClass.tokenCode);
                        }
                        break;
                    default: // must hit one of the above
                        ErrorMessage(99, TokenizerClass.tokenCode);
                        break;
                }
                Debug(false, "statement");
            }
            return 0;
        }

        public static int variable()
        {
            if (!error)
            {
                Debug(true, "variable");
                identifier();
                if (TokenizerClass.tokenCode == 45)
                {
                    GetNextToken(echoOn);
                    simple_expression();
                    if (TokenizerClass.tokenCode == 46)
                    {
                        GetNextToken(echoOn);
                    }
                    else
                    {
                        ErrorMessage(46, TokenizerClass.tokenCode);
                    }
                }
                Debug(false, "variable");
            }
            return 0;
        }

        public static int label()
        {
            if (!error)
            {
                Debug(true, "label");
                if (SymbolTable.GetSymbol(SymbolTable.LookupSymbol(TokenizerClass.nextToken)).Kind.Equals(SymbolTable.Data_Kind.label))
                {
                    identifier();
                }
                else
                {
                    ErrorMessage("Label", (SymbolTable.GetSymbol(SymbolTable.LookupSymbol(TokenizerClass.nextToken)).Data_type.ToString()));
                }
                Debug(false, "label");
            }
            return 0;
        }

        public static int relexpression()
        {
            if (!error)
            {
                Debug(true, "relexpression");
                simple_expression();
                relop();
                simple_expression();
                Debug(false, "relexpression");
            }
            return 0;
        }

        public static int relop()
        {
            if (!error)
            {
                Debug(true, "relop");
                switch (TokenizerClass.tokenCode)
                {
                    case 42: //$EQ
                        GetNextToken(echoOn);
                        break;
                    case 39: //$LSS
                        GetNextToken(echoOn);
                        break;
                    case 38: // $GTR
                        GetNextToken(echoOn);
                        break;
                    case 43: //$NEQ
                        GetNextToken(echoOn);
                        break;
                    case 41: // $LEQ
                        GetNextToken(echoOn);
                        break;
                    case 40:  // $GEQ
                        GetNextToken(echoOn);
                        break;
                    default:
                        ErrorMessage("Relative Operator", TokenizerClass.tokenCode);
                        break;
                }
                Debug(false, "relexpression");
            }
            return 0;
        }

        public static int simple_expression()
        {
            if (!error)
            {
                Debug(true, "simple_expression");
                if ((TokenizerClass.tokenCode == 32) || (TokenizerClass.tokenCode == 33))
                {
                    sign();
                }
                term();
                while (((TokenizerClass.tokenCode == 32) || (TokenizerClass.tokenCode == 33)) && !error)
                {
                    GetNextToken(echoOn);
                    term();
                }

                Debug(false, "simple_expression");
            }
            return 0;
        }

        public static int addop()
        {
            if (!error)
            {
                Debug(true, "addop");
                if ((TokenizerClass.tokenCode != 32) && (TokenizerClass.tokenCode != 33))
                {

                    ErrorMessage(31, 32, TokenizerClass.tokenCode);
                    Debug(false, "addop"); // check this!
                    return 0;
                }
                GetNextToken(echoOn);
                Debug(false, "addop");
            }
            return 1;
        }

        public static int sign()
        {
            if (!error)
            {
                Debug(true, "sign");
                if ((TokenizerClass.tokenCode != 32) && (TokenizerClass.tokenCode != 33))
                {

                    ErrorMessage(32, 33, TokenizerClass.tokenCode);
                    return 0;
                }
                GetNextToken(echoOn);
                Debug(false, "sign");
            }
            return 0;
        }

        public static int term()
        {
            if (!error)
            {
                Debug(true, "term");
                factor();
                while (((TokenizerClass.tokenCode == 31) || (TokenizerClass.tokenCode == 30)) && !error)
                {
                    mulop();
                    factor();
                    // GetNextToken(echoOn);
                }
                Debug(false, "term");
            }
            return 0;
        }

        public static int mulop()
        {
            if (!error)
            {
                Debug(true, "mulop");
                if ((TokenizerClass.tokenCode != 30) && (TokenizerClass.tokenCode != 31))
                {
                    // error = true;
                    ErrorMessage(30, 31, TokenizerClass.tokenCode);

                    return 0;
                }
                GetNextToken(echoOn);
                Debug(false, "mulop");
            }
            return 1;
        }

        public static int factor()
        {
            if (!error)
            {
                Debug(true, "factor");
                if ((TokenizerClass.tokenCode == 51) || (TokenizerClass.tokenCode == 52)) // int or float
                {
                    unsigned_constant();
                }
                else if (TokenizerClass.tokenCode == 50) // identifier
                {
                    variable();
                }
                else if (TokenizerClass.tokenCode == 34) // $(
                {
                    GetNextToken(echoOn);
                    simple_expression();
                    if (TokenizerClass.tokenCode == 35) //$)
                    {
                        GetNextToken(echoOn);
                    }
                    else
                    {
                        ErrorMessage(35, TokenizerClass.tokenCode);
                    }
                }
                else
                {
                    ErrorMessage(50, 51, 52, 34, TokenizerClass.tokenCode);
                }
                Debug(false, "factor");
            }
            return 0;
        }

        public static int type()
        {
            if (!error)
            {
                Debug(true, "type");
                if ((TokenizerClass.tokenCode == 1) || (TokenizerClass.tokenCode == 23) || (TokenizerClass.tokenCode == 24))
                {
                    simple_type();
                }
                else if (TokenizerClass.tokenCode == 12) // $ARRAY
                {
                    GetNextToken(echoOn);
                    if (TokenizerClass.tokenCode == 45) // $LBRACK
                    {
                        GetNextToken(echoOn);
                        if (TokenizerClass.tokenCode == 51) // $INTTYPE?
                        {
                            GetNextToken(echoOn);
                            if (TokenizerClass.tokenCode == 46) // $RBRACK
                            {
                                GetNextToken(echoOn);
                                if (TokenizerClass.tokenCode == 8) //$OF
                                {
                                    GetNextToken(echoOn);
                                    if (TokenizerClass.tokenCode == 1) // $INTEGER
                                    {
                                        GetNextToken(echoOn);
                                    }
                                    else
                                    {
                                        ErrorMessage(1, TokenizerClass.tokenCode);
                                    }
                                }
                                ErrorMessage(8, TokenizerClass.tokenCode);
                            }
                            else
                            {
                                ErrorMessage(46, TokenizerClass.tokenCode);
                            }
                        }
                        else
                        {
                            ErrorMessage(51, TokenizerClass.tokenCode);
                        }

                    }
                    else
                    {
                        ErrorMessage(45, TokenizerClass.tokenCode);
                    }
                }
                else
                {
                    ErrorMessage(12, TokenizerClass.tokenCode);
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
                if ((TokenizerClass.tokenCode == 1) || (TokenizerClass.tokenCode == 23) || (TokenizerClass.tokenCode == 24))
                {
                    GetNextToken(echoOn);
                }
                else
                {
                    ErrorMessage(1, 23, 24, TokenizerClass.tokenCode);
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
                if ((TokenizerClass.tokenCode == 32) || (TokenizerClass.tokenCode == 33))
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
            if (!error)
            {
                Debug(true, "unsigned_constant");
                unsigned_number();
                Debug(false, "unsigned_constant");
            }
            return 0;
        }

        public static int unsigned_number()
        {
            if (!error)
            {
                Debug(true, "unsigned_number");
                if ((TokenizerClass.tokenCode != 51) && (TokenizerClass.tokenCode != 52))
                {
                    ErrorMessage(51, 52, TokenizerClass.tokenCode);
                    return 0;
                }
                GetNextToken(echoOn);
                Debug(false, "unsigned_number");
            }
            return 1;
        }

        public static int identifier()
        {
            if (!error)
            {
                Debug(true, "identifier");
                if (TokenizerClass.tokenCode != 50)
                {
                    ErrorMessage(50, TokenizerClass.tokenCode);
                    return 0;
                }
                // first pass sets the program identifier
                if (uniqueProgIdent == false)
                {
                    uniqueProgIdent = true;
                    ProgIdent = TokenizerClass.nextToken;
                }
                else
                {
                    if (TokenizerClass.nextToken.Equals(ProgIdent))
                    {
                        // if -1 is returned, program identifier is already in symbol table
                        if (SymbolTable.LookupSymbol(ProgIdent) >= 0)
                        {
                            ProgIdentErrorMessage(ProgIdent);
                            return 0;
                        }
                    }
                }
                if (declare_label == true)
                {
                    SymbolTable.UpdateSymbol(SymbolTable.LookupSymbol(TokenizerClass.nextToken), SymbolTable.Data_Kind.label, TokenizerClass.nextToken);
                    declare_label = false;
                }
                else if (declare_var == true)
                {
                    SymbolTable.UpdateSymbol(SymbolTable.LookupSymbol(TokenizerClass.nextToken), SymbolTable.Data_Kind.variable, TokenizerClass.nextToken);
                    declare_var = false;

                }
                GetNextToken(echoOn);
                Debug(false, "identifier");
            }
            return 0;
        }

        public static int stringconst()
        {
            if (!error)
            {
                Debug(true, "stringconst");
                if (TokenizerClass.tokenCode == 53)
                {
                    GetNextToken(echoOn);
                }
                else
                {
                    ErrorMessage(53, TokenizerClass.tokenCode);
                    return 0;
                }
                Debug(false, "stringconst");
            }
            return 0;
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

        public static void ErrorMessage(string rightTokenType, string wrongTokenType)
        {
            if (!error)
            {
                Error();
                Console.WriteLine(rightTokenType + " expected, but " + (wrongTokenType) + " found, in line number " + TokenizerClass.lineNumber);
            }
        }

        public static void ErrorMessage(string rightTokenType, int wrongTokenCode)
        {
            if (!error)
            {
                Error();
                Console.WriteLine(rightTokenType + " expected, but " + ReserveWordClass.LookupMnem(wrongTokenCode) + " found, in line number " + TokenizerClass.lineNumber);
            }
        }


        public static void ErrorMessage(int rightTokenCode, int wrongTokenCode)
        {
            if (!error)
            {
                Error();
                Console.WriteLine(ReserveWordClass.LookupMnem(rightTokenCode) + " expected, but " + ReserveWordClass.LookupMnem(wrongTokenCode) + " found, in line number " + TokenizerClass.lineNumber);
            }
        }

        public static void ErrorMessage(int rightTokenCode1, int rightTokenCode2, int wrongTokenCode)
        {
            if (!error)
            {
                Error();
                Console.WriteLine(ReserveWordClass.LookupMnem(rightTokenCode1) + " or " + ReserveWordClass.LookupMnem(rightTokenCode2) + " expected, but " + ReserveWordClass.LookupMnem(wrongTokenCode) + " found, in line number " + TokenizerClass.lineNumber);
            }
        }

        public static void ErrorMessage(int rightTokenCode1, int rightTokenCode2, int rightTokenCode3, int wrongTokenCode)
        {
            if (!error)
            {
                Error();
                Console.WriteLine(ReserveWordClass.LookupMnem(rightTokenCode1) + ", " + ReserveWordClass.LookupMnem(rightTokenCode2) + ", or" + ReserveWordClass.LookupMnem(rightTokenCode3) + " expected, but " + ReserveWordClass.LookupMnem(wrongTokenCode) + " found, in line number " + TokenizerClass.lineNumber);
            }
        }

        public static void ErrorMessage(int rightTokenCode1, int rightTokenCode2, int rightTokenCode3, int rightTokenCode4, int wrongTokenCode)
        {
            if (!error)
            {
                Error();
                Console.WriteLine(ReserveWordClass.LookupMnem(rightTokenCode1) + ", " + ReserveWordClass.LookupMnem(rightTokenCode2) + ", " + ReserveWordClass.LookupMnem(rightTokenCode3) + ", or " + ReserveWordClass.LookupMnem(rightTokenCode4) + " expected, but " + ReserveWordClass.LookupMnem(wrongTokenCode) + " found, in line number " + TokenizerClass.lineNumber);
            }
        }

        public static void ProgIdentErrorMessage(string token)
        {
            Error();
            Console.WriteLine("Error: " + token + " has already been declared.");
        }

        public static void Error()
        {
            error = true;
            TokenizerClass.tokenizerFinished = true;
        }

        public static void GetNextToken(bool echoOn)
        {
            TokenizerClass.GetNextToken(echoOn);
            while (TokenizerClass.tokenCode == -1)
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
