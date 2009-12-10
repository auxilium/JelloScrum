namespace JelloScrum.Login.Authentication
{
    using Castle.MonoRail.Framework;
    using Container;
    using Model;

    /// <summary>
    /// Base filter class used for authenticating users in a Castle MonoRail web environment
    /// You can use this base functionality by subclassing this filter in your Castle MonoRail project
    /// Uses .NET Forms authentication with cookies to verify if the user is authenticated
    /// If the user is not authenticated, he or she is directed to a login page.
    /// </summary>
    public abstract class AuthenticationFilterBase<T> : IFilter where T : class, IUser
    {
        #region IFilter Members
        ///<summary>
        ///
        ///            Implementors should perform they filter logic and
        ///            return 
        ///<c>true</c> if the action should be processed.
        ///            
        ///</summary>
        ///
        ///<param name="exec">When this filter is being invoked</param>
        ///<param name="context">Current context</param>
        ///<param name="controller">The controller instance</param>
        ///<param name="controllerContext">The controller context.</param>
        ///<returns>
        ///
        ///<c>true</c> if the action
        ///            should be invoked, otherwise 
        ///<c>false</c>
        ///</returns>
        ///
        public bool Perform(ExecuteWhen exec, IEngineContext context, IController controller, IControllerContext controllerContext)
        {
            T gebruiker;
            
            try
            {
                gebruiker = IoC.Resolve<IAuthenticationUtil<T>>().Authenticate(context);
            }
            catch
            {
                SendToLoginPage(context);
                return false;
            }

            if(gebruiker == null)
            {
                SendToLoginPage(context);
                return false;
            }
            return true;
        }

        #endregion

        /// <summary>
        /// Redirect the user to the login page
        /// Method should be implemented in subclass, each project has a different url for the login page
        /// </summary>
        /// <param name="context">Current MonoRail http context</param>
        public abstract void SendToLoginPage(IEngineContext context);
    }
}