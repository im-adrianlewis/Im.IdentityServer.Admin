using Polly.Registry;

namespace Im.Access.GraphPortal.Repositories
{
    public interface IPolicyRegistryFactory
    {
        PolicyRegistry Create();
    }
}