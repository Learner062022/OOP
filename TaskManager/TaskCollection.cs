using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
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

        public async STask Save()
        {
           file = await storageFolder.CreateFileAsync(
                FILENAME,
                CreationCollisionOption.ReplaceExisting);

            using (var stream = await file.OpenStreamForWriteAsync())
            using (var writer = new BinaryWriter(stream, Encoding.UTF8))
            {
                writer.Write(taskLists.Count);

                foreach (var list in taskLists)
        {
                    listSerializer.WriteTaskList(writer, list);
                }
            }
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
        }

        public void RemoveCompletedTasksPerList()
        {
            foreach (TaskList taskList in taskLists)
            {
                taskList.RemoveCompletedTasks();
            }
        }

        public List<Task> TasksSortedByDescription()
        {
            var tasks = taskLists.SelectMany(list => list.Tasks).ToList();
            tasks.Sort((t1, t2) => t1.Description.CompareTo(t2.Description));
            return tasks;
        }

        public List<Task> TasksSortedByDueDate()
        {
            var tasks = taskLists.SelectMany(list => list.Tasks).ToList();
            tasks.Sort((t1, t2) => t1.TargetDate.Value.CompareTo(t2.TargetDate.Value));
            return tasks;
        }
        
        public List<Task> TasksSortedByCreationDate()
        {
            var tasks = taskLists.SelectMany(list => list.Tasks).ToList();
            tasks.Sort((t1, t2) => t1.Created.CompareTo(t2.Created));
            return tasks;
        }
        
        public List<Task> TasksSortedByPriority()
        {
            var tasks = taskLists.SelectMany(list => list.Tasks).ToList();
            tasks.Sort((t1, t2) => t1.Priority.Value.CompareTo(t2.Priority.Value));
            return tasks;
        }

        public List<Habit> GetHabits()
        {
            return taskLists.SelectMany(list => list.Tasks).OfType<Habit>().ToList();
        }

        public List<RepeatingTasks> GetRepeatingTasks()
        {
            return taskLists.SelectMany(list => list.Tasks).OfType<RepeatingTasks>().ToList();
        }
        
        public List<Task> TasksDueToday()
        { 
            return taskLists.SelectMany(list => list.Tasks).Where(task => task.TargetDate == DateTime.Now.Date).ToList();
        }
        
        public List<Task> TasksWithGivenDescription(string description)
        {
            return taskLists.SelectMany(list => list.Tasks).Where(task => task.Description == description).ToList();
        }
    }
}