namespace JelloScrum.Login.QueryObject
{
    using Model;
    using NHibernate.Criterion;

    /// <summary>
    /// Query object for querying users on their username
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class UsersWithUserNameQuery<T> where T : class, IUser
    {
        /// <summary>
        /// Query users by their username
        /// </summary>
        /// <param name="userName"></param>
        /// <returns>/returns>
        public DetachedCriteria GetQuery(string userName)
        {
            DetachedCriteria criteria = DetachedCriteria.For(typeof (T))
                .Add(Restrictions.Eq("UserName", userName));

            return criteria;
        }
    }
}