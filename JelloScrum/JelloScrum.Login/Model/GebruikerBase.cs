namespace Auxilium.Login.Model
{
    using System.Security.Principal;
    using Container;
    using Services;
    using Wachtwoord;

    /// <summary>
    /// Een gebruiker voor een .Net webapplicatie
    /// Implementeert de IIdentity en IPrincipal interfaces, zodat integratie met Forms authentication mogelijk is
    /// </summary>
    public abstract class GebruikerBase : UniqueIdentifyableBase, IIdentity, IGebruiker
    {
        #region Fields
        private const string authenticationType = "Forms";
        protected bool isAuthenticated = false;
        private bool actief;
        private string salt = string.Empty;
        private string wachtWoord = string.Empty;
        private string userName = string.Empty;
        #endregion

        #region Properties
        /// <summary>
        /// De salt waarmee het wachtwoord gehashed is
        /// </summary>
        public virtual string Salt
        {
            get { return this.salt; }
        }

        /// <summary>
        /// Het wachtwoord van de beheerder, gehashed opgeslagen
        /// </summary>
        public virtual string PassWord
        {
            get { return this.wachtWoord; }
        }

        /// <summary>
        /// De gebruikersnaam van de beheerder
        /// </summary>
        public virtual string UserName
        {
            get { return this.userName; }
        }

        /// <summary>
        /// Geeft aan of de gebruiker actief is of niet: 
        /// een niet-actieve gebruiker mag niet inloggen.
        /// </summary>
        public virtual bool Active
        {
            get { return this.actief; }
            set { this.actief = value; }
        }

        #region IPrincipal Members
        /// <summary>
        /// Determines whether the current principal belongs to the specified role.
        /// </summary>
        /// <param name="role">The name of the role for which to check membership.</param>
        /// <returns>
        /// true if the current principal is a member of the specified role; otherwise, false.
        /// </returns>
        public abstract bool IsInRole(string role);

        /// <summary>
        /// Gets the identity of the current principal.
        /// </summary>
        /// <value></value>
        /// <returns>The <see cref="T:System.Security.Principal.IIdentity"/> object associated with the current principal.</returns>
        public virtual IIdentity Identity
        {
            get { return this; }
        }
        #endregion

        #region IIdentity Members
        /// <summary>
        /// Gets the name of the current user.
        /// </summary>
        /// <value></value>
        /// <returns>The name of the user on whose behalf the code is running.</returns>
        public virtual string Name
        {
            get { return UserName; }
        }

        /// <summary>
        /// Gets the type of authentication used.
        /// </summary>
        /// <value></value>
        /// <returns>The type of authentication used to identify the user.</returns>
        public virtual string AuthenticationType
        {
            get { return authenticationType; }
        }

        /// <summary>
        /// Gets a value that indicates whether the user has been authenticated.
        /// </summary>
        /// <value></value>
        /// <returns>true if the user was authenticated; otherwise, false.</returns>
        public virtual bool IsAuthenticated
        {
            get { return this.isAuthenticated; }
        }
        #endregion

        #endregion

        #region Methods
        /// <summary>
        /// Genereert een fonetisch wachtwoord van 8 tekens en zet deze voor deze gebruiker
        /// </summary>
        /// <returns></returns>
        public virtual string GenerateNewPassword()
        {
            string nieuwWachtWoord = WachtWoordHelper.FonetischWachtwoordMaken(8);
            ChangePassWord(nieuwWachtWoord);
            return nieuwWachtWoord;
        }

        /// <summary>
        /// Zet het wachtwoord voor deze gebruiker
        /// Functie maakt een nieuwe salt en encrypt het wachtwoord voor persistentie
        /// </summary>
        /// <param name="newPassWord"></param>
        public virtual void ChangePassWord(string newPassWord)
        {
            if (string.IsNullOrEmpty(newPassWord))
                return;

            this.salt = WachtWoordHelper.GenereerSalt(10);
            this.wachtWoord = WachtWoordHelper.VersleutelenWachtwoord(newPassWord, this.salt);
        }

        /// <summary>
        /// Geeft aan of het opgegeven wachtwoord overeen komt met het wachtwoord van deze gebruiker
        /// </summary>
        /// <param name="passWord"></param>
        /// <returns></returns>
        public virtual bool VerifyPassWord(string passWord)
        {
            return this.wachtWoord == WachtWoordHelper.VersleutelenWachtwoord(passWord, this.salt);
        }

        /// <summary>
        /// Wijzig de gebruikersnaam van deze gebruiker
        /// Kan alleen als de gebruikersnaam uniek is binnen het systeem.
        /// </summary>
        public virtual void ChangeUserName(string newUserName)
        {
            if (IoC.Resolve<ILoginService>().IsValideGebruikersNaam(newUserName, this))
                this.userName = newUserName;
        }
        #endregion
    }
}