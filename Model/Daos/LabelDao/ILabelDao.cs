using System.Collections.Generic;
using Es.Udc.DotNet.WebMovies.Model.Util.Collections;
using Es.Udc.DotNet.WebMovies.Model.Util.Dao;

namespace Es.Udc.DotNet.WebMovies.Model.Daos.LabelDao
{
    public interface ILabelDao
        : IGenericDao<Label, long>
    {

        /// <summary>
        /// Finds a Label by its name
        /// </summary>
        /// <param name="labelName">labelName</param>
        /// <returns>The Label</returns>
        /// <exception cref="InstanceNotFoundException&lt;Label&gt;"/>
        /// <exception cref="DuplicateInstanceException&lt;Label&gt;"/>
        Label FindByText(string text);
        
        /// <summary>
        /// Get the list of Labels for a given link
        /// </summary>
        /// <param name="linkId">ID of the link.</param>
        /// <returns>The List of Labels for that link</returns>
        /// <exception cref="InstanceNotFoundException&lt;Label&gt;"/>
        List<Label> FindForLink(long linkId);
        
        /// <summary>
        /// Get the list of Labels for a given link
        /// </summary>
        /// <param name="linkId">ID of the link.</param>
        /// <returns>The List of Labels for that link</returns>
        /// <exception cref="InstanceNotFoundException&lt;Label&gt;"/>
        List<Label> FindForLinkRated(long linkId);
        
        /// <summary>
        /// Get the list of Labels for a given link
        /// </summary>
        /// <param name="linkId">ID of the link.</param>
        /// <returns>The List of Labels for that link</returns>
        /// <exception cref="InstanceNotFoundException&lt;Label&gt;"/>
        /// <exception cref="NoMoreItemsException&lt;Label&gt;"/>
        ListBlock<Label> ListForLinkRated(long linkId, int startIndex, int count);
        
        /// <summary>
        /// Get the list of Labels in the system ordered by global comment
        /// </summary>
        /// <param name="startIndex">first value to return.</param>
        /// <param name="count">Number of results.</param>
        /// <returns>The List of Labels</returns>
        /// <exception cref="InstanceNotFoundException&lt;Label&gt;"/>
        /// <exception cref="NoMoreItemsException&lt;Label&gt;"/>
        ListBlock<Label> ListAllRated(int startIndex, int count);

    }
}
