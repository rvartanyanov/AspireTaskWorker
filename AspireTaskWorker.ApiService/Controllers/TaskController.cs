using Microsoft.AspNetCore.Mvc;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace AspireTaskWorker.ApiService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TaskController : ControllerBase
    {
        private static readonly ConcurrentDictionary<string, TaskProgress> TaskList = new ConcurrentDictionary<string, TaskProgress>();

        [HttpPost("startTask")]
        public IActionResult StartTask()
        {
            var taskId = Guid.NewGuid().ToString();
            TaskList[taskId] = new TaskProgress { Status = "Started", Progress = 0 };

            _ = Task.Run(() => SimulateTask(taskId));

            return Ok(taskId);
        }

        [HttpGet("taskStatus/{taskId}")]
        public IActionResult GetTaskStatus(string taskId)
        {
            return TaskList.TryGetValue(taskId, out var progress) ? Ok(progress) : NotFound("No task found");
        }

        private async Task SimulateTask(string taskId)
        {
            for (int i = 0; i <= 100; i += 2)
            {
                await Task.Delay(1000); // Simulate work
                TaskList[taskId].Progress = i;
                TaskList[taskId].Status = i == 100 ? "Completed" : "In Progress";
            }
        }
    }

    public class TaskProgress
    {
        public string Id { get; set; }
        public string Status { get; set; }
        public int Progress { get; set; }
    }
}
