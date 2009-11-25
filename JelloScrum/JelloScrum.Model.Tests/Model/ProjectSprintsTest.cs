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
    using JelloScrum.Model.Entities;
    using JelloScrum.Model.Tests;
    using NUnit.Framework;

    [TestFixture]
    public class ProjectSprintsTest: TestBase
    {
        private Project project;
        private Sprint sprint;

        public override void SetUp()
        {
            project = new Project();
            sprint = new Sprint();

            base.SetUp();
        }

        [Test]
        public void TestVoegSprintToe()
        {
            project.VoegSprintToe(sprint);

            Assert.AreEqual(project, project.Sprints[0].Project);
        }

        [Test]
        public void TestVoegMeermaalsDezelfdeSprintToeGaatNiet()
        {
            project.VoegSprintToe(sprint);
            project.VoegSprintToe(sprint);

            Assert.AreEqual(1, project.Sprints.Count);
        }

        [Test, ExpectedException(typeof(NotSupportedException))]
        public void TestHandmatigEenSprintAanDeSprintsCollectieToevoegenGaatNiet()
        {
            project.Sprints.Add(sprint);
            Assert.Fail();
        }
        
    }
}