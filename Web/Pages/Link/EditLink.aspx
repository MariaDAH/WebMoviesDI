<%@ Page Title="" Language="C#" MasterPageFile="~/WebMovies.Master" AutoEventWireup="true" CodeBehind="EditLink.aspx.cs" Inherits="Es.Udc.DotNet.WebMovies.Web.Pages.Link.EditLink" %>

<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder_Title" runat="server">
  <asp:Image ID="imgEdit" meta:resourcekey="imgEdit" runat="server" /> <asp:Localize ID="lclEditLink" meta:resourcekey="lclEditLink" runat="server" /> <asp:Image ID="imgLink" meta:resourcekey="imgLink" runat="server" />
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="ContentPlaceHolder_Content" runat="server">
  <asp:Panel ID="pEditLink" CssClass="link" runat="server">
    <form id="frmEditLink" method="post" runat="server">
      <div class="field">
        <span class="label">
          <asp:Localize ID="lclUrl" runat="server" meta:resourcekey="lclUrl" />
        </span>
        <span class="entry">
          <asp:Label ID="lblUrl" meta:resourcekey="lblUrl" runat="server" />
        </span>
      </div>
      <div class="field">
        <span class="label">
          <asp:Localize ID="lclName" runat="server" meta:resourcekey="lclName" />
        </span>
        <span class="entry">
          <asp:TextBox ID="txtName" Columns="64" runat="server" />
          <asp:RequiredFieldValidator ID="rfvName" runat="server" ControlToValidate="txtName" Display="Dynamic" Text="<%$ Resources: Common, mandatoryField %>" meta:resourcekey="rfvName" style="position: absolute; top: 0px; right: 0px;" />
          <span class="validatorError"><asp:Localize ID="atvName" meta:resourcekey="atvName" Visible="false" runat="server" /></span>
        </span>
      </div>
      <div class="field" style="min-height: 64px;">
        <span class="label">
          <asp:Localize ID="lclDescription" runat="server" meta:resourcekey="lclDescription" />
        </span>
        <span class="entry">
          <asp:TextBox ID="txtDescription" Columns="47" Wrap="true" TextMode="MultiLine" runat="server" meta:resourcekey="txtLoginResource1"></asp:TextBox>
        </span>
      </div>
      <div class="field">
        <span class="label">
          <asp:Localize ID="lclLabels" runat="server" meta:resourcekey="lclLabels" />
        </span>
        <span class="entry">
          <asp:TextBox ID="txtLabels" runat="server" Columns="64" meta:resourcekey="txtLabels" />
        </span>
      </div>
      <div class="button">
        <asp:Button ID="btnEditLink" OnClick="BtnEditLink_Click" meta:resourcekey="btnEditLink" runat="server" />
      </div>
    </form>
  </asp:Panel>
</asp:Content>
