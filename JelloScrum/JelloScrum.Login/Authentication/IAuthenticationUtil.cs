namespace JelloScrum.Login.Authentication
{
    using Castle.MonoRail.Framework;
    using Model;

    /// <summary>
    /// Defines a utility which can be used for authenticating a user in an Castle MonoRail environment.
    /// </summary>
    public interface IAuthenticationUtil<T> where  T : class, IUser
    {
        /// <summary>
        /// Authenticate the user from the context and return it
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        T Authenticate(IEngineContext context);

        /// <summary>
        /// Checks if the username / password combination is valid
        /// Returns the user and writes an authentication cookie in the http Response object 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="userName"></param>
        /// <param name="passWord"></param>
        /// <returns></returns>
        T Authenticate(IEngineContext context, string userName, string passWord);

        /// <summary>
        /// Write an authentication cookie to the http Response object
        /// </summary>
        /// <param name="gebruiker"></param>
        /// <param name="context"></param>
        void SetAuthCookie(T gebruiker, IEngineContext context);

        /// <summary>
        /// Deactivate the authentication cookie so user is not authenticated anymore.
        /// </summary>
        /// <param name="context"></param>
        void SignOut(IEngineContext context);
    }
}