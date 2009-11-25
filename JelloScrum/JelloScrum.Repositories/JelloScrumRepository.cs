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

namespace JelloScrum.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Castle.ActiveRecord;
    using Exceptions;
    using Model;
    using Model.IRepositories;
    using NHibernate;

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class JelloScrumRepository<T> : IJelloScrumRepository<T> where T : class
    {
        //protected static ISession Session
        //{
        //    get { return ActiveRecordMediator<T>.GetSessionFactoryHolder().GetSessionFactory(typeof(ModelBase)).GetCurrentSession(); }
        //}

        public T Get(int id)
        {
            try
            {
                
                //return Session.Get<T>(id);
                return ActiveRecordMediator<T>.Exists(id) ? ActiveRecordMediator<T>.FindByPrimaryKey(id, true) : null;
            }
            catch (Exception e)
            {
                return null;
                //throw new JelloScrumRepositoryException("Het ophalen van gegevens is mislukt.", e);
            }
        }

        public T Load(int id)
        {
            try
            {
                //return  Session.Load<T>(id);
                return ActiveRecordMediator<T>.FindByPrimaryKey(id);
            }
            catch (ObjectNotFoundException e)
            {
               throw new JelloScrumRepositoryException("Er werd geprobeerd een object op te halen dat niet bestaat.",e);
            }
            catch (HibernateException e)
            {
                throw new JelloScrumRepositoryException("Het ophalen van gegevens is mislukt.",e);
            }
        }

        public virtual void Delete(T entity)
        {
            //TODO kijken of die try wel OM die transaction heen moet, of dat deze eigenlijk binnen de delegate moet/
            try
            {
                using (TransactionScope transactionScope = new TransactionScope(TransactionMode.Inherits, OnDispose.Rollback))
                {
                    ActiveRecordMediator<T>.Delete(entity);
                    //Session.Delete(entity);
                    transactionScope.VoteCommit();
                }
            }
            catch (HibernateException e)
            {
                throw new JelloScrumRepositoryException("Het verwijderen van gegevens is mislukt.", e);
            }
        }

        public void DeleteAll()
        {
            throw new NotImplementedException();
        }

        public virtual T Save(T entity)
        {
            //TODO kijken of die try wel OM die transaction heen moet, of dat deze eigenlijk binnen de delegate moet/
            try
            {
                using (TransactionScope transactionScope = new TransactionScope(TransactionMode.Inherits, OnDispose.Rollback))
                {
                    ActiveRecordMediator<T>.Save(entity);
                    //Session.Save(entity);
                    transactionScope.VoteCommit();
                }
            }
            catch (HibernateException e)
            {
                throw new JelloScrumRepositoryException("Het opslaan van gegevens is mislukt.",e);
            }
            return entity;
        }

        public T SaveOrUpdate(T entity)
        {
            //TODO kijken of die try wel OM die transaction heen moet, of dat deze eigenlijk binnen de delegate moet/
            try
            {
                using (TransactionScope transactionScope = new TransactionScope(TransactionMode.Inherits, OnDispose.Rollback))
                {
                    ActiveRecordMediator<T>.Update(entity);
                    //Session.SaveOrUpdate(entity);
                    transactionScope.VoteCommit();
                }
            }
            catch (HibernateException e)
            {
                throw new JelloScrumRepositoryException("Het opslaan van gegevens is mislukt.",e);
            }
            return entity;
        }

        //public T SaveOrUpdateCopy(T entity)
        //{
        //    //TODO kijken of die try wel OM die transaction heen moet, of dat deze eigenlijk binnen de delegate moet/
        //    try
        //    {
        //        using (TransactionScope transactionScope = new TransactionScope(TransactionMode.Inherits, OnDispose.Rollback))
        //        {
        //            ActiveRecordMediator<T>.s
        //            Session.SaveOrUpdateCopy(entity);
        //        }
        //    }
        //    catch (HibernateException e)
        //    {
        //        throw new JelloScrumRepositoryException("Het opslaan van gegevens is mislukt.", e);
        //    }
        //    return entity;
        //}

        public void Update(T entity)
        {
            //TODO kijken of die try wel OM die transaction heen moet, of dat deze eigenlijk binnen de delegate moet/
            try
            {
                using (TransactionScope transactionScope = new TransactionScope(TransactionMode.Inherits, OnDispose.Rollback))
                {
                    ActiveRecordMediator<T>.Update(entity);
                    //Session.Update(entity);
                }
            }
            catch (HibernateException e)
            {
                throw new JelloScrumRepositoryException("Het opslaan van gegevens is mislukt.",e);
            }
        }

        public IList<T> FindAll()
        {
            return ActiveRecordMediator<T>.FindAll();
            //return Session.CreateQuery(string.Format("from {0}", typeof(T))).List<T>();
        }

        public T FindOne()
        {
            throw new NotImplementedException();
        }

        public T FindFirst()
        {
            throw new NotImplementedException();
        }

        public bool Exists()
        {
            throw new NotImplementedException();
        }

        public long Count()
        {
            throw new NotImplementedException();
        }

        public T Create()
        {
            throw new NotImplementedException();
        }
    }
}