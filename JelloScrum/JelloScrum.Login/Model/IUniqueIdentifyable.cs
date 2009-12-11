namespace JelloScrum.Login.Model
{
    using System;

    /// <summary>
    /// Interface which defines classes that have unique identifyable properties
    /// </summary>
    public interface IUniqueIdentifyable
    {
        /// <summary>
        /// The globally unique identifier
        /// </summary>
        Guid Guid { get; }

        /// <summary>
        /// The unique identifier when the object has been persisted.
        /// </summary>
        long Id { get; }
    }
}