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