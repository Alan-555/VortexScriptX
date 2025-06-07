namespace VortexScript.Core.Lang.Statements;

public class TemplateDefStatement(string name, FieldStatement[] fields, Template? parent, bool isAbstract) : Statement
{
    public string name = name;
    public FieldStatement[] fields = fields;
    public Template? parent = parent;
    public bool isAbstract = isAbstract;
    public override T Accept<T>(IStatementVisitor<T> visitor)
    {
        return visitor.VisitTemplateDef(this);
    }
}