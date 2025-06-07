using VortexScript.Core.Lang.Statements;

public interface IStatementVisitor<T>
{
    T VisitTemplateDef(TemplateDefStatement stmt);
    T VisitExpressionStatement(ExpressionStatement stmt);
    T VisitFieldStatement(FieldStatement stmt);
}
