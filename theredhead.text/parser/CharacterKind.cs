namespace theredhead.text.parser;

public enum CharacterKind
{
    None,
    Whitespace,
    Quote,
    Numeric,
    Special,
    GroupOpen,
    GroupClose,
    Seperator,
    Alpha,
    Unknown
}