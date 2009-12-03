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
    using JelloScrum.Model.Tests;
    using NUnit.Framework;

    [TestFixture]
    public class SprintSprintStoriesTest : TestBase
    {
        private Sprint sprint;
        private Story story;
        private Story story2;
        private SprintStory sprintStory;
        private Task task;
        private Task task2;
        private Task task3;
        private Project project;

        public override void SetUp()
        {
            project = new Project();
            sprint = new Sprint();
            project.VoegSprintToe(sprint);
            story = new Story(project, new Gebruiker(), null, StoryType.UserStory);
            story2 = new Story(project, new Gebruiker(), null, StoryType.UserStory);
            sprintStory = new SprintStory();
            task = new Task(story);
            task2 = new Task(story2);

            base.SetUp();
        }

        [Test]
        public void TestMaakSprintStory()
        {
            SprintStory ss = sprint.CreateSprintStoryFor(story);

            Assert.AreEqual(story, ss.Story);
        }
        
        [Test]
        public void TestSprintStoryRelatieMetSprintIsGoed()
        {
            SprintStory ss = sprint.CreateSprintStoryFor(story);
                        
            Assert.AreEqual(sprint, ss.Sprint);
        }

        [Test, ExpectedException(typeof(NotSupportedException))]
        public void TestSprintStoryHandmatigToevoegenAanSprintStoryCollectieVanSprintMagNiet()
        {
            sprint.SprintStories.Add(sprintStory);
            Assert.Fail();
        }

        [Test]
        public void TestBerekenTotaalBestedeTijdAanSprintStories()
        {
            task3 = new Task(story2);

            task.MaakTijdRegistratie(new Gebruiker(), DateTime.Now, sprint, new TimeSpan(1, 15, 10));
            task2.MaakTijdRegistratie(new Gebruiker(), DateTime.Now, sprint, new TimeSpan(1, 20, 10));
            task3.MaakTijdRegistratie(new Gebruiker(), DateTime.Now, sprint, new TimeSpan(1, 25, 10));

            sprint.CreateSprintStoryFor(story);
            sprint.CreateSprintStoryFor(story2);

            Assert.AreEqual(new TimeSpan(4, 0, 30), sprint.TotalTimeSpent());
        }

        [Test]
        public void TestGeefNogNietAfgeslotenSprintStories()
        {
            sprint.CreateSprintStoryFor(story);
            sprint.CreateSprintStoryFor(story2);
            story.Tasks[0].Status = Status.Afgesloten;

            IList<SprintStory> result = sprint.GetAllOpenSprintStories();
            
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(story2, result[0].Story);
        }

        [Test]
        public void TestGeefNogNietAfgeslotenSprintStoriesMetMustHavePrioriteit()
        {
            sprint.CreateSprintStoryFor(story);
            sprint.CreateSprintStoryFor(story2);
            story2.Tasks[0].Status = Status.Afgesloten;
            sprint.SprintStories[0].SprintBacklogPrioriteit = Prioriteit.Must;
            
            IList<SprintStory> result = sprint.GetAllOpenSprintStories(Prioriteit.Must);

            Assert.AreEqual(Prioriteit.Must, result[0].SprintBacklogPrioriteit);
        }
    }
}