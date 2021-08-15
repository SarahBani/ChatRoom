using System;

namespace ChatRoom.Models
{
    public record Message(string Username, string Content)
    {

        public DateTime DateTime { get; } = DateTime.Now;

    }
}
