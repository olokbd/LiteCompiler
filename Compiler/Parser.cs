using System;
using System.Collections.Generic;
using LiteCompiler.AST;
using LiteCompiler.Compiler;

namespace LiteCompiler
{
    public class Parser
    {
        private readonly List<Token> _tokens;
        private int _current = 0;

        public Parser(List<Token> tokens)
        {
            _tokens = tokens;
        }

        public ProgramNode Parse()
        {
            var statements = new List<ASTNode>();

            // Consume SHURU
            if (!Check(TokenType.SHURU))
            {
                throw new Exception("Program must start with 'shuru'");
            }
            Advance(); // consume SHURU
            ConsumeOptional(TokenType.BHAI); // optional bhai after shuru
            ConsumeOptional(TokenType.NEWLINE);

            while (!Check(TokenType.SHESH) && !IsAtEnd())
            {
                var stmt = ParseStatement();
                if (stmt != null)
                {
                    statements.Add(stmt);
                }
                ConsumeOptional(TokenType.NEWLINE);
            }

            // Consume SHESH
            if (!Check(TokenType.SHESH))
            {
                throw new Exception("Program must end with 'shesh'");
            }
            Advance(); // consume SHESH
            ConsumeOptional(TokenType.BHAI); // optional bhai after shesh

            return new ProgramNode(statements);
        }

        private ASTNode ParseStatement()
        {
            if (Check(TokenType.NAM))
            {
                return ParseVariableDeclaration();
            }
            if (Check(TokenType.BOL))
            {
                return ParsePrintStatement();
            }
            if (Check(TokenType.JODI))
            {
                return ParseIfStatement();
            }
            if (Check(TokenType.LEFT_BRACE))
            {
                return ParseBlockStatement();
            }

            // Skip newlines and bhai tokens
            if (Check(TokenType.NEWLINE) || Check(TokenType.BHAI))
            {
                Advance();
                return null;
            }

            if (Check(TokenType.NAHOLE))
            {
                throw new Exception($"Unexpected 'nahole' - 'nahole' can only be used after 'jodi' statement at line {Peek().Line}");
            }

            throw new Exception($"Unexpected token: {Peek().Type} at line {Peek().Line}");
        }

        private VariableDeclarationNode ParseVariableDeclaration()
        {
            Advance(); // consume NAM
            ConsumeOptional(TokenType.BHAI); // optional bhai after nam

            if (!Check(TokenType.IDENTIFIER))
            {
                throw new Exception("Expected variable name after 'nam'");
            }

            string name = Advance().Value;

            if (!Check(TokenType.ASSIGN))
            {
                throw new Exception("Expected '=' after variable name");
            }
            Advance(); // consume =

            var value = ParseExpression();

            return new VariableDeclarationNode(name, value);
        }

        private PrintStatementNode ParsePrintStatement()
        {
            Advance(); // consume BOL
            ConsumeOptional(TokenType.BHAI); // optional bhai after bol

            var expression = ParseExpression();

            return new PrintStatementNode(expression);
        }

        private IfStatementNode ParseIfStatement()
        {
            Advance(); // consume JODI
            ConsumeOptional(TokenType.BHAI); // optional bhai after jodi
            SkipWhitespace(); // Skip any newlines after jodi bhai

            if (!Check(TokenType.LEFT_PAREN))
            {
                throw new Exception("Expected '(' after 'jodi'");
            }
            Advance(); // consume (

            var condition = ParseExpression();

            if (!Check(TokenType.RIGHT_PAREN))
            {
                throw new Exception("Expected ')' after condition");
            }
            Advance(); // consume )
            SkipWhitespace(); // Skip any newlines after condition

            var thenBranch = ParseStatement();
            SkipWhitespace(); // Skip any newlines after then branch

            ASTNode elseBranch = null;
            if (Check(TokenType.NAHOLE))
            {
                Advance(); // consume NAHOLE
                ConsumeOptional(TokenType.BHAI); // optional bhai after nahole
                SkipWhitespace(); // Skip any newlines after nahole bhai
                elseBranch = ParseStatement();
            }

            return new IfStatementNode(condition, thenBranch, elseBranch);
        }

        private BlockStatementNode ParseBlockStatement()
        {
            Advance(); // consume {
            SkipWhitespace(); // Use SkipWhitespace instead of just ConsumeOptional(TokenType.NEWLINE)

            var statements = new List<ASTNode>();

            while (!Check(TokenType.RIGHT_BRACE) && !IsAtEnd())
            {
                var stmt = ParseStatement();
                if (stmt != null)
                {
                    statements.Add(stmt);
                }
                SkipWhitespace(); // Use SkipWhitespace instead of just ConsumeOptional(TokenType.NEWLINE)
            }

            if (!Check(TokenType.RIGHT_BRACE))
            {
                throw new Exception("Expected '}' after block");
            }
            Advance(); // consume }

            return new BlockStatementNode(statements);
        }

        private ASTNode ParseExpression()
        {
            return ParseComparison();
        }

        private ASTNode ParseComparison()
        {
            var expr = ParseTerm();

            while (Match(TokenType.GREATER, TokenType.LESS, TokenType.EQUAL, TokenType.NOT_EQUAL))
            {
                var op = Previous().Type;
                var right = ParseTerm();
                expr = new BinaryExpressionNode(expr, op, right);
            }

            return expr;
        }

        private ASTNode ParseTerm()
        {
            var expr = ParseFactor();

            while (Match(TokenType.PLUS, TokenType.MINUS))
            {
                var op = Previous().Type;
                var right = ParseFactor();
                expr = new BinaryExpressionNode(expr, op, right);
            }

            return expr;
        }

        private ASTNode ParseFactor()
        {
            var expr = ParseUnary();

            while (Match(TokenType.MULTIPLY, TokenType.DIVIDE))
            {
                var op = Previous().Type;
                var right = ParseUnary();
                expr = new BinaryExpressionNode(expr, op, right);
            }

            return expr;
        }

        private ASTNode ParseUnary()
        {
            if (Match(TokenType.MINUS))
            {
                var op = Previous().Type;
                var right = ParseUnary();
                return new UnaryExpressionNode(op, right);
            }

            return ParsePrimary();
        }

        private ASTNode ParsePrimary()
        {
            if (Match(TokenType.NUMBER))
            {
                var value = double.Parse(Previous().Value);
                return new LiteralNode(value);
            }

            if (Match(TokenType.STRING))
            {
                return new LiteralNode(Previous().Value);
            }

            if (Match(TokenType.IDENTIFIER))
            {
                return new IdentifierNode(Previous().Value);
            }

            if (Match(TokenType.LEFT_PAREN))
            {
                var expr = ParseExpression();
                if (!Check(TokenType.RIGHT_PAREN))
                {
                    throw new Exception("Expected ')' after expression");
                }
                Advance();
                return expr;
            }

            throw new Exception($"Unexpected token: {Peek().Type} at line {Peek().Line}");
        }

        private bool Match(params TokenType[] types)
        {
            foreach (var type in types)
            {
                if (Check(type))
                {
                    Advance();
                    return true;
                }
            }
            return false;
        }

        private bool Check(TokenType type)
        {
            if (IsAtEnd()) return false;
            return Peek().Type == type;
        }

        private Token Advance()
        {
            if (!IsAtEnd()) _current++;
            return Previous();
        }

        private bool IsAtEnd()
        {
            return Peek().Type == TokenType.EOF;
        }

        private Token Peek()
        {
            return _tokens[_current];
        }

        private Token Previous()
        {
            return _tokens[_current - 1];
        }

        private void ConsumeOptional(TokenType type)
        {
            if (Check(type))
            {
                Advance();
            }
        }

        private void SkipWhitespace()
        {
            while (Check(TokenType.NEWLINE) || Check(TokenType.BHAI))
            {
                Advance();
            }
        }
    }
}
