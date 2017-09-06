using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Es.Udc.DotNet.WebMovies.Model.Util.Collections
{
    public class DictionaryBlock<TKey, TValue>
        : Dictionary<TKey, TValue>
    {

        public int Index { get; private set; }

        public bool HasMore { get; private set; }

        public DictionaryBlock(int index, bool hasMore)
            : base()
        {
            this.Index = index;
            this.HasMore = hasMore;
        }

        public DictionaryBlock(IDictionary<TKey, TValue> dictionary, int index, bool hasMore)
            : base(dictionary)
        {
            this.Index = index;
            this.HasMore = hasMore;
        }

        public DictionaryBlock(IEqualityComparer<TKey> comparer, int index, bool hasMore)
            : base(comparer)
        {
            this.Index = index;
            this.HasMore = hasMore;
        }

        public DictionaryBlock(int capacity, int index, bool hasMore)
            : base(capacity)
        {
            this.Index = index;
            this.HasMore = hasMore;
        }

        public DictionaryBlock(IDictionary<TKey, TValue> dictionary, IEqualityComparer<TKey> comparer, int index, bool hasMore)
            : base(dictionary, comparer)
        {
            this.Index = index;
            this.HasMore = hasMore;
        }

        public DictionaryBlock(int capacity, IEqualityComparer<TKey> comparer, int index, bool hasMore)
            : base(capacity, comparer)
        {
            this.Index = index;
            this.HasMore = hasMore;
        }

    }
}
