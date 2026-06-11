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
        }

        [TestMethod]
        public void SetTaskNameBlank()
        {
        }

        [TestMethod]
        public void SetListNameBlank()
        {
        }

        [TestMethod]
        public void RepeatingTasksRepeatCorrectly()
        {
        }

        [TestMethod]
        public void ProjectPercentComplete()
        {
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