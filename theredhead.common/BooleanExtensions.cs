namespace theredhead.common;

public static class BooleanExtensions
{
    public static bool And(this bool self, bool other)
    {
        return self && other;
    }

    public static bool Or(this bool self, bool other)
    {
        return self || other;
    }

    public static bool Not(this bool self)
    {
        return !self;
    }

    public static bool Xor(this bool self, bool other)
    {
        return self ^ other;
    }

    public static bool Or(this bool self, Func<bool> block)
    {
        if (! self)
        {
            try
            {
                return block();
            }
            catch
            {
                return false;
            }
        }
        return self;
    }
    public static bool Or(this bool self, Action block)
    {
        if (! self)
        {
            try
            {
                block();
                return true;
            }
            catch
            {
                return false;
            }
        }
        return self;
    }
}
