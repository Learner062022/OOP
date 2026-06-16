using System;

namespace TaskManager
{
    public class Project : TaskList
    {
        public Project(string name) : base(name)
        {
            Name = name;
        }

        public override void AddTask(Task task)
        {
            if (task is RepeatingTasks)
            {
                Console.WriteLine("Habbits and RepeatingTasks aren't allowed in projects.");
                return;
            }

            base.AddTask(task);
        }

        public float Progress
        {
            get
            {
                int numTasks = NumTasks;
                float completedTasks = numTasks - NumIncompleteTasks;
                return numTasks == 0 ? 0 : (completedTasks / numTasks) * 100;
            }
        }
    }
}
