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

namespace JelloScrum.Model.IRepositories
{
    using System.Collections.Generic;
    using Entities;
    using Enumerations;

    /// <summary>
    /// Interface for the gebruiker repository
    /// </summary>
    public interface IGebruikerRepository : IJelloScrumRepository<Gebruiker>
    {
        /// <summary>
        /// Vind de gebruiker aan de hand van zijn / haar gebruikers naam
        /// </summary>
        /// <param name="gebruikersNaam">De gebruikersnaam.</param>
        /// <returns>de gevonden gebruiker, anders null</returns>
        Gebruiker ZoekOpGebruikersNaam(string gebruikersNaam);

        /// <summary>
        /// Geeft een lijst met gebruikers terug aan de hand van de systeem rol
        /// </summary>
        /// <param name="rol">De rol</param>
        /// <returns>De gebruikers die de gegeven rol hebben.</returns>
        IList<Gebruiker> ZoekOpSysteemRol(SysteemRol rol);

        /// <summary>
        /// Alle gebruikers die niet in een sprint zitten
        /// </summary>
        /// <param name="sprint">De sprint</param>
        /// <returns>De gebruikers die niet in de gegeven sprint zitten.</returns>
        IList<Gebruiker> ZoekOpNietInSprint(Sprint sprint);

//        /// <summary>
//        /// Slaat een nieuwe gebruiker op.
//        /// </summary>
//        /// <param name="gebruiker">de gebruiker</param>
//        /// <param name="wachtwoord">het opgegeven wachtwoord</param>
//        /// <returns>De gesavede gebruiker</returns>
//        Gebruiker SaveGebruiker(Gebruiker gebruiker, string wachtwoord);
    }
}