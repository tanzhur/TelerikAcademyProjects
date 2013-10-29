<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="ForumApp._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <asp:ListView runat="server" ID="ListViewQuestions" SelectMethod="ListViewQuestions_GetData" 
                          ItemType="ForumApp.Models.Question">

                <LayoutTemplate>
                    <table>
                        <thead>
                            <tr>
                                <th>
                                    <h1>Recent questions and answers</h1>
                                </th>

                            </tr>
                            <tr>
                                <td>
                                    <asp:LinkButton runat="server" ID="SortByFirstNameButton"
                                                    CommandName="Sort" Text="Sort By date" CommandArgument="PostedDate" />
                                </td>
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
                <EmptyItemTemplate>
                    No Data Available
                </EmptyItemTemplate>
                <EmptyDataTemplate>
                    No Data Available
                </EmptyDataTemplate>
            </asp:ListView>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
