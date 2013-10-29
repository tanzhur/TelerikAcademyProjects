<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Search.aspx.cs" Inherits="ForumApp.Search" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <h3>
        Questions And Answers for "<asp:Literal ID="LiteralParam" runat="server" />"
    </h3>
    <asp:ListView  runat="server" ID="ListViewQuestionsByParam" InsertItemPosition="None" SelectMethod="ListViewQuestionsByParam_GetData"
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
                    <td class="pagerLine pagination" colspan="3">
                        <asp:DataPager ID="DataPagerCustomers"  runat="server" PageSize="3">

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
                    <hr />
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
        <EmptyDataTemplate>
            <div class="container">
                <div class="alert alert-error span8">
                    <h3> Please enter some text into the search box and try again.</h3>
                </div>
            </div>
        </EmptyDataTemplate>
    </asp:ListView>
</asp:Content>
