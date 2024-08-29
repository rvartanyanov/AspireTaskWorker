using StackExchange.Redis;

namespace AspireTaskWorker.Web.Services
{
    public class RedisCacheService
    {
        private readonly IConnectionMultiplexer _redis;
        private readonly IDatabase _database;

        public RedisCacheService(IConnectionMultiplexer connectionMultiplexer)
        {
            _redis = connectionMultiplexer;
            _database = _redis.GetDatabase();
        }

        public void SetTaskProgress(string taskId, int progress)
        {
            _database.StringSet(taskId, progress);
        }

        public string? GetTaskProgress(string taskId)
        {
            return _database.StringGet(taskId);
        }
    }
}
