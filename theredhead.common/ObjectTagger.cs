namespace theredhead.common;

public class Tag {
    public string Name { get; init; } = "Unnamed Tag";
}

public class TagList : List<Tag> {}

public class ObjectTagger {
    public ObjectTagger() {}

    private Dictionary<object, TagList> storage = new();

    public void AddTag(object obj, Tag tag)
    {
        if (!storage.ContainsKey(obj)) {
            storage[obj] = new();
        }
        storage[obj].Add(tag);
    }

    public void RemoveTag(object obj, Tag tag)
    {
        if (!storage.ContainsKey(obj)) {
            if (storage[obj] is TagList list) {
                list.Remove(tag);
            }
        }
    }

    public TagList? GetTags(object obj) => storage.TryGetValue(obj, out TagList? value) ? value : null;
    public bool HasTag(object obj, Tag tag) => storage.TryGetValue(obj, out TagList? value) ? value.Contains(tag) : false;
    public bool HasAnyTag(object obj) => storage.TryGetValue(obj, out TagList? value) ? value.Any() : false;
}
