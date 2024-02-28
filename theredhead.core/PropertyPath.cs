using System.Reflection;
using theredhead.text;

namespace theredhead.core;

public class PropertyPathParser
{
    public T GetValue<T>(object instance)
    {
        throw new NotImplementedException();
    }

    public void GetValue<T>(object instance, T value)
    {
        throw new NotImplementedException();
    }
}

public class PropertyPath
{
    public enum StepType
    {
        Property,
        Indexer,
    }

    public class Step
    {
        public StepType Type { get; init; }
        public string Token { get; init; }

        object? Follow(object instance)
        {
            return Type switch
            {
                StepType.Property => FollowProperty(instance),
                StepType.Indexer => FollowIndex(instance),
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        private object? FollowIndex(object instance)
        {
            throw new NotImplementedException();
            if (instance.GetType().GetMethod("get_Item") is { } method)
            {
                var parameters = method.GetParameters();
                if (parameters.Length == 1 && parameters[0] is { } parameter )
                {
                    // return parameter.ParameterType switch
                    // {
                    //     typeof(string) => FollowIndexByString(method, instance);
                    // }
                }
            }
            
            // if (method.GetParameters().Lengthc==c)
            // return method?.Invoke(instance, Array.Empty<string>(Token)) ?? null;
        }

        private object? FollowProperty(object instance)
        {
            return instance.GetType().GetProperty(Token);
        }
    }

    public PropertyPath(string path)
    {
    }
}
