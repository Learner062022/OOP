using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using Windows.Storage;
using static DylanDeSouzaOOPPart1.RepeatingTasks;

namespace TaskManager
{
    public class TaskCollection
    {
        List<TaskList> taskLists;
        StorageFolder storageFolder;
        StorageFile file;
        const string FILENAME = "myFile.bin";
        string filePath;

        public TaskCollection()
        {
            taskLists = new List<TaskList>();
            storageFolder = ApplicationData.Current.LocalFolder;
            filePath = Path.Combine(storageFolder.Path, FILENAME);
            load();
        }

        FileStream OpenFileStream(FileMode mode)
        {
            return File.Open(filePath, mode);
        }

        async void load()
        {
            try
            {
                file = await storageFolder.CreateFileAsync(FILENAME, CreationCollisionOption.OpenIfExists);
                taskLists.Clear();
            }
            catch (FileNotFoundException)
            {
                file = await storageFolder.CreateFileAsync(FILENAME);
            }

            Debug.WriteLine($"FilePath: {file.Path}");

            using (var reader = new BinaryReader(OpenFileStream(FileMode.Open), Encoding.UTF8, false))
            {
                if (reader.BaseStream.Length == 0)
                {
                    return;
                }

                int numLists = reader.ReadInt32();

                while (numLists > 0)
                {
                    TaskList list = ReadTaskList(reader);
                    taskLists.Add(list);
                    numLists--;
                }
            }
        }

        void save()
        {
            using (var writer = new BinaryWriter(OpenFileStream(FileMode.Create), Encoding.UTF8, false))
            {
                int numLists = taskLists.Count;
                writer.Write(numLists);

                foreach (TaskList list in taskLists)
                {
                    WriteTaskList(writer, list);
                }
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

            if (task.TargetDate.HasValue)
            {
                writer.Write(task.TargetDate.Value.ToBinary());
            }

            writer.Write(task.Priority.Value);
            
            switch (task)
            {
                case Habit habit:
                    writer.Write((int)habit.RepeatFrequency);
                    writer.Write(habit.CompletionStreak);
                    break;

                case RepeatingTasks repeatingTasks:
                    bool hasValue = repeatingTasks.RepeatDate.HasValue;
                    writer.Write(hasValue);

                    if (hasValue)
                    {
                        writer.Write(repeatingTasks.RepeatDate.Value.ToBinary());
                    }

                    break;
            }
        }

        Task ReadTask(BinaryReader reader)
        {
            string taskType = reader.ReadString();

            string description = reader.ReadString();

            bool hasNotes = reader.ReadBoolean();
            string notes = hasNotes ? reader.ReadString() : null;

            bool isComplete = reader.ReadBoolean();
            DateTime created = DateTime.FromBinary(reader.ReadInt64());

            bool hasTargetDate = reader.ReadBoolean();
            DateTime? targetDate;

            if (hasTargetDate)
            {
                targetDate = DateTime.FromBinary(reader.ReadInt64());
            }
            else
            {
                targetDate = null;
            }

            Priority priority = new Priority(reader.ReadInt32());

            Task task = null;

            switch (taskType)
            {
                case "Task":
                    task = new Task(
                        description,
                        notes,
                        isComplete,
                        created,
                        targetDate,
                        priority
                    );

                    break;

                case "RepeatingTasks":
                {
                    Frequency frequency = (Frequency)reader.ReadInt32();
                    DateTime? repeatDate;
                    bool hasRepeatDate = reader.ReadBoolean();

                    if (hasRepeatDate)
                    {
                        repeatDate = DateTime.FromBinary(reader.ReadInt64());
                    }
                    else
                    {
                        repeatDate = null;
                    }

                    task = new RepeatingTasks(
                        description,
                        notes,
                        isComplete,
                        created,
                        targetDate,
                        priority,
                        frequency,
                        repeatDate
                    );

                    break;
                }
                    

                case "Habit":
                {
                    Frequency frequency = (Frequency)reader.ReadInt32();
                    int streak = reader.ReadInt32();

                    task = new Habit(
                        description,
                        notes,
                        isComplete,
                        created,
                        targetDate,
                        priority,
                        frequency,
                        streak
                    );

                    break;
                }
                    
            }
            
            return task;
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