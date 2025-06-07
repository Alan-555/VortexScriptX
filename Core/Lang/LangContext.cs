namespace VortexScript.Core.Lang;

//The context of Vortex Script, includes operators, keywords and such
public class LangContext
{
    public static LangContext Instance => _instance;
    private static readonly LangContext _instance = new();


    public OperatorInfo[] Operators { get; private set; } = [];
    public HashSet<string> OperatorSignatures { get; private set; } = ["+","*","**","="];
    public HashSet<string> Keywords { get; private set; } = ["template","template?"];

    public LangObject[] Globas { get; private set; } = [];

    public void RegisterOperator(OperatorInfo oper)
    {
        Operators = [.. Operators, oper];
        OperatorSignatures.Add(oper.Signature);
    }

    public void RegisterGlobal(OperatorInfo oper)
    {
        Operators = [.. Operators, oper];
    }

}