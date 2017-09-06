using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using Es.Udc.DotNet.WebMovies.Model.Util.Collections;
using Es.Udc.DotNet.WebMovies.Model.Util.Exceptions;
using Es.Udc.DotNet.WebMovies.Model.Util.Dao;

namespace Es.Udc.DotNet.WebMovies.Model.Daos.LabelDao
{
    class LabelDaoEntityFramework
        : GenericDaoEntityFramework<Label, long>, ILabelDao
    {

        public LabelDaoEntityFramework() { }

        #region ILabelDao Members;

        public Label FindByText(string text)
        {
            ObjectSet<Label> labels = Context.CreateObjectSet<Label>();

            var result = from l in labels
                         where l.text == text
                         select l;

            if (result.Count() == 0)
            {
                throw new InstanceNotFoundException<Label>("text", text);
            }
            if (result.Count() > 1)
            {
                throw new DuplicateInstanceException<Label>("text", text);
            }

            return result.First();
        }

        public List<Label> FindForLink(long linkId)
        {
            ObjectSet<Label> labels = Context.CreateObjectSet<Label>();

            var result = from l in labels
                         from k in l.Links
                         where k.linkId == linkId
                         select l;

            if (result.Count() == 0)
            {
                throw new InstanceNotFoundException<Label>("linkId", linkId);
            }

            return result.ToList();
        }

        public List<Label> FindForLinkRated(long linkId)
        {
            ObjectSet<Label> labels = Context.CreateObjectSet<Label>();

            var result = from l in labels
                         from k in l.Links
                         where k.linkId == linkId
                         orderby l.Links.Sum(lk => (int?)(lk.Ratings.Sum(r => (int?)r.value) ?? 0)) ?? 0 descending
                         select l;

            if (result.Count() == 0)
            {
                throw new InstanceNotFoundException<Label>("linkId", linkId);
            }

            return result.ToList();
        }

        public ListBlock<Label> ListForLinkRated(long linkId, int startIndex, int count)
        {
            ObjectSet<Label> labels = Context.CreateObjectSet<Label>();

            var result = (from l in labels
                          from k in l.Links
                          where k.linkId == linkId
                          orderby l.Links.Sum(lk => (int?)(lk.Ratings.Sum(r => (int?)r.value) ?? 0)) ?? 0 descending
                          select l).Skip(startIndex).Take(count + 1).ToList();

            if (result.Count == 0)
            {
                if (startIndex == 0)
                {
                    throw new InstanceNotFoundException<Label>("linkId", linkId);
                }
                else
                {
                    throw new NoMoreItemsException<Label>("linkId", linkId);
                }
            }

            bool hasMore = (result.Count == count + 1);
            if (hasMore)
            {
                result.RemoveAt(count);
            }

            return new ListBlock<Label>(result, startIndex, hasMore);
        }

        public ListBlock<Label> ListAllRated(int startIndex, int count)
        {
            ObjectSet<Label> labels = Context.CreateObjectSet<Label>();

            var result = (from l in labels
                          orderby l.Links.Sum(lk => (int?)(lk.Ratings.Sum(r => (int?)r.value) ?? 0)) ?? 0 descending
                          select l).Skip(startIndex).Take(count + 1).ToList();

            if (result.Count == 0)
            {
                if (startIndex == 0)
                {
                    throw new InstanceNotFoundException<Label>("*", null);
                }
                else
                {
                    throw new NoMoreItemsException<Label>("*", null);
                }
            }

            bool hasMore = (result.Count == count + 1);
            if (hasMore)
            {
                result.RemoveAt(count);
            }

            return new ListBlock<Label>(result, startIndex, hasMore);
        }

        #endregion

    }
}
