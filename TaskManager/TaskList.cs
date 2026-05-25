using System;
using System.Collections.Generic;

namespace TaskManager
{
    public class TaskList
    {
        List<Task> tasks;
        public string name;

        public TaskList(string name)
        {
            tasks = new List<Task>();

            if (string.IsNullOrWhiteSpace(name))
            {
                Console.WriteLine("Name cannot be empty.");
            }
            else
            {
                this.name = name;
            }
        }

        public List<Task> Tasks
        {
            get
            {
                return tasks;
            }
        }

        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    name = value;
                }
            }
        }

        public int NumTasks
        {
            get
            {
                return tasks.Count;
            }
        }

        public int NumIncompleteTasks
        {
            get
            {
                int count = 0;

                foreach (Task t in tasks)
                {
                    if (!t.IsComplete)
                    {
                        count++;
                    }
                }

                return count;
            }
        }

        public virtual void AddTask(Task task)
        {
            tasks.Add(task);
        }

        public void RemoveCompletedTasks()
        {
            for (int i = NumTasks - 1; i >= 0; i--)
            {
                if (tasks[i].IsComplete)
                {
                    tasks.RemoveAt(i);
                }
            }
        }
    }
}