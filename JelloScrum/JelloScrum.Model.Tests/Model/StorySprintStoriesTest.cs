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
    using Entities;
    using Tests;
    using NUnit.Framework;

    [TestFixture]
    public class StorySprintStoriesTest : TestBase
    {
        private Sprint sprint;
        private Story story;
        private SprintStory sprintStory;
        
        public override void SetUp()
        {
            sprint = new Sprint();
            story = new Story(new Project(), new Gebruiker(), null, StoryType.UserStory);
            sprintStory = new SprintStory();
            
            base.SetUp();
        }

        [Test]
        public void TestSprintStoryRelatieMetStoryIsGoed()
        {
            SprintStory ss = sprint.CreateSprintStoryFor(story);
            
            Assert.AreEqual(story, ss.Story);
        }   

        [Test, ExpectedException(typeof(NotSupportedException))]
        public void TestSprintStoryHandmatigToevoegenAanSprintStoryCollectieVanStoryMagNiet()
        {
            story.SprintStories.Add(sprintStory);
            Assert.Fail();
        }

        [Test]
        public void TestMaakSprintStoryNeemtStorySchattingOver()
        {
            SprintStory ss = sprint.CreateSprintStoryFor(story);

            Assert.AreEqual(story.Schatting, ss.Schatting);
        }

        [Test]
        public void TestSprintStoryIsVolledigOpgepakt()
        {
            story.AddTask(new Task());
            story.AddTask(new Task());
            SprintStory result = sprint.CreateSprintStoryFor(story);

            story.Tasks[0].Status = State.Taken;
            story.Tasks[1].Status = State.Taken;

            Assert.IsTrue(result.IsVolledigeOpgepakt);
        }

        [Test]
        public void TestSprintStoryIsNietVolledigOpgepakt()
        {
            story.AddTask(new Task());
            story.AddTask(new Task());
            SprintStory result = sprint.CreateSprintStoryFor(story);

            story.Tasks[0].Status = State.Taken;

            Assert.IsFalse(result.IsVolledigeOpgepakt);
        }
    }
}