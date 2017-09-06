using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Es.Udc.DotNet.WebMovies.Model.Util.Collections
{
    public class ListBlock<T>
        : List<T>
    {

        public int Index { get; private set; }

        public bool HasMore { get; private set; }

        public ListBlock(int index, bool hasMore)
            : base()
        {
            this.Index = index;
            this.HasMore = hasMore;
        }

        public ListBlock(int capacity, int index, bool hasMore)
            : base(capacity)
        {
            this.Index = index;
            this.HasMore = hasMore;
        }

        public ListBlock(IEnumerable<T> collection, int index, bool hasMore)
            : base(collection)
        {
            this.Index = index;
            this.HasMore = hasMore;
        }

    }
}
