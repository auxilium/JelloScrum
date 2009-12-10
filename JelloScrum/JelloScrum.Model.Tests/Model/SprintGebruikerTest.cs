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
    using Creations;
    using Entities;
    using Enumerations;
    using NUnit.Framework;

    [TestFixture]
    public class SprintGebruikerTest : TestBase
    {
        private SprintUser sprintGebruiker;
        private User gebruiker;
        private Task taak;
        private Task taak2;

        public override void SetUp()
        {
            gebruiker = new User();
            sprintGebruiker = Creation.SprintGebruiker(gebruiker);
            taak = new Task();
            taak2 = new Task();

            base.SetUp();
        }

        [Test]
        public void TestPakTaakOpZetRelaties()
        {
            sprintGebruiker.TakeTask(taak);

            Assert.AreEqual(taak, sprintGebruiker.Tasks[0]);
            Assert.AreEqual(taak.AssignedUser, sprintGebruiker);
        }

        [Test]
        public void TestPakTaakOpZetTaakStatus()
        {
            sprintGebruiker.TakeTask(taak);

            Assert.AreEqual(State.Taken, sprintGebruiker.Tasks[0].State);
        }

        [Test, ExpectedException(typeof(NotSupportedException))]
        public void TestHandmatigEenTaakAanDeTakenColletieToevoegenGaatNiet()
        {
            sprintGebruiker.Tasks.Add(taak);
            Assert.Fail();
        }

        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void TestMaakNieuweSprintGebruikerWaarbijGebruikerNullIsFaalt()
        {
            new SprintUser(null, new Sprint(), SprintRole.Developer);
            Assert.Fail();
        }

        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void TestMaakNieuweSprintGebruikerWaarbijSprintNullIsFaalt()
        {
            new SprintUser(new User(), null, SprintRole.Developer);
            Assert.Fail();
        }
        
        [Test]
        public void TestGeefOpgepakteTakenMetSprintBacklogPrioriteitMustHave()
        {
            Story story = Creation.StoryMetSprintStory(gebruiker);
            story.AddTask(taak);

            Story story2 = Creation.StoryMetSprintStoryEnSprintBacklogPrioriteit(gebruiker, Priority.Must, sprintGebruiker.Sprint);
            story2.AddTask(taak2);

            sprintGebruiker.TakeTask(taak);
            sprintGebruiker.TakeTask(taak2);

            Assert.AreEqual(1, sprintGebruiker.GetTakenTasksWithSprintBacklogPriority(Priority.Must).Count);
        }

        [Test]
        public void TestSprintRolToekennenKentSprintRolToe()
        {
            SprintUser sprintGebruiker = Creation.SprintGebruiker(Creation.Gebruiker());
            sprintGebruiker.AddRole(SprintRole.ProductOwner);
            Assert.IsTrue(sprintGebruiker.HasSprintRole(SprintRole.ProductOwner));
        }

        [Test]
        public void TestSprintRolVerwijderenVerwijdertRol()
        {
            SprintUser sprintGebruiker = Creation.SprintGebruiker(Creation.Gebruiker());
            sprintGebruiker.AddRole(SprintRole.ProductOwner);
            sprintGebruiker.RemoveRole(SprintRole.ProductOwner);
            Assert.IsFalse(sprintGebruiker.HasSprintRole(SprintRole.ProductOwner));
        }

    }
}