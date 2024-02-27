namespace theredhead.text.parser;

public struct CharacterInfo
{
    public char Character { get; }
    public CharacterKind Kind { get; }
    public int Line { get; }
    public int Column { get; }

    public CharacterInfo(char ch, CharacterKind kind, int line, int column)
    {
        Character = ch;
        Kind = kind;
        Line = line;
        Column = column;
    }
}