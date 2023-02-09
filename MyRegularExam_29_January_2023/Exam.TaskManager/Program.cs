using System;
using System.Linq;

namespace Exam.TaskManager
{
    public class Program
    {
        public static void Main(string[] args)
        {
            TaskManager taskManager = new TaskManager();

            Task task = new Task("1", "First", 5, "abv");
            Task task2 = new Task("2", "Second", 4, "abv");
            Task task3 = new Task("3", "Third", 3, "abv");

            taskManager.AddTask(task);
            taskManager.AddTask(task2);
            taskManager.AddTask(task3);

            taskManager.ExecuteTask();

            Console.WriteLine(taskManager.Size());

            taskManager.RescheduleTask("1");

            var tasks = taskManager.GetTasksInEETRange(1, 5);

            Console.WriteLine(string.Join(", ", tasks.Select(x => x.Name)));
        }
    }
}
