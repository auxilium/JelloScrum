// Copyright 2009 Auxilium B.V. - http://www.auxilium.nl/
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

namespace JelloScrum.Model.IRepositories
{
    using System.Collections.Generic;

    /// <summary>
    /// Interface for the JelloSrumRepository
    /// </summary>
    /// <typeparam name="T">a stronly typed object</typeparam>
    public interface IJelloScrumRepository<T> 
    {
        /// <summary>
        /// Gets object T with the given id, or null if no object T with given id exists.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        T Get(int id);

        /// <summary>
        /// Loads the object with the given id from the databse. If no object with that id exists, an exception is thrown.
        /// In case of proxy / lazy loading you get a proxy with only the id set. Later initialisation can result in an exception
        /// (because only then nhibernate really tries to get the object from the database(so it might not exist)).
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        T Load(int id);

        /// <summary>
        /// Delete the given entity from the database
        /// </summary>
        /// <param name="entity">The entity.</param>
        void Delete(T entity);

        /// <summary>
        /// Delete all objects T from the database
        /// </summary>
        void DeleteAll();

        /// <summary>
        /// Save the given entity in the database
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        T Save(T entity);

        /// <summary>
        /// Save a new or updated entity in the database
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        T SaveOrUpdate(T entity);
        
        /// <summary>
        /// Save the updated entity in the database
        /// </summary>
        /// <param name="entity">The entity.</param>
        void Update(T entity);

        /// <summary>
        /// Gets a list of all objects of type T
        /// </summary>
        /// <returns></returns>
        IList<T> FindAll();

        /// <summary>
        /// Find one (1) object of the given type. If more than one objects are found an exception is thrown.
        /// </summary>
        /// <returns></returns>
        T FindOne();

        /// <summary>
        /// Find the first result
        /// </summary>
        /// <returns></returns>
        T FindFirst();

        /// <summary>
        /// Determines of the given object exists
        /// </summary>
        /// <returns></returns>
        bool Exists();

        /// <summary>
        /// Count
        /// </summary>
        /// <returns></returns>
        long Count();

        /// <summary>
        /// Create a new instance of type T
        /// </summary>
        /// <returns></returns>
        T Create();
    }
}