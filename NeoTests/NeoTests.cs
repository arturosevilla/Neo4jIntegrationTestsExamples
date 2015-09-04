using System;
using Neo4jClient;
using Neo4jClient.Transactions;
using NUnit.Framework;
using TestModel;

namespace NeoTests
{
    /// <summary>
    ///     Examples of unit tests using Neo4jClient
    /// </summary>
    [TestFixture]
    public class NeoTests
    {
        [SetUp]
        public void SetupTransactionContext()
        {
            // open the transaction, by default all future transactions will open and join this transaction.
            // This means that even if they call Commit(), the transaction will not be commited until this
            // transaction is the one that calls Commit(). However this will not happen in TearDown,
            // and therefore the whole changes are rollbacked.
            _graphClient.BeginTransaction();
        }

        [TearDown]
        public void EndTransactionContext()
        {
            // end the transaction as failure
            _graphClient.EndTransaction();
        }

        private EntityRepository _repository;
        private ITransactionalGraphClient _graphClient;

        [TestFixtureSetUp]
        public void SetupTests()
        {
            _graphClient = new GraphClient(new Uri("http://localhost:7474/db/data"));
            _graphClient.Connect();

            _repository = new EntityRepository(_graphClient);
        }

        [Test]
        public void DontRetrieveEntitiesByIncorrectIds()
        {
            // create an entity
            Guid entityId = Guid.NewGuid();
            Entity entity = new Entity
            {
                Id = entityId,
                Name = "Test"
            };

            // save it
            _repository.Add(entity);

            // try to retrieve it by using another id
            Entity storedEntity = _repository.FindById(Guid.NewGuid());
            Assert.IsNull(storedEntity);
        }

        [Test]
        public void StoreAndRetrieveEntity()
        {
            // create an entity
            Guid entityId = Guid.NewGuid();
            Entity entity = new Entity
            {
                Id = entityId,
                Name = "Test"
            };

            // save it
            _repository.Add(entity);

            // retrieve it
            Entity storedEntity = _repository.FindById(entityId);
            Assert.IsNotNull(storedEntity);
            Assert.AreEqual("Test", storedEntity.Name);
            Assert.AreEqual(entityId, storedEntity.Id);
        }

        [Test]
        public void StoreEntity()
        {
            // create an entity
            Guid entityId = Guid.NewGuid();
            Entity entity = new Entity
            {
                Id = entityId,
                Name = "Test"
            };

            // save it
            Entity createdEntity = _repository.Add(entity);
            Assert.IsNotNull(createdEntity);
            Assert.AreEqual("Test", createdEntity.Name);
            Assert.AreEqual(entityId, createdEntity.Id);
        }
    }
}