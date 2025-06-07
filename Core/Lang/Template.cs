using VortexScript.Core.Builtins;

namespace VortexScript.Core.Lang;

//A template is like a class
public class Template
{
    public string Name { get; }
    public Template Parent { get; }
    private readonly Dictionary<string, object?> _members = new();

    public Template(string name, Template parent)
    {
        Name = name;
        Parent = parent;
    }

    public void Define(string name, object? value)
    {
        _members[name] = value;
    }

    public object? Lookup(string name)
    {
        if (_members.TryGetValue(name, out var val)) return val;
        return Parent?.Lookup(name);
    }

    public override string ToString() => $"template {Name}";


    public static implicit operator Template(BuiltinType type)
    {
        if (BuiltinTemplates.BuiltinTemplateFromType.TryGetValue(type,out var ret))
            return ret;
        throw new Exception("Implicit conversion to a builtin template for enum member "+type+" failed.");
    }
}
