<%@ Page Title="" Language="C#" MasterPageFile="~/WebMovies.Master" AutoEventWireup="true" CodeBehind="Movie.aspx.cs" Inherits="Es.Udc.DotNet.WebMovies.Web.Pages.Movie.Movie" %>

<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder_Title" runat="server">
  <asp:Image ID="imgMovie" meta:resourcekey="imgMovie" runat="server"/> <asp:Label ID="lblTitle" runat="server" />
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="ContentPlaceHolder_Content" runat="server">
  <asp:Panel ID="pMovie" CssClass="movie" runat="server">
    <div class="description"><asp:Label ID="lblDescription" runat="server" /></div>
    <div class="price">
      <asp:HyperLink ID="lnkPurchase" runat="server">
        <asp:Image ID="imgPurchase" meta:resourcekey="imgPurchase" runat="server" />&nbsp;<asp:Label ID="lblPrice" runat="server" />
      </asp:HyperLink>
    </div>
    <div class="links">
      <asp:HyperLink ID="lnkViewLinks" runat="server">
        <asp:Image ID="imgViewLinks" meta:resourcekey="imgViewLinks" runat="server" />
        <sub><asp:Label ID="lblLinkCount" runat="server" /></sub>
      </asp:HyperLink>
      &nbsp;
      <asp:HyperLink ID="lnkAddLink" meta:resourcekey="lnkAddLink" runat="server" />
    </div>
  </asp:Panel>
  <asp:Panel ID="pError" CssClass="error" Visible="false" runat="server">
    <p><asp:Localize ID="lclError" meta:resourcekey="lclError" runat="server" /></p>
    <asp:HyperLink ID="lnkReturn" NavigateUrl="/Pages/MainPage.aspx" meta:resourcekey="lnkReturn" runat="server" />
  </asp:Panel>
  <span style="visibility: hidden;">·</span>
</asp:Content>
