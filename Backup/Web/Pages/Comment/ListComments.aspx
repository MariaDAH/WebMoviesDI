<%@ Page Title="" Language="C#" MasterPageFile="~/WebMovies.Master" AutoEventWireup="true" CodeBehind="ListComments.aspx.cs" Inherits="Es.Udc.DotNet.WebMovies.Web.Pages.Comment.ListComments" %>

<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder_Title" runat="server">
  <asp:Image ID="imgComments" meta:resourcekey="imgComments" runat="server" /> <asp:Localize ID="lclComments" meta:resourcekey="lclComments" runat="server" /> <asp:Localize ID="lclFor" meta:resourcekey="lclFor" runat="server" /> <asp:Hyperlink ID="lnkForWhat" runat="server"><asp:Label ID="lblForWhat" runat="server" />&nbsp;<asp:Image ID="imgForWhat" runat="server" /></asp:Hyperlink>
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="ContentPlaceHolder_Content" runat="server">
  <form id="frmListComments" runat="server">
    <asp:ListView ID="lvListComments" OnPreRender="LvListComments_PreRender" runat="server">
      <LayoutTemplate>
        <ul class="list">
          <asp:PlaceHolder ID="itemPlaceholder" runat="server" />
        </ul>
      </LayoutTemplate>
      <ItemTemplate>
        <li class="comment">
          <div class="author"><a href="/Pages/Comment/ListComments.aspx?userId=<%# Eval("AuthorId") %>"><asp:Image ID="imgAuthor" meta:resourcekey="imgAuthor" runat="server"/> <%# Eval("AuthorName") %></a></div>
          <div class="date"><asp:Image ID="imgDate" meta:resourcekey="imgDate" runat="server"/> <%# Eval("Date") %></div>
          <div class="text"><%# Eval("Text") %></div>
          <div class="control"><%# Edit((long)Eval("CommentId")) %> <%# Remove((long)Eval("CommentId")) %></div>
        </li>
      </ItemTemplate>
      <EmptyDataTemplate>
        <div class="error">
          <p><asp:Localize ID="lclNoComments" meta:resourcekey="lclNoComments" runat="server" /></p>
          <asp:HyperLink ID="lnkReturn" CssClass="linkButton" NavigateUrl="/Pages/MainPage.aspx" meta:resourcekey="lnkReturn" runat="server" />
        </div>
      </EmptyDataTemplate>
    </asp:ListView>

    <div class="links">
      <asp:DataPager ID="dpListComments" PagedControlID="lvListComments" runat="server">
        <Fields>
          <asp:NextPreviousPagerField ButtonType="Link" ButtonCssClass="previous" ShowFirstPageButton="false" ShowNextPageButton="false" ShowLastPageButton="false" meta:resourcekey="nppfListLinks" runat="server" />
          <asp:NextPreviousPagerField ButtonType="Link" ButtonCssClass="next" ShowFirstPageButton="false" ShowPreviousPageButton="false" ShowLastPageButton="false" meta:resourcekey="nppfListLinks" runat="server" />
        </Fields>
      </asp:DataPager>
    </div>
    <div class="footNote" style="position: absolute; bottom: 0px; left: 48%;"><asp:Hyperlink ID="lnkAddComment" meta:resourcekey="lnkAddComment" Visible="false" runat="server" /></div>
  </form>
</asp:Content>
