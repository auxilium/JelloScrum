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

    public class ProjectQuery
    {
        public string zoekterm;
        
        public ICriteria GetQuery(ISession session)
        {
            ICriteria crit = session.CreateCriteria(typeof (Project));

            if (!string.IsNullOrEmpty(zoekterm))
            {
                string interneZoekterm = "%" + zoekterm + "%";
                crit.Add(Restrictions.Or(Restrictions.Like("Naam", interneZoekterm),Restrictions.Like("Omschrijving", interneZoekterm)));
            }

            return crit;
        }
    }
}