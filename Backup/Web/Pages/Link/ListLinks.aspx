<%@ Page Title="" Language="C#" MasterPageFile="~/WebMovies.Master" AutoEventWireup="true" CodeBehind="ListLinks.aspx.cs" Inherits="Es.Udc.DotNet.WebMovies.Web.Pages.Link.ListLinks" %>

<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder_Title" runat="server">
  <asp:Image ID="imgLinks" meta:resourcekey="imgLink" runat="server" />&nbsp;<asp:Label ID="lclLinks" meta:resourcekey="lclLinks" runat="server" />&nbsp;<asp:Label ID="lclFor" meta:resourcekey="lclFor" Visible="false" runat="server" />&nbsp;<asp:HyperLink ID="lnkForWhat" Visible="false" runat="server"><asp:Label ID="lblForWhat" runat="server" />&nbsp;<asp:Image ID="imgForWhat" runat="server" /></asp:HyperLink>
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="ContentPlaceHolder_Content" runat="server">
  <form id="frmListLinks" runat="server">
    <asp:ListView ID="lvListLinks" OnPreRender="LvListLinks_PreRender" runat="server">
      <LayoutTemplate>
        <ul class="list">
          <asp:PlaceHolder ID="itemPlaceholder" runat="server" />
        </ul>
      </LayoutTemplate>
      <ItemTemplate>
        <li class="<%# Css((long)Eval("LinkId")) %>">
          <div class="name"><a href="/Pages/Link/Link.aspx?linkId=<%# Eval("LinkId") %>"><asp:Image ID="imgLink" meta:resourcekey="imgLink" runat="server"/> <%# Eval("Name") %></a> <a href="http://<%# Eval("Url") %>"><asp:Image ID="imgUrl" meta:resourcekey="imgUrl" runat="server"/></a></div>
          <div class="description"><asp:Image ID="imgDescription" meta:resourcekey="imgDescription" runat="server"/> <%# Eval("Description") %></div>
          <div class="movie"><a href="/Pages/Movie/MovieXml.aspx?movieId=<%# Eval("MovieId") %>"><asp:Image ID="imgMovie" meta:resourcekey="imgMovie" runat="server"/> <%# MovieTitle((long)Eval("MovieId")) %></a></div>
          <div class="labels"><asp:Image ID="imgLabels" meta:resourcekey="imgLabels" runat="server"/> <%# Labels((long)Eval("LinkId")) %></div>
          <div class="reference"><a href="/Pages/Link/ListLinks.aspx?userId=<%# Eval("UserId") %>"><asp:Image ID="imgAuthor" meta:resourcekey="imgAuthor" runat="server"/> <%# Eval("UserName") %></a> · <asp:Image ID="imgDate" meta:resourcekey="imgDate" runat="server"/> <%# Eval("Date") %></div>
          <div class="favorite"><%# Favorite((long)Eval("LinkId")) %></div>
          <div class="rating">
            <asp:LinkButton ID="lnkRateDown" CssClass='<%# RateDownCss((long)Eval("LinkId")) %>' Enabled='<%# RateEnabled((long)Eval("LinkId")) %>' OnCommand="RateDown" CommandArgument='<%# Eval("LinkId") %>' runat="server"><asp:Image ID="imgRateDown" meta:resourcekey="imgRateDown" runat="server" /></asp:LinkButton>
            <%# Eval("Rating") %>
            <asp:LinkButton ID="lnkRateUp" CssClass='<%# RateUpCss((long)Eval("LinkId")) %>' Enabled='<%# RateEnabled((long)Eval("LinkId")) %>' OnCommand="RateUp" CommandArgument='<%# Eval("LinkId") %>' runat="server"><asp:Image ID="imgRateUp" meta:resourcekey="imgRateUp" runat="server" /></asp:LinkButton>
          </div>
          <div class="comments"><a href="/Pages/Comment/ListComments.aspx?linkId=<%# Eval("LinkId") %>"><asp:Image ID="imgComments" meta:resourcekey="imgComments" runat="server" /></a> <a href="/Pages/Comment/AddComment.aspx?linkId=<%# Eval("LinkId") %>"><asp:Image ID="imgAddComment" meta:resourcekey="imgAddComment" runat="server" /></a></div>
          <div class="control"><asp:LinkButton ID="lnkReported" OnCommand="ReportRead" CommandArgument='<%# Eval("LinkId") %>' Visible='<%# ReportReadVisible((long)Eval("LinkId")) %>' runat="server"><asp:Image ID="imgReported" meta:resourcekey="imgReported" runat="server" /></asp:LinkButton> <%# Edit((long)Eval("LinkId")) %> <%# Remove((long)Eval("LinkId")) %></div>
        </li>
      </ItemTemplate>
      <EmptyDataTemplate>
        <div class="error">
          <p><asp:Localize ID="lclNoLinks" meta:resourcekey="lclNoLinks" runat="server" /></p>
          <asp:HyperLink ID="lnkReturn" CssClass="linkButton" NavigateUrl="/Pages/MainPage.aspx" meta:resourcekey="lnkReturn" runat="server" />
        </div>
      </EmptyDataTemplate>
    </asp:ListView>
    <asp:HyperLink ID="lnkOrderBy" CssClass="orderBy" runat="server"><asp:Image ID="imgOrderBy" meta:resourcekey="imgOrderBy" runat="server" />&nbsp;<asp:Image ID="imgByWhat" runat="server" /></asp:HyperLink>
    <div class="links">
      <asp:DataPager ID="dpListLinks" PagedControlID="lvListLinks" runat="server">
        <Fields>
          <asp:NextPreviousPagerField ButtonType="Link" ButtonCssClass="previous" ShowFirstPageButton="false" ShowNextPageButton="false" ShowLastPageButton="false" meta:resourcekey="nppfListLinks" runat="server" />
          <asp:NextPreviousPagerField ButtonType="Link" ButtonCssClass="next" ShowFirstPageButton="false" ShowPreviousPageButton="false" ShowLastPageButton="false" meta:resourcekey="nppfListLinks" runat="server" />
        </Fields>
      </asp:DataPager>
    </div>
    <div class="footNote" style="position: absolute; bottom: 0px; left: 48%;"><asp:Hyperlink ID="lnkAddLink" meta:resourcekey="lnkAddLink" Visible="false" runat="server" /></div>
  </form>
</asp:Content>
