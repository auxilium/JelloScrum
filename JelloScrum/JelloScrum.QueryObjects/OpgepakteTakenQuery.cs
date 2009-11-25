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
    /// Geef alle opgepakte taken.
    ///  - Specifieer in sprint voor welke sprint dit moet gelden
    ///  - Specificeer in behalveVoorDezeSprintGebruiker van welke sprintgebruiker de taken niet meegenomen moeten worden.
    /// </summary>
    public class OpgepakteTakenQuery
    {
        public Sprint Sprint;
        public SprintGebruiker BehalveVoorDezeSprintGebruiker;

        public ICriteria GetQuery(ISession session)
        {
            ICriteria criteria = session.CreateCriteria(typeof(Task)).Add(Restrictions.Eq("Status", Status.Opgepakt));
                        
            if (this.BehalveVoorDezeSprintGebruiker != null)
                criteria.Add(Restrictions.Not(Restrictions.Eq("Behandelaar", this.BehalveVoorDezeSprintGebruiker)));

            if (this.Sprint != null)
                criteria.CreateCriteria("Behandelaar").Add(Restrictions.Eq("Sprint", this.Sprint));

            return criteria;
        }
    }
}