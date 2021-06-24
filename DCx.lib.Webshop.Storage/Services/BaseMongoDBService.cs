using DCx.lib.Webshop.Storage.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace DCx.lib.Webshop.Storage.Services
{
    public class DatabaseSettings : IDatabaseSettings
    {
        public string CollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }

    public interface IDatabaseSettings
    {
        string CollectionName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }

    public class BaseMongoDBService<T> where T : MongoEntity
    {
        private readonly IMongoDatabase _database;
        protected readonly IMongoCollection<T> _documents;
        private string _collectionName;

        public BaseMongoDBService(IDatabaseSettings settings)
        {
            _collectionName = typeof(T).Name;

            var client = new MongoClient(settings.ConnectionString);
            _database = client.GetDatabase(settings.DatabaseName);
            _documents = _database.GetCollection<T>(_collectionName);
        }

        public List<T> Get() =>
            _documents.Find(document => true).ToList();

        public List<T> Get(Expression<Func<T, bool>> filter) =>
            _documents.Find(filter).ToList();


        public T Get(string id) =>
            _documents.Find(document => document.Id == id).FirstOrDefault();


        public T Create(T document)
        {
            _documents.InsertOne(document);
            return document;
        }

        public void Update(string id, T documentIn) =>
            _documents.ReplaceOne(document => document.Id == id, documentIn);

        public void Remove(T documentIn) =>
            _documents.DeleteOne(document => document.Id == documentIn.Id);

        public void Remove(string id) =>
            _documents.DeleteOne(document => document.Id == id);

        public void DeleteMany(FilterDefinition<T> definition) => _documents.DeleteMany(definition);

        public void DropCollection() => _database.DropCollection(_collectionName);
    }
}
