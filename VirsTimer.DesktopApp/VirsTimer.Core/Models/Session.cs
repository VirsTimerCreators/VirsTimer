using System;

namespace VirsTimer.Core.Models
{
    public class Session
    {
        public Guid Id { get; }
        public string Name { get; set; }

        public Session(string? name = null)
        {
            Id = Guid.NewGuid();
            Name = name ?? Id.ToString();
        }

        public Session(Guid id, string? name = null)
        {
            Id = id;
            Name = name ?? Id.ToString();
        }
    }
}
