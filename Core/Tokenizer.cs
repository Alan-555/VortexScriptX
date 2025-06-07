using VortexScript.Core.Lang;

namespace VortexScript.Core;

public class Tokenizer
{
    private readonly string _source;
    private int _pos = 0;
    private int _line = 1;
    private int _col = 1;
    private readonly LangContext _context;

    private LexerMode _mode = LexerMode.Normal;
    private readonly Stack<LexerMode> _modeStack = new();

    public Tokenizer(string source, LangContext context)
    {
        _source = source;
        _context = context;
    }

    public List<Token> Tokenize()
    {
        var tokens = new List<Token>();

        while (!IsAtEnd())
        {
            SkipWhitespace();

            var startCol = _col;
            var ch = Peek();

            switch (_mode)
            {
                case LexerMode.Normal:
                    tokens.Add(LexNormal(ch, startCol));
                    break;

                case LexerMode.String:
                    tokens.Add(LexStringContent());
                    break;

                case LexerMode.Interpolation:
                    tokens.Add(LexInterpolatedExpression());
                    break;

                default:
                    tokens.Add(new Token(TokenType.Unknown, ch.ToString(), _line, _col));
                    Advance();
                    break;
            }
        }

        tokens.Add(new Token(TokenType.EOF, "", _line, _col));
        return tokens;
    }

    private Token LexNormal(char ch, int col)
    {
        if (char.IsLetter(ch)) return LexIdentifier();
        if (char.IsDigit(ch)) return LexNumber();
        if (ch == '"')
        {
            EnterMode(LexerMode.String);
            Advance();
            return new Token(TokenType.Literal, "\"", _line, col); // Start of string
        }
        if (ch == '(') { Advance(); return new Token(TokenType.LeftParen, "(", _line, col); }
        if (ch == ')') { Advance(); return new Token(TokenType.RightParen, ")", _line, col); }
        if (ch == '{') { Advance(); return new Token(TokenType.BlockStart, "{", _line, col); }
        if (ch == '}') { Advance(); return new Token(TokenType.BlockEnd, "}", _line, col); }
        if (ch == ';') { Advance(); return new Token(TokenType.Semicolon, ";", _line, col); }
        if (ch == ',') { Advance(); return new Token(TokenType.Comma, ",", _line, col); }
        var keyword = MatchExplicitTokens(_source, _pos, _context.Keywords);
        if (keyword != null)
            return new Token(TokenType.Keyword, keyword, _line, col);
        var op = MatchExplicitTokens(_source, _pos, _context.OperatorSignatures);
        if (op != null)
            return new Token(TokenType.Operator, op, _line, col);
        Advance();
        return new Token(TokenType.Unknown, ch.ToString(), _line, col);
    }

    private Token LexIdentifier()
    {
        int start = _pos;
        int startCol = _col;
        while (!IsAtEnd() && char.IsLetterOrDigit(Peek())) Advance();
        string value = _source[start.._pos];
        if (_context.Keywords.Contains(value))
            return new Token(TokenType.Keyword, value, _line, startCol);
        else
            return new Token(TokenType.Name, value, _line, startCol);
    }

    private Token LexNumber()
    {
        int start = _pos;
        int startCol = _col;
        while (!IsAtEnd() && char.IsDigit(Peek())) Advance();
        string value = _source[start.._pos];
        return new Token(TokenType.Literal, value, _line, startCol);
    }

    private Token LexStringContent()
    {
        int start = _pos;
        int startCol = _col;
        string s = "";
        while (!IsAtEnd())
        {
            char ch = Peek();
            if (ch == '"')
            {
                ExitMode();
                Advance();
                return new Token(TokenType.Literal, s, _line, startCol); // End of string
            }
            s += ch;
            Advance();
        }
        return new Token(TokenType.Unknown, _source[start.._pos], _line, startCol);
    }

    private Token LexInterpolatedExpression()
    {
        // Placeholder: implement based on your language rules
        return new Token(TokenType.Unknown, "?", _line, _col);
    }

    private string? MatchExplicitTokens(string source, int start, HashSet<string> matchFor)
    {
        int maxLen = 0;
        string? match = null;

        // Max operator length (optional optimization)
        int maxOpLength = matchFor.Max(op => op.Length);

        for (int len = 1; len <= maxOpLength && (start + len) <= source.Length; len++)
        {
            string candidate = source.Substring(start, len);
            if (matchFor.Contains(candidate))
            {
                match = candidate;
                maxLen = len;
            }
        }

        if (match != null)
        {
            Advance(maxLen); // move lexer forward
            return match;
        }

        return null; // not an operator
    }



    private void EnterMode(LexerMode mode) => _modeStack.Push(_mode = mode);
    private void ExitMode()
    {
        if (_modeStack.Count > 0)
            _modeStack.Pop();
        else
        {
            _mode = LexerMode.Normal;
            return;
        }
        if (_modeStack.TryPeek(out var mode))
            _mode = mode;
        else
            _mode = LexerMode.Normal;

    }


    private char Peek() => IsAtEnd() ? '\0' : _source[_pos];
    private char Advance(int move = 1)
    {
        _pos += move;
        _col += move;
        if (IsAtEnd()) return '\0';
        char ch = _source[_pos];
        return ch;
    }

    private bool IsAtEnd() => _pos >= _source.Length;
    private void SkipWhitespace()
    {
        while (!IsAtEnd() && char.IsWhiteSpace(Peek()))
        {
            if (Peek() == '\n')
            {
                _line++;
                _col = 1;
            }
            Advance();
        }
    }
}



public readonly struct Token
{
    public TokenType Type { get; }
    public string Value { get; }
    public int Line { get; }
    public int Column { get; }

    public Token(TokenType type, string value, int line, int column)
    {
        Type = type;
        Value = value;
        Line = line;
        Column = column;
    }

    public override string ToString() => $"{Type}('{Value}') at {Line}:{Column}";
}

public enum TokenType
{
    Name,
    Literal,
    Operator,
    Keyword,
    LeftParen,
    RightParen,
    BlockStart,
    BlockEnd,
    EOF,
    Semicolon,
    Comma,
    Unknown
}

public enum LexerMode
{
    Normal,
    String,          // Inside a string literal
    Interpolation,   // Inside interpolated expression
    // Add more as needed
}