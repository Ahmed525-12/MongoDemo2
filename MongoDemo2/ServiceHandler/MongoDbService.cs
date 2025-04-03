using MongoDB.Driver;

namespace MongoDemo2.ServiceHandler
{
    public class MongoDbService
    {
        private readonly IConfiguration _configuration;
        private readonly IMongoDatabase? _database;

        public MongoDbService(IConfiguration configuration)
        {
            _configuration = configuration;
            // Retrieve the connection string from configuration.
            var connectionString = _configuration.GetConnectionString("DbConnection");

            // Throw an exception if the connection string is missing or empty.
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentException("Database connection string 'DbConnection' is missing in the configuration.");
            }

            // Create the MongoUrl instance.
            var mongoUrl = MongoUrl.Create(connectionString);

            // Check that the database name is specified.
            if (string.IsNullOrEmpty(mongoUrl.DatabaseName))
            {
                throw new ArgumentException("Database name is missing in the connection string.");
            }

            // Initialize the MongoDB client and get the database.
            var mongoClient = new MongoClient(mongoUrl);
            _database = mongoClient.GetDatabase(mongoUrl.DatabaseName);
        }

        public IMongoCollection<T> GetCollection<T>()
        {
            var collectionName = typeof(T).Name;
            return _database.GetCollection<T>(collectionName);
        }
    }
}