using System;

namespace DylanDeSouzaOOPPart1
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
            bool isComplete,
            Frequency frequency,
            string notes = null
        ) : base(description, notes)
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