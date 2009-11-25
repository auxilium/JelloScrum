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
    using JelloScrum.Model.Entities;
    using JelloScrum.Model.Enumerations;
    using NUnit.Framework;

    [TestFixture]
    public class StoryTaskTest: TestBase
    {
        private Project project;
        private Sprint sprint;
        private Story story;
        private Task task;
        private Task task2;
        
        public override void SetUp()
        {
            project = new Project();
            sprint = new Sprint();
            project.VoegSprintToe(sprint);
            story = new Story(project, new Gebruiker(), null, StoryType.UserStory);
            task = new Task();
            task2 = new Task();
           
            base.SetUp();
        }

        [Test]
        public void TestVoegTaskToe()
        {
            story.VoegTaskToe(task);

            Assert.AreEqual(story, story.Tasks[0].Story);
        }

        [Test]
        public void TestMeermaalsDezelfdeTaskToevoegenGaatNiet()
        {
            story.VoegTaskToe(task);
            story.VoegTaskToe(task);

            Assert.AreEqual(1, story.Tasks.Count);
        }

        [Test, ExpectedException(typeof(NotSupportedException))]
        public void HandmatigEenTaskAanDeTaskCollectionToevoegenGaatNiet()
        {
            story.Tasks.Add(task);
            Assert.Fail();
        }

        [Test]
        public void TestBerekenTotaalBestedeTijd()
        {
            story.VoegTaskToe(task);
            story.VoegTaskToe(task2);

            task.MaakTijdRegistratie(new Gebruiker(), DateTime.Now, sprint, new TimeSpan(1,30,15) );
            task.MaakTijdRegistratie(new Gebruiker(), DateTime.Now, sprint, new TimeSpan(3,10,26) );
            task2.MaakTijdRegistratie(new Gebruiker(), DateTime.Now, sprint, new TimeSpan(2,42,58) );

            Assert.AreEqual(new TimeSpan(7, 23, 39), story.TotaalBestedeTijd());
        }

        [Test]
        public void TestBepaalWelkeTakenNogNietZijnOpgepakt()
        {
            task.Status = Status.Opgepakt;
            
            story.VoegTaskToe(task);
            story.VoegTaskToe(task2); //task2 heeft nog de default status: NietOpgepakt

            IList<Task> result = story.GeefTakenMetStatus(Status.NietOpgepakt);

            Assert.IsTrue(result.Contains(task2));
            Assert.AreEqual(1, result.Count);
        }

        [Test]
        public void TestBepaalStoryStatusAanDeHandVanTasksStatus_GeenOpgepakteStories()
        {
            story.VoegTaskToe(task);
            story.VoegTaskToe(task2);

            Assert.AreEqual(Status.NietOpgepakt, story.Status);
        }
               
        [Test]
        public void TestBepaalStoryStatusAanDeHandVanTasksStatus_EenOpgepakteStory()
        {
            task.Status = Status.Opgepakt;

            story.VoegTaskToe(task);
            story.VoegTaskToe(task2);

            Assert.AreEqual(Status.Opgepakt, story.Status);
        }

        [Test]
        public void TestBepaalStoryStatusAanDeHandVanTasksStatus_AlleStoriesZijnAfgesloten()
        {
            task.Status = Status.Afgesloten;
            task2.Status = Status.Afgesloten;

            story.VoegTaskToe(task);
            story.VoegTaskToe(task2);

            Assert.AreEqual(Status.Afgesloten, story.Status);
        }

        [Test]
        public void TestUrenSchattingTakenKleinerUrenSchattingStory()
        {
            task.Schatting = new TimeSpan(0,1,0,0);
            task2.Schatting = new TimeSpan(0, 1, 0, 0);

            story.VoegTaskToe(task);
            story.VoegTaskToe(task2);
            story.Schatting = new TimeSpan(0, 3, 0, 0);

            Assert.IsTrue(story.CheckSchattingTaken());
        }

        [Test]
        public void TestUrenSchattingTakenGelijkUrenSchattingStory()
        {
            task.Schatting = new TimeSpan(0, 1, 0, 0);
            task2.Schatting = new TimeSpan(0, 1, 0, 0);

            story.VoegTaskToe(task);
            story.VoegTaskToe(task2);
            story.Schatting = new TimeSpan(0, 2, 0, 0);

            Assert.IsTrue(story.CheckSchattingTaken());
        }

        [Test]
        public void TestUrenSchattingTakenGroterUrenSchattingStory()
        {
            task.Schatting = new TimeSpan(0, 1, 0, 0);
            task2.Schatting = new TimeSpan(0, 2, 0, 0);

            story.VoegTaskToe(task);
            story.VoegTaskToe(task2);
            story.Schatting = new TimeSpan(0, 2, 0, 0);

            Assert.IsFalse(story.CheckSchattingTaken());
        }
    }
}