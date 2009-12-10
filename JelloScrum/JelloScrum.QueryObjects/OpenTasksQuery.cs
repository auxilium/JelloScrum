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

namespace JelloScrum.QueryObjects
{
    using Model.Entities;
    using Model.Enumerations;
    using NHibernate;
    using NHibernate.Criterion;

    /// <summary>
    /// Query open tasks
    /// </summary>
    public class OpenTasksQuery
    {
        public Sprint Sprint;

        /// <summary>
        /// Gets all open tasks (for a specific sprint if specified)
        /// </summary>
        /// <param name="session">The session.</param>
        /// <returns></returns>
        public ICriteria GetQuery(ISession session)
        {
            ICriteria criteria = session.CreateCriteria(typeof(Task)).Add(Restrictions.Eq("State", State.Open));
            
            if (Sprint != null)
                criteria.CreateCriteria("Story").CreateCriteria("SprintStories").Add(Restrictions.Eq("Sprint", Sprint));
            
            return criteria;
        }
    }
}