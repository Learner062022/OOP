using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Windows.Storage;
using System.Diagnostics;

namespace DylanDeSouzaOOPPart1
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
            writer.Write(list.Name);
            writer.Write(list.NumTasks);

            foreach (var task in list.Tasks)
            {
                WriteTask(writer, task);
            }
        }

        TaskList ReadTaskList(BinaryReader reader)
        {
            string name = reader.ReadString();
            TaskList list = new TaskList(name);
            int numTasks = reader.ReadInt32();

            while (numTasks > 0)
            {
                Task task = ReadTask(reader);
                list.AddTask(task);
                numTasks--;
            }

            return list;
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