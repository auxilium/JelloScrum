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

namespace JelloScrum.Repositories
{
    using System;
    using System.Collections.Generic;
    using Castle.ActiveRecord.Queries;
    using Exceptions;
    using JelloScrum.Model.IRepositories;
    using Model.Entities;
    using Model.Services;
    using Repositories;

    /// <summary>
    /// Service voor gebruikers
    /// </summary>
    public class UserRepository : JelloScrumRepository<User>, IUserRepository
    {
        #region IGebruikerRepository Members

        /// <summary>
        /// Vind de gebruiker aan de hand van zijn / haar gebruikers naam
        /// </summary>
        /// <param name="gebruikersNaam"></param>
        /// <returns>de gevonden gebruiker, anders null</returns>
        public User ZoekOpGebruikersNaam(string gebruikersNaam)
        {
            if (string.IsNullOrEmpty(gebruikersNaam))
            {
                throw new NullReferenceException("Geen gebruikersnaam opgegeven");
            }

            string hql = "SELECT g FROM User g WHERE g.UserName = :naam";
            SimpleQuery<User> query = new SimpleQuery<User>(typeof(User), hql);
            query.SetParameter("naam", gebruikersNaam);

            IList<User> list = query.Execute();

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

        public IList<User> ZoekOpSysteemRol(JelloScrum.Model.Enumerations.SystemRole rol)
        {
            string hql = "SELECT g FROM User g WHERE g.SystemRole = :SysteemRol";
            SimpleQuery<User> query = new SimpleQuery<User>(typeof(User), hql);
            query.SetParameter("SysteemRol", rol);
            IList<User> list = query.Execute();
            return list;
        }


        public IList<User> ZoekOpNietInSprint(Sprint sprint)
        {
            ////TODO Moet mooier via een HQL kunnen..
            ////Filter de gebruikers die voorkomen in de sprint.Gebruikers zodat een lijst overblijft waar deze mensen niet inzitten..
            //ICollection<Gebruiker> list = FindAll();
            //foreach (Gebruiker gebruiker in list)
            //{
            //}

            return new List<User>();
        }


//        public Gebruiker SaveGebruiker(Gebruiker gebruiker, string wachtwoord)
//        {
//            IPasswordService passwordService = IoC.Resolve<IPasswordService>();
//            if (string.IsNullOrEmpty(gebruiker.Salt))
//            {
//                gebruiker.Salt = passwordService.GenerateSalt();
//            }
//            if (string.IsNullOrEmpty(wachtwoord))
//            {
//                wachtwoord = passwordService.GeneratePhoneticPassword();
//            }
//            gebruiker.Wachtwoord = passwordService.EncryptPassword(wachtwoord, gebruiker.Salt);
//            IoC.Resolve<IEmailService>().VerzendWachtwoord(gebruiker, wachtwoord);
//            return Save(gebruiker);  
//        }

        #endregion
    }
}
