<%@ Page Title="" Language="C#" MasterPageFile="~/WebMovies.Master" AutoEventWireup="true" CodeBehind="Comment.aspx.cs" Inherits="Es.Udc.DotNet.WebMovies.Web.Pages.Comment.Comment" %>

<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder_Title" runat="server">
  <asp:Image ID="imgComment" meta:resourcekey="imgComment" runat="server"/> <asp:Localize ID="lclComment" meta:resourcekey="lclComment" runat="server" /> <asp:Localize ID="lclFor" meta:resourcekey="lclFor" runat="server" /> <asp:Localize ID="lclLink" meta:resourcekey="lclLink" runat="server" /> <asp:HyperLink ID="lnkLink" runat="server"><asp:Label ID="lblLink" meta:resourcekey="lblLink" runat="server" />&nbsp;<asp:Image ID="imgLink" meta:resourcekey="imgLink" runat="server"/></asp:HyperLink>
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="ContentPlaceHolder_Content" runat="server">
  <div class="comment">
    <div class="text"><asp:Label ID="lblText" runat="server" /></div>
    <div class="author"><asp:HyperLink ID="lnkAuthor" runat="server"><asp:Image ID="imgAuthor" meta:resourcekey="imgAuthor" runat="server"/>&nbsp;<asp:Label ID="lblAuthor" runat="server" /></asp:HyperLink></div>
    <div class="date"><asp:Image ID="imgDate" meta:resourcekey="imgDate" runat="server"/>&nbsp;<asp:Label ID="lblDate" runat="server" /></div>
    <asp:Panel ID="pControl" CssClass="control" Visible="false" runat="server"><asp:HyperLink ID="lnkEditComment" meta:resourcekey="lnkEditComment" runat="server" /> <asp:HyperLink ID="lnkRemoveComment" meta:resourcekey="lnkRemoveComment" runat="server" /></asp:Panel>
  </div>
</asp:Content>
