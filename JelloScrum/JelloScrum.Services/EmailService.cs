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

using System;
using System.Collections.Generic;
using Castle.Components.Common.EmailSender;

namespace JelloScrum.Services
{
    using Castle.Core.Logging;
    using JelloScrum.Model.Entities;
    using JelloScrum.Model.Services;

    public class EmailService : IEmailService
    {
        private readonly ILogger logger = NullLogger.Instance;
        private readonly IEmailSender emailSender;
        private readonly ITemplateParserService emailTemplateParserService;

        private string bccEmailAddress = string.Empty;
        private string fromEmailAddress = string.Empty;

        public EmailService(ILogger logger, IEmailSender emailSender, ITemplateParserService emailTemplateParserService, string bccEmailAddress, string fromEmailAddress)
        {
            this.logger = logger;
            this.emailSender = emailSender;
            this.emailTemplateParserService = emailTemplateParserService;
            this.bccEmailAddress = bccEmailAddress;
            this.fromEmailAddress = fromEmailAddress;
        }

        #region IEmailService Members

        /// <summary>
        /// Verzend een email naar de ontvanger
        /// </summary>
        /// <returns>een true als er geen foutmelding was</returns>
        public void Send(string fromEmailAddress, string emailAdres, string onderwerp, string templateName, Dictionary<string, object> propertyBag, MessageAttachment[] attachments)
        {
            if (string.IsNullOrEmpty(emailAdres))
            {
                throw new Exception("Geen emailadres opgegeven");
            }
            if (string.IsNullOrEmpty(onderwerp))
            {
                throw new Exception("Geen onderwerp opgegeven");
            }

            Message message = new Message();
            message.Body = emailTemplateParserService.Parse("email", templateName, propertyBag);
            message.To = emailAdres;

            if (!string.IsNullOrEmpty(bccEmailAddress))
            {
                message.Bcc = bccEmailAddress;
            }
            if (!string.IsNullOrEmpty(fromEmailAddress))
            {
                message.From = fromEmailAddress;
            }
            else
            {
                message.From = this.fromEmailAddress;
            }
            message.Subject = onderwerp;
            message.Format = Format.Html;

            if (attachments != null)
            {
                foreach (MessageAttachment attachment in attachments)
                {
                    if (attachment != null)
                    {
                        message.Attachments.Add(attachment);
                    }
                }
            }
            Send(message);
        }

        /// <summary>
        /// Verzend het wachtwoord naar gebruiker
        /// </summary>
        /// <param name="gebruiker"></param>
        /// <param name="wachtwoord"></param>
        public void SendPassword(User gebruiker, string wachtwoord)
        {
            Dictionary<string, object> propertyBag = new Dictionary<string, object>();
            propertyBag.Add("gebruiker", gebruiker);
            propertyBag.Add("password", wachtwoord);
            Send(string.Empty, gebruiker.Email, "Uw nieuwe Jellow wachtwoord", "password", propertyBag, null);
        }

        public void Send(Message message)
        {
            try
            {
                emailSender.Send(message);
            }
            catch (Exception e)
            {
                logger.Fatal("Het versturen van de E-Mail is mislukt. ", e.Message);
                throw new Exception("Het versturen van de E-Mail is mislukt, Probeer het later nog eens.");
            }
        }

        #endregion
    }
}