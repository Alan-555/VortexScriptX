namespace VortexScript.Core.Lang.Statements;
public abstract class Statement
{
    public abstract T Accept<T>(IStatementVisitor<T> visitor);
}
