namespace JelloScrum.QueryObjects
{
    using Model.Entities;
    using NHibernate;
    using NHibernate.Criterion;

    public class NietIngeplandeStoriesQuery
    {
        public Project project;

        public ICriteria GetQuery(ISession session)
        {
            ICriteria criteria = session.CreateCriteria(typeof(Story));
            criteria.Add(Restrictions.IsEmpty("SprintStories"));

            if(project != null)
            {
                criteria.CreateCriteria("Project").Add(Restrictions.Eq("Id", project.Id));
            }
            return criteria;
        }
    }
}
