<%@ Page Title="" Language="C#" MasterPageFile="~/WebMovies.Master" AutoEventWireup="true" CodeBehind="ListFavorites.aspx.cs" Inherits="Es.Udc.DotNet.WebMovies.Web.Pages.Favorite.ListFavorites" %>

<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder_Title" runat="server">
  <asp:Image ID="imgFavoriteTitle" meta:resourcekey="imgFavorite" runat="server" />&nbsp;<asp:Label ID="lclFavoriteList" meta:resourcekey="lclFavoriteList" runat="server" />
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="ContentPlaceHolder_Content" runat="server">
  <form id="frmFavorite" runat="server">
    <asp:ListView ID="lvListFavorites" OnPreRender="LvListFavorites_PreRender" runat="server">
      <LayoutTemplate>
        <ul class="list">
          <asp:PlaceHolder ID="itemPlaceholder" runat="server" />
        </ul>
      </LayoutTemplate>
      <ItemTemplate>
        <li class="<%# Css((long)Eval("LinkId")) %>">
          <div class="link"><a href="/Pages/Movie/MovieXml.aspx?movieId=<%# MovieId((long)Eval("LinkId")) %>"><asp:Image ID="imgMovie" meta:resourcekey="imgMovie" runat="server"/> <%# MovieTitle((long)Eval("LinkId"))%></a> <a href="/Pages/Link/Link.aspx?linkId=<%# Eval("LinkId") %>"><asp:Image ID="imgLink" meta:resourcekey="imgLink" runat="server"/> <%# LinkName((long)Eval("LinkId")) %></a></div>
          <div class="name"><a href="/Pages/Favorite/Favorite.aspx?linkId=<%# Eval("LinkId") %>"><asp:Image ID="imgFavorite" meta:resourcekey="imgFavorite" runat="server"/> <%# Eval("Name") %></a></div>
          <div class="description"><asp:Image ID="imgDescription" meta:resourcekey="imgDescription" runat="server"/> <%# Eval("Description") %></div>
          <div class="control"><a href="/Pages/Favorite/EditFavorite.aspx?linkId=<%# Eval("LinkId") %>"><asp:Image ID="imgEditFavorite" meta:resourcekey="imgEditFavorite" runat="server"/></a> <a href="/Pages/Favorite/RemoveFavorite.aspx?linkId=<%# Eval("LinkId") %>"><asp:Image ID="imgRemoveFavorite" meta:resourcekey="imgRemoveFavorite" runat="server"/></a></div>
        </li>
      </ItemTemplate>
      <EmptyDataTemplate>
        <div class="error">
          <p><asp:Localize ID="lclNoFavorites" meta:resourcekey="lclNoFavorites" runat="server" /></p>
          <asp:HyperLink ID="lnkReturn" CssClass="linkButton" NavigateUrl="/Pages/MainPage.aspx" meta:resourcekey="lnkReturn" runat="server" />
        </div>
      </EmptyDataTemplate>
    </asp:ListView>
    <asp:HyperLink ID="lnkOrderBy" CssClass="orderBy" runat="server"><asp:Image ID="imgOrderBy" meta:resourcekey="imgOrderBy" runat="server" />&nbsp;<asp:Image ID="imgByWhat" runat="server" /></asp:HyperLink>
    <div class="links">
      <asp:DataPager ID="dpListFavorites" PagedControlID="lvListFavorites" runat="server">
        <Fields>
          <asp:NextPreviousPagerField ButtonType="Link" ButtonCssClass="previous" ShowFirstPageButton="false" ShowNextPageButton="false" ShowLastPageButton="false" meta:resourcekey="nppfListFavorites" runat="server" />
          <asp:NextPreviousPagerField ButtonType="Link" ButtonCssClass="next" ShowFirstPageButton="false" ShowPreviousPageButton="false" ShowLastPageButton="false" meta:resourcekey="nppfListFavorites" runat="server" />
        </Fields>
      </asp:DataPager>
    </div>
  </form>
</asp:Content>
