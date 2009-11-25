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
            story.VoegTaskToe(new Task());
            story.VoegTaskToe(new Task());

            story.Tasks[0].Status = Status.Opgepakt;
            story.Tasks[1].Status = Status.Afgesloten;

            Assert.IsTrue(story.IsTePlannen);
        }

        [Test]
        public void TestStoryMagNietIngeplandWordenOmdatDezeAfgerondIs()
        {
            story.VoegTaskToe(new Task());
            story.VoegTaskToe(new Task());

            story.Tasks[0].Status = Status.Afgesloten;
            story.Tasks[1].Status = Status.Afgesloten;
            
            Assert.IsFalse(story.IsTePlannen);
        }

        [Test]
        public void TestStoryMagNietIngeplandWordenOmdatDezeNogInEenNietAfgeslotenSprintZit()
        {
            story.VoegTaskToe(new Task());
            story.VoegTaskToe(new Task());

            story.Tasks[0].Status = Status.Afgesloten;
            story.Tasks[1].Status = Status.Opgepakt;
            
            sprint.MaakSprintStoryVoor(story);
            sprint2.MaakSprintStoryVoor(story);
            sprint2.IsAfgesloten = true;

            Assert.IsFalse(story.IsTePlannen);
        }

        [Test]
        public void TestStoryMagIngeplandWordenOmdatDezeInEenAfgeslotenSprintZit()
        {
            story.VoegTaskToe(new Task());
            story.VoegTaskToe(new Task());

            story.Tasks[0].Status = Status.Afgesloten;
            story.Tasks[1].Status = Status.Opgepakt;
            sprint.MaakSprintStoryVoor(story);
            sprint.IsAfgesloten = true;

            Assert.IsTrue(story.IsTePlannen);
        }
    }
}
