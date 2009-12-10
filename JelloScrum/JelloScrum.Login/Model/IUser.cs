namespace JelloScrum.Login.Model
{
    using System.Security.Principal;

    /// <summary>
    /// Interface for user classes.
    /// </summary>
    public interface IUser : IPrincipal, IUniqueIdentifyable
    {
        /// <summary>
        /// The username of this user
        /// The username must be unique.
        /// </summary>
        string UserName { get; }

        /// <summary>
        /// The password of this user in encrypted form
        /// </summary>
        string PassWord { get; }

        /// <summary>
        /// Salt used to encrypt the password
        /// </summary>
        string Salt { get; }

        /// <summary>
        /// Indicates if the user is active at this moment
        /// </summary>
        bool IsActive { get; set; }

        /// <summary>
        /// Change the password of this user to the specified password.
        /// Uses a 10 byte salt to encrypt the password and sets the encrypted password to the PassWord property
        /// </summary>
        /// <param name="newPassWord"></param>
        void ChangePassWord(string newPassWord);


        /// <summary>
        /// Verifies if the specified password is correct for this user
        /// </summary>
        /// <param name="passWord"></param>
        /// <returns></returns>
        bool VerifyPassWord(string passWord);

        /// <summary>
        /// Generate a new, phonetic but random password of eight characters.
        /// Method returns the unencrypted password, while the encrypted password is set to the PassWord property.
        /// </summary>
        /// <returns></returns>
        string GenerateNewPassword();

        /// <summary>
        /// Change the username of this user.
        /// Methods checks if the username is unique and valid.
        /// </summary>
        void ChangeUserName(string newUserName);
    }
}