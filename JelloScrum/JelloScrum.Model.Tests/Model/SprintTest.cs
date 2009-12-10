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
    using Creations;
    using Entities;
    using Enumerations;
    using NUnit.Framework;

    [TestFixture]
    public class SprintTest : TestBase
    {
        private Sprint sprint;

        public override void SetUp()
        {
            base.SetUp();
            sprint = new Sprint();
        }

        [Test]
        public void TestMaandagAanSprintToevoegen()
        {
            sprint.AddWorkday(WorkDay.Monday);
            Assert.IsTrue(sprint.HasWorkday(WorkDay.Monday), "Maandag is niet aan de sprint toegevoegd!");
        }

        [Test]
        public void TestGeefSprintStoryVanStory()
        {
            Story story = Creation.Story();
            Story story2 = Creation.Story();

            sprint.CreateSprintStoryFor(story);
            sprint.CreateSprintStoryFor(story2);

            SprintStory ss = sprint.GetSprintStoryFor(story2);

            Assert.AreEqual(ss.Story, story2);
            Assert.AreEqual(2, sprint.SprintStories.Count);
        }

        [Test]
        public void TestSluitSprintAf()
        {
            Task task = Creation.TaskMetCompleteHierarchie();
            sprint = task.AssignedUser.Sprint;
            sprint.Close();

            Assert.AreEqual(State.Open, task.State);
            Assert.IsTrue(sprint.IsClosed);
        }

        [Test]
        public void TestSluitSprintAfMaaktLogBericht()
        {
            Task task = Creation.TaskMetCompleteHierarchie();
            task.AssignedUser.Sprint.Close();

            Assert.AreEqual(1, task.LogMessages.Count);
            Assert.AreEqual("Sprint closed", task.LogMessages[0].Title);
        }
    }
}