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
            Frequency frequency
        ) : base(description)
        {
            this.frequency = frequency;
        }

        public override void ToggleCompleteStatus()
        {
            base.ToggleCompleteStatus();

            if (IsComplete)
            {
                repeatDate = DateTime.Now.Date.AddDays((int)frequency);
            }
        }

        public DateTime? RepeatDate => repeatDate;
    }
}