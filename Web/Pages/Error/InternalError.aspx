<%@ Page Language="C#" MasterPageFile="~/WebMovies.Master" AutoEventWireup="true" Codebehind="InternalError.aspx.cs" Inherits="Es.Udc.DotNet.WebMovies.Web.Pages.Error.InternalError" meta:resourcekey="Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_Welcome" runat="server">
  &nbsp;
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder_Title" runat="server">
  <asp:Label ID="lblErrorTitle" meta:resourcekey="lblErrorTitle" runat="server" />
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="ContentPlaceHolder_Content" runat="server">
  <asp:Panel ID="pError" CssClass="error" runat="server">
    <p><asp:Localize ID="lclError" meta:resourcekey="lclError" runat="server" /></p>
    <asp:HyperLink ID="lnkReturn" CssClass="linkButton" meta:resourcekey="lnkReturn" runat="server" />
  </asp:Panel>
</asp:Content>
