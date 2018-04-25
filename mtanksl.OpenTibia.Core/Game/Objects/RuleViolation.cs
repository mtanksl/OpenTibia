using System;

namespace OpenTibia
{
    public class RuleViolation
    {
        public RuleViolation()
        {
            this.creationDate = DateTime.Now;
        }

        private DateTime creationDate;

        public uint Time
        {
            get
            {
                return (uint)DateTime.Now.Subtract(creationDate).TotalSeconds;
            }
        }

        public Player Reporter { get; set; }

        public Player Assignee { get; set; }
        
        public string Message { get; set; }
    }
}