using System;

namespace VirsTimer.Core.Models
{
    public class Session
    {
        public string Id { get; }
        public string Name { get; set; }

        public Session(string? name = null)
        {
            Id = Guid.NewGuid().ToString();
            Name = name ?? Id;
        }

        public Session(string id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
