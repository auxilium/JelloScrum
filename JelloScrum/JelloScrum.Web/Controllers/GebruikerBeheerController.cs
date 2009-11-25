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

namespace JelloScrum.Web.Controllers
{
    using System;
    using System.Configuration;
    using System.Drawing;
    using System.IO;
    using System.Web;

    using SD = System.Drawing;
    using System.Drawing.Imaging;
    using System.Drawing.Drawing2D;
    using System.Collections.Generic;

    using Castle.MonoRail.ActiveRecordSupport;
    using Castle.MonoRail.Framework;
    using JelloScrum.Model.Entities;
    using JelloScrum.Model.Enumerations;


    /// <summary>
    /// Controller voor alle Gebruiker beheer acties
    /// </summary>
    [Layout("jellobeheer")]
    public class GebruikerBeheerController : SecureController
    {

        /// <summary>
        /// Index
        /// </summary>
        public void Index()
        {
            PropertyBag.Add("systemRols", Enum.GetNames(typeof(SysteemRol)));
            Titel = "Gebruikers";
        }

        /// <summary>
        /// Gebruikers beheer vanuit een project
        /// </summary>
        /// <param name="sprint"></param>
        public void Index([ARFetch("id")] Sprint sprint)
        {
            PropertyBag.Add("sprint", sprint);
            PropertyBag.Add("gebruikers", GebruikerRepository.ZoekOpNietInSprint(sprint));
            RenderView("sprintindex");
            CancelLayout();
        }

        /// <summary>
        /// Geeft een lijst terug met gebruikers aan de hand van de systeem rol
        /// </summary>
        /// <param name="rol"></param>
        public void ListGebruikers(SysteemRol rol)
        {
            PropertyBag.Add("gebruikers", GebruikerRepository.ZoekOpSysteemRol(rol));
            CancelLayout();
        }

        /// <summary>
        /// Laad de gebruiker
        /// </summary>
        /// <param name="gebruiker"></param>
        public void LoadGebruiker([ARFetch("id")] Gebruiker gebruiker)
        {
            PropertyBag.Add("item", gebruiker);
            RenderView("edit");
            CancelLayout();
        }

        /// <summary>
        /// Nieuwe gebruiker aan maken
        /// </summary>
        /// <param name="rol"></param>
        public void Nieuw(SysteemRol rol)
        {
            PropertyBag.Add("item", new Gebruiker(rol));
            RenderView("edit");
            CancelLayout();
        }

        /// <summary>
        /// Upload plaatje om avatar van te maken
        /// </summary>
        /// <param name="postedFile"></param>
        public void Upload(HttpPostedFile postedFile)
        {
            string[] allowedExtensions = { ".png", ".jpeg", ".jpg", ".gif" };
            string postedFileName = postedFile.FileName.Substring(postedFile.FileName.LastIndexOf("\\") + 1);
            string avatarPath = HttpContext.Current.Request.PhysicalApplicationPath +
                                ConfigurationManager.AppSettings["avatarBigPath"];
            
            try
            {
                if (string.IsNullOrEmpty(postedFileName) || (!((IList<string>)allowedExtensions).Contains(Path.GetExtension(postedFileName).ToLower())))
                    throw new Exception("Kan avatar niet uploaden");


                postedFile.SaveAs(Path.Combine(avatarPath, postedFileName));
                
                CurrentUser.BigAvatar = ConfigurationManager.AppSettings["avatarRelativePath"] + "big/" + postedFileName;
                GebruikerRepository.Save(CurrentUser);
            }
            catch (Exception ex)
            {
                AddInfoMessageToFlashBag("Kan avatar niet uploaden");
            }

            RedirectToAction("GebruikersProfiel");
        }
        /// <summary>
        /// Saved een nieuwe of bestaande gebruiker.
        /// </summary>
        /// <param name="gebruiker"></param>
        /// <param name="avatar"></param>
        public void Save([ARDataBind("item", AutoLoadBehavior.NewInstanceIfInvalidKey)] Gebruiker gebruiker,
                [ARDataBind("avatar", AutoLoadBehavior.NewInstanceIfInvalidKey)] Avatar avatar)
        {
            try
            {
                //Als er geen selectie is gemaakt, hoeven we ook geen avatar te maken.
                if(avatar.Width != 0 && avatar.Height != 0)
                {
                    gebruiker.SmallAvatar = gebruiker.BigAvatar.Replace("big", "small");

                    byte[] cropImage = avatar.CropResize(Path.GetFileName(gebruiker.BigAvatar));
                    using (MemoryStream ms = new MemoryStream(cropImage, 0, cropImage.Length))
                    {
                        ms.Write(cropImage, 0, cropImage.Length);
                        using (SD.Image croppedImage = SD.Image.FromStream(ms, true))
                        {
                            string saveTo =  Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, ConfigurationManager.AppSettings["avatarSmallPath"] + Path.GetFileName(gebruiker.SmallAvatar));
                            croppedImage.Save(saveTo, croppedImage.RawFormat);
                                
                        }
                    }

                }
                GebruikerRepository.Save(gebruiker);
            }
            catch (Exception e)
            {
                AddErrorMessageToFlashBag(e.Message);
            }

            RedirectToAction("GebruikersProfiel");
        }

        /// <summary>
        /// Instellen gebruikersprofiel
        /// </summary>
        public void GebruikersProfiel()
        {
            Titel = "Bewerk gebruikersprofiel";

            PropertyBag.Add("item", CurrentUser);
            RenderView("edit");
        }

        /// <summary>
        /// Beheer de lijst met mijn projecten
        /// </summary>
        public void MijnProjecten()
        {
            Titel = "Kies mijn projecten";
            ICollection<Project> projects = ProjectRepository.FindAll();
            PropertyBag.Add("projecten", projects);
            if (projects.Count == 0)
            {
                AddErrorMessageToPropertyBag("Er zijn nog geen projecten gedefini&euml;erd.");
            }
            else
            {
                foreach (ProjectShortList list in CurrentUser.ProjectShortList)
                {
                    projects.Remove(list.Project);
                }
            }
            PropertyBag.Add("item", CurrentUser);
        }

        /// <summary>
        /// 
        /// </summary>
        public void ProjectToevoegenAanShortList([ARFetch("id")] Project project)
        {
            try
            {
                CurrentUser.ShortListProjectToevoegen(project);
                GebruikerRepository.Save(CurrentUser);
            }
            catch
            {
                AddErrorMessageToFlashBag("Het toevoegen van dit project aan de shortlist is niet gelukt, probeer het nogmaals.");
            }
            RedirectToAction("mijnprojecten");
        }

        /// <summary>
        /// 
        /// </summary>
        public void ProjectVerwijderenVanShortList([ARFetch("id")] ProjectShortList projectShortList)
        {
            try
            {
                ProjectShortListRepository.Delete(projectShortList);
            }
            catch
            {
                AddErrorMessageToFlashBag("Het verwijderen van dit project uit de shortlist is niet gelukt, probeer het nogmaals.");
            }
            RedirectToAction("mijnprojecten");
        }


    }
}