# Banglish Compiler

A custom programming language compiler built in C# that combines Bengali-inspired keywords with English syntax, demonstrating complete compiler implementation from lexical analysis to execution.

## 🌟 Overview

Banglish is a domain-specific programming language that allows developers to write code using Bengali-inspired keywords while maintaining familiar programming constructs. This project showcases the full compiler pipeline including tokenization, parsing, AST generation, and interpretation.

## 🚀 Features

### Language Constructs

-   **Variable Declarations**: `nam bhai x = 7`
-   **Print Statements**: `bol bhai "Hello World"`
-   **Conditional Logic**: `jodi bhai (condition) { ... } nahole bhai { ... }`
-   **Arithmetic Operations**: `+`, `-`, `*`, `/`
-   **Comparison Operations**: `>`, `<`, `==`, `!=`
-   **Block Statements**: `{ ... }`
-   **String and Numeric Literals**
-   **Parenthesized Expressions**

### Compiler Features

-   **Complete Lexical Analysis**: Tokenizes source code with proper whitespace handling
-   **Recursive Descent Parser**: Builds Abstract Syntax Tree with operator precedence
-   **AST Interpreter**: Executes programs using visitor pattern
-   **AST Visualization**: TreeView integration for syntax tree display
-   **Comprehensive Error Handling**: Detailed syntax and runtime error messages
-   **Multi-line Support**: Flexible formatting with newline handling

## 📝 Banglish Keywords

| Keyword  | English Equivalent | Usage                     |
| -------- | ------------------ | ------------------------- |
| `shuru`  | start/begin        | Program entry point       |
| `shesh`  | end                | Program termination       |
| `nam`    | name/declare       | Variable declaration      |
| `bol`    | say/print          | Output statement          |
| `jodi`   | if                 | Conditional statement     |
| `nahole` | else               | Alternative condition     |
| `bhai`   | (separator)        | General keyword separator |

## 💻 Example Programs

### Basic Arithmetic

```banglish
shuru bhai
nam bhai x = 15
nam bhai y = 25
bol bhai x + y
shesh bhai
```
