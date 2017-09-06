<%@ Page Title="" Language="C#" MasterPageFile="~/WebMovies.Master" AutoEventWireup="true" CodeBehind="ListMoviesXml.aspx.cs" Inherits="Es.Udc.DotNet.WebMovies.Web.Pages.Movie.ListMoviesXml" %>

<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder_Title" runat="server">
  <asp:Image ID="imgSearch" meta:resourcekey="imgSearch" runat="server" /> <asp:Localize ID="lclSearch" meta:resourcekey="lclSearch" runat="server" /> <asp:Image ID="imgMovie" meta:resourcekey="imgMovie" runat="server"/>
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="ContentPlaceHolder_Content" runat="server">
  <asp:Xml ID="movieListXml" runat="server" />

  <div class="links">
    <asp:HyperLink ID="lnkPrevious" CssClass="previous" meta:resourcekey="lnkPrevious" runat="server" />
    <asp:HyperLink ID="lnkNext" CssClass="next" meta:resourcekey="lnkNext" runat="server" />
  </div>
</asp:Content>
