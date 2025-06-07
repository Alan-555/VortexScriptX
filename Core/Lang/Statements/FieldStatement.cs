namespace VortexScript.Core.Lang.Statements;

public class FieldStatement(ExpressionStatement type, string name, ExpressionStatement? initializer) : Statement
{
    public ExpressionStatement type = type;
    public string name = name;
    public ExpressionStatement? initializer = initializer;
    public override T Accept<T>(IStatementVisitor<T> visitor)
    {
        return visitor.VisitFieldStatement(this);
    }
}