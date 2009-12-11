namespace JelloScrum.Login.Services
{
    using Model;

    /// <summary>
    /// Interface for a service which takes care of different login services,
    /// like checking a password, retrieving users by their username or id or checking if a username is valid
    /// </summary>
    public interface ILoginService<T> where T : class, IUser
    {
        /// <summary>
        /// Check if the username / password combination is valid for user in this application
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="passWord"></param>
        /// <returns></returns>
        bool CheckPassWord(string userName, string passWord);

        /// <summary>
        /// Checks if the username / password combination is valid for a 
        /// user in this application and if the user is still allowed to login
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="passWord"></param>
        /// <returns></returns>
        bool IsAllowedToLogin(string userName, string passWord);

        /// <summary>
        /// Retrieve the user object by it's Id
        /// Return NULL if user doesn't exist
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        T GetUser(long id);


        /// <summary>
        /// Retrieve the user object by the username / password combination
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="passWord"></param>
        /// <returns></returns>
        T GetUser(string userName, string passWord);

        /// <summary>
        /// Indicates if the username is valid and unique in this application
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        bool IsUserNameValid(string userName, T user);
    }
}