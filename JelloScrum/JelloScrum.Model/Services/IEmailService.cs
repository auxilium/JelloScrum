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

namespace JelloScrum.Model.Services
{
    using System.Collections.Generic;
    using Castle.Components.Common.EmailSender;
    using Entities;

    /// <summary>
    /// Interface for the EmailService
    /// </summary>
    public interface IEmailService
    {
        /// <summary>
        /// Send an email
        /// </summary>
        /// <param name="fromAddress">The address the email is sent from.</param>
        /// <param name="toAddress">The address the email is sent to.</param>
        /// <param name="subject">The subject.</param>
        /// <param name="templateName">Name of the template.</param>
        /// <param name="propertyBag">The property bag.</param>
        /// <param name="attachments">The attachments.</param>
        void Send(string fromAddress, string toAddress, string subject, string templateName, Dictionary<string, object> propertyBag, MessageAttachment[] attachments);

        /// <summary>
        /// Send a password to the given user.
        /// </summary>
        /// <param name="user">The user</param>
        /// <param name="password">The password</param>
        void SendPassword(Gebruiker user, string password);

        /// <summary>
        /// Send the given message
        /// </summary>
        /// <param name="message"></param>
        void Send(Message message);
    }
}