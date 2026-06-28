using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
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
            task = new Task("task1", null, new DateTime(2026, 06, 28));
            project = new Project("project1");
            repeatingTask = new RepeatingTasks("repeatingTask1", RepeatingTasks.Frequency.Daily);
            habit = new Habit("habit1", RepeatingTasks.Frequency.Weekly);
            
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

        [TestMethod]
        public async STask SavingAndLoadingMixed()
        {
            list.AddTask(task);
            list.AddTask(habit);
            list.AddTask(repeatingTask);
            collection.AddTaskList(list);
            collection.AddTaskList(project);
            await collection.Save();
            await collection.Load();

            Assert.IsInstanceOfType<Task>(collection.TaskLists[0].Tasks[0]);
            Assert.AreEqual("task1", collection.TaskLists[0].Tasks[0].Description);
            Assert.IsInstanceOfType<Habit>(collection.TaskLists[0].Tasks[1]);
            Assert.AreEqual("habit1", collection.TaskLists[0].Tasks[1].Description);
            Assert.IsInstanceOfType<RepeatingTasks>(collection.TaskLists[0].Tasks[2]);
            Assert.AreEqual("repeatingTask1", collection.TaskLists[0].Tasks[2].Description);
            Assert.IsInstanceOfType<Project>(collection.TaskLists[1]);
            Assert.AreEqual("project1", collection.TaskLists[1].Name);
        }

        [TestMethod]
        public void SortTasksByDescription()
        {
            task.Description = "clean room";
            list.AddTask(task);

            Task task2 = new Task("buy groceries");
            list.AddTask(task2);

            collection.AddTaskList(list);

            var sortedTasks = collection.TasksSortedByDescription();

            CollectionAssert.AreEqual(
                list.Tasks.OrderBy(t => t.Description).ToList(),
                sortedTasks);
        }

        [TestMethod]
        public void SortTasksByDueDate()
        {
            Task task2 = new Task("buy groceries", null, new DateTime(2026, 01, 01));

            list.AddTask(task);
            list.AddTask(task2);

            collection.AddTaskList(list);

            var sortedTasks = collection.TasksSortedByDueDate();

            CollectionAssert.AreEqual(
                list.Tasks.OrderBy(t => t.TargetDate).ToList(),
                sortedTasks);
        }

        [TestMethod]
        public void SortTasksByCreationDate()
        {
            list.AddTask(task);

            Task task2 = new Task(new DateTime(2026, 01, 01), "buy groceries");
            list.AddTask(task2);

            collection.AddTaskList(list);

            var sortedTasks = collection.TasksSortedByCreationDate();

            CollectionAssert.AreEqual(
                list.Tasks.OrderBy(t => t.Created).ToList(),
                sortedTasks);
        }

        [TestMethod]
        public void SortTasksByPriority()
        {
            Task task2 = new Task("buy groceries");
            task2.Priority++;

            list.AddTask(task);
            list.AddTask(task2);

            collection.AddTaskList(list);

            var sortedTasks = collection.TasksSortedByPriority();

            CollectionAssert.AreEqual(
                list.Tasks.OrderBy(t => t.Priority.Value).ToList(),
                sortedTasks);
        }
    }
}