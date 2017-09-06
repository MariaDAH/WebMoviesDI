<%@ Page Title="" Language="C#" MasterPageFile="~/WebMovies.Master" AutoEventWireup="true" CodeBehind="ListMovies.aspx.cs" Inherits="Es.Udc.DotNet.WebMovies.Web.Pages.Movie.ListMovies" %>

<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder_Title" runat="server">
   <asp:Image ID="imgSearch" meta:resourcekey="imgSearch" runat="server" /> <asp:Localize ID="lclSearch" meta:resourcekey="lclSearch" runat="server" /> <asp:Image ID="imgMovie" meta:resourcekey="imgMovie" runat="server" />
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="ContentPlaceHolder_Content" runat="server">
  <form id="frmListMovies" runat="server">
    <asp:ListView ID="lvListMovies" runat="server">
      <LayoutTemplate>
        <ul class="list">
          <asp:PlaceHolder ID="itemPlaceholder" runat="server" />
        </ul>
      </LayoutTemplate>
      <ItemTemplate>
        <li class="movie">
          <div class="title"><a href="Movie.aspx?movieId=<%# Eval("MovieId") %>"><%# MovieTitle((long)Eval("MovieId"), (string)Eval("Title"))%></a></div>
          <div class="price"><%# Currency((double)Eval("Price")) %></div>
          <div class="links">
            <asp:HyperLink ID="lnkViewLinks" NavigateUrl="<%# ViewLinksUrl((long)Eval(&quot;MovieId&quot;)) %>" meta:resourcekey="lnkViewLinks" runat="server"><asp:Image ID="imgLinks" meta:resourcekey="imgLinks" runat="server" /><sub><asp:Label Text="<%# Eval(&quot;LinkCount&quot;) %>" runat="server" /></sub></asp:HyperLink>
            <asp:HyperLink ID="lnkAddLink" NavigateUrl="<%# AddLinkUrl((long)Eval(&quot;MovieId&quot;)) %>" meta:resourcekey="lnkAddLink" runat="server" />
          </div>
        </li>
      </ItemTemplate>
      <EmptyDataTemplate>
      <div class="error">
        <asp:Localize ID="lclNoResults" meta:resourcekey="lclNoResults" runat="server" />
      </div>
      </EmptyDataTemplate>
    </asp:ListView>
  </form>

  <div class="links">
    <asp:HyperLink ID="lnkPrevious" CssClass="previous" meta:resourcekey="lnkPrevious" runat="server" />
    <asp:HyperLink ID="lnkNext" CssClass="next" meta:resourcekey="lnkNext" runat="server" />
  </div>
</asp:Content>
