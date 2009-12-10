namespace JelloScrum.Model.Services
{
    using System.Collections.Generic;

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IJelloScrumRepository<T> where T : ModelBase
    {
        /// <summary>
        /// Gets the specified id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        T Get(int id);
        /// <summary>
        /// Loads the specified id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        T Load(int id);
        /// <summary>
        /// Deletes the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        void Delete(T entity);
        /// <summary>
        /// Deletes all.
        /// </summary>
        void DeleteAll();
        /// <summary>
        /// Saves the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        T Save(T entity);
        /// <summary>
        /// Saves the or update.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        T SaveOrUpdate(T entity);
        /// <summary>
        /// Saves the or update copy.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        T SaveOrUpdateCopy(T entity);
        /// <summary>
        /// Updates the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        void Update(T entity);
        /// <summary>
        /// Finds all.
        /// </summary>
        /// <returns></returns>
        ICollection<T> FindAll();
        /// <summary>
        /// Finds the one.
        /// </summary>
        /// <returns></returns>
        T FindOne();
        /// <summary>
        /// Finds the first.
        /// </summary>
        /// <returns></returns>
        T FindFirst();
        /// <summary>
        /// Existses this instance.
        /// </summary>
        /// <returns></returns>
        bool Exists();
        /// <summary>
        /// Counts this instance.
        /// </summary>
        /// <returns></returns>
        long Count();
        /// <summary>
        /// Creates this instance.
        /// </summary>
        /// <returns></returns>
        T Create();
    }
}