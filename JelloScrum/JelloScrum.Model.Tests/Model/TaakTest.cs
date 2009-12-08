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
    public class TaakTest : TestBase
    {
        private Task taak;
        
        public override void SetUp()
        {
            taak = new Task();

            base.SetUp();
        }

        [Test]
        public void TestSaveTaakAlsDone()
        {
            taak.Close();

            Assert.IsTrue(taak.State == State.Closed);
        }

        [Test]
        public void TestTaakOppakkenGeeftEenBehandelaarDeTaak()
        {
            Task task = Creation.Task();
            SprintUser sprintGebruiker = Creation.SprintGebruiker(Creation.Gebruiker());
            sprintGebruiker.TakeTask(task);

            Assert.IsTrue(sprintGebruiker.Tasks.Contains(task));

        }

        [Test]
        public void TestTaakAlsNietOpgepaktZettenZorgtDatBehandelaarTaakNietMeerHeeft()
        {
            Task task = Creation.Task();
            SprintUser sprintGebruiker = Creation.SprintGebruiker(Creation.Gebruiker());
            sprintGebruiker.TakeTask(task);
            sprintGebruiker.UnAssignTask(task);
            Assert.IsTrue(sprintGebruiker.Tasks.Contains(task) == false);
        }

        [Test]
        public void TestTaakOvernemenPaktTaakAfVanBehandelaar()
        {
            Task task = Creation.Task();
            SprintUser sprintGebruiker1 = Creation.SprintGebruiker(Creation.Gebruiker());
            SprintUser sprintGebruiker2 = Creation.SprintGebruiker(Creation.Gebruiker());
            sprintGebruiker1.TakeTask(task);
            sprintGebruiker2.TakeOverTask(task);
            Assert.IsTrue(sprintGebruiker1.Tasks.Contains(task) == false && sprintGebruiker2.Tasks.Contains(task));
        }
    }
}