using System;

namespace DylanDeSouzaOOPPart1
{
    public class Task
    {
        string description;
        string notes;
        bool isComplete;
        DateTime created;
        DateTime? targetDate;
        Priority priority;

        public Task(string description, string notes = null)
        {
            created = DateTime.Now.Date;

            if (string.IsNullOrWhiteSpace(description))
            {
                Console.WriteLine("Description cannot be null or empty.");
            }
            else
            {
                this.description = description;
            }

            isComplete = false;
            this.notes = notes;
            priority = new Priority(0);
        }

        public Task(
            string description,
            string notes,
            bool isComplete,
            DateTime created,
            DateTime? targetDate,
            Priority priority)
        {
            this.description = description;
            this.notes = notes;
            this.isComplete = isComplete;
            this.created = created;
            this.targetDate = targetDate;
            this.priority = priority;
        }

        public string Description
        {
            get
            {
                return description;
            }
            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    description = value;
                }
            }
        }

        public string Notes
        {
            get
            {
                return notes;
            }
            set
            {
                notes = value;
            }
        }

        public bool IsComplete
        {
            get
            {
                return isComplete;
            }
        }

        public DateTime Created
        {
            get
            {
                return created;
            }
        }

        public DateTime? TargetDate
        {
            get
            {
                return targetDate;
            }
        }

        public Priority Priority
        {
            get
            {
                return priority;
            }
        }

        public bool? Overdue
        {
            get
            {
                if (targetDate is null)
                {
                    return null;
                }

                return !isComplete && DateTime.Now.Date > targetDate.Value;
            }
        }

        public virtual void ToggleCompleteStatus()
        {
            isComplete = !isComplete;
        }
    }
}