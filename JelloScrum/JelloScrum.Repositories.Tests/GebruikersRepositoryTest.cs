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

namespace JelloScrum.Repositories.Tests
{
    using System;
    using System.Collections.Generic;
    using Container;
    using Creations;
    using JelloScrum.Model.Entities;
    using JelloScrum.Model.Enumerations;
    using JelloScrum.Model.IRepositories;
    using NUnit.Framework;

    [TestFixture]
    public class GebruikerRepositoryTest : TestBase
    {
        #region Setup/Teardown
        private IUserRepository gebruikerRepository;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            gebruikerRepository = IoC.Resolve<IUserRepository>();
        }

        #endregion

        /// <summary>
        /// Test of een gebruiker opgeslagen wordt en teruggelezen kan worden.
        /// </summary>
        [Test]
        public void TestOpgeslagenGebruikerKanWordenTeruggelezen()
        {
            User gebruiker = gebruikerRepository.Save(Creation.Gebruiker("TestUser"));
            //UnitOfWork.CurrentSession.Clear();
            User dbgebruiker = gebruikerRepository.Get(gebruiker.Id);

            Assert.AreEqual(dbgebruiker.UserName, "TestUser");
        }

        [Test]
        public void TestZoekenOpGebruikersNaamGeeftResultaat()
        {
            User gebruiker = Creation.Gebruiker("BenIkInBeeld");
            User dbGebruiker = gebruikerRepository.ZoekOpGebruikersNaam("BenIkInBeeld");
            Assert.IsTrue(gebruiker.Equals(dbGebruiker), "Gevonden gebruiker was niet de verwachte gebruiker");
        }

        [Test]
        public void TestZoekenOpGebruikersNaamDieNietBestaatGeeftNullAlsResultaat()
        {
            Creation.Gebruiker("BenIkInBeeld");
            User dbGebruiker = gebruikerRepository.ZoekOpGebruikersNaam("IkZoekIetsAnders");
            Assert.IsNull(dbGebruiker, "Er is een gebruiker gevonden, terwijl dit niet had moeten gebeuren");
        }

        /// <summary>
        /// Beetje bizar dit, want gebruikersnaam zou gewoon uniek moeten zijn in mijn beleving.
        /// </summary>
        [Test]
        public void TestZoekOpGebruikersNaamDieTweeKeerVoorkomtGeeftNullAlsResultaat()
        {
            Creation.Gebruiker("BenIkInBeeld");
            Creation.Gebruiker("BenIkInBeeld");

            User dbGebruiker = gebruikerRepository.ZoekOpGebruikersNaam("BenIkInBeeld");
            Assert.IsNull(dbGebruiker, "Er is een gebruiker gevonden, terwijl het resultaat null moest zijn");
        }

        [Test, ExpectedException(typeof(NullReferenceException))]
        public void TestZoekenOpLegeStringGeeftNullReferenceException()
        {
            gebruikerRepository.ZoekOpGebruikersNaam(""); 
            Assert.Fail();
        }

        [Test, ExpectedException(typeof(NullReferenceException))]
        public void TestZoekenOpNullGeeftNullReferenceException()
        {
            gebruikerRepository.ZoekOpGebruikersNaam(null);
            Assert.Fail();
        }

        [Test]
        public void TestZoekenOpRolGeeftJuisteResultaat()
        {
            Creation.Gebruiker("Gebruiker1", SystemRole.User);
            Creation.Gebruiker("Gebruiker2", SystemRole.User);
            IList<User> gebruikers = gebruikerRepository.ZoekOpSysteemRol(SystemRole.User);
            Assert.IsTrue(gebruikers.Count == 2, gebruikers.Count.ToString());
        }

        [Test]
        public void TestZoekenOpRolGeeftGeenAndereRollen()
        {
            Creation.Gebruiker("Gebruiker1", SystemRole.User);
            Creation.Gebruiker("Administrator1", SystemRole.Administrator);
            IList<User> gebruikers = gebruikerRepository.ZoekOpSysteemRol(SystemRole.User);
            Assert.IsTrue(gebruikers.Count == 1 && gebruikers[0].SystemRole == SystemRole.User, "Er zijn ook andere rollen gevonden.");
        }
    }
}