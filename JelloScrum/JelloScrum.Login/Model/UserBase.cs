namespace JelloScrum.Login.Model
{
    using System.Security.Principal;
    using Container;
    using Services;
    using Wachtwoord;

    /// <summary>
    /// Base class for users in a .Net context
    /// Implements IIdentity and IPrincipal
    /// Contains standard behaviour for dealing with passwords, encrypting and salts.
    /// Methods and properties are mostly virtual, so you can override the default behaviour.
    /// Use this class as a base class for users in your .Net application
    /// </summary>
    public abstract class UserBase<T> : UniqueIdentifyableBase, IIdentity, IUser where T : class, IUser
    {
        #region Fields
        private const string authenticationType = "Forms";
        private bool isAuthenticated = false;
        private bool actief = true;
        private string salt = string.Empty;
        private string passWord = string.Empty;
        private string userName = string.Empty;
        #endregion

        #region Properties
        /// <summary>
        /// Salt used to encrypt the password
        /// </summary>
        public virtual string Salt
        {
            get { return this.salt; }
        }

        /// <summary>
        /// The password of this user in encrypted form
        /// </summary>
        public virtual string PassWord
        {
            get { return this.passWord; }
        }

        /// <summary>
        /// The username of this user
        /// </summary>
        public virtual string UserName
        {
            get { return this.userName; }
        }

        /// <summary>
        /// Indicates if the user is active at this moment
        /// </summary>
        public virtual bool IsActive
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
        /// Generate a new, phonetic but random password of eight characters.
        /// Method returns the unencrypted password, while the encrypted password is set to the PassWord property.
        /// </summary>
        /// <returns></returns>
        public virtual string GenerateNewPassword()
        {
            string nieuwWachtWoord = PassWordHelper.GeneratePhoneticPassWord(8);
            ChangePassWord(nieuwWachtWoord);
            return nieuwWachtWoord;
        }

        /// <summary>
        /// Change the password of this user to the specified password.
        /// Uses a 10 byte salt to encrypt the password and sets the encrypted password to the PassWord property
        /// </summary>
        /// <param name="newPassWord"></param>
        public virtual void ChangePassWord(string newPassWord)
        {
            if (string.IsNullOrEmpty(newPassWord))
                return;

            this.salt = PassWordHelper.GenerateSalt(10);
            this.passWord = PassWordHelper.EncryptPassWord(newPassWord, this.salt);
        }

        /// <summary>
        /// Verifies if the specified password is correct for this user
        /// </summary>
        /// <param name="passWord"></param>
        /// <returns></returns>
        public virtual bool VerifyPassWord(string passWord)
        {
            return this.passWord == PassWordHelper.EncryptPassWord(passWord, this.salt);
        }

        /// <summary>
        /// Change the username of this user.
        /// Methods checks if the username is unique and valid.
        /// </summary>
        public virtual void ChangeUserName(string newUserName)
        {
            if (IoC.Resolve<ILoginService<T>>().IsUserNameValid(newUserName, this as T))
                this.userName = newUserName;
        }
        #endregion
    }
}