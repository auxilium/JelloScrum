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
    using Entities;
    using Enumerations;
    using NUnit.Framework;

    [TestFixture]
    public class StoryTest : TestBase
    {
        private Sprint sprint;
        private Sprint sprint2;
        private Story story;

        public override void SetUp()
        {
            sprint = new Sprint();
            sprint2 = new Sprint();
            story = new Story(new Project(), new Gebruiker(), null, StoryType.UserStory);

            base.SetUp();
        }

        [Test]
        public void TestStoryMagIngeplandWordenOmdatDezeNietAfgerondIs()
        {
            story.AddTask(new Task());
            story.AddTask(new Task());

            story.Tasks[0].Status = State.Taken;
            story.Tasks[1].Status = State.Closed;

            Assert.IsTrue(story.IsPlannable);
        }

        [Test]
        public void TestStoryMagNietIngeplandWordenOmdatDezeAfgerondIs()
        {
            story.AddTask(new Task());
            story.AddTask(new Task());

            story.Tasks[0].Status = State.Closed;
            story.Tasks[1].Status = State.Closed;
            
            Assert.IsFalse(story.IsPlannable);
        }

        [Test]
        public void TestStoryMagNietIngeplandWordenOmdatDezeNogInEenNietAfgeslotenSprintZit()
        {
            story.AddTask(new Task());
            story.AddTask(new Task());

            story.Tasks[0].Status = State.Closed;
            story.Tasks[1].Status = State.Taken;
            
            sprint.CreateSprintStoryFor(story);
            sprint2.CreateSprintStoryFor(story);
            sprint2.IsAfgesloten = true;

            Assert.IsFalse(story.IsPlannable);
        }

        [Test]
        public void TestStoryMagIngeplandWordenOmdatDezeInEenAfgeslotenSprintZit()
        {
            story.AddTask(new Task());
            story.AddTask(new Task());

            story.Tasks[0].Status = State.Closed;
            story.Tasks[1].Status = State.Taken;
            sprint.CreateSprintStoryFor(story);
            sprint.IsAfgesloten = true;

            Assert.IsTrue(story.IsPlannable);
        }
    }
}
