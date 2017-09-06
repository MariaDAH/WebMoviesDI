<%@ Page Title="" Language="C#" MasterPageFile="~/WebMovies.Master" AutoEventWireup="true" CodeBehind="AddLink.aspx.cs" Inherits="Es.Udc.DotNet.WebMovies.Web.Pages.Link.AddLink" %>

<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder_Title" runat="server">
  <asp:Image ID="imgAdd" meta:resourcekey="imgAdd" runat="server" /> <asp:Localize ID="lclAddLink" meta:resourcekey="lclAddLink" runat="server" /> <asp:Image ID="imgLink" meta:resourcekey="imgLink" runat="server" />
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="ContentPlaceHolder_Content" runat="server">
  <div class="link">
    <form id="frmAddLink" method="post" runat="server">
      <div class="field">
        <span class="label">
          <asp:Localize ID="lclUrl" runat="server" meta:resourcekey="lclUrl" />
        </span>
        <span class="entry">
          <asp:TextBox ID="txtUrl" runat="server" Columns="64" meta:resourcekey="txtUrl" />
          <asp:RequiredFieldValidator ID="rfvUrl" ControlToValidate="txtUrl" Display="Dynamic" Text="<%$ Resources: Common, mandatoryField %>" meta:resourcekey="rfvUrl" style="position: absolute; top: 0px; right: 0px;" runat="server" />
          <asp:RegularExpressionValidator ID="revUrl" ControlToValidate="txtUrl" Display="Dynamic" ValidationExpression=".*\w+([-.]\w+)*\.\w+([-.]\w+)*.*" meta:resourcekey="revUrl" style="position: absolute; top: 0px; right: 0px;" runat="server" />
          <span class="validatorError"><asp:Localize ID="atvUrl" meta:resourcekey="atvUrl" Visible="false" runat="server" /></span>
        </span>
      </div>
      <div class="field">
        <span class="label">
          <asp:Localize ID="lclName" runat="server" meta:resourcekey="lclName" />
        </span>
        <span class="entry">
          <asp:TextBox ID="txtName" runat="server" Columns="64" meta:resourcekey="txtName"></asp:TextBox>
          <asp:RequiredFieldValidator ID="rfvName" runat="server" ControlToValidate="txtName" Display="Dynamic" Text="<%$ Resources: Common, mandatoryField %>" meta:resourcekey="rfvName" style="position: absolute; top: 0px; right: 0px;" />
          <span class="validatorError"><asp:Localize ID="atvName" meta:resourcekey="atvName" Visible="false" runat="server" /></span>
        </span>
      </div>
      <div class="field" style="min-height: 64px;">
        <span class="label">
          <asp:Localize ID="lclDescription" runat="server" meta:resourcekey="lclDescription" />
        </span>
        <span class="entry">
          <asp:TextBox ID="txtDescription" Columns="47" Wrap="true" TextMode="MultiLine" runat="server"></asp:TextBox>
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
        <asp:Button ID="btnAddLink" OnClick="BtnAddLink_Click" meta:resourcekey="btnAddLink" runat="server" />
      </div>
    </form>
  </div>
</asp:Content>
