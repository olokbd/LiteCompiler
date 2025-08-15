namespace LiteCompiler.AST
{
    public abstract class ASTNode
    {
        public abstract T Accept<T>(IVisitor<T> visitor);
    }

    public interface IVisitor<T>
    {
        T VisitProgramNode(ProgramNode node);
        T VisitVariableDeclarationNode(VariableDeclarationNode node);
        T VisitPrintStatementNode(PrintStatementNode node);
        T VisitIfStatementNode(IfStatementNode node);
        T VisitBlockStatementNode(BlockStatementNode node);
        T VisitBinaryExpressionNode(BinaryExpressionNode node);
        T VisitUnaryExpressionNode(UnaryExpressionNode node);
        T VisitLiteralNode(LiteralNode node);
        T VisitIdentifierNode(IdentifierNode node);
    }
}
