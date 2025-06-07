using VortexScript.Core.Lang;

namespace VortexScript.Core.Builtins;

public static class BuiltinTemplates
{
    public static Dictionary<BuiltinType, Template> BuiltinTemplateFromType = new()
    {
        {BuiltinType.type,new("Type",null)},
    };

}

public enum BuiltinType {
    type,
    int_,
    string_,

}