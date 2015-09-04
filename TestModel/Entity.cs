using System;
using System.Runtime.Serialization;

namespace TestModel
{
    /// <summary>
    ///     model entity.
    /// </summary>
    [DataContract]
    public class Entity
    {
        /// <summary>
        ///     Id
        /// </summary>
        [DataMember]
        public Guid Id { get; set; }

        /// <summary>
        ///     Name
        /// </summary>
        [DataMember]
        public string Name { get; set; }
    }
}