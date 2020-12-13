using System;

namespace Core
{
    public abstract class Message
    {
        protected Message()
        {
            DateTime = DateTime.Now;
        }

        public DateTime DateTime { get; set; }
    }
}
