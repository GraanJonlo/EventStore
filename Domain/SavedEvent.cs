using System;
using Newtonsoft.Json.Linq;

namespace Domain
{
    public class SavedEvent
    {
        public Guid Id { get; set; }
        public string Type { get; set; }
        public JObject Data { get; set; }
        public DateTimeOffset Updated { get; set; }
    }
}