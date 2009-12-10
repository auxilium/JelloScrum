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

//namespace JelloScrum.Tests.Helpers
//{
//    using System;
//    using System.Collections.Generic;

//    internal class Utilities
//    {
//        /// <summary>
//        /// Mocks INTERFACE-Type T and adds the instance of INTERFACE-Type T to the container
//        /// Please note that T must be an INTERFACE!!!
//        /// </summary>
//        /// <typeparam name="T"></typeparam>
//        /// <param name="mockRepository"></param>
//        /// <returns></returns>
//        public static T MockInterfaceAndAddMockToContainer<T>(MockRepository mockRepository)
//        {
//            T mock = mockRepository.StrictMock<T>();
//            AddToContainer(mock);
//            return mock;
//        }

//        /// <summary>
//        /// Adds the instance of INTERFACE-Type T to the container
//        /// Please note that T must be an INTERFACE!!!
//        /// </summary>
//        /// <typeparam name="T"></typeparam>
//        /// <param name="instance"></param>
//        public static void AddToContainer<T>(T instance)
//        {
//            string key = typeof (T).Name;
//            if (IoC.Container.Kernel.HasComponent(key))
//                IoC.Container.Kernel.RemoveComponent(key);

//            IoC.Container.Kernel.AddComponentInstance(key, typeof (T), instance);
//        }

//        /// <summary>
//        /// Adds the instance of INTERFACE-Type T to the container
//        /// Please note that T must be an INTERFACE!!!
//        /// </summary>
//        /// <typeparam name="T"></typeparam>
//        /// <param name="instanceType"></param>
//        public static void AddToContainer<T>(Type instanceType)
//        {
//            string key = typeof (T).Name;
//            if (IoC.Container.Kernel.HasComponent(key))
//                IoC.Container.Kernel.RemoveComponent(key);

//            IoC.Container.AddComponent(key, typeof (T), instanceType);
//        }

//        public static void AddDependencyToContainer<T>(string key, object value)
//        {
//            Dictionary<string, object> dependencies = new Dictionary<string, object>();
//            dependencies.Add(key, value);
//            IoC.Container.Kernel.RegisterCustomDependencies(typeof (T), dependencies);
//        }
//    }
//}