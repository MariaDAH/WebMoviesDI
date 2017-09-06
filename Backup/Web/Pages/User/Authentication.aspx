<%@ Page Language="C#" MasterPageFile="~/WebMovies.Master" AutoEventWireup="true" Codebehind="Authentication.aspx.cs" Inherits="Es.Udc.DotNet.WebMovies.Web.Pages.User.Authentication" meta:resourcekey="Page" %>

<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder_Title" runat="server">
  <asp:Localize ID="lclMenuExplanation" runat="server" meta:resourcekey="lclMenuExplanation" />
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="ContentPlaceHolder_Content" runat="server">
  <div class="content" id="form">
    <form id="AuthenticationForm" method="POST" runat="server">
      <div class="field">
        <span class="label">
          <asp:Localize ID="lclLogin" runat="server" meta:resourcekey="lclLogin" />
        </span>
        <span class="entry">
          <asp:TextBox ID="txtLogin" runat="server" Columns="16"></asp:TextBox>
          <asp:RequiredFieldValidator ID="rfvLogin" runat="server" ControlToValidate="txtLogin" Display="Dynamic" Text="<%$ Resources: Common, mandatoryField %>"/>
          <asp:Label ID="lblLoginError" runat="server" ForeColor="Red" Style="position: relative" Visible="False" meta:resourcekey="lblLoginError"></asp:Label>
        </span>
      </div>
      <div class="field">
        <span class="label">
          <asp:Localize ID="lclPassword" runat="server" meta:resourcekey="lclPassword" />
        </span>
        <span class="entry">
          <asp:TextBox TextMode="Password" ID="txtPassword" runat="server" Columns="16"></asp:TextBox>
          <asp:RequiredFieldValidator ID="rfvPassword" runat="server" ControlToValidate="txtPassword" Display="Dynamic" Text="<%$ Resources: Common, mandatoryField %>"/>
          <asp:Label ID="lblPasswordError" runat="server" ForeColor="Red" Style="position: relative" Visible="False" meta:resourcekey="lblPasswordError"></asp:Label>
        </span>
      </div>
      <div class="checkbox">
        <asp:CheckBox ID="checkRememberPassword" runat="server" TextAlign="Left" meta:resourcekey="checkRememberPassword" /><span class="checkboxSpan" />
      </div>
      <div class="button">
        <asp:Button ID="btnLogin" runat="server" OnClick="BtnLoginClick" meta:resourcekey="btnLogin" />
      </div>
    </form>
  </div>
  <div class="footNote">
    <asp:HyperLink ID="lnkRegister" runat="server" NavigateUrl="~/Pages/User/Register.aspx" meta:resourcekey="lnkRegister" />
  </div>
</asp:Content>
