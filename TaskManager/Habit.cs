using System;

namespace TaskManager
{
    public class Habit : RepeatingTasks
    {
        int completionStreak = 0;
        DateTime? lastCompleteDate;

        public Habit(
            string description,
            Frequency frequency,
            string notes = null
        ) : base(description, frequency, notes)
        {
            Description = description;
            Notes = notes;
        }

        public Habit(
            string description,
            string notes,
            bool isComplete,
            DateTime created,
            DateTime? targetDate,
            Priority priority,
            Frequency frequency,
            int completionStreak
        ) : base(description, notes, isComplete, created, targetDate, priority, frequency, null)
        {
            this.completionStreak = completionStreak;
        }

        public int CompletionStreak
        {
            get
            {
                return completionStreak;
            }
        }

        public void UpdateStreak()
        {
            if (!RepeatDate.HasValue) return;

            bool isScheduledToday = DateTime.Now.Date == RepeatDate.Value.Date;
            bool completedOnScheduledDay = IsComplete && isScheduledToday;

            if (!IsComplete && DateTime.Now.Date > RepeatDate.Value.Date)
            {
                completionStreak = 0;
            }

            else if (completedOnScheduledDay && lastCompleteDate != DateTime.Now.Date)
            {
                completionStreak++;
                lastCompleteDate = DateTime.Now.Date;
            }
        }
    }
}
