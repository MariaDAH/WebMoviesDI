using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Es.Udc.DotNet.WebMovies.Model.Util.Collections
{
    public class SortedListBlock<T, T2>
        : SortedList<T, T2>
    {

        public int Index { get; private set; }

        public bool HasMore { get; private set; }
        
        public SortedListBlock(int index, bool hasMore)
            : base()
        {
            this.Index = index;
            this.HasMore = hasMore;
        }
        
        public SortedListBlock(int capacity, int index, bool hasMore)
            : base(capacity)
        {
            this.Index = index;
            this.HasMore = hasMore;
        }
        
        public SortedListBlock(IComparer<T> comparer, int index, bool hasMore)
            : base(comparer)
        {
            this.Index = index;
            this.HasMore = hasMore;
        }
        
        public SortedListBlock(IDictionary<T, T2> dictionary, int index, bool hasMore)
            : base(dictionary)
        {
            this.Index = index;
            this.HasMore = hasMore;
        }
        
        public SortedListBlock(int capacity, IComparer<T> comparer, int index, bool hasMore)
            : base(capacity, comparer)
        {
            this.Index = index;
            this.HasMore = hasMore;
        }
        
        public SortedListBlock(IDictionary<T, T2> dictionary, IComparer<T> comparer, int index, bool hasMore)
            : base(dictionary, comparer)
        {
            this.Index = index;
            this.HasMore = hasMore;
        }

    }
}
