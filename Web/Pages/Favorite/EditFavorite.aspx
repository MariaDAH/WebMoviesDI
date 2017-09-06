<%@ Page Title="" Language="C#" MasterPageFile="~/WebMovies.Master" AutoEventWireup="true" CodeBehind="EditFavorite.aspx.cs" Inherits="Es.Udc.DotNet.WebMovies.Web.Pages.Favorite.EditFavorite" %>

<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder_Title" runat="server">
  <asp:Image ID="imgEdit" meta:resourcekey="imgEdit" runat="server" /> <asp:Localize ID="lclEdit" meta:resourcekey="lclEdit" runat="server" /> <asp:HyperLink ID="lnkEditWhat" runat="server"><asp:Image ID="imgEditWhat" runat="server" />&nbsp;<asp:Label ID="lblEditWhat" runat="server" /></asp:HyperLink> <asp:Localize ID="lclInFavorites" meta:resourcekey="lclInFavorites" runat="server" /> <asp:Image ID="imgFavorite" meta:resourcekey="imgFavorite" runat="server" />
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="ContentPlaceHolder_Content" runat="server">
  <asp:Panel ID="pFavorite" CssClass="favorite" runat="server">
    <form id="frmEditFavorite" method="post" runat="server">
      <div class="field">
        <span class="label">
          <asp:Localize ID="lclName" meta:resourcekey="lclName" runat="server" />
        </span>
        <span class="entry">
          <asp:TextBox ID="txtName" Columns="64" runat="server" />
          <asp:RequiredFieldValidator ID="rfvName" ControlToValidate="txtName" Display="Dynamic" meta:resourcekey="rfvName" style="position: absolute; top: 0px; right: 0px;" runat="server" />
        </span>
      </div>
      <div class="field" style="min-height: 64px;">
        <span class="label">
          <asp:Localize ID="lclDescription" meta:resourcekey="lclDescription" runat="server" />
        </span>
        <span class="entry">
          <asp:TextBox ID="txtDescription" runat="server" Columns="47" Rows="3" TextMode="MultiLine" Wrap="true" />
          <asp:RequiredFieldValidator ID="rfvDesciption" ControlToValidate="txtDescription" Display="Dynamic" meta:resourcekey="rfvDesciption" style="position: absolute; top: 0px; right: 0px;" runat="server" />
        </span>
      </div>
      <asp:Button ID="btnEditFavorite" OnClick="btnEditFavorite_Click" meta:resourcekey="btnEditFavorite" runat="server" />
    </form>
  </asp:Panel>
  <asp:Panel ID="pError" CssClass="error" Visible="false" runat="server">
    <asp:Localize ID="lclError" meta:resourcekey="lclError" runat="server"/>
  </asp:Panel>
</asp:Content>
