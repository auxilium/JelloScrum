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

namespace JelloScrum.Model.Entities
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using SD = System.Drawing;
    using System.IO;
    using System.Web;

    using Castle.ActiveRecord;

    /// <summary>
    /// De Avatar klasse
    /// </summary>
    public class Avatar
    {
        #region Fields

            private int x1Cord;
            private int y1Cord;
            private int width;
            private int height;
            private SD.Image originalImage;
            private string avatarImage;
            private string avatarPath;

        #endregion

        #region Constructors

        /// <summary>
        /// Empty constructor
        /// </summary>
        public Avatar()
        {
        }

        #endregion

        #region Properties


        /// <summary>
        /// x1 coordinaat van de crop selection
        /// </summary>
        public virtual int X1Cord
        {
            get { return x1Cord; }
            set { x1Cord = value; }
        }


        /// <summary>
        /// y1 coordinaat van de crop selection
        /// </summary>
        public virtual int Y1Cord
        {
            get { return y1Cord; }
            set { y1Cord = value; }
        }


        /// <summary>
        /// breedte van de crop selection
        /// </summary>
        public virtual int Width
        {
            get { return width; }
            set { width = value; }
        }
        
        /// <summary>
        /// hoogte van de crop selection
        /// </summary>
        public virtual int Height
        {
            get { return height; }
            set { height = value; }
        }

        /// <summary>
        /// Orginele plaatje dat is geupload
        /// </summary>
        public virtual SD.Image OriginalImage
        {
            get { return originalImage; }
            set { originalImage = value; }
        }

        /// <summary>
        /// Bijgesneden plaatje dat als avatar gebruikt wordt
        /// </summary>
        public virtual string AvatarImage
        {
            get { return avatarImage; }
            set { avatarImage = value; }
        }

        public virtual string AvatarPath
        {
            get { return HttpContext.Current.Request.PhysicalApplicationPath + ConfigurationManager.AppSettings["avatarBigPath"]; }
            set { avatarPath = value; }
        }

        #endregion

        #region Methodes

        /// <summary>
        /// Crop de avatar tot aan de selectie
        /// Resize de selectie naar 40px bij 40px
        /// </summary>
        /// <returns></returns>
        public byte[] CropResize(string fileName)
        {
            string img = Path.Combine(AvatarPath, fileName);

            try
            {
                using (originalImage = SD.Image.FromFile(img))
                {

                    SD.Bitmap croppedNotScaled = new SD.Bitmap(Width,Height);
                    SD.Graphics graph = SD.Graphics.FromImage(croppedNotScaled);
                    graph.DrawImage(originalImage,new Rectangle(0,0,Width,Height),new Rectangle(X1Cord,Y1Cord,Width,Height),GraphicsUnit.Pixel);
                    
                    graph.Dispose();
                    SD.Bitmap newAvatar = new SD.Bitmap(40, 40);
                    graph = SD.Graphics.FromImage(newAvatar);
                    graph.SmoothingMode = SmoothingMode.HighQuality;
                    graph.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    graph.DrawImage(croppedNotScaled,new Rectangle(0,0,40,40),new Rectangle(0,0,Width,Height),SD.GraphicsUnit.Pixel);
                    graph.Dispose();

                    MemoryStream ms = new MemoryStream();
                    newAvatar.Save(ms, SD.Imaging.ImageFormat.Bmp);
                    return ms.GetBuffer();

                }

            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
        #endregion
    }
}
