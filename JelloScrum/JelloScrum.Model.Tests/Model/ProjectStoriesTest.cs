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
    public class ProjectStoriesTest : TestBase
    {
        private Project project;
        private Story story;
        private Story story2;

        public override void SetUp()
        {
            project = new Project();
            story = new Story(new Project(), new User(), null, StoryType.UserStory);
            story2 = new Story(new Project(), new User(), null, StoryType.UserStory);

            base.SetUp();
        }

        [Test]
        public void TestVoegStoryToe()
        {
            project.AddStory(story);

            Assert.AreEqual(project, project.Stories[0].Project);
        }

        [Test]
        public void TestMeermaalsDezelfdeTaskToevoegenGaatNiet()
        {
            project.AddStory(story);
            project.AddStory(story);

            Assert.AreEqual(1, project.Stories.Count);  
        }

        [Test, ExpectedException(typeof(NotSupportedException))]
        public void TestHandmatigEenStoryAanDeStoriesCollectieToevoegenGaatNiet()
        {
            project.Stories.Add(story);
            Assert.Fail();
        }

        [Test]
        public void TestGeefStoriesZonderMoSCoWPrioriteit()
        {
            story.ProductBacklogPriority = Priority.Must;

            project.AddStory(story);
            project.AddStory(story2);

            IList<Story> result = project.GetAllStoriesWithUndefinedPriorities();

            Assert.IsTrue(result.Contains(story2), "GeefStoriesZonderMoSCoWPrioriteit() geeft niet de goede story terug.");
            Assert.AreEqual(1, result.Count);
        }
    }
}
