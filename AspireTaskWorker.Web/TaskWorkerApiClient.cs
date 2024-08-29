using AspireTaskWorker.ApiService.Models;
using AspireTaskWorker.Web.Services;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

public class TaskWorkerApiClient
{
    private readonly HttpClient _httpClient;

    public TaskWorkerApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<string> StartTask()
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("api/task/startTask", new { });

            if (!response.IsSuccessStatusCode)
            {
                throw new ArgumentException("Failed to start the task.");
            }

            return await response.Content.ReadAsStringAsync();
        }
        catch (HttpRequestException ex)
        {
            throw new ArgumentException("An error occurred while starting the task.", ex);
        }
    }

    public async Task<TaskProgress> CheckTaskStatus(string taskId)
    {
        if (string.IsNullOrWhiteSpace(taskId))
        {
            throw new ArgumentException("Task ID cannot be null or empty.", taskId);
        }

        try
        {
            var response = await _httpClient.GetFromJsonAsync<TaskProgress>($"api/task/taskStatus/{taskId}");
            if (response == null)
            {
                throw new ArgumentException("Failed to retrieve task status.");
            }

            return response;
        }
        catch (HttpRequestException ex)
        {
            throw new ArgumentException($"An error occurred while checking the status for Task ID: {taskId}.", ex);
        }
    }
}
