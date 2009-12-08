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
    using JelloScrum.Model.Entities;
    using NHibernate;
    using NHibernate.Criterion;

    public class AlleTijdregistratiesTussenDatumsPerSprint
    {
        public Sprint sprint;
        public DateTime startDate = new DateTime();
        public DateTime endDate = new DateTime();

        public ICriteria GetQuery(ISession session)
        {
            ICriteria crit = session.CreateCriteria(typeof(TimeRegistration));

            if (startDate != new DateTime() || endDate != new DateTime() || sprint != null)
            {
                crit.Add(Restrictions.Eq("Sprint", sprint));
                crit.Add(Restrictions.Ge("Date", startDate));
                crit.Add(Restrictions.Le("Date", endDate));
            }

            return crit;
        }
    }
}