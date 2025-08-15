using System.Collections.Generic;
using LiteCompiler.AST;
using LiteCompiler.Compiler;

namespace LiteCompiler.AST
{
    public class ProgramNode : ASTNode
    {
        public List<ASTNode> Statements { get; set; }

        public ProgramNode(List<ASTNode> statements)
        {
            Statements = statements;
        }

        public override T Accept<T>(IVisitor<T> visitor)
        {
            return visitor.VisitProgramNode(this);
        }
    }

    public class VariableDeclarationNode : ASTNode
    {
        public string Name { get; set; }
        public ASTNode Value { get; set; }

        public VariableDeclarationNode(string name, ASTNode value)
        {
            Name = name;
            Value = value;
        }

        public override T Accept<T>(IVisitor<T> visitor)
        {
            return visitor.VisitVariableDeclarationNode(this);
        }
    }

    public class PrintStatementNode : ASTNode
    {
        public ASTNode Expression { get; set; }

        public PrintStatementNode(ASTNode expression)
        {
            Expression = expression;
        }

        public override T Accept<T>(IVisitor<T> visitor)
        {
            return visitor.VisitPrintStatementNode(this);
        }
    }

    public class IfStatementNode : ASTNode
    {
        public ASTNode Condition { get; set; }
        public ASTNode ThenBranch { get; set; }
        public ASTNode ElseBranch { get; set; }

        public IfStatementNode(ASTNode condition, ASTNode thenBranch, ASTNode elseBranch = null)
        {
            Condition = condition;
            ThenBranch = thenBranch;
            ElseBranch = elseBranch;
        }

        public override T Accept<T>(IVisitor<T> visitor)
        {
            return visitor.VisitIfStatementNode(this);
        }
    }

    public class BlockStatementNode : ASTNode
    {
        public List<ASTNode> Statements { get; set; }

        public BlockStatementNode(List<ASTNode> statements)
        {
            Statements = statements;
        }

        public override T Accept<T>(IVisitor<T> visitor)
        {
            return visitor.VisitBlockStatementNode(this);
        }
    }

    public class BinaryExpressionNode : ASTNode
    {
        public ASTNode Left { get; set; }
        public TokenType Operator { get; set; }
        public ASTNode Right { get; set; }

        public BinaryExpressionNode(ASTNode left, TokenType op, ASTNode right)
        {
            Left = left;
            Operator = op;
            Right = right;
        }

        public override T Accept<T>(IVisitor<T> visitor)
        {
            return visitor.VisitBinaryExpressionNode(this);
        }
    }

    public class UnaryExpressionNode : ASTNode
    {
        public TokenType Operator { get; set; }
        public ASTNode Operand { get; set; }

        public UnaryExpressionNode(TokenType op, ASTNode operand)
        {
            Operator = op;
            Operand = operand;
        }

        public override T Accept<T>(IVisitor<T> visitor)
        {
            return visitor.VisitUnaryExpressionNode(this);
        }
    }

    public class LiteralNode : ASTNode
    {
        public object Value { get; set; }

        public LiteralNode(object value)
        {
            Value = value;
        }

        public override T Accept<T>(IVisitor<T> visitor)
        {
            return visitor.VisitLiteralNode(this);
        }
    }

    public class IdentifierNode : ASTNode
    {
        public string Name { get; set; }

        public IdentifierNode(string name)
        {
            Name = name;
        }

        public override T Accept<T>(IVisitor<T> visitor)
        {
            return visitor.VisitIdentifierNode(this);
        }
    }
}
