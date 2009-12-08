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
    using Entities;
    using Enumerations;
    using NUnit.Framework;

    [TestFixture]
    public class GebruikerSprintGebruikerTest : TestBase
    {
        private Sprint sprint;
        public override void SetUp()
        {
            sprint = new Sprint();
            base.SetUp();
        }

        [Test]
        public void TestVindSprintGebruikerVanGebruikerVoorSprint()
        {
            User gb = new User();
            sprint.AddUser(gb, SprintRole.Developer);

            SprintUser sg = gb.GetSprintUserFor(sprint);

            Assert.AreEqual(gb, sg.User);
        }

        [Test]
        public void TestVindGeenSprintGebruikerVanAndereSprints()
        {
            User gb = new User();
            Sprint sprint2 = new Sprint();
            sprint2.AddUser(gb, SprintRole.Developer);
            sprint.AddUser(gb, SprintRole.Developer);

            SprintUser sg = gb.GetSprintUserFor(sprint);

            Assert.AreEqual(sprint, sg.Sprint);
        }

        [Test]
        public void TestGeefActieveSprintGebruikerVanGebruiker()
        {
            User gb = new User();
            gb.ActiveSprint = sprint;
            sprint.AddUser(gb, SprintRole.Developer);

            SprintUser sg = gb.GetActiveSprintUser();

            Assert.AreEqual(sg.User, gb);
        }

        [Test]
        public void TestGeefActieveSprintGebruikerVanGebruikerTerwijlDezeNogNietGezetIs()
        {
            User gb = new User();
            sprint.AddUser(gb, SprintRole.Developer);

            SprintUser sg = gb.GetActiveSprintUser();

            Assert.AreEqual(null, sg);
        }
    }
}