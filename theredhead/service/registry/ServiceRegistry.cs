namespace theredhead.remoting.service.registry;

using theredhead.common;
public class ServiceLocation
{
    public required Type ServiceType { get; init; }
    public required Url Endpoint { get; init; }
}

public enum ServiceStatus
{
    Unknown,
    Available,
    Down
}                       

public class ServiceRegistry
{
    private readonly List<ServiceLocation> registry = new();

    public bool HasServiceEndpoint(Type type) 
        => GetServiceEndpoints(type).Any();
    public IEnumerable<Url> GetServiceEndpoints(Type type)
        => registry.Where(r => r.ServiceType == type).Select(r => r.Endpoint);
    
    public void RegisterServiceEndpoint(Type type, Url endpoint)
    {
        registry.Add(new ServiceLocation()
        {
            ServiceType = type,
            Endpoint = endpoint 
        });
    }
}