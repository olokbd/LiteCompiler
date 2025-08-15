using System;
using System.Collections.Generic;
using LiteCompiler.AST;
using LiteCompiler.Compiler;
using LiteCompiler;

namespace LiteCompiler
{
    public class LiteCompiler
    {
        private readonly Interpreter _interpreter;

        public LiteCompiler()
        {
            _interpreter = new Interpreter();
        }

        public string Compile(string source)
        {
            try
            {
                _interpreter.ClearOutput();

                // Tokenize
                var lexer = new Lexer(source);
                var tokens = lexer.Tokenize();

                // Parse
                var parser = new Parser(tokens);
                var ast = parser.Parse();

                // Execute
                ast.Accept(_interpreter);

                return _interpreter.GetOutput();
            }
            catch (Exception ex)
            {
                return $"Error: {ex.Message}";
            }
        }

        public ProgramNode Parse(string source)
        {
            _interpreter.ClearOutput();

            // Tokenize
            var lexer = new Lexer(source);
            var tokens = lexer.Tokenize();

            // Parse
            var parser = new Parser(tokens);
            return parser.Parse();
        }

        public List<Token> GetTokens(string source)
        {
            try
            {
                var lexer = new Lexer(source);
                return lexer.Tokenize();
            }
            catch (Exception ex)
            {
                throw new Exception($"Tokenization error: {ex.Message}");
            }
        }

        public ProgramNode GetAST(string source)
        {
            try
            {
                var lexer = new Lexer(source);
                var tokens = lexer.Tokenize();
                var parser = new Parser(tokens);
                return parser.Parse();
            }
            catch (Exception ex)
            {
                throw new Exception($"Parsing error: {ex.Message}");
            }
        }
    }
}
