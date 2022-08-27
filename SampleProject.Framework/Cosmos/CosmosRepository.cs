using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using System.Linq.Expressions;

namespace SampleProject.Framework.Cosmos
{
    public abstract class CosmosRepository
    {
        private readonly Container _container;

        protected CosmosRepository(Container container)
        {
            _container = container;
        }

        public async Task AddItemAsync<T>(T item) where T : QueryModelBase<string>
        {
            await _container.CreateItemAsync(item, new PartitionKey(item.PartitionKey));
        }

        public async Task AddItemsAsync<T>(List<T> items, string partitionKey) where T : QueryModelBase<string>
        {
            var transactionalBatch = _container.CreateTransactionalBatch(new PartitionKey(partitionKey));
            foreach (var item in items)
            {
                transactionalBatch.CreateItem(item);
            }
            //todo check delete transaction 
            await transactionalBatch.ExecuteAsync();
        }

        public async Task DeleteItemAsync<T>(string partitionKey, string id)
        {
            await _container.DeleteItemAsync<T>(id, new PartitionKey(partitionKey));
        }

        public async Task<T> GetItemAsync<T>(string partitionKey, string id)
        {
            try
            {
                var response = await _container.ReadItemAsync<T>(id, new PartitionKey(partitionKey));

                return response.Resource;
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return default;
            }
        }

        public async Task<List<T>> GetItemAsyncUsingLinq<T>(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken, int take = 10, int skip = 0)
        {
            try
            {
                using var setIterator = _container.GetItemLinqQueryable<T>()
                    .Where(predicate)
                    .Skip(skip)
                    .Take(take)
                    .ToFeedIterator();

                var results = new List<T>();

                while (setIterator.HasMoreResults)
                {
                    var response = await setIterator.ReadNextAsync(cancellationToken);

                    results.AddRange(response.ToList());
                }

                return results;
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return default;
            }
        }

        public async Task<List<T>> GetItemAsyncUsingLinq<T>(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken)
        {
            try
            {
                using var setIterator = _container.GetItemLinqQueryable<T>().Where(predicate).ToFeedIterator();
                var results = new List<T>();
                while (setIterator.HasMoreResults)
                {
                    var response = await setIterator.ReadNextAsync(cancellationToken);

                    results.AddRange(response.ToList());
                }

                return results;
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return default;
            }
        }

        public async Task<T> GetFirstItemAsyncUsingLinq<T>(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken)
        {
            try
            {
                using var setIterator = _container.GetItemLinqQueryable<T>()
                    .Where(predicate)
                    .Skip(0)
                    .Take(1)
                    .ToFeedIterator();

                var results = new List<T>();

                while (setIterator.HasMoreResults)
                {
                    var response = await setIterator.ReadNextAsync(cancellationToken);

                    results.AddRange(response.ToList());
                }

                return results.FirstOrDefault();
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return default;
            }
        }

        public async Task<bool> IsExist<T>(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken)
        {
            try
            {
                using var setIterator = _container.GetItemLinqQueryable<T>().Where(predicate)
                    .Skip(0).Take(1).ToFeedIterator();
                var results = new List<T>();
                while (setIterator.HasMoreResults)
                {
                    var response = await setIterator.ReadNextAsync(cancellationToken);

                    results.AddRange(response.ToList());
                }

                return results.Any();
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return false;
            }
        }

        public async Task<List<T>> GetItemsAsync<T>(string queryString)
        {
            var query = _container.GetItemQueryIterator<T>(new QueryDefinition(queryString));
            var results = new List<T>();
            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();
                results.AddRange(response.Resource.ToList());
            }

            return results;
        }

        public async Task<T> GetFirstItemsAsync<T>(string queryString)
        {
            var query = _container.GetItemQueryIterator<T>(new QueryDefinition(queryString));
            var results = new List<T>();
            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();
                results.AddRange(response.Resource.ToList());
            }

            return results.FirstOrDefault();
        }
        public async Task<List<TResult>> GetItemsAsyncBySelectMany<TSource, TResult>(string queryString, Func<TSource, IEnumerable<TResult>> selector)
        {
            var query = _container.GetItemQueryIterator<TSource>(new QueryDefinition(queryString));
            var results = new List<TResult>();
            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();
                results.AddRange(response.Resource.ToList().SelectMany(selector));
            }

            return results;
        }

        public async Task UpdateItemAsync<T>(string partitionKey, T item) where T : QueryModelBase<string>
        {
            await _container.ReplaceItemAsync<T>(item, item.Id, new PartitionKey(partitionKey),
                new ItemRequestOptions() { IfMatchEtag = item._etag });
        }

        public async Task UpdateItemAsync<T>(List<T> items, string partitionKey) where T : QueryModelBase<string>
        {
            var transactionalBatch = _container.CreateTransactionalBatch(new PartitionKey(partitionKey));
            foreach (var item in items)
            {
                transactionalBatch.ReplaceItem(item.Id, item, new TransactionalBatchItemRequestOptions() { IfMatchEtag = item._etag });
            }
            await transactionalBatch.ExecuteAsync();
        }

        public async Task DeleteItemAsync<T>(List<T> items, string partitionKey) where T : QueryModelBase<string>
        {
            var transactionalBatch = _container.CreateTransactionalBatch(new PartitionKey(partitionKey));
            foreach (var item in items)
            {
                transactionalBatch.DeleteItem(item.Id);
            }
            await transactionalBatch.ExecuteAsync();
        }

        public async Task ReInsertItemsAsync<T>(List<T> oldItems, List<T> newItems, string partitionKey) where T : QueryModelBase<string>
        {
            var transactionalBatch = _container.CreateTransactionalBatch(new PartitionKey(partitionKey));
            foreach (var item in oldItems)
            {
                transactionalBatch.DeleteItem(item.Id);
            }
            foreach (var item in newItems)
            {
                transactionalBatch.CreateItem(item);
            }
            await transactionalBatch.ExecuteAsync();
        }
    }
}