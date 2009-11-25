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

namespace JelloScrum.Tests
{
    using System.IO;
    using Castle.ActiveRecord;
    using JelloScrum.Model.Tests;
    using NUnit.Framework;

    public class DBTestBase : TestBase
    {
        [SetUp]
        public override void SetUp()
        {
            //delete an existing database.
            //if no file is found file.delete fails silently
            File.Delete("test.db");

            base.SetUp();
        //    UnitOfWork.Start();
            ActiveRecordStarter.CreateSchema();
       }

        [TearDown]
        public override void TearDown()
        {
            base.TearDown();
            ActiveRecordStarter.DropSchema();
        //    UnitOfWork.Current.Dispose();
        }
    }
}