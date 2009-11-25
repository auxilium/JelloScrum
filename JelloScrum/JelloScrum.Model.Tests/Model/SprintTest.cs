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
            sprint.VoegWerkDagToe(WerkDag.Maandag);
            Assert.IsTrue(sprint.HeeftWerkDag(WerkDag.Maandag), "Maandag is niet aan de sprint toegevoegd!");
        }

        [Test]
        public void TestGeefSprintStoryVanStory()
        {
            Story story = Creation.Story();
            Story story2 = Creation.Story();

            sprint.MaakSprintStoryVoor(story);
            sprint.MaakSprintStoryVoor(story2);

            SprintStory ss = sprint.GeefSprintStoryVanStory(story2);

            Assert.AreEqual(ss.Story, story2);
            Assert.AreEqual(2, sprint.SprintStories.Count);
        }

        [Test]
        public void TestSluitSprintAf()
        {
            Task task = Creation.TaskMetCompleteHierarchie();
            sprint = task.Behandelaar.Sprint;
            sprint.SluitSprintAf();

            Assert.AreEqual(Status.NietOpgepakt, task.Status);
            Assert.IsTrue(sprint.IsAfgesloten);
        }

        [Test]
        public void TestSluitSprintAfMaaktLogBericht()
        {
            Task task = Creation.TaskMetCompleteHierarchie();
            task.Behandelaar.Sprint.SluitSprintAf();

            Assert.AreEqual(1, task.LogBerichten.Count);
            Assert.AreEqual("Sprint gesloten", task.LogBerichten[0].Titel);
        }
    }
}