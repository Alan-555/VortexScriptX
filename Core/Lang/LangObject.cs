namespace VortexScript.Core.Lang;

//A LangObject is an instance of a template
public class LangObject
{
    // Fields specific to this instance
    private readonly Dictionary<string, object?> _fields = new();

    // The template that defines this object's structure and behavior
    public Template Template { get; }

    public LangObject(Template template)
    {
        Template = template ?? throw new ArgumentNullException(nameof(template));
    }

    public void SetField(string name, object? value)
    {
        _fields[name] = value;
    }

    public bool TryGetField(string name, out object? value)
    {
        return _fields.TryGetValue(name, out value);
    }

    public object? Get(string name)
    {
        // Check own fields
        if (_fields.TryGetValue(name, out var value))
        {
            return value;
        }

        // Check template hierarchy
        return Template.Lookup(name);
    }

    public bool HasField(string name) => _fields.ContainsKey(name);

    public override string ToString() => $"object of {Template.Name}";
}
