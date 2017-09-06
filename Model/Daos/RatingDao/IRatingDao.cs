using Es.Udc.DotNet.WebMovies.Model.Util.Collections;
using Es.Udc.DotNet.WebMovies.Model.Util.Dao;

namespace Es.Udc.DotNet.WebMovies.Model.Daos.RatingDao
{
    public interface IRatingDao
        : IGenericDao<Rating, long>
    {

        /// <summary>
        /// Finds the ratings by linkId
        /// </summary>
        /// <param name="linkId">linkId</param>
        /// <param name="startIndex">startIndex</param>
        /// <param name="count">count</param>
        /// <returns>The List of ratings for this link</returns>
        /// <exception cref="InstanceNotFoundException&lt;Rating&gt;"/>
        /// <exception cref="NoMoreItemsException&lt;Rating&gt;"/>
        ListBlock<Rating> ListForLink(long linkId, int startIndex, int count);
        
        /// <summary>
        /// Finds the comment made by the user with userId about a link with linkId
        /// </summary>
        /// <param name="linkId">linkId</param>
        /// <param name="userId">userId</param>
        /// <returns>The Rating</returns>
        /// <exception cref="InstanceNotFoundException&lt;Rating&gt;"/>
        /// <exception cref="DuplicateInstanceException&lt;Rating&gt;"/>
        Rating FindForUserAndLink(long linkId, long userId);
        
        /// <summary>
        /// Finds the comment made by the user with userId about a link with linkId
        /// </summary>
        /// <param name="linkId">linkId</param>
        /// <param name="userId">userId</param>
        /// <returns>The Rating</returns>
        bool ExistsForUserAndLink(long linkId, long userId);

        /// <summary>
        /// Calculates the rating value for a given link with linkId
        /// </summary>
        /// <param name="linkId">linkId</param>
        /// <returns>The rating value calculated for the given link</returns>
        int CalculateValueForLink(long linkId);

        /// <summary>
        /// Calculates the rating value for a given label
        /// </summary>
        /// <param name="label">label</param>
        /// <returns>The rating value calculated for the given label</returns>
        int CalculateValueForLabel(string label);

        /// <summary>
        /// Calculates the rating value for a given user with userId
        /// </summary>
        /// <param name="userId">userId</param>
        /// <returns>The rating value calculated for the given user</returns>
        int CalculateValueForUser(long userId);

    }
}
