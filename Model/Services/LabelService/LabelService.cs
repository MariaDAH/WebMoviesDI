using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Es.Udc.DotNet.WebMovies.Model.Daos.LabelDao;
using Es.Udc.DotNet.WebMovies.Model.Daos.LinkDao;
using Es.Udc.DotNet.WebMovies.Model.Daos.RatingDao;
using Es.Udc.DotNet.WebMovies.Model.Daos.UserProfileDao;
using Es.Udc.DotNet.WebMovies.Model.Util;
using Es.Udc.DotNet.WebMovies.Model.Util.Collections;
using Es.Udc.DotNet.WebMovies.Model.Util.Exceptions;
using Microsoft.Practices.Unity;
using Es.Udc.DotNet.WebMovies.Model.Services.LinkService;
using Es.Udc.DotNet.WebMovies.Model.Services.UserService;

namespace Es.Udc.DotNet.WebMovies.Model.Services.LabelService
{
    public class LabelService
        : ILabelService
    {

        [Dependency]
        public IUserProfileDao UserDao { private get; set; }

        [Dependency]
        public ILinkDao LinkDao { private get; set; }

        [Dependency]
        public ILabelDao LabelDao { private get; set; }

        [Dependency]
        public IRatingDao RatingDao { private get; set; }

        #region ILabelService Members

        public DictionaryBlock<string, long> GetMostValuedLabels(int startIndex, int count)
        {
            ListBlock<Label> labels;

            try
            {
                labels = LabelDao.ListAllRated(startIndex, count);
            }
            catch (InstanceNotFoundException<Label>)
            {
                return new DictionaryBlock<string, long>(startIndex, false);
            }
            catch (NoMoreItemsException<Label>)
            {
                return new DictionaryBlock<string, long>(startIndex, false);
            }

            DictionaryBlock<string, long> details = new DictionaryBlock<string, long>(labels.Index, labels.HasMore);
            foreach (Label label in labels)
            {
                int rating = RatingDao.CalculateValueForLabel(label.text);

                details.Add(label.text, rating);
            }

            return details;
        }
        
        public DictionaryBlock<string, long> GetLabelsForLink(long linkId, int startIndex, int count)
        {
            if (!LinkDao.Exists(linkId)) {
                throw new InstanceNotFoundException<LinkDetails>("linkId", linkId);
            }

            ListBlock<Label> labels;

            try
            {
                labels = LabelDao.ListForLinkRated(linkId, startIndex, count);
            }
            catch (InstanceNotFoundException<Label>)
            {
                return new DictionaryBlock<string, long>(startIndex, false);
            }
            catch (NoMoreItemsException<Label>)
            {
                return new DictionaryBlock<string, long>(startIndex, false);
            }

            DictionaryBlock<string, long> details = new DictionaryBlock<string, long>(labels.Index, labels.HasMore);
            foreach (Label label in labels)
            {
                int rating = RatingDao.CalculateValueForLabel(label.text);

                details.Add(label.text, rating);
            }

            return details;
        }
        
        public void SetLabelsForLink(long userId, long linkId, List<string> labelTexts)
        {
            if (!UserDao.Exists(userId))
            {
                throw new InstanceNotFoundException<UserProfileDetails>("userId", userId);
            }

            Link link;
            try
            {
                link = LinkDao.Find(linkId);
            }
            catch (InstanceNotFoundException<Link> ex)
            {
                throw new InstanceNotFoundException<LinkDetails>(ex.Properties);
            }

            if (link.userId != userId)
            {
                throw new UserNotAuthorizedException<LinkDetails>(userId, "linkId", linkId);
            }

            List<Label> labelsToSet = new List<Label>();
            foreach (string labelText in labelTexts.Distinct())
            {
                if (labelText != null)
                {
                    string trimmedLabelText = labelText.Trim();

                    if (trimmedLabelText != "")
                    {
                        Label label;
                        try
                        {
                            label = LabelDao.FindByText(trimmedLabelText);
                        }
                        catch (InstanceNotFoundException<Label>)
                        {
                            label = Label.CreateLabel(-1, trimmedLabelText);
                            LabelDao.Create(label);
                        }
                        catch (DuplicateInstanceException<Label> ex)
                        {
                            throw new InternalErrorException(ex);
                        }
                        if (!labelsToSet.Contains(label))
                        {
                            labelsToSet.Add(label);
                        }
                    }
                }
            }

            List<Label> labelsAlreadyForLink;
            try
            {
                labelsAlreadyForLink = LabelDao.FindForLink(linkId);
            }
            catch (InstanceNotFoundException<Label>)
            {
                labelsAlreadyForLink = new List<Label>();
            }
            foreach (Label labelAlreadyForLink in labelsAlreadyForLink)
            {
                if (!labelsToSet.Contains(labelAlreadyForLink))
                {
                    link.Labels.Remove(labelAlreadyForLink);

                    if (labelAlreadyForLink.Links.Count == 1)
                    {
                        labelAlreadyForLink.Links.Remove(link);
                        try
                        {
                            LabelDao.Update(labelAlreadyForLink);
                        }
                        catch (InstanceNotFoundException<Label> ex)
                        {
                            throw new InternalErrorException(ex);
                        }
                    }
                    else
                    {
                        try
                        {
                            LabelDao.Remove(labelAlreadyForLink.labelId);
                        }
                        catch (InstanceNotFoundException<Label> ex)
                        {
                            throw new InternalErrorException(ex);
                        }
                    }
                }
                else
                {
                    labelsToSet.Remove(labelAlreadyForLink);
                }
            }

            foreach (Label label in labelsToSet)
            {
                if (!link.Labels.Contains(label))
                {
                    link.Labels.Add(label);

                    label.Links.Add(link);
                    try
                    {
                        LabelDao.Update(label);
                    }
                    catch (InstanceNotFoundException<Label> ex)
                    {
                        throw new InternalErrorException(ex);
                    }
                }
            }

            try
            {
                LinkDao.Update(link);
            }
            catch (InstanceNotFoundException<Link> ex)
            {
                throw new InternalErrorException(ex);
            }
        }

        public void RemoveLabelsForLink(long userId, long linkId)
        {
            if (!UserDao.Exists(userId))
            {
                throw new InstanceNotFoundException<UserProfileDetails>("userId", userId);
            }

            Link link;
            try
            {
                link = LinkDao.Find(linkId);
            }
            catch (InstanceNotFoundException<Link> ex)
            {
                throw new InstanceNotFoundException<LinkDetails>(ex.Properties);
            }

            if (link.UserProfile.userId != userId)
            {
                throw new UserNotAuthorizedException<LinkDetails>(userId, linkId);
            }

            List<Label> removableLabels = new List<Label>();
            foreach (Label label in link.Labels)
            {
                if (label.Links.Contains(link))
                {
                    removableLabels.Add(label);
                }
            }

            foreach (Label label in removableLabels)
            {
                    label.Links.Remove(link);
                    link.Labels.Remove(label);

                    LinkDao.Update(link);

                    if (label.Links.Count > 0)
                    {
                        try
                        {
                            LabelDao.Update(label);
                        }
                        catch (InstanceNotFoundException<Label> ex)
                        {
                            throw new InternalErrorException(ex);
                        }
                    }
                    else
                    {
                        try
                        {
                            LabelDao.Remove(label.labelId);
                        }
                        catch (InstanceNotFoundException<Label> ex)
                        {
                            throw new InternalErrorException(ex);
                        }
                    }
                }
        }

        #endregion

    }
}
