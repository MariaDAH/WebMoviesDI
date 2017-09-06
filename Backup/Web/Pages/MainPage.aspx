<%@ Page Language="C#" MasterPageFile="~/WebMovies.Master" AutoEventWireup="true" Codebehind="MainPage.aspx.cs" Inherits="Es.Udc.DotNet.WebMovies.Web.Pages.MainPage" meta:resourcekey="Page" %>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder_Menu" runat="server">
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="ContentPlaceHolder_Content" runat="server">
  <div class="content" id="form">
    <asp:Localize ID="lclSearchMovie" runat="server" meta:resourcekey="lclSearchMovie" />
    <form id="SearchMovieForm" method="POST" runat="server">
      <asp:TextBox ID="txtSearchKeywords" Columns="64" CssClass="searchText" runat="server" />
      <asp:DropDownList ID="ddlSearchEngine" CssClass="searchSelect" meta:resourcekey="ddlSearchEngine" runat="server">
        <asp:ListItem Value="webshop" Text="<%$ Resources: Common, webshop %>" />
        <asp:ListItem Value="webshopxml" Text="<%$ Resources: Common, webshopxml %>" />
      </asp:DropDownList>
      <asp:ImageButton ID="btnSearchButon" CssClass="searchButton" OnClick="BtnSearchButtonClick" meta:resourcekey="btnSearchButon" runat="server" />
    </form>
  </div>
</asp:Content>
