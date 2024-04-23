namespace theredhead.common;

[Experimental]
public interface IServiceRegistration {
    public string Name { get; }
    public VersionNumber Version { get; }
}
public interface IServiceRegistry
{
    public bool HaveService(string name, VersionNumber version);
    
}
