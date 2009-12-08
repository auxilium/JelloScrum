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
        private User gebruiker;

        public override void SetUp()
        {
            project = new Project();
            sprint = new Sprint();
            gebruiker = new User();
            project.AddSprint(sprint);

            base.SetUp();
        }

        [Test]
        public void TestVoegNieuweGebruikerAanSprintToe()
        {
            SprintUser sg = sprint.AddUser(gebruiker, SprintRole.Developer);

            Assert.AreEqual(sg.Sprint, sprint);
            Assert.AreEqual(sg.User, gebruiker);
            Assert.IsTrue(sprint.SprintUsers.Contains(sg));
        }
        
        [Test]
        public void TestVoegGebruikerAanSprintToeWaarAlEenSprintGebruikerVoorDieSprintEnGebruikerBestaat()
        {
            SprintUser sg1 = sprint.AddUser(gebruiker, SprintRole.Developer);
            SprintUser sg2 = sprint.AddUser(gebruiker, SprintRole.Developer);
            
            Assert.AreEqual(sg1, sg2);
        }

        [Test, ExpectedException(typeof(NotSupportedException))]
        public void TestSprintGebruikerDirectAanCollectieToevoegenGaatNiet()
        {
            sprint.SprintUsers.Add(new SprintUser());
            Assert.Fail();
        }

        [Test]
        public void TestVerwerkenVanGebruikersMetEenNieuweGebruikerLevertEenExtraSprintGebruikerOp()
        {
            Sprint sprint = Creation.SprintMetScrumMasterEnProductOwner();
            sprint.AddUser(Creation.Gebruiker(), SprintRole.Developer);
            Assert.IsTrue(sprint.SprintUsers.Count == 3);

        }
    }
}