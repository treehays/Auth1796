using Auth1796.Core.Application.Repositories.Common.Interfaces;

namespace Auth1796.Core.Application.Utilities.Interfaces;

public interface ISerializerService : ITransientService
{
    string Serialize<T>(T obj);

    string Serialize<T>(T obj, Type type);

    T Deserialize<T>(string text);
}