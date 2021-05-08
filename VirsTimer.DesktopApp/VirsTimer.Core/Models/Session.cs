using System;

namespace VirsTimer.Core.Models
{
    public class Session
    {
        public string Name { get; set; }

        public Session(string? name = null)
        {
            Name = name ?? Guid.NewGuid().ToString();
        }
    }
}
