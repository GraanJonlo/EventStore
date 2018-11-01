using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Domain
{
    public class Streams
    {
        private readonly IStreamStore _streamStore;

        public Streams(IStreamStore streamStore)
        {
            _streamStore = streamStore;
        }

        public async Task<WriteResult> WriteTo(string stream, Event @event)
        {
            var nextExpectedVersion = await _streamStore.Write(stream, @event);

            return new WriteResult {NextExpectedVersion = nextExpectedVersion};
        }

        public async Task<JObject> Read(string stream, int id)
        {
            return await _streamStore.Read(stream, id);
        }
    }
}