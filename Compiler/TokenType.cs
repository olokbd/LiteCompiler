namespace LiteCompiler.Compiler
{
    public enum TokenType
    {
        // Keywords
        SHURU,      // shuru - start
        SHESH,      // shesh - end
        BOL,        // bol - print/say
        JODI,       // jodi - if
        NAHOLE,     // nahole - else
        BHAI,       // bhai - general keyword
        NAM,        // nam - name/declare

        // Literals
        NUMBER,
        STRING,
        IDENTIFIER,

        // Operators
        PLUS,       // +
        MINUS,      // -
        MULTIPLY,   // *
        DIVIDE,     // /
        ASSIGN,     // =
        GREATER,    // >
        LESS,       // <
        EQUAL,      // ==
        NOT_EQUAL,  // !=

        // Delimiters
        LEFT_PAREN,     // (
        RIGHT_PAREN,    // )
        LEFT_BRACE,     // {
        RIGHT_BRACE,    // }

        // Special
        NEWLINE,
        EOF
    }
}
