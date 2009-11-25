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

    public class NogNietAfgerondeStoriesQuery
    {
        public Project Project;
        
        public ICriteria GetQuery(ISession session)
        {
            ICriteria criteria = session.CreateCriteria(typeof(Story));
            DetachedCriteria detachedCriteria =
                DetachedCriteria.For(typeof(Task))
                .SetProjection(Projections.Property("Id"))
                .Add(Restrictions.Not(Restrictions.Eq("Status", Status.Afgesloten)));

            if(Project != null)
            {
                criteria.CreateCriteria("Project").Add(Restrictions.Eq("Id", Project.Id));
            }


            criteria.CreateCriteria("Tasks").Add(Subqueries.PropertyIn("Id", detachedCriteria)).SetResultTransformer(CriteriaSpecification.DistinctRootEntity);

            return criteria;
        }
    }
}
