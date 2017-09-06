<%@ Page Title="" Language="C#" MasterPageFile="~/WebMovies.Master" AutoEventWireup="true" CodeBehind="EditComment.aspx.cs" Inherits="Es.Udc.DotNet.WebMovies.Web.Pages.Comment.EditComment" %>

<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder_Title" runat="server">
  <asp:Image ID="imgEdit" meta:resourcekey="imgEdit" runat="server" /> <asp:Localize ID="lclEditComment" meta:resourcekey="lclEditComment" runat="server" /> <asp:Image ID="imgComment" meta:resourcekey="imgComment" runat="server" />
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="ContentPlaceHolder_Content" runat="server">
  <asp:Panel ID="pEditComment" CssClass="comment" runat="server">
    <form id="frmEditComment" method="post" runat="server">
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
        <asp:Button ID="btnEditComment" runat="server" OnClick="BtnEditCommentClick" meta:resourcekey="btnEditComment" />
      </div>
    </form>
  </asp:Panel>
</asp:Content>
