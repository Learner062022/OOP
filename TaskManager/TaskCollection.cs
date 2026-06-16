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
    }
}