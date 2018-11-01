using System;
using Newtonsoft.Json.Linq;

namespace Domain
{
    public class Event
    {
        public Guid Id { get; set; }
        public string Type { get; set; }
        public int ExpectedVersion { get; set; }
        public JObject Data { get; set; }
    }
}
