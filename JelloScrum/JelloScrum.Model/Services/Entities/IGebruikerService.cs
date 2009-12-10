namespace JelloScrum.Model.Services
{
    using System.Collections.Generic;
    using Enumerations;
    using Model.Entities;

    /// <summary>
    /// 
    /// </summary>
    public interface IGebruikerService : IGenericService<Gebruiker>
    {
        /// <summary>
        /// Vind de gebruiker aan de hand van zijn / haar gebruikers naam
        /// </summary>
        /// <param name="gebruikersNaam"></param>
        /// <returns>de gevonden gebruiker, anders null</returns>
        Gebruiker FindGebruiker(string gebruikersNaam);

        /// <summary>
        /// Geeft een lijst met gebruikers terug aan de hand van de systeem rol
        /// </summary>
        /// <param name="rol"></param>
        /// <returns></returns>
        IList<Gebruiker> FindGebruikerBySysteemRol(SysteemRol rol);

        /// <summary>
        /// Alle gebruikers die niet in een sprint zitten
        /// </summary>
        /// <param name="sprint"></param>
        /// <returns></returns>
        IList<Gebruiker> FindAllNotInSprint(Sprint sprint);

        /// <summary>
        /// Slaat een nieuwe gebruiker op
        /// </summary>
        /// <param name="gebruiker"></param>
        void SaveGebruiker(Gebruiker gebruiker);

        /// <summary>
        /// Slaat een nieuwe gebruiker op.
        /// </summary>
        /// <param name="gebruiker">de gebruiker</param>
        /// <param name="wachtwoord">het opgegeven wachtwoord</param>
        void SaveGebruiker(Gebruiker gebruiker, string wachtwoord);
    }
}
