using System.Collections.Generic;

namespace TaskManager
{
    public class TaskCollection
    {
        List<TaskList> taskLists;

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