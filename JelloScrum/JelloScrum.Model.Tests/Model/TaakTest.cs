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

            Assert.IsTrue(taak.Status == Status.Afgesloten);
        }

        [Test]
        public void TestTaakOppakkenGeeftEenBehandelaarDeTaak()
        {
            Task task = Creation.Task();
            SprintGebruiker sprintGebruiker = Creation.SprintGebruiker(Creation.Gebruiker());
            sprintGebruiker.PakTaakOp(task);

            Assert.IsTrue(sprintGebruiker.Taken.Contains(task));

        }

        [Test]
        public void TestTaakAlsNietOpgepaktZettenZorgtDatBehandelaarTaakNietMeerHeeft()
        {
            Task task = Creation.Task();
            SprintGebruiker sprintGebruiker = Creation.SprintGebruiker(Creation.Gebruiker());
            sprintGebruiker.PakTaakOp(task);
            sprintGebruiker.GeefTaakAf(task);
            Assert.IsTrue(sprintGebruiker.Taken.Contains(task) == false);
        }

        [Test]
        public void TestTaakOvernemenPaktTaakAfVanBehandelaar()
        {
            Task task = Creation.Task();
            SprintGebruiker sprintGebruiker1 = Creation.SprintGebruiker(Creation.Gebruiker());
            SprintGebruiker sprintGebruiker2 = Creation.SprintGebruiker(Creation.Gebruiker());
            sprintGebruiker1.PakTaakOp(task);
            sprintGebruiker2.NeemTaakOver(task);
            Assert.IsTrue(sprintGebruiker1.Taken.Contains(task) == false && sprintGebruiker2.Taken.Contains(task));
        }
    }
}