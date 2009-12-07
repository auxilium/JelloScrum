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
        private SprintGebruiker sprintGebruiker;
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
            sprintGebruiker.PakTaakOp(taak);

            Assert.AreEqual(taak, sprintGebruiker.Taken[0]);
            Assert.AreEqual(taak.Behandelaar, sprintGebruiker);
        }

        [Test]
        public void TestPakTaakOpZetTaakStatus()
        {
            sprintGebruiker.PakTaakOp(taak);

            Assert.AreEqual(State.Taken, sprintGebruiker.Taken[0].Status);
        }

        [Test, ExpectedException(typeof(NotSupportedException))]
        public void TestHandmatigEenTaakAanDeTakenColletieToevoegenGaatNiet()
        {
            sprintGebruiker.Taken.Add(taak);
            Assert.Fail();
        }

        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void TestMaakNieuweSprintGebruikerWaarbijGebruikerNullIsFaalt()
        {
            new SprintGebruiker(null, new Sprint(), SprintRole.Developer);
            Assert.Fail();
        }

        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void TestMaakNieuweSprintGebruikerWaarbijSprintNullIsFaalt()
        {
            new SprintGebruiker(new User(), null, SprintRole.Developer);
            Assert.Fail();
        }
        
        [Test]
        public void TestGeefOpgepakteTakenMetSprintBacklogPrioriteitMustHave()
        {
            Story story = Creation.StoryMetSprintStory(gebruiker);
            story.AddTask(taak);

            Story story2 = Creation.StoryMetSprintStoryEnSprintBacklogPrioriteit(gebruiker, Priority.Must, sprintGebruiker.Sprint);
            story2.AddTask(taak2);

            sprintGebruiker.PakTaakOp(taak);
            sprintGebruiker.PakTaakOp(taak2);

            Assert.AreEqual(1, sprintGebruiker.GeefOpgepakteTakenMetSprintBacklogPrioriteit(Priority.Must).Count);
        }

        [Test]
        public void TestSprintRolToekennenKentSprintRolToe()
        {
            SprintGebruiker sprintGebruiker = Creation.SprintGebruiker(Creation.Gebruiker());
            sprintGebruiker.VoegRolToe(SprintRole.ProductOwner);
            Assert.IsTrue(sprintGebruiker.HeeftSprintRol(SprintRole.ProductOwner));
        }

        [Test]
        public void TestSprintRolVerwijderenVerwijdertRol()
        {
            SprintGebruiker sprintGebruiker = Creation.SprintGebruiker(Creation.Gebruiker());
            sprintGebruiker.VoegRolToe(SprintRole.ProductOwner);
            sprintGebruiker.VerwijderRol(SprintRole.ProductOwner);
            Assert.IsFalse(sprintGebruiker.HeeftSprintRol(SprintRole.ProductOwner));
        }

    }
}