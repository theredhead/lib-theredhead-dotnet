namespace theredhead.common;

/// <summary>
/// Provides a simple way to transform back and forth from string to instance
/// </summary>
public interface IStringRepresentable {
    /// <summary>
    /// Present the current instance as a string that can be used to recreate current state in another instance.
    /// </summary>
    /// <returns>the representation</returns>
    string ToStringRepresentation();

    /// <summary>
    /// Initialize the current instance with the state represented by the given string.
    /// </summary>
    /// <param name="representation"></param>
    /// <returns>success</returns>
    bool InitWithStringRepresentation(string representation);
}


public class Factory<T> where T : IStringRepresentable, new() {
    public T CreateInstance(string representation) {
        var instance = new T();
        if (instance.InitWithStringRepresentation(representation)) {
            return instance;
        }
        throw new Exception("Failed to create instance from representation");
    }
}

