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
    using NHibernate;
    using NHibernate.Criterion;

    /// <summary>
    /// Query for tasks
    /// </summary>
    public class TasksOfSprintQuery
    {
        public Sprint Sprint;

        /// <summary>
        /// Query for all tasks that belong to the given sprint.
        /// </summary>
        /// <param name="session">The session.</param>
        /// <returns></returns>
        public ICriteria GetQuery(ISession session)
        {
            ICriteria crit = session.CreateCriteria(typeof (Task));
            
            if (Sprint != null)
            {
                crit.CreateCriteria("Story").CreateCriteria("SprintStories").Add(Restrictions.Eq("Sprint", Sprint));
            }

            return crit;
        }
    }
}