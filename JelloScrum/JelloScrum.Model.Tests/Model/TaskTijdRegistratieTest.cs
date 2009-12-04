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
    using Enumerations;
    using JelloScrum.Model.Entities;
    using JelloScrum.Model.Tests;
    using NUnit.Framework;

    [TestFixture]
    public class TaskTijdRegistratieTest : TestBase
    {
        private Project project;
        private Story story;
        private Task task;
        private Gebruiker gebruiker;
        private Sprint sprint;
        private TimeSpan tijd;

        public override void SetUp()
        {
            project = new Project();
            
            task = new Task();
            gebruiker = new Gebruiker();
            sprint = new Sprint();
            project.AddSprint(sprint);
            tijd = new TimeSpan(1, 30, 00); //1,5 uur

            story = new Story(project, gebruiker, null, StoryType.UserStory);
            story.AddTask(task);

            base.SetUp();
        }

        [Test]
        public void TestVoegTijdRegistratieToe()
        {
            task.MaakTijdRegistratie(gebruiker, DateTime.Now, sprint, tijd);

            Assert.AreEqual(1, task.TijdRegistraties.Count);
        }

        [Test]
        public void TestVoegTijdRegistratieToeZetRelaties()
        {
            task.MaakTijdRegistratie(gebruiker, DateTime.Now, sprint, tijd);

            Assert.IsTrue(task.TijdRegistraties[0].Task == task);
        }

        [Test, ExpectedException(typeof(NotSupportedException))]
        public void TestTijdRegistratieHandmatigToevoegenMagNiet()
        {
            TijdRegistratie tijdRegistratie = new TijdRegistratie(gebruiker, DateTime.Now, sprint, task, tijd);
            task.TijdRegistraties.Add(tijdRegistratie);
            Assert.Fail();
        }

        [Test]
        public void TestVoegTijdRegistratiesVanVerschillendeGebruikersToe()
        {
            Gebruiker gebruiker2 = new Gebruiker();

            task.MaakTijdRegistratie(gebruiker, DateTime.Now, sprint, tijd);
            task.MaakTijdRegistratie(gebruiker2, DateTime.Now, sprint, tijd);

            Assert.AreEqual(2, task.TijdRegistraties.Count);
            Assert.IsTrue(task.TijdRegistraties[0].Gebruiker != task.TijdRegistraties[1].Gebruiker);
        }
        
        [Test]
        public void TestVoegTijdRegistratiesVanVerschillendeSprintsToe()
        {
            Sprint sprint2 = new Sprint();
            project.AddSprint(sprint2);

            task.MaakTijdRegistratie(gebruiker, DateTime.Now, sprint, tijd);
            task.MaakTijdRegistratie(gebruiker, DateTime.Now, sprint2, tijd);
            
            Assert.IsTrue(task.TijdRegistraties[0].Sprint != task.TijdRegistraties[1].Sprint);
        }
                
        [Test]
        public void TestVoegTijdRegistratieToeZonderTijd()
        {
            TimeSpan tijd2 = new TimeSpan();

            task.MaakTijdRegistratie(gebruiker, DateTime.Now, sprint, tijd2);
            Assert.AreEqual(0, task.TijdRegistraties.Count);
        }

        [Test]
        public void TestVoegNieuweTijdRegistratieZonderTijdToe()
        {
            TimeSpan tijd2 = new TimeSpan();
            //eerst tijd registreren
            task.MaakTijdRegistratie(gebruiker, DateTime.Today, sprint, tijd);
            //daarna een lege tijd registreren
            task.MaakTijdRegistratie(gebruiker, DateTime.Today, sprint, tijd2);
            Assert.AreEqual(0, task.TijdRegistraties.Count);
        }

        [Test, ExpectedException(typeof(ArgumentException))]
        public void TestVoegGeenTijdRegistratieToeWaarbijDeGegevenSprintNietBijDitProjectHoort()
        {
            TimeSpan tijd2 = new TimeSpan();

            task.MaakTijdRegistratie(gebruiker, DateTime.Now, new Sprint(), tijd2);
            Assert.Fail();
        }

        [Test]
        public void TestBerekenTotaalBestedeTijd()
        {
            TimeSpan tijd2 = new TimeSpan(3, 29, 15);
            task.MaakTijdRegistratie(gebruiker, DateTime.Now, sprint, tijd);
            task.MaakTijdRegistratie(gebruiker, DateTime.Now.AddDays(1), sprint, tijd2);

            TimeSpan expected = tijd + tijd2;

            Assert.AreEqual(expected, task.TotaalBestedeTijd());
        }
    }
}