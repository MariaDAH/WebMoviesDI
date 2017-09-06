using System;
using System.Data;
using System.Data.Objects.DataClasses;
using Es.Udc.DotNet.WebMovies.Model.Util.Exceptions;

namespace Es.Udc.DotNet.WebMovies.Model.Util.Dao
{
    public class GenericDaoEntityFramework<E, PK>
        : Es.Udc.DotNet.ModelUtil.Dao.GenericDaoEntityFramework<E, PK>,
        IGenericDao<E, PK> where E : IEntityWithKey
    {

        #region IGenericDao<E> Members

        public new void Create(E entity)
        {
            try
            {
                base.Create(entity);
            }
            catch (Es.Udc.DotNet.ModelUtil.Exceptions.InstanceNotFoundException e)
            {
                throw new InstanceNotFoundException<E>(e.Key);
            }
            catch (Es.Udc.DotNet.ModelUtil.Exceptions.DuplicateInstanceException e)
            {
                throw new DuplicateInstanceException<E>(e.Key);
            }
        }

        public new E Find(PK id)
        {
            try
            {
                return base.Find(id);
            }
            catch (Es.Udc.DotNet.ModelUtil.Exceptions.InstanceNotFoundException e)
            {
                throw new InstanceNotFoundException<E>(e.Key);
            }
            catch (Es.Udc.DotNet.ModelUtil.Exceptions.DuplicateInstanceException e)
            {
                throw new DuplicateInstanceException<E>(e.Key);
            }
        }

        public new bool Exists(PK id)
        {
            try
            {
                return base.Exists(id);
            }
            catch (Es.Udc.DotNet.ModelUtil.Exceptions.InstanceNotFoundException e)
            {
                throw new InstanceNotFoundException<E>(e.Key);
            }
            catch (Es.Udc.DotNet.ModelUtil.Exceptions.DuplicateInstanceException e)
            {
                throw new DuplicateInstanceException<E>(e.Key);
            }
        }

        public new void Update(E entity)
        {
            try
            {
                Context.GetObjectByKey(entity.EntityKey);
            }
            catch (ObjectNotFoundException)
            {
                throw new InstanceNotFoundException<E>(entity.EntityKey);
            }

            try
            {
                base.Update(entity);
            }
            catch (Es.Udc.DotNet.ModelUtil.Exceptions.InstanceNotFoundException e)
            {
                throw new InstanceNotFoundException<E>(e.Key);
            }
            catch (Es.Udc.DotNet.ModelUtil.Exceptions.DuplicateInstanceException e)
            {
                throw new DuplicateInstanceException<E>(e.Key);
            }
        }

        public new void Remove(PK id)
        {
            try
            {
                base.Remove(id);
            }
            catch (Es.Udc.DotNet.ModelUtil.Exceptions.InstanceNotFoundException e)
            {
                throw new InstanceNotFoundException<E>(e.Key);
            }
            catch (Es.Udc.DotNet.ModelUtil.Exceptions.DuplicateInstanceException e)
            {
                throw new DuplicateInstanceException<E>(e.Key);
            }
        }

        #endregion

    }
}
