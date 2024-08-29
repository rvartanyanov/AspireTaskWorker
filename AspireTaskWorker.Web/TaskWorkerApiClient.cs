using AspireTaskWorker.ApiService.Controllers;

public class TaskWorkerApiClient
{
    private readonly HttpClient _httpClient;

    public TaskWorkerApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<string> StartTask()
    {
        var response = await _httpClient.PostAsJsonAsync("api/task/startTask", new { });
        return await response.Content.ReadAsStringAsync();
    }

    public async Task<TaskProgress> CheckTaskStatus(string taskId)
    {
        return await _httpClient.GetFromJsonAsync<TaskProgress>($"api/task/taskStatus/{taskId}");
    }
}
