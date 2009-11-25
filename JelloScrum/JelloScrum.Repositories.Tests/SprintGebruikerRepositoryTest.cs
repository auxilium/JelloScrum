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
    using Container;
    using Creations;
    using JelloScrum.Model.Entities;
    using Exceptions;
    using JelloScrum.Model.Enumerations;
    using JelloScrum.Model.IRepositories;
    using NUnit.Framework;

    [TestFixture]
    public class SprintGebruikerRepositoryTest : TestBase
    {
        #region Setup/Teardown

        private ISprintGebruikerRepository sprintGebruikerRepository;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            sprintGebruikerRepository = IoC.Resolve<ISprintGebruikerRepository>();
        }

        [Test]
        public void TestOpgeslagenSprintGebruikerKanTerugGelezenWorden()
        {
            SprintGebruiker sprintGebruiker = sprintGebruikerRepository.Save(Creation.SprintGebruiker(Creation.Gebruiker(), Creation.Sprint(), SprintRol.Developer));
            //UnitOfWork.CurrentSession.Clear();
            SprintGebruiker dbSprintGebruiker = sprintGebruikerRepository.Get(sprintGebruiker.Id);

            Assert.IsFalse(dbSprintGebruiker == null);            
        }

        #endregion
    }
}