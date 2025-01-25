using MongoDB.Driver;

namespace prescriptionSystemApi.context
{
    public class noSqlContext
    {
        private readonly IMongoDatabase _database;

        public noSqlContext(IConfiguration configuration) {
            var connectionString = configuration["MongoDBSettings:ConnectionString"];
            var databaseName = configuration["MongoDBSettings:DatabaseName"];

            var client = new MongoClient(connectionString);
            _database = client.GetDatabase(databaseName);
        }

        public IMongoCollection<T> GetCollection<T>(string name)
        {
            return _database.GetCollection<T>(name);
        }
    }
}
