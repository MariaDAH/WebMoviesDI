using System.Collections.Generic;
using Es.Udc.DotNet.ModelUtil.Transactions;
using Es.Udc.DotNet.WebMovies.Model.Daos.LabelDao;
using Es.Udc.DotNet.WebMovies.Model.Daos.LinkDao;
using Es.Udc.DotNet.WebMovies.Model.Daos.RatingDao;
using Es.Udc.DotNet.WebMovies.Model.Daos.UserProfileDao;
using Es.Udc.DotNet.WebMovies.Model.Util.Collections;

namespace Es.Udc.DotNet.WebMovies.Model.Services.LabelService
{
    public interface ILabelService
    {

        IUserProfileDao UserDao { set; }

        ILinkDao LinkDao { set; }

        ILabelDao LabelDao { set; }

        IRatingDao RatingDao { set; }

        /// <summary>
        /// Lists the most valued details in the system
        /// </summary>
        /// <param name="startIndex">Index of the first value to return.</param>
        /// <param name="count">Number of values to return.</param>
        /// <returns>Returns a list of the most valued details in the system limited to the parameters given.</returns>
        [Transactional]
        DictionaryBlock<string, long> GetMostValuedLabels(int startIndex, int count);
        
        /// <summary>
        /// Gets the labels for a link
        /// </summary>
        /// <param name="linkId">Id of the link.</param>
        /// <param name="startIndex">Start Index</param>
        /// <param name="count">Count</param>
        /// <returns>A Dictionary with the details for a given link and their associated values</returns>
        /// <exception cref="InstanceNotFoundException&lt;LinkDetails&gt;" />
        [Transactional]
        DictionaryBlock<string, long> GetLabelsForLink(long linkId, int startIndex, int count);

        /// <summary>
        /// Sets the labels for a link
        /// </summary>
        /// <param name="userId">Id of the user.</param>
        /// <param name="linkId">Id of the link.</param>
        /// <exception cref="InstanceNotFoundException&lt;UserProfileDetails&gt;" />
        /// <exception cref="InstanceNotFoundException&lt;LinkDetails&gt;" />
        /// <exception cref="UserNotAuthorizedException&lt;LinkDetails&gt;" />
        [Transactional]
        void SetLabelsForLink(long userId, long linkId, List<string> labels);

        /// <summary>
        /// Removes the labels for a link
        /// </summary>
        /// <param name="userId">Id of the user.</param>
        /// <param name="linkId">Id of the link.</param>
        /// <exception cref="InstanceNotFoundException&lt;UserProfileDetails&gt;" />
        /// <exception cref="InstanceNotFoundException&lt;LinkDetails&gt;" />
        /// <exception cref="UserNotAuthorizedException&lt;LinkDetails&gt;" />
        [Transactional]
        void RemoveLabelsForLink(long userId, long linkId);

    }
}
