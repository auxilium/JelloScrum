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
    /// Interface for the IJelloSrumRepository
    /// </summary>
    /// <typeparam name="T">een sterk-getypeerd object</typeparam>
    public interface IJelloScrumRepository<T> 
    {
       
        /// <summary>
        /// Geeft een sterk-getypeerd object met het gespecificeerde id terug of null als het object niet bestaat.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        T Get(int id);

        /// <summary>
        /// Geeft het object terug met de gespecificeerde id of een uitzondering als het object niet blijkt te bestaan.
        /// In geval van een proxy/lazy loading krijg je een proxy terug met alleen de id gezet. Later initialiseren
        /// kan resulteren in een uitzondering.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        T Load(int id);

        /// <summary>
        /// Het gespecificeerde object verwijderen uit de database.
        /// </summary>
        /// <param name="entity">The entity.</param>
        void Delete(T entity);

        /// <summary>
        /// Verwijdert alle objecten van type T uit de database.
        /// </summary>
        void DeleteAll();

        /// <summary>
        /// Slaat het gespecificeerde object op in de database.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        T Save(T entity);

        /// <summary>
        /// Slaat een nieuwe of gewijzigd object op in de database.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        T SaveOrUpdate(T entity);

        /// <summary>
        /// Slaat een nieuw of gewijzigd object op, eventueel als kopie onder bepaalde voorwaarden die ik niet zo 1-2-3 begrijp.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
       // T SaveOrUpdateCopy(T entity);

        /// <summary>
        /// Slaat wijzigingen van het gespecificeerde object op in de database.
        /// </summary>
        /// <param name="entity">The entity.</param>
        void Update(T entity);

        /// <summary>
        /// Geeft een collectie van alle objecten van het type T.
        /// </summary>
        /// <returns></returns>
        IList<T> FindAll();

        /// <summary>
        /// Vind één(1) object van het gegeven type. Worden er meer gevonden resulteert dit in een uitzondering.
        /// </summary>
        /// <returns></returns>
        T FindOne();

        /// <summary>
        /// Vind het eerste resultaat
        /// </summary>
        /// <returns></returns>
        T FindFirst();

        /// <summary>
        /// Geeft aan of iets bestaat of zo.
        /// </summary>
        /// <returns></returns>
        bool Exists();

        /// <summary>
        /// Doet iets met tellen of iets dergelijks.
        /// </summary>
        /// <returns></returns>
        long Count();

        /// <summary>
        /// Maakt een nieuwe instantie aan van type T.
        /// </summary>
        /// <returns></returns>
        T Create();
    }
}