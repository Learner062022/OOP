using System;

namespace TaskManager
{
    public class Habit : RepeatingTasks
    {
        int completionStreak = 0;
        DateTime? lastCompleteDate;

        public Habit(
            string description,
            bool isComplete,
            Frequency frequency,
            string notes = null
        ) : base(description, isComplete, frequency, notes)
        {
        }

        public int CompletionStreak => completionStreak;

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
