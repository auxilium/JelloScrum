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

namespace JelloScrum.Services
{
    using System;
    using System.Security;
    using System.Security.Cryptography;
    using System.Text;
    using System.Threading;
    using System.Web;
    using Castle.MonoRail.Framework;
    using Container;
    using JelloScrum.Model.Entities;
    using JelloScrum.Model.IRepositories;
    using JelloScrum.Model.Services;

    public class AuthenticationService : IAuthenticationService
    {
        private readonly IGebruikerRepository userService = IoC.Resolve<IGebruikerRepository>();
        private string cookie_name = string.Empty;

        public string Cookie_name
        {
            get { return cookie_name; }
            set { cookie_name = value; }
        }

        /// <summary>
        /// Zet het koekje naar het verleden, en verlaat de sessie.
        /// </summary>
        /// <param name="context">Gebruiker context</param>
        public void SignOut(IEngineContext context)
        {
            HttpCookie koekje = CreateCookie(-1);
            koekje.Value = context.Request.ReadCookie(Cookie_name);
            context.Response.CreateCookie(koekje);
            HttpContext.Current.Session.Abandon();
        }

        /// <summary>
        /// Zet het authenticatie koekje
        /// </summary>
        /// <param name="user">gebruiker die inlogt.</param>
        /// <param name="context">Gebruikers context</param>
        public void SetAuthCookie(User user, IEngineContext context)
        {
            HttpCookie cookie = CreateCookie(7);

            string idText = user.Id.ToString();
            byte[] encodedPlaintext = Encoding.Unicode.GetBytes(idText);
            byte[] ciphertext = ProtectedData.Protect(encodedPlaintext, new byte[] {}, DataProtectionScope.LocalMachine);
            string cookieText = Convert.ToBase64String(ciphertext);

            cookie.Value = cookieText;
            context.Response.CreateCookie(cookie);
        }

        /// <summary>
        /// Veriveerd de gebruikers recht om in te loggen
        /// </summary>
        /// <param name="context">De gebruiker context</param>
        /// <returns>een gebruiker</returns>
        public User Authenticatie(IEngineContext context)
        {
            string cookieText = context.Request.ReadCookie(Cookie_name);

            if (string.IsNullOrEmpty(cookieText))
            {
                throw new SecurityException("cookieText == null");
            }

            byte[] cipherText = Convert.FromBase64String(cookieText);
            string authenticationticket;

            try
            {
                byte[] uncipheredText = ProtectedData.Unprotect(cipherText, new byte[] {}, DataProtectionScope.LocalMachine);
                authenticationticket = Encoding.Unicode.GetString(uncipheredText);
            }
            catch (Exception e)
            {
                context.Response.CreateCookie(CreateCookie(-1));
                throw new SecurityException("De gebruikers identificatie is niet geldig" + Environment.NewLine + e);
            }

            if (string.IsNullOrEmpty(authenticationticket))
            {
                throw new SecurityException("authenticationticket == null");
            }

            int userId;
            bool result = int.TryParse(authenticationticket, out userId);

            if (result == false)
            {
                throw new SecurityException("De gebruikers identificatie is niet geldig");
            }

            User user = userService.Load(userId);

            if (user == null)
            {
                throw new SecurityException("De gebruiker uit het koekje bestaat niet meer.");
            }

            if (!user.IsActive)
            {
                throw new SecurityException("Deze gebruiker is niet actief");
            }

            context.CurrentUser = user;

            Thread.CurrentPrincipal = user;
            return user;
        }

        /// <summary>
        /// Maakt het koekje
        /// </summary>
        /// <param name="aantalDagen">Aantal dagen dat het koekje geldig is</param>
        /// <returns>een HTTPCookie koekje</returns>
        private HttpCookie CreateCookie(int aantalDagen)
        {
            HttpCookie cookie = new HttpCookie(Cookie_name);
            cookie.Domain = "";
            cookie.Expires = DateTime.Now.AddDays(aantalDagen);
            return cookie;
        }
    }
}