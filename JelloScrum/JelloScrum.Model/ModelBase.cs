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

namespace JelloScrum.Model
{
    using System;
    using Castle.ActiveRecord;

    /// <summary>
    /// This is the abstract baseclass used for all entities used in JelloScrum.
    /// </summary>
    public abstract class ModelBase : IEquatable<ModelBase>
    {
        private long id = 0;
        private Guid guid = Guid.NewGuid();

        /// <summary>
        /// This sets the mandatory guid for entities.
        /// </summary>
        public ModelBase()
        {
        }

        /// <summary>
        /// This is the unique ID for all entities in Foleta
        /// </summary>
        [PrimaryKey(PrimaryKeyType.Identity)]
        public virtual long Id
        {
            get { return this.id; }
            set { this.id = value; }
        }

        /// <summary>
        /// This is the unique guid for all entities in Foleta
        /// </summary>
        [Property]
        public virtual Guid Guid
        {
            get { return this.guid; }
            set { this.guid = value; }
        }

        /// <summary>
        /// Equality operator so we can have == semantics
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(ModelBase x, ModelBase y)
        {
            return Equals(x, y);
        }

        /// <summary>
        /// Inequality operator so we can have != semantics
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(ModelBase x, ModelBase y)
        {
            return !(x == y);
        }

        /// <summary>
        /// Bepaald of het gegeven modelbase object gelijk is aan dit modelbase object.
        /// </summary>
        /// <param name="modelBase">Het modelbase object.</param>
        /// <returns>true als de objecten gelijk zijn, anders false.</returns>
        public virtual bool Equals(ModelBase modelBase)
        {
            if (modelBase == null) return false;
            return Id == modelBase.Id && Equals(Guid, modelBase.Guid);
        }

        /// <summary>
        /// Determines whether the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <param name="obj">The <see cref="T:System.Object"/> to compare with the current <see cref="T:System.Object"/>.</param>
        /// <returns>
        /// true if the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>; otherwise, false.
        /// </returns>
        /// <filterPriority>2</filterPriority>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj)) return true;
            return Equals(obj as ModelBase);
        }

        /// <summary>
        /// Serves as a hash function for a particular type.
        /// </summary>
        /// <returns>
        /// A hash code for the current <see cref="T:System.Object"/>.
        /// </returns>
        /// <filterPriority>2</filterPriority>
        public override int GetHashCode()
        {
            return 29 * this.Id.GetHashCode() * this.Guid.GetHashCode();
        }
    }
}