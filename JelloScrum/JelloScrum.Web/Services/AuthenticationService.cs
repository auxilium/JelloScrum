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

namespace JelloScrum.Web.Services
{
    using System;
    using System.Security;
    using System.Security.Cryptography;
    using System.Text;
    using System.Threading;
    using System.Web;
    using Castle.MonoRail.Framework;
    using JelloScrum.Model;

    /// <summary>
    /// Represetns the Authentication services.
    /// </summary>
    public class AuthenticationService
    {
        private const string COOKIE_NAME = "JelloScrumAuthenticationticket";

        // /// <summary>
        // /// Sets the authentication cookie. The cookie is valid for one week.
        // /// </summary>
        // /// <param name="user">The user.</param>
        // /// <param name="rememberMe">if set to <c>true</c>, to user will be remembered.</param>
        // /// <param name="context">The monorail engine context which can write the cookie</param>
        // public virtual void SetAuthCookie(User user, bool rememberMe, IEngineContext context)
        // {
        // DateTime day = DateTime.Now.AddDays(1);
        // string idText = user.Id.ToString();
        // byte[] encodedPlaintext = Encoding.Unicode.GetBytes(idText);
        // byte[] ciphertext = ProtectedData.Protect(encodedPlaintext, new byte[] {}, DataProtectionScope.LocalMachine);
        // string cookieText = Convert.ToBase64String(ciphertext);
        // HttpCookie cookie = new HttpCookie(COOKIE_NAME, cookieText);

        // //set the domain to nothing, else the domain is /login, which will not work in, for example, /projectsurvey
        // cookie.Domain = "";
        // cookie.Expires = day;
        // context.Response.CreateCookie(cookie);
        // }

        // /// <summary>
        // /// Signs the out the user.
        // /// </summary>
        // public virtual void SignOut(IEngineContext context)
        // {
        // string cookieText = context.Request.ReadCookie(COOKIE_NAME);

        // //create the same cookie, but with an invalid expiry date

        // HttpCookie cookie = new HttpCookie(COOKIE_NAME, cookieText);

        // cookie.Domain = "";

        // cookie.Expires = DateTime.Now.AddDays(-1);

        // context.Response.CreateCookie(cookie);

        // HttpContext.Current.Session.Abandon();
        // }


        // /// <summary>
        // /// Authenticates the specified context.
        // /// </summary>
        // /// <param name="context">The context.</param>
        // /// <returns></returns>
        // public virtual User Authenticate(IEngineContext context)
        // {
        // string cookieText = context.Request.ReadCookie(COOKIE_NAME);

        // if (cookieText == null || cookieText == string.Empty)
        // {
        // return null;
        // }

        // //decipher the encoded id
        // byte[] cipherText = Convert.FromBase64String(cookieText);
        // string authenticationticket;

        // //try unciphering the encoded id, if it fails, throw a security exception
        // try
        // {
        // byte[] uncipheredText =
        // ProtectedData.Unprotect(cipherText, new byte[] {}, DataProtectionScope.LocalMachine);

        // authenticationticket = Encoding.Unicode.GetString(uncipheredText);
        // }

        // catch (Exception e)
        // {
        // context.Response.CreateCookie(COOKIE_NAME, string.Empty, DateTime.Today.AddDays(-1));
        // throw new SecurityException("The ID in your authenticationcookie is invalid" + Environment.NewLine + e);
        // }

        // //check if there is a result
        // if (authenticationticket == null || authenticationticket == string.Empty)
        // {
        // return null;
        // }

        // int userId;
        // bool result = int.TryParse(authenticationticket, out userId);

        // if (result == false)
        // throw new SecurityException("The ID in your authenticationcookie is invalid");

        // User user = Repository<User>.Load(userId);

        // if (user == null)
        // {
        // throw new SecurityException("The user with this ID has been deleted. This cannot log on anymore.");
        // }

        // context.CurrentUser = user;

        // // So we can use principal permission
        // Thread.CurrentPrincipal = user;

        // return user;
        // }
    }
}