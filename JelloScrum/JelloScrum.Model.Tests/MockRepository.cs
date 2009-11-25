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

//namespace JelloScrum.Tests
//{
//    public class MockRepository
//    {
//        private Rhino.Mocks.MockRepository repository = new Rhino.Mocks.MockRepository();

//        public T DynamicMock<T>(params object[] argumentsForConstructor) where T : class
//        {
//            T mock = repository.DynamicMock<T>(argumentsForConstructor);
//            Helper.AddToIoCContainer(mock);
//            return mock;
//        }

//        public T PartialMock<T>(params object[] argumentsForConstructor) where T : class
//        {
//            T mock = repository.PartialMock<T>(argumentsForConstructor);
//            Helper.AddToIoCContainer(mock);
//            return mock;
//        }

//        public T StrictMock<T>(params object[] argumentsForConstructor)
//        {
//            T mock = repository.StrictMock<T>(argumentsForConstructor);
//            Helper.AddToIoCContainer(mock);
//            return mock;
//        }

//        public void ReplayAll()
//        {
//            repository.ReplayAll();
//        }

//        public void VerifyAll()
//        {
//            repository.VerifyAll();
//        }
//    }
//}