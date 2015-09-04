using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Neo4jClient;
using Neo4jClient.Transactions;

namespace TestModel
{
    /// <summary>
    ///     Repository for entity.
    /// </summary>
    public class EntityRepository
    {
        private ITransactionalGraphClient _graphClient;

        public EntityRepository(ITransactionalGraphClient graphClient)
        {
            _graphClient = graphClient;
        }

        private string GetLabel()
        {
            return typeof (Entity).Name;
        }

        /// <summary>
        ///     Create and saves an entity using a transaction.
        /// </summary>
        /// <param name="entity">The entity to store.</param>
        /// <returns>The created entity.</returns>
        public Entity Add(Entity entity)
        {
            if (entity.Id == Guid.Empty)
            {
                entity.Id = Guid.NewGuid();
            }

            using (ITransaction transaction = _graphClient.BeginTransaction())
            {
                Entity createdEntity = _graphClient.Cypher
                    .Create(string.Format("(e:{0} {{entity}})", GetLabel()))
                    .WithParam("entity", entity)
                    .Return(e => e.As<Entity>())
                    .Results
                    .SingleOrDefault();

                transaction.Commit();

                return createdEntity;
            }
        }

        /// <summary>
        ///     Find an entity without using transactions.
        /// </summary>
        /// <param name="entityId"></param>
        /// <returns></returns>
        public Entity FindById(Guid entityId)
        {
            return _graphClient.Cypher
                .Match(string.Format("(e:{0})", GetLabel()))
                .Where<Entity>(e => e.Id == entityId)
                .Return(e => e.As<Entity>())
                .Results
                .SingleOrDefault();
        }
    }
}
