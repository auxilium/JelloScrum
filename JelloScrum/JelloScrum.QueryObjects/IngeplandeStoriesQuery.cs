namespace JelloScrum.QueryObjects
{
    using Model.Entities;
    using NHibernate;
    using NHibernate.Criterion;

    public class IngeplandeStoriesQuery
    {
        public Sprint Sprint;

        /// <summary>
        /// Initializes a new instance of the <see cref="IngeplandeStoriesQuery"/> class.
        /// </summary>
        public IngeplandeStoriesQuery()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IngeplandeStoriesQuery"/> class.
        /// </summary>
        /// <param name="sprint">The sprint.</param>
        public IngeplandeStoriesQuery(Sprint sprint)
        {
            Sprint = sprint;
        }

        /// <summary>
        /// Geeft de Criteria voor het opvragen van: Stories die ingepland zijn in een bepaalde sprint
        /// </summary>
        /// <param name="session">The session.</param>
        /// <returns></returns>
        public ICriteria GetQuery(ISession session)
        {
            ICriteria criteria = session.CreateCriteria(typeof (Story));
            if (Sprint != null)
            {
                // all SprintStories of the story where sprint.id == Sprint.Id
                criteria.CreateCriteria("SprintStories").CreateCriteria("Sprint").Add(Restrictions.Eq("Id", Sprint.Id));
            }
            else
            {
                // all stories with no sprintstories
                criteria.CreateCriteria("SprintStories").Add(Restrictions.IsNotNull("Id"));
            }
            return criteria;
        }
    }
}
