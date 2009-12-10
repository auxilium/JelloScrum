namespace JelloScrum.Login.Authentication
{
    using System;
    using System.Security;
    using System.Security.Cryptography;
    using System.Text;
    using System.Threading;
    using System.Web;
    using Castle.MonoRail.Framework;
    using Model;
    using Services;

    /// <summary>
    /// Utility class for authentication of users in a Castle MonoRail project.
    /// Uses .NET Forms authentication and cookies to authenticate users. 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class AuthenticationUtil<T> : IAuthenticationUtil<T> where T : class, IUser
    {
        private readonly string cookieName = "Auxilium_Authentication_Cookie";
        private readonly ILoginService<T> loginService;
        private readonly int numberOfDaysCookieIsValid;

        public AuthenticationUtil(ILoginService<T> loginService, string cookieName, int? numberOfDaysCookieIsValid)
        {
            this.loginService = loginService;
            if (!string.IsNullOrEmpty(cookieName))
                this.cookieName = cookieName;
            if (numberOfDaysCookieIsValid.HasValue)
                this.numberOfDaysCookieIsValid = numberOfDaysCookieIsValid.Value;
        }

        /// <summary>
        /// Authenticate the current user by checking if the user credentials 
        /// in the ASP.Net cookie are present and valid.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public T Authenticate(IEngineContext context)
        {
            string cookieText = context.Request.ReadCookie(this.cookieName);

            if (string.IsNullOrEmpty(cookieText))
            {
                return null;
            }

            //Read the cookie text into a byte array
            byte[] cipherText = Convert.FromBase64String(cookieText);
            string authenticationticket;

            //try to uncipher the cookie text, the user id is encrypted, so decrypt here
            try
            {
                byte[] uncipheredText = ProtectedData.Unprotect(cipherText, new byte[] {}, DataProtectionScope.LocalMachine);
                authenticationticket = Encoding.Unicode.GetString(uncipheredText);
            }
            catch (Exception e)
            {
                context.Response.CreateCookie(CreateCookie(-1));
                throw new SecurityException("The provided user identification is not valid." + Environment.NewLine + e);
            }

            //Let's see if we have any result
            if (string.IsNullOrEmpty(authenticationticket))
            {
                throw new SecurityException("The provided user identification is not valid.");
            }

            long userId;
            bool result = long.TryParse(authenticationticket, out userId);

            if (result == false)
            {
                throw new SecurityException("The provided user identification is not valid.");
            }
            
            T user = this.loginService.GetUser(userId);

            if (user == null)
            {
                throw new SecurityException("The user specified in the credentials cannot be found in the database");
            }
            if (!user.IsActive)
            {
                throw new SecurityException("The user has been deactivated.");
            }

            context.CurrentUser = user;

            Thread.CurrentPrincipal = user;
            return user;
        }

        /// <summary>
        /// Authenticate the current user by checking if the specified user credentials are valid.
        /// This method also writes a new cookie into the http Response.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="userName"></param>
        /// <param name="passWord"></param>
        /// <returns></returns>
        public T Authenticate(IEngineContext context, string userName, string passWord)
        {
            if (!this.loginService.IsAllowedToLogin(userName, passWord))
            {
                return null;
            }

            T user = this.loginService.GetUser(userName, passWord);
            SetAuthCookie(user, context); 
            context.CurrentUser = user;

            Thread.CurrentPrincipal = user;
            return user;
        }

        /// <summary>
        /// Write an ASP.Net cookie into the http response.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="context"></param>
        public void SetAuthCookie(T user, IEngineContext context)
        {
            HttpCookie cookie = CreateCookie(this.numberOfDaysCookieIsValid);

            string idText = user.Id.ToString();
            byte[] encodedPlaintext = Encoding.Unicode.GetBytes(idText);
            byte[] ciphertext = ProtectedData.Protect(encodedPlaintext, new byte[] {}, DataProtectionScope.LocalMachine);
            string cookieText = Convert.ToBase64String(ciphertext);

            cookie.Value = cookieText;
            context.Response.CreateCookie(cookie);
        }

        /// <summary>
        /// Replace the ASP.Net cookie with one which does not lead to a succesfull user authentication.
        /// </summary>
        /// <param name="context">The current MonoRail http context</param>
        public void SignOut(IEngineContext context)
        {
            HttpCookie cookie = CreateCookie(-1);
            cookie.Value = context.Request.ReadCookie(this.cookieName);
            context.Response.CreateCookie(cookie);
            HttpContext.Current.Session.Abandon();
        }

        /// <summary>
        /// Create a valid ASP.Net cookie, which leads to a succesfull user authentication.
        /// The cookie is valid for the specified number of days.
        /// </summary>
        /// <param name="numberOfDays">Number of days the cookie is valid.</param>
        /// <returns></returns>
        private HttpCookie CreateCookie(int numberOfDays)
        {
            HttpCookie cookie = new HttpCookie(this.cookieName);
            cookie.Domain = "";
            cookie.Expires = DateTime.Now.AddDays(numberOfDays);
            return cookie;
        }
    }
}