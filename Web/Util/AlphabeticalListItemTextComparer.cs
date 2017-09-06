using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace Es.Udc.DotNet.WebMovies.Web.Util
{
    public class AlphabeticalListItemTextComparer
        : IComparer<ListItem>
    {

        public int Compare(ListItem x, ListItem y)
        {
            return x.Text.CompareTo(y.Text);
        }

    }
}