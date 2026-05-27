using System;

namespace TaskManager
{
    public class RepeatingTasks : Task
    {
        Frequency frequency;
        DateTime? repeatDate;

        public enum Frequency
        {
            Weekly = 7,
            Daily = 1
        }

        public RepeatingTasks(
            string description,
            Frequency frequency,
            string notes = null,
            DateTime? repeatDate = null
        ) : base(description, notes)
        {
            this.frequency = frequency;
            this.repeatDate = repeatDate;
        }

        public RepeatingTasks(
            string description,
            string notes,
            bool isComplete,
            DateTime created,
            DateTime? targetDate,
            Priority priority,
            Frequency frequency,
            DateTime? repeatDate = null
        ) : base(description, notes, isComplete, created, targetDate, priority)
        {
            this.frequency = frequency;
            this.repeatDate = repeatDate;
        }

        public override void ToggleCompleteStatus()
        {
            base.ToggleCompleteStatus();

            if (IsComplete)
            {
                if (repeatDate.HasValue)
                {
                    repeatDate = repeatDate.Value.AddDays((int)frequency);
                }
                
            }
        }

        public DateTime? RepeatDate
        {
            get
            {
                return repeatDate;
            }
        }

        public Frequency RepeatFrequency
        {
            get
            {
                return frequency;
            }
        }
    }
}