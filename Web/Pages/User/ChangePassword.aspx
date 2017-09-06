<%@ Page Language="C#" MasterPageFile="~/WebMovies.Master" AutoEventWireup="true" Codebehind="ChangePassword.aspx.cs" Inherits="Es.Udc.DotNet.WebMovies.Web.Pages.User.ChangePassword" meta:resourcekey="Page" %>

<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder_Title" runat="server">
  <asp:Localize ID="lclMenuExplanation" runat="server" meta:resourcekey="lclMenuExplanation" />
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="ContentPlaceHolder_Content" runat="server">
  <div class="content" id="form">
    <form id="ChangePasswordForm" method="post" runat="server">
      <div class="field">
        <span class="label">
          <asp:Localize ID="lclOldPassword" runat="server" meta:resourcekey="lclOldPassword" />
        </span>
        <span class="entry">
          <asp:TextBox ID="txtOldPassword" TextMode="Password" runat="server" Width="100" Columns="16"></asp:TextBox>
          <asp:RequiredFieldValidator ID="rfvOldPassword" runat="server" ControlToValidate="txtOldPassword" Display="Dynamic" Text="<%$ Resources: Common, mandatoryField %>"/>
          <asp:Label ID="lblOldPasswordError" runat="server" ForeColor="Red" Visible="False" meta:resourcekey="lblOldPasswordError"></asp:Label>
        </span>
      </div>
      <div class="field">
        <span class="label">
          <asp:Localize ID="lclNewPassword" runat="server" meta:resourcekey="lclNewPassword" />
        </span>
        <span class="entry">
          <asp:TextBox TextMode="Password" ID="txtNewPassword" runat="server" Width="100" Columns="16"></asp:TextBox>
          <asp:RequiredFieldValidator ID="rfvNewPassword" runat="server" ControlToValidate="txtNewPassword" Display="Dynamic" Text="<%$ Resources: Common, mandatoryField %>"/>
          <asp:CompareValidator ID="cvCreateNewPassword" runat="server" ControlToCompare="txtOldPassword" ControlToValidate="txtNewPassword" Display="Dynamic" Operator="NotEqual" meta:resourcekey="cvCreateNewPassword"></asp:CompareValidator>
        </span>
      </div>
      <div class="field">
        <span class="label">
          <asp:Localize ID="lclRetypePassword" runat="server" meta:resourcekey="lclRetypePassword" />
        </span>
        <span class="entry">
          <asp:TextBox TextMode="Password" ID="txtRetypePassword" runat="server" Width="100" Columns="16"></asp:TextBox>
          <asp:RequiredFieldValidator ID="rfvRetypePassword" runat="server" ControlToValidate="txtRetypePassword" Display="Dynamic" Text="<%$ Resources: Common, mandatoryField %>"/>
          <asp:CompareValidator ID="cvPasswordCheck" runat="server" ControlToCompare="txtNewPassword" ControlToValidate="txtRetypePassword" Display="Dynamic" meta:resourcekey="cvPasswordCheck"></asp:CompareValidator>
        </span>
      </div>
      <div class="button">
        <asp:Button ID="btnChangePassword" runat="server" OnClick="BtnChangePasswordClick" meta:resourcekey="btnChangePassword" />
      </div>
    </form>
  </div>
  <asp:HyperLink ID="lnkUpdateProfile" runat="server" NavigateUrl="~/Pages/User/UpdateUserProfile.aspx" meta:resourcekey="lnkUpdateProfile" />
</asp:Content>
