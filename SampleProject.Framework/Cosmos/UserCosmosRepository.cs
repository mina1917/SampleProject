using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;

namespace SampleProject.Framework.Cosmos
{
    public class UserCosmosRepository : CosmosRepository
    {
        private const string UserContainerName = "User";

        public UserCosmosRepository(CosmosClient cosmosClient, IConfiguration configuration)
            : base(GetContainer(cosmosClient, configuration))
        {
        }

        private static Container GetContainer(CosmosClient cosmosClient, IConfiguration configuration)
        {
            var databaseName = configuration.GetSection("CosmosDb:DatabaseName").Value;
            return cosmosClient.GetContainer(databaseName, UserContainerName);
        }
    }
}