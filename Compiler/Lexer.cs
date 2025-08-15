using System;
using System.Collections.Generic;
using System.Text;
using LiteCompiler.Compiler;

namespace LiteCompiler
{
    public class Lexer
    {
        private readonly string _source;
        private int _current = 0;
        private int _line = 1;
        private int _column = 1;

        private readonly Dictionary<string, TokenType> _keywords = new Dictionary<string, TokenType>
        {
            { "shuru", TokenType.SHURU },
            { "shesh", TokenType.SHESH },
            { "bol", TokenType.BOL },
            { "jodi", TokenType.JODI },
            { "nahole", TokenType.NAHOLE },
            { "bhai", TokenType.BHAI },
            { "nam", TokenType.NAM }
        };

        public Lexer(string source)
        {
            _source = source;
        }

        public List<Token> Tokenize()
        {
            var tokens = new List<Token>();

            while (!IsAtEnd())
            {
                var token = ScanToken();
                if (token != null)
                {
                    tokens.Add(token);
                }
            }

            tokens.Add(new Token(TokenType.EOF, "", _line, _column));
            return tokens;
        }

        private Token ScanToken()
        {
            char c = Advance();

            switch (c)
            {
                case ' ':
                case '\r':
                case '\t':
                    // Ignore whitespace
                    return null;
                case '\n':
                    var newlineToken = new Token(TokenType.NEWLINE, "\n", _line, _column - 1);
                    _line++;
                    _column = 1;
                    return newlineToken;
                case '+':
                    return new Token(TokenType.PLUS, "+", _line, _column - 1);
                case '-':
                    return new Token(TokenType.MINUS, "-", _line, _column - 1);
                case '*':
                    return new Token(TokenType.MULTIPLY, "*", _line, _column - 1);
                case '/':
                    return new Token(TokenType.DIVIDE, "/", _line, _column - 1);
                case '(':
                    return new Token(TokenType.LEFT_PAREN, "(", _line, _column - 1);
                case ')':
                    return new Token(TokenType.RIGHT_PAREN, ")", _line, _column - 1);
                case '{':
                    return new Token(TokenType.LEFT_BRACE, "{", _line, _column - 1);
                case '}':
                    return new Token(TokenType.RIGHT_BRACE, "}", _line, _column - 1);
                case '=':
                    if (Match('='))
                    {
                        return new Token(TokenType.EQUAL, "==", _line, _column - 2);
                    }
                    return new Token(TokenType.ASSIGN, "=", _line, _column - 1);
                case '>':
                    return new Token(TokenType.GREATER, ">", _line, _column - 1);
                case '<':
                    return new Token(TokenType.LESS, "<", _line, _column - 1);
                case '!':
                    if (Match('='))
                    {
                        return new Token(TokenType.NOT_EQUAL, "!=", _line, _column - 2);
                    }
                    throw new Exception($"Unexpected character '!' at line {_line}, column {_column - 1}");
                case '"':
                    return ScanString();
                default:
                    if (IsDigit(c))
                    {
                        return ScanNumber();
                    }
                    if (IsAlpha(c))
                    {
                        return ScanIdentifier();
                    }
                    throw new Exception($"Unexpected character '{c}' at line {_line}, column {_column - 1}");
            }
        }

        private Token ScanString()
        {
            var value = new StringBuilder();
            int startColumn = _column - 1;

            while (Peek() != '"' && !IsAtEnd())
            {
                if (Peek() == '\n')
                {
                    _line++;
                    _column = 1;
                }
                value.Append(Advance());
            }

            if (IsAtEnd())
            {
                throw new Exception($"Unterminated string at line {_line}");
            }

            // Consume closing "
            Advance();

            return new Token(TokenType.STRING, value.ToString(), _line, startColumn);
        }

        private Token ScanNumber()
        {
            var value = new StringBuilder();
            int startColumn = _column - 1;

            // Go back one to include the first digit
            _current--;
            _column--;

            while (IsDigit(Peek()))
            {
                value.Append(Advance());
            }

            // Handle decimal numbers
            if (Peek() == '.' && IsDigit(PeekNext()))
            {
                value.Append(Advance()); // consume '.'
                while (IsDigit(Peek()))
                {
                    value.Append(Advance());
                }
            }

            return new Token(TokenType.NUMBER, value.ToString(), _line, startColumn);
        }

        private Token ScanIdentifier()
        {
            var value = new StringBuilder();
            int startColumn = _column - 1;

            // Go back one to include the first character
            _current--;
            _column--;

            while (IsAlphaNumeric(Peek()))
            {
                value.Append(Advance());
            }

            string text = value.ToString();
            TokenType type = _keywords.ContainsKey(text) ? _keywords[text] : TokenType.IDENTIFIER;

            return new Token(type, text, _line, startColumn);
        }

        private char Advance()
        {
            _column++;
            return _source[_current++];
        }

        private bool Match(char expected)
        {
            if (IsAtEnd()) return false;
            if (_source[_current] != expected) return false;

            _current++;
            _column++;
            return true;
        }

        private char Peek()
        {
            if (IsAtEnd()) return '\0';
            return _source[_current];
        }

        private char PeekNext()
        {
            if (_current + 1 >= _source.Length) return '\0';
            return _source[_current + 1];
        }

        private bool IsAtEnd()
        {
            return _current >= _source.Length;
        }

        private bool IsDigit(char c)
        {
            return c >= '0' && c <= '9';
        }

        private bool IsAlpha(char c)
        {
            return (c >= 'a' && c <= 'z') ||
                   (c >= 'A' && c <= 'Z') ||
                   c == '_';
        }

        private bool IsAlphaNumeric(char c)
        {
            return IsAlpha(c) || IsDigit(c);
        }
    }
}
