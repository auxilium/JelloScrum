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
    using System;
    using Model.Entities;
    using NHibernate;
    using NHibernate.Criterion;

    /// <summary>
    /// Query timeregistrations 
    /// </summary>
    public class AllTimeRegistrationsBetweenDatesForSprint
    {
        public Sprint Sprint;
        public DateTime? StartDate;
        public DateTime? EndDate;

        /// <summary>
        /// Query timeregistrations (between the given start and enddate for the given sprint)
        /// One of Sprint/StartDate/EndDate needs to be set.
        /// </summary>
        /// <param name="session">The session.</param>
        /// <returns></returns>
        public ICriteria GetQuery(ISession session)
        {
            ICriteria crit = session.CreateCriteria(typeof(TimeRegistration));

            if (StartDate.HasValue || EndDate.HasValue || Sprint != null)
            {
                crit.Add(Restrictions.Eq("Sprint", Sprint));
                crit.Add(Restrictions.Ge("Date", StartDate));
                crit.Add(Restrictions.Le("Date", EndDate));
            }

            return crit;
        }
    }
}