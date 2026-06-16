using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using Windows.Storage;
using STask = System.Threading.Tasks.Task;

namespace TaskManager
{
    public class TaskCollection
    {
        List<TaskList> taskLists;
        StorageFolder storageFolder;
        StorageFile file;
        const string FILENAME = "myFile.bin";
        string filePath;
        TaskListSerializer listSerializer;

        public TaskCollection(TaskListSerializer listSerializer)
        {
            taskLists = new List<TaskList>();
            storageFolder = ApplicationData.Current.LocalFolder;
            filePath = Path.Combine(storageFolder.Path, FILENAME);

            this.listSerializer = listSerializer;
        }

        public async STask Load()
        {
            file = await storageFolder.CreateFileAsync(
                FILENAME,
                CreationCollisionOption.OpenIfExists);

            Debug.WriteLine($"FilePath: {filePath}");

            using (var stream = await file.OpenStreamForReadAsync())
            using (var reader = new BinaryReader(stream, Encoding.UTF8))
            {
                if (reader.BaseStream.Length == 0)
                {
                    return;
                }

                int numLists = reader.ReadInt32();

                while (numLists > 0)
                {
                    TaskList list = listSerializer.ReadTaskList(reader);
                    taskLists.Add(list);
                    numLists--;
                }
            }
        }

        public List<TaskList> TaskLists
        {
            get
            {
                return taskLists;
                }
            }

        void WriteTaskList(BinaryWriter writer, TaskList list)
        {
            writer.Write(list.GetType().Name);
            writer.Write(list.Name);
            writer.Write(list.NumTasks);

            foreach (var task in list.Tasks)
            {
                WriteTask(writer, task);
            }
        }

        TaskList ReadTaskList(BinaryReader reader)
        {
            string taskListType = reader.ReadString();
            string name = reader.ReadString();

            TaskList list;

            switch(taskListType)
            {
                case "Project":
                    list = new Project(name);
                    break;

                default:
                    list = new TaskList(name);
                    break;
            }

            int numTasks = reader.ReadInt32();

            while (numTasks > 0)
            {
                Task task = ReadTask(reader);
                list.AddTask(task);
                numTasks--;
            }

            return list;
        }

        void WriteOptionalDate(BinaryWriter writer, DateTime? date)
        {
            bool hasValue = date.HasValue;
            writer.Write(hasValue);

            if (hasValue)
            {
                writer.Write(date.Value.ToBinary());
            }
        }

        void WriteTask(BinaryWriter writer, Task task)
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
            writer.Write(task.TargetDate.HasValue);

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

        Task ReadTask(BinaryReader reader)
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

        public int TotalNumTasks
        {
            get
            {
                int total = 0;

                foreach (TaskList list in taskLists)
                {
                    total += list.NumTasks;
                }

                return total;
            }
        }

        public int TotalNumIncompleteTasks
        {
            get
            {
                int amount = 0;

                foreach (TaskList taskList in taskLists)
                {
                    amount += taskList.NumIncompleteTasks;
                }

                return amount;
            }
        }

        public void AddTaskList(TaskList taskList)
        {
            taskLists.Add(taskList);
            save();
        }

        public void RemoveCompletedTasksPerList()
        {
            foreach (TaskList taskList in taskLists)
            {
                taskList.RemoveCompletedTasks();
            }
        }
    }
}