using System;
using Es.Udc.DotNet.WebMovies.Model.Util.Exceptions;
using System.Data.Objects;

namespace Es.Udc.DotNet.WebMovies.Model.Util.Dao
{
    /// <summary>
    /// Interfaces with Generic Dao Operations
    /// </summary>
    /// <typeparam name="E">Entity Type</typeparam>
    /// <typeparam name="PK">Primary Key Type.</typeparam>
    public interface IGenericDao<E, PK>
        : Es.Udc.DotNet.ModelUtil.Dao.IGenericDao<E, PK>
    {

        /// <summary>
        /// Creates the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <exception cref="DuplicateInstanceException&lt;E&gt;"></exception>
        new void Create(E entity);

        /// <summary>
        /// Finds the entity with the specified primary key.
        /// </summary>
        /// <param name="id">The entity id.</param>
        /// <returns>The entity</returns>
        /// <exception cref="InstanceNotFoundException&lt;E&gt;"></exception>
        new E Find(PK id);

        /// <summary>
        /// Determines if the specified entity exists.
        /// </summary>
        /// <param name="id">The  entity id.</param>
        /// <returns>True if entity exists, else otherwise.</returns>
        new bool Exists(PK id);

        /// <summary>
        /// Updates the specified entity.
        /// </summary>
        /// <param name="entity">The entity to be updated.</param>
        /// <exception cref="InstanceNotFoundException&lt;E&gt;"></exception>
        new void Update(E entity);

        /// <summary>
        /// Removes the specified entity.
        /// </summary>
        /// <param name="id">The entity id.</param>
        /// <exception cref="InstanceNotFoundException&lt;E&gt;"></exception>
        new void Remove(PK id);

    }
}
