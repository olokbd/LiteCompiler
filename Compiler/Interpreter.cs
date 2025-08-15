using System;
using System.Collections.Generic;
using System.Text;
using LiteCompiler.AST;
using LiteCompiler.Compiler;

namespace LiteCompiler
{
    public class Interpreter : IVisitor<object>
    {
        private readonly Dictionary<string, object> _variables = new Dictionary<string, object>();
        private readonly StringBuilder _output = new StringBuilder();

        public string GetOutput()
        {
            return _output.ToString();
        }

        public void ClearOutput()
        {
            _output.Clear();
        }

        public object VisitProgramNode(ProgramNode node)
        {
            object result = null;
            foreach (var statement in node.Statements)
            {
                result = statement.Accept(this);
            }
            return result;
        }

        public object VisitVariableDeclarationNode(VariableDeclarationNode node)
        {
            var value = node.Value.Accept(this);
            _variables[node.Name] = value;
            return value;
        }

        public object VisitPrintStatementNode(PrintStatementNode node)
        {
            var value = node.Expression.Accept(this);
            var output = value?.ToString() ?? "null";
            _output.AppendLine(output);
            return value;
        }

        public object VisitIfStatementNode(IfStatementNode node)
        {
            var condition = node.Condition.Accept(this);

            if (IsTruthy(condition))
            {
                return node.ThenBranch.Accept(this);
            }
            else if (node.ElseBranch != null)
            {
                return node.ElseBranch.Accept(this);
            }

            return null;
        }

        public object VisitBlockStatementNode(BlockStatementNode node)
        {
            object result = null;
            foreach (var statement in node.Statements)
            {
                result = statement.Accept(this);
            }
            return result;
        }

        public object VisitBinaryExpressionNode(BinaryExpressionNode node)
        {
            var left = node.Left.Accept(this);
            var right = node.Right.Accept(this);

            switch (node.Operator)
            {
                case TokenType.PLUS:
                    if (left is double l1 && right is double r1)
                        return l1 + r1;
                    if (left is string || right is string)
                        return left?.ToString() + right?.ToString();
                    break;
                case TokenType.MINUS:
                    if (left is double l2 && right is double r2)
                        return l2 - r2;
                    break;
                case TokenType.MULTIPLY:
                    if (left is double l3 && right is double r3)
                        return l3 * r3;
                    break;
                case TokenType.DIVIDE:
                    if (left is double l4 && right is double r4)
                    {
                        if (r4 == 0)
                            throw new Exception("Division by zero");
                        return l4 / r4;
                    }
                    break;
                case TokenType.GREATER:
                    if (left is double l5 && right is double r5)
                        return l5 > r5;
                    break;
                case TokenType.LESS:
                    if (left is double l6 && right is double r6)
                        return l6 < r6;
                    break;
                case TokenType.EQUAL:
                    return IsEqual(left, right);
                case TokenType.NOT_EQUAL:
                    return !IsEqual(left, right);
            }

            throw new Exception($"Invalid binary operation: {left} {node.Operator} {right}");
        }

        public object VisitUnaryExpressionNode(UnaryExpressionNode node)
        {
            var operand = node.Operand.Accept(this);

            switch (node.Operator)
            {
                case TokenType.MINUS:
                    if (operand is double d)
                        return -d;
                    break;
            }

            throw new Exception($"Invalid unary operation: {node.Operator} {operand}");
        }

        public object VisitLiteralNode(LiteralNode node)
        {
            return node.Value;
        }

        public object VisitIdentifierNode(IdentifierNode node)
        {
            if (_variables.ContainsKey(node.Name))
            {
                return _variables[node.Name];
            }

            throw new Exception($"Undefined variable: {node.Name}");
        }

        private bool IsTruthy(object value)
        {
            if (value == null) return false;
            if (value is bool b) return b;
            if (value is double d) return d != 0;
            if (value is string s) return !string.IsNullOrEmpty(s);
            return true;
        }

        private bool IsEqual(object left, object right)
        {
            if (left == null && right == null) return true;
            if (left == null) return false;
            return left.Equals(right);
        }
    }
}
