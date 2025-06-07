namespace VortexScript.Core.Lang.Statements;

public class ExpressionStatement(Token[] tokens,Template? expectedReturnType ) : Statement
{
    public Token[] tokens = tokens;

    public Template? expectedReturnType = expectedReturnType;
    public override T Accept<T>(IStatementVisitor<T> visitor)
    {
        return visitor.VisitExpressionStatement(this);
    }
}