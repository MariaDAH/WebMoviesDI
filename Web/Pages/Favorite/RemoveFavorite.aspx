<%@ Page Title="" Language="C#" MasterPageFile="~/WebMovies.Master" AutoEventWireup="true" CodeBehind="RemoveFavorite.aspx.cs" Inherits="Es.Udc.DotNet.WebMovies.Web.Pages.Favorite.RemoveFavorite" %>

<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder_Title" runat="server">
  <asp:Image ID="imgRemove" meta:resourcekey="imgRemove" runat="server"/> <asp:Localize ID="lclRemoveLink" meta:resourcekey="lclRemoveLink" runat="server" /> <asp:HyperLink ID="lnkLink" runat="server"><asp:Image ID="imgLink" meta:resourcekey="imgLink" runat="server"/>&nbsp;<asp:Label ID="lblLink" meta:resourcekey="lblLink" runat="server" /></asp:HyperLink> <asp:Localize ID="lclFromFavorites" meta:resourcekey="lclFromFavorites" runat="server" /> <asp:Image ID="imgFavoriteTitle" meta:resourcekey="imgFavorite" runat="server"/>
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="ContentPlaceHolder_Content" runat="server">
  <asp:Panel ID="pRemoveFavorite" CssClass="favorite" runat="server">
    <div class="name"><asp:Image ID="imgFavorite" meta:resourcekey="imgFavorite" runat="server"/>&nbsp;<asp:Label ID="lblName" runat="server" /></div>
    <div class="desciption"><asp:Image ID="imgDescription" meta:resourcekey="imgDescription" runat="server"/>&nbsp;<asp:Label ID="lblDescription" runat="server" /></div>
    <form id="frmRemoveFavorite" method="post" runat="server">
      <asp:HyperLink ID="lnkReturn" CssClass="linkButton" meta:resourcekey="lnkReturn" runat="server" />
      <asp:LinkButton ID="lnkConfirm" CssClass="linkButton" OnClick="Remove" meta:resourcekey="lnkConfirm" runat="server" />
    </form>
  </asp:Panel>
  <asp:Panel ID="pError" CssClass="error" Visible="false" runat="server">
    <p><asp:Localize ID="lclError" meta:resourcekey="lclError" runat="server"/></p>
    <asp:HyperLink ID="lnkReturnError" CssClass="linkButton" meta:resourcekey="lnkReturn" runat="server" />
  </asp:Panel>
</asp:Content>
