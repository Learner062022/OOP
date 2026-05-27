using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace DylanDeSouzaOOPPart1
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
            _ = new TaskCollection();
            Task task = new Task("first task");
            _ = new RepeatingTasks(task.Description, RepeatingTasks.Frequency.Weekly);
            _ = new TaskList("first task list");
            Project project = new Project("first project");
            project.AddTask(task);
            //collection.AddTaskList(project);
            //collection.AddTaskList(list);
        }
    }
}
