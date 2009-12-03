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

namespace JelloScrum.Model.Tests.Model
{
    using System;
    using System.Collections.Generic;
    using Creations;
    using Entities;
    using Enumerations;
    using NUnit.Framework;

    [TestFixture]
    public class SprintSprintGebruikerTest : TestBase
    {
        private Project project;
        private Sprint sprint;
        private Gebruiker gebruiker;

        public override void SetUp()
        {
            project = new Project();
            sprint = new Sprint();
            gebruiker = new Gebruiker();
            project.AddSprint(sprint);

            base.SetUp();
        }

        [Test]
        public void TestVoegNieuweGebruikerAanSprintToe()
        {
            SprintGebruiker sg = sprint.AddUser(gebruiker, SprintRol.Developer);

            Assert.AreEqual(sg.Sprint, sprint);
            Assert.AreEqual(sg.Gebruiker, gebruiker);
            Assert.IsTrue(sprint.SprintGebruikers.Contains(sg));
        }
        
        [Test]
        public void TestVoegGebruikerAanSprintToeWaarAlEenSprintGebruikerVoorDieSprintEnGebruikerBestaat()
        {
            SprintGebruiker sg1 = sprint.AddUser(gebruiker, SprintRol.Developer);
            SprintGebruiker sg2 = sprint.AddUser(gebruiker, SprintRol.Developer);
            
            Assert.AreEqual(sg1, sg2);
        }

        [Test, ExpectedException(typeof(NotSupportedException))]
        public void TestSprintGebruikerDirectAanCollectieToevoegenGaatNiet()
        {
            sprint.SprintGebruikers.Add(new SprintGebruiker());
            Assert.Fail();
        }

        [Test]
        public void TestVerwerkenVanGebruikersMetEenNieuweGebruikerLevertEenExtraSprintGebruikerOp()
        {
            Sprint sprint = Creation.SprintMetScrumMasterEnProductOwner();
            sprint.AddUser(Creation.Gebruiker(), SprintRol.Developer);
            Assert.IsTrue(sprint.SprintGebruikers.Count == 3);

        }
    }
}