<%@ Page Title="" Language="C#" MasterPageFile="~/WebMovies.Master" AutoEventWireup="true" CodeBehind="Link.aspx.cs" Inherits="Es.Udc.DotNet.WebMovies.Web.Pages.Link.Link" %>

<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder_Title" runat="server">
  <asp:Image ID="imgLink" meta:resourcekey="imgLink" runat="server"/> <asp:Label ID="lblName" runat="server" />
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="ContentPlaceHolder_Content" runat="server">
  <asp:Panel ID="pLink" CssClass="link" runat="server">
    <div class="url"><asp:HyperLink ID="lnkUrl" runat="server"><asp:Label ID="lblUrl" runat="server" />&nbsp;<asp:Image ID="imgUrl" meta:resourcekey="imgUrl" runat="server"/></asp:HyperLink></div>
    <div class="reference"><asp:HyperLink ID="lnkAuthor" runat="server"><asp:Image ID="imgAuthor" meta:resourcekey="imgAuthor" runat="server"/>&nbsp;<asp:Label ID="lblAuthor" runat="server" /></asp:HyperLink> <asp:Image ID="imgDate" meta:resourcekey="imgDate" runat="server"/>&nbsp;<asp:Label ID="lblDate" runat="server" /></div>
    <div class="movie"><asp:HyperLink ID="lnkMovie" runat="server"><asp:Image ID="imgMovie" meta:resourcekey="imgMovie" runat="server"/>&nbsp;<asp:Label ID="lblMovie" runat="server" /></asp:HyperLink></div>
    <div class="description"><asp:Image ID="imgDescription" meta:resourcekey="imgDescription" runat="server"/>&nbsp;<asp:Label ID="lblDescription" runat="server" /></div>
    <div class="labels">
      <asp:Image ID="imgLabels" meta:resourcekey="imgLabels" runat="server"/>&nbsp;
      <asp:ListView ID="lvLabels" runat="server">
        <LayoutTemplate>
          <ul>
            <asp:PlaceHolder ID="itemPlaceholder" runat="server" />
          </ul>
        </LayoutTemplate>
        <ItemTemplate>
          <li><a href="/Pages/Link/ListLinks.aspx?label=<%# Eval("Text") %>" style="font-size: <%# Eval("Value") %>px"><%# Eval("Text") %></a></li>
        </ItemTemplate>
      </asp:ListView>
    </div>
    <div class="favorite"><asp:HyperLink ID="lnkFavorite" meta:resourcekey="lnkFavorite" runat="server" /></div>
    <form id="frmLink" method="post" runat="server">
      <div class="rating">
        <asp:LinkButton ID="lnkRateUp" OnClick="RateUp" runat="server"><asp:Image ID="imgRateUp" meta:resourcekey="imgRateUp" runat="server" /></asp:LinkButton><br />
        <asp:Label ID="lblRating" runat="server" /><br />
        <asp:LinkButton ID="lnkRateDown" OnClick="RateDown" runat="server"><asp:Image ID="imgRateDown" meta:resourcekey="imgRateDown" runat="server" /></asp:LinkButton>
      </div>
      <div class="control"><asp:LinkButton ID="lnkReported" OnClick="ReportRead" Visible="false" runat="server"><asp:Image ID="imgReported" meta:resourcekey="imgReported" runat="server" /></asp:LinkButton> <asp:HyperLink ID="lnkEditLink" meta:resourcekey="lnkEditLink" runat="server" /><asp:HyperLink ID="lnkRemoveLink" meta:resourcekey="lnkRemoveLink" runat="server" /></div>
    </form>
  </asp:Panel>
  <div class="comments"><asp:HyperLink ID="lnkComments" meta:resourcekey="lnkComments" runat="server" />&nbsp;<asp:HyperLink ID="lnkAddComment" meta:resourcekey="lnkAddComment" runat="server" /></div>
</asp:Content>
