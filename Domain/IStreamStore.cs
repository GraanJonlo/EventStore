using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Domain
{
    public interface IStreamStore
    {
        Task<int> Write(string stream, Event @event);

        Task<JObject> Read(string stream, int id);
    }
}