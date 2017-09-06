<%@ Page Title="" Language="C#" MasterPageFile="~/WebMovies.Master" AutoEventWireup="true" CodeBehind="AddComment.aspx.cs" Inherits="Es.Udc.DotNet.WebMovies.Web.Pages.Comment.AddComment" %>

<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder_Title" runat="server">
  <asp:Image ID="imgAdd" meta:resourcekey="imgAdd" runat="server" /> <asp:Localize ID="lclAddComment" meta:resourcekey="lclAddComment" runat="server" /> <asp:Image ID="imgComment" meta:resourcekey="imgComment" runat="server" />
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="ContentPlaceHolder_Content" runat="server">
  <asp:Panel ID="pAddComment" CssClass="comment" runat="server">
    <form id="frmAddComment" method="post" runat="server">
      <div class="movie"><asp:HyperLink ID="lnkMovie" runat="server"><asp:Image ID="imgMovie" meta:resourcekey="imgMovie" runat="server" />&nbsp;<asp:Label ID="lblMovie" runat="server" /></asp:HyperLink></div>
      <div class="link"><asp:HyperLink ID="lnkLink" runat="server"><asp:Image ID="imgLink" meta:resourcekey="imgLink" runat="server" />&nbsp;<asp:Label ID="lblLink" runat="server" /></asp:HyperLink></div>
      <div class="field" style="min-height: 64px;">
        <span class="label">
          <asp:Localize ID="lclText" runat="server" meta:resourcekey="lclText" />
        </span>
        <span class="entry">
          <asp:TextBox ID="txtText" runat="server" Columns="47" Rows="3" TextMode="MultiLine" Wrap="true" />
          <asp:RequiredFieldValidator ID="rfvText" ControlToValidate="txtText" Display="Dynamic" meta:resourcekey="rfvText" style="position: absolute; top: 0px; right: 0px;" runat="server" />
        </span>
      </div>
      <div class="button">
        <asp:Button ID="btnAddComment" runat="server" OnClick="BtnAddCommentClick" meta:resourcekey="btnAddComment" />
      </div>
    </form>
  </asp:Panel>
</asp:Content>
