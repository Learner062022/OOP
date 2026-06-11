using Microsoft.VisualStudio.TestTools.UnitTesting;
using TaskManager;

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

        [TestInitialize]
        public void Setup()
        {
            collection = new TaskCollection();
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
        }

        [TestMethod]
        public void RepeatingTaskWithIncompleteInfo()
        {
        }

        [TestMethod]
        public void PlaceRepeatingTaskInProject()
        {
        }

        [TestMethod]
        public void CompleteIncompleteTaskUpdatesCount()
        {
        }

        [TestMethod]
        public void DeleteTasksUpdatesCount()
        {
        }

        [TestMethod]
        public void EmptyListUpdatesCount()
        {
        }
    }
}