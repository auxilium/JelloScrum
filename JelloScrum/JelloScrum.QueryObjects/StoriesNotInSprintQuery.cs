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
    /// Query stories not in the given sprint.
    /// </summary>
    public class StoriesNotInSprintQuery
    {
        /// <summary>
        /// Query all stories not in the given sprint.
        /// </summary>
        /// <param name="session">The session.</param>
        /// <param name="sprint">The sprint.</param>
        /// <returns></returns>
        public ICriteria GetQuery(ISession session, Sprint sprint)
        {
            NotClosedStoriesQuery allStoriesInProject = new NotClosedStoriesQuery();
            
            if (sprint.Project != null)
                allStoriesInProject.Project = sprint.Project;

            ICriteria criteria = allStoriesInProject.GetQuery(session);
            
            DetachedCriteria allSprintStoriesBelongingToSprint = DetachedCriteria.For(typeof (SprintStory))
                .SetProjection(Projections.Property("Story.Id"))
                .Add(Restrictions.Eq("Sprint", sprint));

            criteria.Add(Subqueries.PropertyNotIn("Id", allSprintStoriesBelongingToSprint)).SetResultTransformer(
                CriteriaSpecification.DistinctRootEntity);

            return criteria;
        }
    }
}
