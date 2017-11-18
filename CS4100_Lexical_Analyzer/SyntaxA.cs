using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS4100_Lexical_Analyzer
// CS4100 Fall 2017 Lexical Analyzer Project - Fosha
{
    class SyntaxA
    {
        public SyntaxA()
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
        public static bool syntax_error = false;
        public static int errorLine;

        // public static int tokenCode = TokenizerClass.tokenCode;

        public static void Analyze(bool echoon)
        {
            echoOn = echoon;
            if ((TokenizerClass.tokenCode >= 0) && (TokenizerClass.tokenCode < 100)) // skip -1
            {
                program();
                if (!syntax_error)
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
                declaration_section = false; // declarations are complete
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
                        declare_label = true;
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
                    declare_var = true;
                    identifier();
                    while ((TokenizerClass.tokenCode == 44) && !error) // $COMMA
                    {
                        declare_var = true;
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
                    // check if token is declared and is a label
                    while (!error && (SymbolTable.LookupSymbol(TokenizerClass.nextToken)) >= 0 && (SymbolTable.GetSymbol(SymbolTable.LookupSymbol(TokenizerClass.nextToken)).Kind.ToString()) == "label")
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
                    case 50: // variable  - check for enum type here? does it matter if variable or constant?

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
                        // some error handling here?
                        if ((TokenizerClass.tokenCode != 36) && (TokenizerClass.tokenCode != 11))
                        {
                            ErrorMessage(36,11,TokenizerClass.tokenCode);
                            
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
            if (!error)
            {
                Debug(true, "variable");
                if ((SymbolTable.GetSymbol(SymbolTable.LookupSymbol(TokenizerClass.nextToken)).Kind.ToString()) == "undeclared")
                {
                    UnDeclaredError(TokenizerClass.nextToken);
                    SymbolTable.UpdateSymbol(SymbolTable.LookupSymbol(TokenizerClass.nextToken), SymbolTable.Data_Kind.variable, 0);
                }
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

        public static int label() // non-declaration section
        {
            if (!error)
            {
                Debug(true, "label");
                // if undeclared, update to label and give value of 0. meaning used
                if ((SymbolTable.GetSymbol(SymbolTable.LookupSymbol(TokenizerClass.nextToken)).Kind.ToString()) == "undeclared")
                {
                    UnDeclaredError(TokenizerClass.nextToken);
                    SymbolTable.UpdateSymbol(SymbolTable.LookupSymbol(TokenizerClass.nextToken), SymbolTable.Data_Kind.label, 0);
                }

                else if ((SymbolTable.GetSymbol(SymbolTable.LookupSymbol(TokenizerClass.nextToken)).Kind.ToString()) == "label")
                {
                    // if label has been referenced, update value - set index line reference to 0. Will change to index in part 4. 
                    // A 0 value  will be read at the end of the analysis as declared and used
                    // Should this error handling be done in identifier() ?
                    SymbolTable.UpdateSymbol(SymbolTable.LookupSymbol(TokenizerClass.nextToken), SymbolTable.Data_Kind.label, 0);
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
                            // return 0;
                        }
                    }
                }
                if (declaration_section == true)
                {
                    // if in the declaration section, these two blocks will log an error if the label or variable has already been declared
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
                            //return 0;
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
                            //return 0;
                        }
                    }
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

        public static void ErrorMessage()
        {
            if (!error)
            {
                Console.WriteLine("Many possible tokens expected, but not " + (ReserveWordClass.LookupMnem(TokenizerClass.tokenCode)) + ", error in or before line number " + TokenizerClass.lineNumber);
                Error();
            }
        }


        public static void ErrorMessage(string rightTokenType, string wrongTokenType = "wrong token")
        {
            if (!error)
            {
                Console.WriteLine(rightTokenType + " expected, but " + (wrongTokenType) + " found, in or before line number " + TokenizerClass.lineNumber);
                Error();
            }
        }

        public static void ErrorMessage(string rightTokenType, int wrongTokenCode)
        {

            if (!error)
            {
                Console.WriteLine(rightTokenType + " expected, but " + ReserveWordClass.LookupMnem(wrongTokenCode) + " found, in or before line number " + TokenizerClass.lineNumber);
                Error();
            }
        }


        public static void ErrorMessage(int rightTokenCode, int wrongTokenCode)
        {
            if (!error)
            {
                Console.WriteLine(ReserveWordClass.LookupMnem(rightTokenCode) + " expected, but " + ReserveWordClass.LookupMnem(wrongTokenCode) + " found, in or before line number " + TokenizerClass.lineNumber);
                Error();
            }
        }

        public static void ErrorMessage(int rightTokenCode1, int rightTokenCode2, int wrongTokenCode)
        {
            if (!error)
            {
                Console.WriteLine(ReserveWordClass.LookupMnem(rightTokenCode1) + " or " + ReserveWordClass.LookupMnem(rightTokenCode2) + " expected, but " + ReserveWordClass.LookupMnem(wrongTokenCode) + " found, in or before line number " + TokenizerClass.lineNumber);
                Error();
            }
        }

        public static void ErrorMessage(int rightTokenCode1, int rightTokenCode2, int rightTokenCode3, int wrongTokenCode)
        {
            if (!error)
            {
                Console.WriteLine(ReserveWordClass.LookupMnem(rightTokenCode1) + ", " + ReserveWordClass.LookupMnem(rightTokenCode2) + ", or" + ReserveWordClass.LookupMnem(rightTokenCode3) + " expected, but " + ReserveWordClass.LookupMnem(wrongTokenCode) + " found, in or before line number " + TokenizerClass.lineNumber);
                Error();
            }
        }

        public static void ErrorMessage(int rightTokenCode1, int rightTokenCode2, int rightTokenCode3, int rightTokenCode4, int wrongTokenCode)
        {
            if (!error)
            {
                Console.WriteLine(ReserveWordClass.LookupMnem(rightTokenCode1) + ", " + ReserveWordClass.LookupMnem(rightTokenCode2) + ", " + ReserveWordClass.LookupMnem(rightTokenCode3) + ", or " + ReserveWordClass.LookupMnem(rightTokenCode4) + " expected, but " + ReserveWordClass.LookupMnem(wrongTokenCode) + " found, in or before line number " + TokenizerClass.lineNumber);
                Error();
            }
        }

        public static void AlreadyDeclaredError(string token)
        {
            Console.WriteLine("Error in line " + TokenizerClass.lineNumber + ": " + token + " has already been declared.");
            //Error();
        }

        public static void UnDeclaredError(string token)
        {
            Console.WriteLine("Error in line " + TokenizerClass.lineNumber + ": " + token + " has not been declared.");
            //Error();
        }

        public static void Error()
        {
            syntax_error = true; // set global flag for end message
            error = true;
            if (!TokenizerClass.tokenizerFinished)
            {
                Console.WriteLine("Error found. Moving to start of next statement");
            }
            else
            {
                Console.WriteLine("Error found. Unwinding stack to top of CFG.");
            }

            while (!TokenizerClass.tokenizerFinished)
            {
                FindStatementStart();
                error = false;
                statement();
               
            }
        }
        
        public static void FindStatementStart()
        {
            while ((!TokenizerClass.tokenizerFinished) && (!isStatement(TokenizerClass.tokenCode)))
            {
                if (TokenizerClass.tokenCode == 48)
                {
                    TokenizerClass.tokenizerFinished = true;
                }
                GetNextToken(echoOn);
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
