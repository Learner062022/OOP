using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace TaskManager
{
    public class TaskListSerializer
    {
        TaskSerializer taskSerializer;

        public TaskListSerializer(TaskSerializer serializer)
        {
            taskSerializer = serializer;
        }

        public void WriteTaskList(BinaryWriter writer, TaskList list)
        {
            writer.Write(list.GetType().Name);
            writer.Write(list.Name);
            writer.Write(list.NumTasks);

            foreach (var task in list.Tasks)
            {
                taskSerializer.WriteTask(writer, task);
            }
        }

        public TaskList ReadTaskList(BinaryReader reader)
        {
            string taskListType = reader.ReadString();
            string name = reader.ReadString();

            TaskList list;

            switch (taskListType)
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
                Task task = taskSerializer.ReadTask(reader);
                list.AddTask(task);
                numTasks--;
            }

            return list;
        }
    }
}
