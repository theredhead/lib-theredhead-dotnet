namespace theredhead.text.parser;

// parse characters into character classes, into grouped character classes

public class CharacterParser
{
    public delegate void CharacterParsedHandler(object sender, CharacterParsedEventArgs args);
    public event CharacterParsedHandler? CharacterParsed;

    private Dictionary<char, CharacterKind> map = new() {
        { ' ', CharacterKind.Whitespace },
        { '\t', CharacterKind.Whitespace },
        { '\r', CharacterKind.Whitespace },
        { '\n', CharacterKind.Whitespace },
        { '\'', CharacterKind.Quote },
        { '"', CharacterKind.Quote },
        { '1', CharacterKind.Numeric },
        { '2', CharacterKind.Numeric },
        { '3', CharacterKind.Numeric },
        { '4', CharacterKind.Numeric },
        { '5', CharacterKind.Numeric },
        { '6', CharacterKind.Numeric },
        { '7', CharacterKind.Numeric },
        { '8', CharacterKind.Numeric },
        { '9', CharacterKind.Numeric },
        { '0', CharacterKind.Numeric },
        { '!', CharacterKind.Special },
        { '@', CharacterKind.Special },
        { '#', CharacterKind.Special },
        { '$', CharacterKind.Special },
        { '%', CharacterKind.Special },
        { '^', CharacterKind.Special },
        { '&', CharacterKind.Special },
        { '*', CharacterKind.Special },
        { '(', CharacterKind.GroupOpen },
        { ')', CharacterKind.GroupClose },
        { '-', CharacterKind.Special },
        { '=', CharacterKind.Special },
        { '_', CharacterKind.Special },
        { '+', CharacterKind.Special },
        { ';', CharacterKind.Special },
        { '`', CharacterKind.Special },
        { '~', CharacterKind.Special },
        { '±', CharacterKind.Special },
        { '±', CharacterKind.Special },

        { '[', CharacterKind.GroupOpen },
        { ']', CharacterKind.GroupClose },
        { '{', CharacterKind.GroupOpen },
        { '}', CharacterKind.GroupClose },
        { '<', CharacterKind.GroupOpen },
        { '>', CharacterKind.GroupClose },

        { ',', CharacterKind.Seperator },
        { '.', CharacterKind.Seperator },
        { '/', CharacterKind.Seperator },
        { '\\', CharacterKind.Seperator },

    };

    public CharacterKind CharacterKindFor(char ch) => map.GetValueOrDefault(ch, CharacterKind.Unknown);

    public IEnumerable<CharacterInfo> Parse(IEnumerable<char> input)
    {
        var line = 0;
        var column = 0;

        foreach(char ch in input)
        {
            var kind = CharacterKindFor(ch);
            var info = new CharacterInfo(ch, kind, line, column);

            CharacterParsed?.Invoke(this, new CharacterParsedEventArgs(info));

            yield return info;
        }
    }
}

public class CharacterParsedEventArgs
{
    public CharacterInfo CharacterInfo { get; set; }

    public CharacterParsedEventArgs(CharacterInfo characterInfo)
    {
        CharacterInfo = characterInfo;
    }
}