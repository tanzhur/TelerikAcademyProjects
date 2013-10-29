<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Category.aspx.cs" Inherits="ForumApp.Category" %>
<asp:Content ID="ContentCategory" ContentPlaceHolderID="MainContent" runat="server">
       <h1><asp:Literal ID="LiteralCategoryTitle" runat="server" /></h1>
    <asp:ListView runat="server" ID="ListViewQuestions" SelectMethod="ListViewQuestions_GetData"
        ItemType="ForumApp.Models.Question">

        <LayoutTemplate>
            <table>
                <thead>
                    <tr>
                        <th>
                        </th>
                    </tr>
                </thead>
                <tbody>
                    <asp:PlaceHolder runat="server" ID="itemPlaceholder"></asp:PlaceHolder>
                </tbody>
                <tfoot>
                    <td class="pagerLine" colspan="3">
                        <asp:DataPager ID="DataPagerCustomers" runat="server" PageSize="2">
                            <Fields>
                                <asp:NextPreviousPagerField ShowFirstPageButton="True"
                                    ShowNextPageButton="False"
                                    ShowPreviousPageButton="False" />
                                <asp:NumericPagerField />
                                <asp:NextPreviousPagerField ShowLastPageButton="True"
                                    ShowNextPageButton="False"
                                    ShowPreviousPageButton="False" />
                            </Fields>
                        </asp:DataPager>
                    </td>
                </tfoot>
            </table>
        </LayoutTemplate>
        <ItemTemplate>
            <tr>
                <td>
                    <asp:Literal Text='<%#: String.Format("{0} Answers", Item.Answers.Count) %>' runat="server" />
                    <asp:HyperLink runat="server" Text="<%#: Item.Title %>"
                        NavigateUrl='<%# String.Format("~/Question?questionId={0}", Item.Id.ToString()) %>' />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal Text='<%# String.Format("answered {0} ago in {1} by {2}", 
                        GetTimeDifference(Item.PostedDate), Item.Category.Title, Item.AspNetUser.UserName)  %>'
                        runat="server" />
                </td>
            </tr>
        </ItemTemplate>

    </asp:ListView>
</asp:Content>
