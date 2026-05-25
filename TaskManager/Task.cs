using System;

namespace TaskManager
{
    public class Task
    {
        string description;
        string? notes;
        bool isComplete;
        DateTime created;
        DateTime? targetDate;
        Priority priority;

        public Task(
            string description,
            string? notes = null,
            DateTime? targetDate = null)
        {
            created = DateTime.Now;

            if (string.IsNullOrWhiteSpace(description))
            {
                Console.WriteLine("Description cannot be null or empty.");
            }
            else
            {
                this.description = description;
            }

            this.notes = notes;
            isComplete = false;
            this.targetDate = targetDate;
            priority = new Priority(0);
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

        public string? Notes
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
            protected set
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
            isComplete = !IsComplete;
        }
    }
}