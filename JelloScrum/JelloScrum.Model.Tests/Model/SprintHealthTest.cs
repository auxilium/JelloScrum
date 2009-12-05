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
    using Creations;
    using Entities;
    using Enumerations;
    using Helpers;
    using NUnit.Framework;

    [TestFixture]
    public class SprintHealthTest : TestBase
    {
        private Sprint testCaseSprint;
        private Project testCaseProject;
        private Story[] testCaseStories = new Story[10];
        private Gebruiker[] testCaseDevelopers = new Gebruiker[4];

        public override void SetUp()
        {
            Random random = new Random();
            
            base.SetUp();

            // 4 gebruikers
            for (int i = 0; i<testCaseDevelopers.Length; i++ )
            {
                testCaseDevelopers[i] = Creation.Gebruiker(SystemRole.User);
            }

            // 1 project
            testCaseProject = Creation.Project();

            // 1 sprint
            testCaseSprint = Creation.Sprint(testCaseProject);
            testCaseSprint.WerkDagen = 40; // 4 devvers voor 2 weken = 4 * 2 * 5 dagen.

            foreach (Gebruiker developer in testCaseDevelopers)
            {
                testCaseSprint.AddUser(developer, SprintRole.Developer);
            }
            testCaseSprint.SprintGebruikers[0].VoegRolToe(SprintRole.ScrumMaster);

            // 1 story, 2 taken, beide afgesloten
            const int hoursPerStoryPoint = 2;
            Story testCaseStory1 = Creation.Story(testCaseProject, StoryPoint.Eight, hoursPerStoryPoint, Priority.Must, testCaseDevelopers[random.Next(0,3)]);
            testCaseSprint.CreateSprintStoryFor(testCaseStory1);

            Task task1_1 = Creation.Task();
            testCaseStory1.AddTask(task1_1);
            task1_1.RegisterTime(testCaseDevelopers[random.Next(0, 3)], DateTime.Now, testCaseSprint, new TimeSpan(8,0,0));
            task1_1.Close();
            Task task1_2 = Creation.Task();
            testCaseStory1.AddTask(task1_2);
            task1_2.RegisterTime(testCaseDevelopers[random.Next(0, 3)], DateTime.Now, testCaseSprint, new TimeSpan(12, 0, 0));
            task1_2.Close();
        }

        [Test]
        public void TestStoryPointVelocityMet1Story2TakenBeideAfgeslotenGeeftJuisteStoryPointsVelocity()
        {
            Velocity velocity = SprintHealthHelper.GetVelocity(testCaseSprint);
            //0.2 want er zijn 8 storypunten afgerond met 40 mandagen
            Assert.AreEqual(0.2m, velocity.StoryPointVelocity, velocity.StoryPointVelocity.ToString());
        }

        [Test]
        public void TestStoryPointVelocityMet1Story2TakenBeideAfgeslotenGeeftJuisteHoursPointsVelocity()
        {
            Velocity velocity = SprintHealthHelper.GetVelocity(testCaseSprint);
            //0.2 want er zijn 8 storypunten afgerond met 40 mandagen
            Assert.AreEqual(0.2m, velocity.StoryPointVelocity, velocity.StoryPointVelocity.ToString());
        }

    }

}