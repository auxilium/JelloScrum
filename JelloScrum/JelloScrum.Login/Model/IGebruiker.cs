namespace Auxilium.Login.Model
{
    using System.Security.Principal;

    /// <summary>
    /// De interface waar gebruikers van een systeem aan moeten voldoen
    /// </summary>
    public interface IGebruiker : IPrincipal, IUniqueIdentifyable
    {
        /// <summary>
        /// De gebruikersnaam waarmee de authenticatie en authorisatie uitgevoerd wordt
        /// </summary>
        string UserName { get; }

        /// <summary>
        /// Het wachtwoord van de gebruiker
        /// </summary>
        string PassWord { get; }

        /// <summary>
        /// De salt, wordt gebruikt om het wachtwoord mee te encrypten
        /// </summary>
        string Salt { get; }

        /// <summary>
        /// Geeft aan of de gebruiker actief is of niet: 
        /// een niet-actieve gebruiker mag niet inloggen.
        /// </summary>
        bool Active { get; set; }

        /// <summary>
        /// Wijzig het wachtwoord voor deze gebruiker
        /// </summary>
        /// <param name="newPassWord"></param>
        void ChangePassWord(string newPassWord);


        /// <summary>
        /// Geeft aan of het opgegeven wachtwoord overeen komt met het wachtwoord van deze gebruiker
        /// </summary>
        /// <param name="passWord"></param>
        /// <returns></returns>
        bool VerifyPassWord(string passWord);

        /// <summary>
        /// Genereert een wachtwoord zet deze voor deze gebruiker
        /// </summary>
        /// <returns></returns>
        string GenerateNewPassword();

        /// <summary>
        /// Wijzig de gebruikersnaam van deze gebruiker
        /// </summary>
        /// <param name="newUserName"></param>
        void ChangeUserName(string newUserName);
    }
}