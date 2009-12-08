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

namespace JelloScrum.Repositories.Exceptions
{
    using System;
    using System.Runtime.Serialization;
    using System.Text;
    using JelloScrum.Model.Services;


    public class JelloScrumRepositoryException : Exception
    {
        public JelloScrumRepositoryException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public JelloScrumRepositoryException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public JelloScrumRepositoryException(string message) : base(message)
        {
        }

        public JelloScrumRepositoryException()
        {
        }

        public string LogMessage
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine(string.Format("{0} threw an exception", Source));
                sb.AppendLine(Message);
                Exception inner = InnerException;
                while (inner != null)
                {
                    sb.AppendLine(inner.Message);
                    inner = inner.InnerException;
                }
                return sb.ToString();
            }
        }
    }
}