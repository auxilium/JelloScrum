namespace JelloScrum.Services
{
    using System;
    using System.Collections.Generic;
    using Castle.ActiveRecord.Queries;
    using JelloScrum.Model.Entities;
    using JelloScrum.Model.Enumerations;
    using JelloScrum.Model.Services;
    using Rhino.Commons;

    /// <summary>
    /// Service voor gebruikers
    /// </summary>
    public class GebruikerService : GenericService<Gebruiker>, IGebruikerService
    {
        #region IGebruikerService Members

        /// <summary>
        /// Vind de gebruiker aan de hand van zijn / haar gebruikers naam
        /// </summary>
        /// <param name="gebruikersNaam"></param>
        /// <returns>de gevonden gebruiker, anders null</returns>
        public Gebruiker FindGebruiker(string gebruikersNaam)
        {
            if (gebruikersNaam == null)
            {
                throw new Exception("Geen gebruikersnaam opgegeven");
            }

            string hql = "SELECT g FROM Gebruiker g WHERE g.GebruikersNaam = :naam";
            SimpleQuery<Gebruiker> query = new SimpleQuery<Gebruiker>(typeof (Gebruiker), hql);
            query.SetParameter("naam", gebruikersNaam);

            IList<Gebruiker> list = query.Execute();

            if (list.Count > 1)
            {
                return null;
            }

            if (list.Count == 0)
            {
                return null;
            }

            return list[0];
        }

        /// <summary>
        /// Geeft een lijst met gebruikers terug aan de hand van de systeem rol
        /// </summary>
        /// <param name="rol"></param>
        /// <returns></returns>
        public IList<Gebruiker> FindGebruikerBySysteemRol(SysteemRol rol)
        {
            string hql = "SELECT g FROM Gebruiker g WHERE g.SysteemRol = :SysteemRol";
            SimpleQuery<Gebruiker> query = new SimpleQuery<Gebruiker>(typeof (Gebruiker), hql);
            query.SetParameter("SysteemRol", rol);
            IList<Gebruiker> list = query.Execute();
            return list;
        }

        /// <summary>
        /// Geeft een lijst met gebruikers terug aan de hand van de systeem rol
        /// </summary>
        /// <param name="sprint"></param>
        /// <returns></returns>
        public IList<Gebruiker> FindAllNotInSprint(Sprint sprint)
        {
            ////TODO Moet mooier via een HQL kunnen..
            ////Filter de gebruikers die voorkomen in de sprint.Gebruikers zodat een lijst overblijft waar deze mensen niet inzitten..
            //ICollection<Gebruiker> list = FindAll();
            //foreach (Gebruiker gebruiker in list)
            //{
            //}

            return new List<Gebruiker>();
        }

        private readonly IPasswordService passwordService = IoC.Resolve<IPasswordService>();

        public void SaveGebruiker(Gebruiker gebruiker)
        {
            SaveGebruiker(gebruiker, passwordService.GeneratePhoneticPassword());
        }

        /// <summary>
        /// Slaat een nieuwe gebruiker op.
        /// </summary>
        /// <param name="gebruiker">de gebruiker</param>
        /// <param name="wachtwoord">het opgegeven wachtwoord</param>
        public void SaveGebruiker(Gebruiker gebruiker, string wachtwoord)
        {
            if (string.IsNullOrEmpty(gebruiker.Salt))
            {
                gebruiker.Salt = passwordService.GenerateSALT();
            }
            if (string.IsNullOrEmpty(wachtwoord))
            {
                wachtwoord = passwordService.GeneratePhoneticPassword();
            }
            gebruiker.Wachtwoord = passwordService.EncryptPassword(wachtwoord, gebruiker.Salt);
            IoC.Resolve<IEmailService>().VerzendWachtwoord(gebruiker, wachtwoord);
            Save(gebruiker);    
        }

        #endregion
    }
}