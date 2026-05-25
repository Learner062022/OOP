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
            Created = DateTime.Now.Date;

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
        }

        public Task(
            string description,
            string notes,
            bool isComplete,
            DateTime created,
            DateTime? targetDate,
            int priorityVal)
        {
            this.description = description;
            this.notes = notes;
            IsComplete = isComplete;
            Created = created;
            this.targetDate = targetDate;
            priority = new Priority(priorityVal);
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
            private set
            {
                isComplete = value;
            }
        }

        public DateTime Created
        {
            get
            {
                return created;
            }
            private set
            {
                created = value;
            }
        }

        public DateTime? TargetDate
        {
            get
            {
                return targetDate;
            }
            set
            {
                targetDate = value;
            }
        }

        public Priority Prio
        {
            get
            {
                return priority;
            }
            set
            {
                priority = value;
            }
        }

        public struct Priority
        {
            int value;

            public int Value
            {
                get
                {
                    return value;
                }
            }

            public Priority(int v)
            {
                value = Math.Clamp(v, -1, 1);
            }

            public static Priority operator ++(Priority p)
            {
                if (p.value < 1)
                {
                    p.value++;
                }

                return p;
            }

            public static Priority operator --(Priority p)
            {
                if (p.value > -1)
                {
                    p.value--;
                }

                return p;
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

                return !IsComplete && DateTime.Now.Date > targetDate.Value;
            }
        }

        public virtual void ToggleCompleteStatus()
        {
            IsComplete = !IsComplete;
        }
    }
}