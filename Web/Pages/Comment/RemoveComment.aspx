<%@ Page Title="" Language="C#" MasterPageFile="~/WebMovies.Master" AutoEventWireup="true" CodeBehind="RemoveComment.aspx.cs" Inherits="Es.Udc.DotNet.WebMovies.Web.Pages.Comment.RemoveComment" %>

<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder_Title" runat="server">
  <asp:Image ID="imgRemove" meta:resourcekey="imgRemove" runat="server"/> <asp:Localize ID="lclRemoveComment" meta:resourcekey="lclRemoveComment" runat="server" /> <asp:Localize ID="lclFor" meta:resourcekey="lclFor" runat="server" /> <asp:Localize ID="lclLink" meta:resourcekey="lclLink" runat="server" /> <asp:HyperLink ID="lnkLink" runat="server"><asp:Label ID="lblLink" meta:resourcekey="lblLink" runat="server" />&nbsp;<asp:Image ID="imgLink" meta:resourcekey="imgLink" runat="server"/></asp:HyperLink>
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="ContentPlaceHolder_Content" runat="server">
  <div class="comment">
    <div class="text"><asp:Label ID="lblText" runat="server" /></div>
    <div class="author"><asp:HyperLink ID="lnkAuthor" runat="server"><asp:Image ID="imgAuthor" meta:resourcekey="imgAuthor" runat="server"/>&nbsp;<asp:Label ID="lblAuthor" runat="server" /></asp:HyperLink></div>
    <div class="date"><asp:Image ID="imgDate" meta:resourcekey="imgDate" runat="server"/>&nbsp;<asp:Label ID="lblDate" runat="server" /></div>
    <form id="frmRemoveComment" method="post" runat="server">
      <asp:HyperLink ID="lnkReturn" CssClass="linkButton" meta:resourcekey="lnkReturn" runat="server" />
      <asp:LinkButton ID="lnkConfirm" CssClass="linkButton" OnClick="Remove" meta:resourcekey="lnkConfirm" runat="server" />
    </form>
  </div>
  <asp:Panel ID="pError" CssClass="error" Visible="false" runat="server">
    <asp:Localize ID="lclError" meta:resourcekey="lclError" runat="server"/>
    <asp:HyperLink ID="lnkReturnError" meta:resourcekey="lnkReturn" runat="server" />
  </asp:Panel>
</asp:Content>
