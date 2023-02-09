using System;
using System.Collections.Generic;
using System.Linq;

namespace Exam.TaskManager
{
    public class TaskManager : ITaskManager
    {
        private readonly IDictionary<string, Task> executableTasks;
        private readonly IDictionary<string, Task> executedTasks;
        private int positions;

        public TaskManager()
        {
            this.executableTasks = new Dictionary<string, Task>();
            this.executedTasks = new Dictionary<string, Task>();
            this.positions = 0;
        }

        public void AddTask(Task task)
        {
            if (!this.executableTasks.ContainsKey(task.Id))
            {
                task.CurrentPosition = this.positions;
                this.positions++;
                this.executableTasks.Add(task.Id, task);
            }
        }

        public bool Contains(Task task)
        {
            if (this.executableTasks.ContainsKey(task.Id))
            {
                return true;
            }
            else if (this.executedTasks.ContainsKey(task.Id))
            {
                return true;
            }

            return false;
        }

        public void DeleteTask(string taskId)
        {
            if (!this.executableTasks.ContainsKey(taskId)
                && this.executedTasks.ContainsKey(taskId))
            {
                this.executedTasks.Remove(taskId);
            }
            else if (this.executableTasks.ContainsKey(taskId)
                && !this.executedTasks.ContainsKey(taskId))
            {
                this.executableTasks.Remove(taskId);
            }
            else
            {
                throw new ArgumentException();
            }
        }

        public Task ExecuteTask()
        {
            Task taskToExecute = this.executableTasks.Values
                .FirstOrDefault();

            if (taskToExecute == null)
            {
                throw new ArgumentException();
            }

            taskToExecute.CurrentPosition = -1;
            this.executableTasks.Remove(taskToExecute.Id);
            this.executedTasks.Add(taskToExecute.Id, taskToExecute);

            return taskToExecute;
        }

        public IEnumerable<Task> GetAllTasksOrderedByEETThenByName()
        {
            var exetubaleTasksQueue = this.executableTasks.Values.ToList();
            var executedTasksQueue = this.executedTasks.Values.ToList();

            exetubaleTasksQueue.AddRange(executedTasksQueue);

            return exetubaleTasksQueue.Union(executedTasksQueue)
                .OrderByDescending(t => t.EstimatedExecutionTime)
                .ThenBy(t => t.Name.Length);
        }

        public IEnumerable<Task> GetDomainTasks(string domain)
        {
            var tasksWithDivenDomain = this.executableTasks.Values
                .Where(t => t.Domain == domain)
                .ToArray();

            if (tasksWithDivenDomain.Length == 0)
            {
                throw new ArgumentException();
            }

            return tasksWithDivenDomain;
        }

        public Task GetTask(string taskId)
        {
            if (!this.executableTasks.ContainsKey(taskId))
            {
                throw new ArgumentException();
            }

            Task taskToGet = this.executableTasks[taskId];

            return taskToGet;
        }

        public IEnumerable<Task> GetTasksInEETRange(int lowerBound, int upperBound)
        {
            return this.executableTasks.Values
                .Where(t => t.EstimatedExecutionTime >= lowerBound
                && t.EstimatedExecutionTime <= upperBound)
                .OrderBy(t => t.CurrentPosition)
                .ToArray();
        }

        public void RescheduleTask(string taskId)
        {
            if (!this.executedTasks.ContainsKey(taskId))
            {
                throw new ArgumentException();
            }

            Task taskToReschedule = this.executedTasks[taskId];
            this.executedTasks.Remove(taskId);
            taskToReschedule.CurrentPosition = this.positions;
            this.executableTasks.Add(taskId, taskToReschedule);
        }

        public int Size()
        {
            return this.executableTasks.Count;
        }
    }
}
