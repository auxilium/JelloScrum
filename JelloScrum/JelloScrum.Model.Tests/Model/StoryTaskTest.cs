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
            project.AddSprint(sprint);
            story = new Story(project, new User(), null, StoryType.UserStory);
            task = new Task();
            task2 = new Task();
           
            base.SetUp();
        }

        [Test]
        public void TestVoegTaskToe()
        {
            story.AddTask(task);

            Assert.AreEqual(story, story.Tasks[0].Story);
        }

        [Test]
        public void TestMeermaalsDezelfdeTaskToevoegenGaatNiet()
        {
            story.AddTask(task);
            story.AddTask(task);

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
            story.AddTask(task);
            story.AddTask(task2);

            task.RegisterTime(new User(), DateTime.Now, sprint, new TimeSpan(1,30,15) );
            task.RegisterTime(new User(), DateTime.Now, sprint, new TimeSpan(3,10,26) );
            task2.RegisterTime(new User(), DateTime.Now, sprint, new TimeSpan(2,42,58) );

            Assert.AreEqual(new TimeSpan(7, 23, 39), story.TotalTimeSpent());
        }

        [Test]
        public void TestBepaalWelkeTakenNogNietZijnOpgepakt()
        {
            task.State = State.Taken;
            
            story.AddTask(task);
            story.AddTask(task2); //task2 heeft nog de default status: NietOpgepakt

            IList<Task> result = story.GetTasksWith(State.Open);

            Assert.IsTrue(result.Contains(task2));
            Assert.AreEqual(1, result.Count);
        }

        [Test]
        public void TestBepaalStoryStatusAanDeHandVanTasksStatus_GeenOpgepakteStories()
        {
            story.AddTask(task);
            story.AddTask(task2);

            Assert.AreEqual(State.Open, story.State);
        }
               
        [Test]
        public void TestBepaalStoryStatusAanDeHandVanTasksStatus_EenOpgepakteStory()
        {
            task.State = State.Taken;

            story.AddTask(task);
            story.AddTask(task2);

            Assert.AreEqual(State.Taken, story.State);
        }

        [Test]
        public void TestBepaalStoryStatusAanDeHandVanTasksStatus_AlleStoriesZijnAfgesloten()
        {
            task.State = State.Closed;
            task2.State = State.Closed;

            story.AddTask(task);
            story.AddTask(task2);

            Assert.AreEqual(State.Closed, story.State);
        }

        [Test]
        public void TestUrenSchattingTakenKleinerUrenSchattingStory()
        {
            task.Estimation = new TimeSpan(0,1,0,0);
            task2.Estimation = new TimeSpan(0, 1, 0, 0);

            story.AddTask(task);
            story.AddTask(task2);
            story.Estimation = new TimeSpan(0, 3, 0, 0);

            Assert.IsTrue(story.IsEstimatedTimeOfTasksLessThenEstimatedTimeOfStory());
        }

        [Test]
        public void TestUrenSchattingTakenGelijkUrenSchattingStory()
        {
            task.Estimation = new TimeSpan(0, 1, 0, 0);
            task2.Estimation = new TimeSpan(0, 1, 0, 0);

            story.AddTask(task);
            story.AddTask(task2);
            story.Estimation = new TimeSpan(0, 2, 0, 0);

            Assert.IsTrue(story.IsEstimatedTimeOfTasksLessThenEstimatedTimeOfStory());
        }

        [Test]
        public void TestUrenSchattingTakenGroterUrenSchattingStory()
        {
            task.Estimation = new TimeSpan(0, 1, 0, 0);
            task2.Estimation = new TimeSpan(0, 2, 0, 0);

            story.AddTask(task);
            story.AddTask(task2);
            story.Estimation = new TimeSpan(0, 2, 0, 0);

            Assert.IsFalse(story.IsEstimatedTimeOfTasksLessThenEstimatedTimeOfStory());
        }
    }
}