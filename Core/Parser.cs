using VortexScript.Core.Builtins;
using VortexScript.Core.Lang;
using VortexScript.Core.Lang.Statements;

namespace VortexScript.Core;

public class Parser
{
    private readonly List<Token> tokens;
    private int current = 0;


    public Parser(List<Token> tokens)
    {
        this.tokens = tokens;
    }

    private Token Advance() => tokens[current++];

    private Token Peek() => tokens[current];

    private Token Previous() => tokens[current - 1];

    private bool Match(params TokenID[] types)
    {
        int oldCurrent = current;
        foreach (var type in types)
        {
            if (!Check(type))
            {
                return false;
            }
            Advance();
        }
        current = oldCurrent;
        return true;
    }
    private bool MatchAndConsume(params TokenID[] types)
    {
        foreach (var type in types)
        {
            if (!Check(type))
            {
                return false;
            }
            Advance();
        }
        return true;
    }

    private bool Check(TokenID token) => !IsAtEnd() && Peek().Type == token.type && Peek().Value == token.value;
    private bool Check(TokenType type) => !IsAtEnd() && Peek().Type == type;

    private bool IsAtEnd() => Peek().Type == TokenType.EOF;

    private string Consume(TokenType type, string? message = null)
    {
        if (Check(type)) return Advance().Value;
        message ??= "Expected " + type.ToString() + " token";
        throw new ParseError(Peek(), message);
    }
    private bool TryConsume(TokenType type)
    {
        if (Check(type)) return true;
        return false;
    }


    public Statement ParseStatement()
    {
        if (Match((TokenType.Keyword, "template"))) return ParseTemplate();
        return ParseExpressionWithTerminator(TokenType.Semicolon, null);
    }

    public TemplateDefStatement ParseTemplate()
    {
        string keyword = Consume(TokenType.Keyword); //TODO: abstract
        string name = Consume(TokenType.Name);
        Consume(TokenType.BlockStart);
        List<FieldStatement> fields = [];
        while (Peek().Type != TokenType.BlockEnd)
        {
            var fieldType = ParseExpressionMax(1, BuiltinType.type);
            var fieldName = Consume(TokenType.Name);
            ExpressionStatement? initializer = null;
            if (MatchAndConsume((TokenType.Operator, "=")))
            {
                initializer = ParseExpressionWithTerminator(TokenType.Comma, null);
            }
            Consume(TokenType.Comma);
            fields.Add(new(fieldType, fieldName,initializer));
        }
        Consume(TokenType.BlockEnd);
        return new(name, [.. fields], null, keyword == "abstract!");
    }

    public ExpressionStatement ParseExpressionMax(int maxTokens, BuiltinType? expectedReturnType)
    {
        for (int i = 0; i < maxTokens; i++)
            Consume(Peek().Type);
        return new([], expectedReturnType);
    }
    public ExpressionStatement ParseExpressionWithTerminator(TokenType terminator, BuiltinType? expectedReturnType)
    {
        while (Peek().Type != terminator && !IsAtEnd())
            Consume(Peek().Type);
        if (IsAtEnd())
            throw new ParseError(Peek(), "Expected " + terminator.ToString());
        return new([], expectedReturnType);
    }

    public ExpressionStatement ParseExpression(BuiltinType? expectedReturnType)
    {
        return new([], expectedReturnType);
    }

}

public struct TokenID(TokenType type, string value)
{
    public TokenType type = type;
    public string value = value;

    public static implicit operator TokenID((TokenType, string) obj)
    {
        return new(obj.Item1, obj.Item2);
    }
}

public class ParseError(Token token, string? message) : Exception
{
    public readonly Token token = token;
    public readonly string? message = message;

    public override string ToString()
    {
        return message + "\nat " + token.Line + ":" + token.Column;
    }
}