using System;

namespace OpenTibia.Common.Objects
{
    public class RuleViolation
    {
        public RuleViolation()
        {
            this.creationDate = DateTime.UtcNow;
        }

        private DateTime creationDate;

        public uint Time
        {
            get
            {
                return (uint)DateTime.UtcNow.Subtract(creationDate).TotalSeconds;
            }
        }

        public Player Reporter { get; set; }

        public Player Assignee { get; set; }
        
        public string Message { get; set; }
    }
}