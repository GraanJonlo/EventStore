using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain;
using Newtonsoft.Json.Linq;

namespace FakeDataStore
{
    public class FakeStreamStore : IStreamStore
    {
        private readonly object _lock = new object();

        private readonly IDictionary<string, List<SavedEvent>> _streams = new Dictionary<string, List<SavedEvent>>();

        private readonly ISet<Guid> _seen = new HashSet<Guid>();

        public async Task<int> Write(string stream, Event @event)
        {
            int result;

            lock (_lock)
            {
                if (@event.ExpectedVersion == -1 && _streams.ContainsKey(stream))
                {
                    throw new WrongExpectedVersionException();
                }

                if (@event.ExpectedVersion >= 0 && (!_streams.ContainsKey(stream) || _streams[stream].Count - 1 != @event.ExpectedVersion))
                {
                    throw new WrongExpectedVersionException();
                }

                if (@event.ExpectedVersion == -1)
                {
                    _streams.Add(stream, new List<SavedEvent>());
                }
                else if (@event.ExpectedVersion == -2 && !_streams.ContainsKey(stream))
                {
                    _streams.Add(stream, new List<SavedEvent>());
                }

                var saved = new SavedEvent
                    {Id = @event.Id, Type = @event.Type, Data = @event.Data, Updated = DateTimeOffset.UtcNow};

                _streams[stream].Add(saved);
                _seen.Add(@event.Id);

                result = _streams[stream].Count - 1;
            }

            return result;
        }

        public async Task<JObject> Read(string stream, int id)
        {
            lock (_lock)
            {
                if (!_streams.ContainsKey(stream) || _streams[stream].Count - 1 < id)
                {
                    throw new NotFoundException();
                }

                return _streams[stream][id].Data;
            }
        }
    }
}