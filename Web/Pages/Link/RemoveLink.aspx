<%@ Page Title="" Language="C#" MasterPageFile="~/WebMovies.Master" AutoEventWireup="true" CodeBehind="RemoveLink.aspx.cs" Inherits="Es.Udc.DotNet.WebMovies.Web.Pages.Link.RemoveLink" %>

<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder_Title" runat="server">
  <asp:Image ID="imgRemove" meta:resourcekey="imgRemove" runat="server" /> <asp:Localize ID="lclRemoveLink" meta:resourcekey="lclRemoveLink" runat="server" /> <asp:HyperLink ID="lnkLinkName" runat="server"> <asp:Label ID="lblLinkName" runat="server" />&nbsp;<asp:Image ID="imgLink" meta:resourcekey="imgLink" runat="server" /></asp:HyperLink>
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="ContentPlaceHolder_Content" runat="server">
  <asp:Panel ID="pRemoveLink" CssClass="link" runat="server">
    <form id="frmRemoveLink" method="post" runat="server">
      <p><asp:Localize ID="lclConfirm" meta:resourcekey="lclConfirm" runat="server" /></p>
      <asp:HyperLink ID="lnkReturn" CssClass="linkButton" meta:resourcekey="lnkReturn" runat="server" />
      <asp:LinkButton ID="lnkConfirm" CssClass="linkButton" OnClick="Remove" meta:resourcekey="lnkConfirm" runat="server" />
    </form>
  </asp:Panel>
  <asp:Panel ID="pError" CssClass="error" Visible="false" runat="server">
    <asp:Localize ID="lclError" meta:resourcekey="lclError" runat="server"/>
    <asp:HyperLink ID="lnkReturnError" meta:resourcekey="lnkReturn" runat="server" />
  </asp:Panel>
</asp:Content>
