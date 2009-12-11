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
    using System.Collections.ObjectModel;
    using System.Security.Principal;
    using Castle.ActiveRecord;
    using Castle.Components.Validator;
    using Enumerations;
    using Login.Model;

    /// <summary>
    /// Represents a user
    /// </summary>
    [ActiveRecord(Lazy = false, Table = "JelloScrumUser")]
    public class User : UserBase<User>
    {
        #region fields

        private string name = string.Empty;
        private string fullName = string.Empty;
        private SystemRole systemRole = SystemRole.User;
        private IList<SprintUser> sprintUsers = new List<SprintUser>();
        private IList<ProjectShortList> projectShortList = new List<ProjectShortList>();
        private string email = string.Empty;
        private Sprint activeSprint;
        private string bigAvatar = string.Empty;
        private string smallAvatar = string.Empty;
        private Guid guid = Guid.NewGuid();
        private long id;
        #endregion

        #region constructors
        
        /// <summary>
        /// Initializes a new instance of the <see cref="User"/> class.
        /// </summary>
        public User()
        {
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="User"/> class.
        /// </summary>
        /// <param name="role">The role.</param>
        public User(SystemRole role)
        {
            systemRole = role;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="User"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="systemRole">The system role.</param>
        public User(string name, SystemRole systemRole)
        {
            this.name = name;
            this.systemRole = systemRole;
        }

        #endregion

        #region properties

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        [Property, ValidateNonEmpty("Please provide a name.")]
        public virtual string Name
        {
            get { return name; }
            set { name = value; }
        }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        /// <value>The password.</value>
        [Property(CustomAccess = "field.camelcase")]
        public override string PassWord
        {
            get { return base.PassWord; }
            
        }

        /// <summary>
        /// Gets or sets the full name
        /// </summary>
        /// <value>The full name.</value>
        [Property]
        public virtual string FullName
        {
            get { return fullName; }
            set { fullName = value; }
        }

        /// <summary>
        /// The Salt
        /// </summary>
        [Property(CustomAccess = "field.camelcase")]
        public override string Salt
        {
            get { return base.Salt; }
        }

        /// <summary>
        /// The username
        /// </summary>
        [Property(CustomAccess = "field.camelcase")]
        public override string UserName
        {
            get { return base.UserName; }
        }

        /// <summary>
        /// Is the user active (and allowed to login)?
        /// </summary>
        [Property]
        public override bool IsActive
        {
            get { return base.IsActive; }
            set { base.IsActive = value; }
        }

        /// <summary>
        /// The systemrole of this user
        /// </summary>
        [Property]
        public virtual SystemRole SystemRole
        {
            get { return systemRole; }
            set { systemRole = value; }
        }

        /// <summary>
        /// The active sprint for this user.
        /// </summary>
        /// <value>The actieve sprint.</value>
        [BelongsTo]
        public virtual Sprint ActiveSprint
        {
            get { return activeSprint; }
            set { activeSprint = value; }
        }

        /// <summary>
        /// Gets a readonly collection of sprintusers this user has.
        /// Add sprintuser with <see cref="AddSprintUser"/>
        /// </summary>
        [HasMany(Cascade = ManyRelationCascadeEnum.SaveUpdate, Inverse = true, Lazy = true, Access = PropertyAccess.FieldCamelcase)]
        public IList<SprintUser> SprintUsers
        {
            get { return new ReadOnlyCollection<SprintUser>(sprintUsers); }
        }

        /// <summary>
        /// Gets a readonly collection of projects this user has on his shortlist.
        /// </summary>
        [HasMany(Cascade = ManyRelationCascadeEnum.SaveUpdate, Inverse = true, Lazy = true, Access = PropertyAccess.FieldCamelcase)]
        public virtual IList<ProjectShortList> ProjectShortList
        {
            get { return new ReadOnlyCollection<ProjectShortList>(projectShortList); }
        }

        /// <summary>
        /// E-mail address
        /// </summary>
        [Property]
        public virtual string Email
        {
            get { return email; }
            set { email = value; }
        }

        /// <summary>
        /// The big avatar
        /// </summary>
        [Property]
        public virtual string BigAvatar
        {
            get { return bigAvatar; }
            set { bigAvatar = value; }
        }

        /// <summary>
        /// The small avatar
        /// </summary>
        [Property]
        public virtual string SmallAvatar
        {
            get { return smallAvatar; }
            set { smallAvatar = value; }
        }

        #endregion

        #region IPrincipal Members

        /// <summary>
        /// The Identity
        /// </summary>
        /// <value></value>
        /// <returns>The <see cref="T:System.Security.Principal.IIdentity"/> object associated with the current principal.</returns>
        public override IIdentity Identity
        {
            get { return new GenericIdentity(UserName, "User"); }
        }

        /// <summary>
        /// Determines whether the current principal belongs to the specified role.
        /// </summary>
        /// <param name="role">The name of the role for which to check membership.</param>
        /// <returns>
        /// true if the current principal is a member of the specified role; otherwise, false.
        /// </returns>
        public override bool IsInRole(string role)
        {
            return role == GetType().Name;
        }

        /// <summary>
        /// Does the user have this role?
        /// </summary>
        /// <param name="role">The role.</param>
        /// <returns>
        /// 	<c>true</c> if [is in role] [the specified role]; otherwise, <c>false</c>.
        /// </returns>
        public bool IsInRole(SystemRole role)
        {
            return role == systemRole;
        }

        #endregion

        #region methods

        /// <summary>
        /// Add a sprintuser
        /// </summary>
        /// <param name="sprintUser">The sprint user.</param>
        internal virtual void AddSprintUser(SprintUser sprintUser)
        {
            if (!sprintUsers.Contains(sprintUser))
                sprintUsers.Add(sprintUser);
            sprintUser.User = this;
        }

        /// <summary>
        /// Removes the sprint user.
        /// </summary>
        /// <param name="sprintUser">The sprint user.</param>
        internal virtual void RemoveSprintUser(SprintUser sprintUser)
        {
            sprintUsers.Remove(sprintUser);
            sprintUser.User = null;
        }

        /// <summary>
        /// Remove this user from the given sprint. This is done by detaching the sprintuser from this user and the given sprint
        /// </summary>
        /// <param name="sprint">The sprint.</param>
        public virtual void RemoveFromSprint(Sprint sprint)
        {
            if (sprint == null)
                throw new ArgumentNullException("sprint", "The sprint cannot be null.");

            SprintUser sprintUser = sprint.GetSprintUserFor(this);
            if (sprintUser == null)
                return;

            sprintUser.DecoupleSprintUser();
        }

        /// <summary>
        /// Gets the sprintuser for this user and the given sprint.
        /// </summary>
        /// <param name="sprint">The sprint.</param>
        /// <returns>The sprintuser</returns>
        public SprintUser GetSprintUserFor(Sprint sprint)
        {
            foreach (SprintUser sprintUser in sprintUsers)
            {
                if (sprintUser.Sprint == sprint)
                    return sprintUser;
            }
            return null;
            
        }

        /// <summary>
        /// Gets the sprintuser for the active sprint of this user.
        /// </summary>
        /// <returns>The sprintuser</returns>
        public SprintUser GetActiveSprintUser()
        {
            return ActiveSprint == null ? null : GetSprintUserFor(ActiveSprint);
        }

        /// <summary>
        /// Adds the given project to the projects shortlist.
        /// </summary>
        /// <param name="project">The project.</param>
        public void AddProjectToShortList(Project project)
        {
            foreach (ProjectShortList shortList in ProjectShortList)
            {
                if(shortList.Project.Equals(project))
                    return;
            }

            projectShortList.Add(new ProjectShortList(this,project));
        }

        /// <summary>
        /// Removes the given project from the projects shortlist.
        /// </summary>
        /// <param name="project">The project.</param>
        public void RemoveProjectFromShortList(ProjectShortList project)
        {
            if (ProjectShortList.Contains(project))
                ProjectShortList.Remove(project);
        }

        #endregion

        /// <summary>
        /// A globally unique identifier for the object
        /// </summary>
        [Property(Access = PropertyAccess.FieldCamelcase)]
        public override Guid Guid
        {
            get { return guid; }
        }

        /// <summary>
        /// A unique identifier for the object after the object was persisted.
        /// Usually this identifier is used in the database as primary key.
        /// </summary>
        [PrimaryKey(PrimaryKeyType.Identity, CustomAccess = "field.camelcase")]
        public override long Id
        {
            get { return id; }
        }
    }
}