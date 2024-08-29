using AspireTaskWorker.ApiService.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace AspireTaskWorker.ApiService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TaskController : ControllerBase
    {
        // Store tasks in a dictionary by taskId
        private static readonly ConcurrentDictionary<string, TaskProgress> taskListDictionary = new ConcurrentDictionary<string, TaskProgress>();

        [HttpPost("startTask")]
        public IActionResult StartTask()
        {
            var taskId = Guid.NewGuid().ToString();

            // Creates new task and adds it to the list.
            var initialProgress = new TaskProgress
            {
                Id = taskId,
                Status = "Started",
                Progress = 0
            };

            taskListDictionary[taskId] = initialProgress;

            // Task.Run is used to run a piece of code asynchronously
            Task.Run(() => SimulateTaskAsync(taskId));

            return Ok(taskId);
        }

        [HttpGet("taskStatus/{taskId}")]
        public IActionResult GetTaskStatus(string taskId)
        {
            if (taskListDictionary.TryGetValue(taskId, out var progress))
            {
                return Ok(progress);
            }

            return NotFound("No task found");
        }

        private async Task SimulateTaskAsync(string taskId)
        {
            for (int progress = 0; progress <= 100; progress += 2)
            {
                await Task.Delay(1000); // Simulate work with a delay

                UpdateTaskProgress(taskId, progress);

                UpdateTaskStatus(taskId, progress);
            }
        }

        private void UpdateTaskProgress(string taskId, int progress)
        {
            if (taskListDictionary.TryGetValue(taskId, out var taskProgress))
            {
                taskProgress.Progress = progress;
            }
        }

        private void UpdateTaskStatus(string taskId, int progress)
        {
            if (taskListDictionary.TryGetValue(taskId, out var taskProgress))
            {
                taskProgress.Status = progress == 100 ? "Completed" : "In Progress";
            }
        }
    }
}
