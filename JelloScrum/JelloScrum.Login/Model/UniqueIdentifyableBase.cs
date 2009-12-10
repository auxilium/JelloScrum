namespace JelloScrum.Login.Model
{
    using System;

    /// <summary>
    /// Base class for entities
    /// Designed to identify a unique entity
    /// </summary>
    public abstract class UniqueIdentifyableBase : IUniqueIdentifyable
    {
        /// <summary>
        /// A globally unique identifier for the object
        /// </summary>
        public abstract Guid Guid { get ; }
        
        /// <summary>
        /// A unique identifier for the object after the object was persisted.
        /// Usually this identifier is used in the database as primary key.
        /// </summary>
        public abstract long Id { get ; }

        /// <summary>
        /// Equality operator so we can have == semantics
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(UniqueIdentifyableBase x, UniqueIdentifyableBase y)
        {
            return Equals(x, y);
        }

        /// <summary>
        /// Inequality operator so we can have != semantics
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(UniqueIdentifyableBase x, UniqueIdentifyableBase y)
        {
            return !(x == y);
        }

        /// <summary>
        /// Bepaald of het gegeven modelbase object gelijk is aan dit modelbase object.
        /// </summary>
        /// <param name="modelBase">Het modelbase object.</param>
        /// <returns>true als de objecten gelijk zijn, anders false.</returns>
        public virtual bool Equals(UniqueIdentifyableBase modelBase)
        {
            return modelBase != null && modelBase.GetType() == GetType() && Equals(this.Guid, modelBase.Guid);
        }

        /// <summary>
        /// Determines whether the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <param name="obj">The <see cref="T:System.Object"/> to compare with the current <see cref="T:System.Object"/>.</param>
        /// <returns>
        /// true if the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>; otherwise, false.
        /// </returns>
        /// <exception cref="T:System.NullReferenceException">The <paramref name="obj"/> parameter is null.</exception>
        public override bool Equals(object obj)
        {
            return ReferenceEquals(this, obj) || Equals(obj as UniqueIdentifyableBase);
        }

        /// <summary>
        /// Serves as a hash function for a particular type.
        /// </summary>
        /// <returns>
        /// A hash code for the current <see cref="T:System.Object"/>.
        /// </returns>
        public override int GetHashCode()
        {
            return 29 * this.Id.GetHashCode() * this.Guid.GetHashCode();
        }
    }
}