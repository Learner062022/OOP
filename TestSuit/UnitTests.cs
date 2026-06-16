using Microsoft.VisualStudio.TestTools.UnitTesting;
using TaskManager;
using STask = System.Threading.Tasks.Task;

namespace TestSuit
{
    [TestClass]
    public class UnitTests
    {
        TaskCollection collection;
        TaskList list;
        Task task;
        Project project;
        RepeatingTasks repeatingTask;
        Habit habit;
        TaskSerializer taskSerializer;
        TaskListSerializer listSerializer;

        [TestInitialize]
        public void Setup()
        {
            taskSerializer = new TaskSerializer();
            listSerializer = new TaskListSerializer(taskSerializer);
            collection = new TaskCollection(listSerializer);
            list = new TaskList("list1");
            task = new Task("task1");
            project = new Project("project1");
            repeatingTask = new RepeatingTasks("repeatingTask1", RepeatingTasks.Frequency.Daily);
            habit = new Habit("habit1", Habit.Frequency.Weekly);
        }

        [TestMethod]
        public void AddingTasksAndCount()
        {
            list.AddTask(task);
            Assert.AreEqual(1, list.NumTasks);
        }

        [TestMethod]
        public void AddListToCollection()
        {
            list.AddTask(task);
            collection.AddTaskList(list);
            Assert.AreEqual(1, collection.TotalNumTasks);
        }

        [TestMethod]
        public void SetTaskDescriptionBlank()
        {
            task.Description = "";
            Assert.AreEqual("task1", task.Description);
        }

        [TestMethod]
        public void SetListNameBlank()
        {
            list.Name = "";
            Assert.AreEqual("list1", list.Name);
        }

        [TestMethod]
        public void RepeatingTasksRepeatCorrectly()
        {
        }

        [TestMethod]
        public void ProjectPercentComplete()
        {
            task.ToggleCompleteStatus();
            project.AddTask(task);
            Assert.AreEqual(100, project.Progress);
        }

        [TestMethod]
        public void PlaceHabitInProject()
        {
            project.AddTask(habit);
            Assert.AreEqual(0, project.NumTasks);
        }

        [TestMethod]
        public void RepeatingTaskWithIncompleteInfo()
        {
        }

        [TestMethod]
        public void PlaceRepeatingTaskInProject()
        {
            project.AddTask(repeatingTask);
            Assert.AreEqual(0, project.NumTasks);
        }

        [TestMethod]
        public void CompleteIncompleteTaskUpdatesCount()
        {
            list.AddTask(task);
            task.ToggleCompleteStatus();
            Assert.IsTrue(task.IsComplete);
            Assert.AreEqual(0, list.NumIncompleteTasks);
            task.ToggleCompleteStatus();
            Assert.IsFalse(task.IsComplete);
            Assert.AreEqual(1, list.NumIncompleteTasks);
        }

        [TestMethod]
        public void DeleteTasksUpdatesCount()
        {
            list.AddTask(task);
            task.ToggleCompleteStatus();
            list.RemoveCompletedTasks();
            Assert.AreEqual(0, list.NumTasks);
        }

        [TestMethod]
        public async STask SavingAndLoadingPropject()
        {
            project.AddTask(task);
            collection.AddTaskList(project);
            await collection.Save();
            await collection.Load();
            Assert.IsInstanceOfType<Project>(collection.TaskLists[0]);
            Assert.AreEqual("project1", collection.TaskLists[0].Name);
        }

        [TestMethod]
        public async STask SavingAndLoadingHabbit()
        {
            list.AddTask(habit);
            collection.AddTaskList(list);
            await collection.Save();
            await collection.Load();
            Assert.IsInstanceOfType<Habit>(collection.TaskLists[0].Tasks[0]);
        }

        [TestMethod]
        public async STask SavingAndLoadingRepeatingTasks()
        {
            list.AddTask(repeatingTask);
            collection.AddTaskList(list);
            await collection.Save();
            await collection.Load();
            Assert.IsInstanceOfType<RepeatingTasks>(collection.TaskLists[0].Tasks[0]);
        }

        [TestMethod]
        public async STask SavingAndLoadingTask()
        {
            list.AddTask(task);
            collection.AddTaskList(list);
            await collection.Save();
            await collection.Load();
            Assert.IsInstanceOfType<Task>(collection.TaskLists[0].Tasks[0]);
        }
    }
}