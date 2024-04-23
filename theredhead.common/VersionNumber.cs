namespace theredhead.common;

public class VersionNumber : IStringRepresentable
{
    public uint Major { get; set; } = 0;
    public uint Minor { get; set; } = 0;
    public uint Revision { get; set; } = 0;
    public uint Patch { get; set; } = 0;

    public VersionNumber()
    {        
    }
    public VersionNumber(string versionNumber) : this()
    {
        if (!TryParse(versionNumber))
        {
            throw new ArgumentException("Invalid version number", nameof(versionNumber));
        }
    }

    public VersionNumber Copy(uint? major, uint? minor, uint? revision, uint? patch) {
        var copy = new VersionNumber() {
            Major = major ?? Major,
            Minor = minor ?? Minor,
            Revision = revision ?? Revision,
            Patch = patch ?? Patch
        };
        return copy;
    }

    public bool TryParse(string versionNumber) 
    {
        Major = 0;
        Minor = 0;
        Revision = 0;
        Patch = 0;

        var subject = versionNumber.ToLowerInvariant().TrimStart('v');
        if (subject.TrySplitAtFirst(".", out var part, out var rest) && uint.TryParse(part, out var major)) {
            Major = major;
            if (rest.TrySplitAtFirst(".", out part, out rest) && uint.TryParse(part, out var minor)) {
                Minor = minor;
                if (rest.TrySplitAtFirst(".", out part, out rest) && uint.TryParse(part, out var revision)) {
                    Revision = revision;
                    if (rest.TrySplitAtFirst(".", out part, out _) && uint.TryParse(part, out var patch)) {
                        Patch = patch;
                    }
                }
            }
            return true;
        }
        else if (uint.TryParse(subject, out major)) 
        {
            Major = major;
            return true;
        }

        return false;
    }

    public static bool TryParse(string versionNumber, out VersionNumber version) 
    {
        version = new VersionNumber();
        return version.TryParse(versionNumber);
    }

    public string ToStringRepresentation() => ToString();
    public bool InitWithStringRepresentation(string representation) => TryParse(representation);

    public override string ToString() => $"v{Major}.{Minor}.{Revision}.{Patch}";

    public bool IsNewerThan(VersionNumber other) {
        if (Major > other.Major) return true;
        if (Major < other.Major) return false;

        if (Minor > other.Minor) return true;
        if (Minor < other.Minor) return false;

        if (Revision > other.Revision) return true;
        if (Revision < other.Revision) return false;

        if (Patch > other.Patch) return true;
        if (Patch < other.Patch) return false;

        return false;
    }
}
