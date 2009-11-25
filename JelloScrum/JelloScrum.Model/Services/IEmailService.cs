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
    /// Interface voor de e-mail Service
    /// </summary>
    public interface IEmailService
    {
        /// <summary>
        /// Verzend een email naar de ontvanger
        /// </summary>
        /// <param name="bronAdres">Het bron adres.</param>
        /// <param name="doelAdres">Het doel adres.</param>
        /// <param name="onderwerp">het onderwerp.</param>
        /// <param name="templateName">Name of the template.</param>
        /// <param name="propertyBag">De property bag.</param>
        /// <param name="attachments">De attachments.</param>
        void Verzend(string bronAdres, string doelAdres, string onderwerp, string templateName, Dictionary<string, object> propertyBag, MessageAttachment[] attachments);

        /// <summary>
        /// Verzend het wachtwoord naar gebruiker
        /// </summary>
        /// <param name="gebruiker"></param>
        /// <param name="wachtwoord"></param>
        void VerzendWachtwoord(Gebruiker gebruiker, string wachtwoord);

        /// <summary>
        /// Verzend een email naar de ontvanger
        /// </summary>
        /// <param name="message"></param>
        void Verzend(Message message);
    }
}