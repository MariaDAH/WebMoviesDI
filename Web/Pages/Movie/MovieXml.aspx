<%@ Page Title="" Language="C#" MasterPageFile="~/WebMovies.Master" AutoEventWireup="true" CodeBehind="MovieXml.aspx.cs" Inherits="Es.Udc.DotNet.WebMovies.Web.Pages.Movie.MovieXml" %>

<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder_Title" runat="server">
  <asp:Label ID="lblTitle" Text="&nbsp;" runat="server" />
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="ContentPlaceHolder_Content" runat="server">
  <asp:Xml ID="movieXml" runat="server" />
</asp:Content>
