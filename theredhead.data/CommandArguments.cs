using System.Collections;
using theredhead.text;

namespace theredhead.data;

public class CommandArguments : IEnumerable<KeyValuePair<string, object>> {
    private readonly Dictionary<string, object> _dict = new Dictionary<string, object>();
    private readonly QuoteKind _paramNameQuoteKind;

    public string[] Names => _dict.Keys.ToArray();
    
    public CommandArguments(string keyPrefix) {
        _paramNameQuoteKind = new QuoteKind(keyPrefix, "");
    }
    public CommandArguments(string keyPrefix, params IEnumerable[] inputs) : this(keyPrefix)
    {
        if (inputs != null) {
            AddEnumerables(inputs);
        }
    }
    public CommandArguments(QuoteKind paramNameQuoteKind, params IEnumerable[] inputs)
    {
        _paramNameQuoteKind = paramNameQuoteKind;
        if (inputs != null) {
            AddEnumerables(inputs);
        }
    }

    public object? this[string name] {
        get {
            var quotedName = CreateParameterNameFrom(name);
            if (_dict.ContainsKey(quotedName)) {
                return _dict[quotedName];
            }
            return null;
        }
        set {
            var quotedName = CreateParameterNameFrom(name);
            if (_dict.ContainsKey(quotedName)) {
                _dict[quotedName] = value!;
                return;
            }
            else
            {
                Add(name, value!);
            }
        }
    }

    public bool ContainsKey(string name) {
        var quotedName = CreateParameterNameFrom(name);
        return _dict.ContainsKey(name) || _dict.ContainsKey(quotedName);
    }

    private void AddEnumerables(IEnumerable[] inputs) {
        foreach(var input in inputs) {
            if (input != null) {
                AddEnumerable(input);
            }
        }
    }
    private void AddEnumerable(IEnumerable input)
    {
        if (input is string[] strings) {
            AddStrings(strings);
        }
        else if (input is Dictionary<string, object> dict) {
            AddDictionary(dict);
        }
    }

    private void AddDictionary(Dictionary<string, object> dict)
    {
        foreach(var kvp in dict) {
            Add(kvp.Key, kvp.Value);
        }
    }

    private object CreateParameterValueFrom(object value)
    {
        if (value is ISqlValue sqlValue) {
            return sqlValue.ToSqlValue();
        }
        return value ?? DBNull.Value;
    }

    private string CreateParameterNameFrom(string key)
    {
        return _paramNameQuoteKind.Quote(_paramNameQuoteKind.Unquote(key));
    }

    private void AddStrings(string[] strings)
    {
        foreach(var s in strings) {
            Add(s, s);
        }
    }

    private void Add(string name, object value) {
        var _name = CreateParameterNameFrom(name);
        var _value = CreateParameterValueFrom(value);
        _dict.Add(_name, _value);
    }

    public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
    {
        foreach(var kvp in _dict) {
            yield return kvp;
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        foreach(var kvp in _dict) {
            yield return kvp;
        }
    }
}
