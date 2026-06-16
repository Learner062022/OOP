using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using static TaskManager.RepeatingTasks;

namespace TaskManager
{
    public class TaskSerializer
    {
        public void WriteOptionalDate(BinaryWriter writer, DateTime? date)
        {
            bool hasValue = date.HasValue;
            writer.Write(hasValue);

            if (hasValue)
            {
                writer.Write(date.Value.ToBinary());
            }
        }

        DateTime? ReadOptionalDate(BinaryReader reader)
        {
            bool hasDate = reader.ReadBoolean();
            DateTime? optionalDate;

            if (hasDate)
            {
                optionalDate = DateTime.FromBinary(reader.ReadInt64());
            }
            else
            {
                optionalDate = null;
            }

            return optionalDate;
        }

        public Task ReadTask(BinaryReader reader)
        {
            string taskType = reader.ReadString();
            string description = reader.ReadString();

            string notes = reader.ReadBoolean() ? reader.ReadString() : null;
            bool isComplete = reader.ReadBoolean();

            DateTime created = DateTime.FromBinary(reader.ReadInt64());
            DateTime? targetDate = ReadOptionalDate(reader);
            Priority priority = new Priority(reader.ReadInt32());

            switch (taskType)
            {
                case "Task":
                {
                    return new Task(
                        description,
                        notes,
                        isComplete,
                        created,
                        targetDate,
                        priority
                    );
                }

                case "RepeatingTasks":
                {
                    Frequency frequency = (Frequency)reader.ReadInt32();
                    DateTime? repeatDate = ReadOptionalDate(reader);

                    return new RepeatingTasks(
                        description,
                        notes,
                        isComplete,
                        created,
                        targetDate,
                        priority,
                        frequency,
                        repeatDate
                    );
                }

                case "Habit":
                {
                    Frequency frequency = (Frequency)reader.ReadInt32();
                    DateTime? repeatDate = ReadOptionalDate(reader);
                    int streak = reader.ReadInt32();

                    return new Habit(
                        description,
                        notes,
                        isComplete,
                        created,
                        targetDate,
                        priority,
                        frequency,
                        streak
                    );
                }
            }

            return null;
        }

        public void WriteTask(BinaryWriter writer, Task task)
        {
            writer.Write(task.GetType().Name);
            writer.Write(task.Description);

            bool hasNotes = !string.IsNullOrWhiteSpace(task.Notes);
            writer.Write(hasNotes);

            if (hasNotes)
            {
                writer.Write(task.Notes);
            }

            writer.Write(task.IsComplete);
            writer.Write(task.Created.ToBinary());

            WriteOptionalDate(writer, task.TargetDate);
            writer.Write(task.Priority.Value);

            switch (task)
            {
                case Habit habit:
                    writer.Write((int)habit.RepeatFrequency);
                    WriteOptionalDate(writer, habit.RepeatDate);
                    writer.Write(habit.CompletionStreak);
                    break;

                case RepeatingTasks repeatingTasks:
                    writer.Write((int)repeatingTasks.RepeatFrequency);
                    WriteOptionalDate(writer, repeatingTasks.RepeatDate);
                    break;
            }
        }
    }
}
