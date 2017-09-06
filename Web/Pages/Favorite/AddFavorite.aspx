<%@ Page Title="" Language="C#" MasterPageFile="~/WebMovies.Master" AutoEventWireup="true" CodeBehind="AddFavorite.aspx.cs" Inherits="Es.Udc.DotNet.WebMovies.Web.Pages.Favorite.AddFavorite" %>

<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder_Title" runat="server">
  <asp:Image ID="imgAdd" meta:resourcekey="imgAdd" runat="server" /> <asp:Localize ID="lclAdd" meta:resourcekey="lclAdd" runat="server" /> <asp:HyperLink ID="lnkAddWhat" runat="server"><asp:Image ID="imgAddWhat" runat="server" />&nbsp;<asp:Label ID="lblAddWhat" runat="server" /></asp:HyperLink> <asp:Localize ID="lclToFavorites" meta:resourcekey="lclToFavorites" runat="server" /> <asp:Image ID="imgFavorite" meta:resourcekey="imgFavorite" runat="server" />
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="ContentPlaceHolder_Content" runat="server">
  <div class= "favorite">
    <form id="frmAddFavorite" method="post" runat="server">
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
      <asp:Button ID="btnAddFavorite" OnClick="btnAddFavorite_Click" meta:resourcekey="btnAddFavorite" runat="server" />
    </form>
  </div>
  <asp:Panel ID="pError" CssClass="error" Visible="false" runat="server">
    <asp:Localize ID="lclError" meta:resourcekey="lclError" runat="server"/>
  </asp:Panel>
</asp:Content>
