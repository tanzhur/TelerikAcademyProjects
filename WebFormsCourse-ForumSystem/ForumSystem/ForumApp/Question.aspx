<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Question.aspx.cs" Inherits="ForumApp.Question" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:Literal ID="Content" runat="server" />
    <%--<asp:UpdatePanel runat="server" ID="UpdatePanelQuestionTitle" UpdateMode="Conditional">
        <ContentTemplate>--%>
            <asp:FormView 
                ID="FormViewQuestion" 
                runat="server"
                DataKeyNames="Id" 
                SelectMethod="FormViewQuestion_GetItem"
                ItemType="ForumApp.Models.Question"
                UpdateMethod="FormViewQuestion_UpdateItem">
                <ItemTemplate>
                    <table>
                        <tr>
                            <td>
                                <h1><%#: Item.Title %></h1>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal Text='<%#: Item.Content %>' runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal Text='<%# String.Format("answered {0} ago in {1} by {2}", 
                                             GetTimeDifference(Item.PostedDate), Item.Category.Title, Item.AspNetUser.UserName)  %>'
                                    runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:LinkButton ID="LinkButtonEdit" Visible='<%# GetEditButton(Item.AspNetUser.UserName)%>' Text="Edit" CommandName="Edit" runat="server" />
                            </td>
                        </tr>
                    </table>
                </ItemTemplate>
                <EditItemTemplate>
                    <tr>
                        <td>
                            <asp:TextBox ID="TextBoxTitle" TextMode="MultiLine" Rows="2" Text='<%# BindItem.Title %>' runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="TextBoxContent" TextMode="MultiLine" Rows="8" Text='<%# BindItem.Content %>' runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:LinkButton Text="Save" ID="ButtonUpdateAnswer" runat="server" CommandName="Update"/>
                            <asp:LinkButton Text="Cancel" ID="ButtonCancelUpdateAnswer" runat="server" CommandName="Cancel" />
                        </td>
                    </tr>
                </EditItemTemplate>
            </asp:FormView>
<%--        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="ButtonUpdateAnswer"/>
        </Triggers>
    </asp:UpdatePanel>--%>

<%--    <asp:UpdatePanel ID="UpdatePanelAnswers" runat="server" UpdateMode="Conditional">
        <ContentTemplate>--%>
            <asp:ListView runat="server" ID="ListViewAnswers" SelectMethod="ListViewAnswers_GetData"
                DataKeyNames="Id"
                ItemType="ForumApp.Models.Answer"
                InsertMethod="ListViewAnswers_InsertItem"
                UpdateMethod="ListViewAnswers_UpdateItem"
                DeleteMethod="ListViewAnswers_DeleteItem"
                OnItemCanceling="ListViewAnswers_ItemCanceling">
                <LayoutTemplate>
                    <table>
                        <thead>
                            <tr>
                                <th>
                                    <asp:LinkButton ID="LinkButtonAnswer" Visible='false' Text="Answer" OnClick="LinkButtonAnswer_Click" runat="server" />
                                </th>

                            </tr>
                        </thead>
                        <tbody>
                            <asp:PlaceHolder runat="server" ID="itemPlaceholder"></asp:PlaceHolder>
                        </tbody>
                        <tfoot>
                            <td class="pagerLine" colspan="3">
                                <asp:DataPager ID="DataPagerCustomers" runat="server" PageSize="15">
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
                            <asp:Literal Text='<%# MatchURLS((Server.HtmlEncode(Item.Content))) %>' runat="server" />
                            <%--<asp:HyperLink runat="server" Text="<%#: Item.Title %>"
                NavigateUrl='<%# String.Format("~/Question?questionId={0}", Item.Id.ToString()) %>' />--%>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal Text='<%# String.Format("answered {0} ago by {1}",
                                 //(
                                 //    (DateTime.Now-Item.PostedDate).Days >= 1 ? (DateTime.Now-Item.PostedDate).Days+" days"
                                 //    : (DateTime.Now-Item.PostedDate).Hours < 1 ? (DateTime.Now-Item.PostedDate).Minutes + " minutes"
                                 //    : (DateTime.Now-Item.PostedDate).Hours + " hours"
                                 //)
                                 GetTimeDifference(Item.PostedDate)
                                 , Item.AspNetUser.UserName)  %>'
                                runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:LinkButton ID="LinkButtonEdit" Visible='<%# GetEditButton(Item.AspNetUser.UserName)%>' Text="Edit" CommandName="Edit" runat="server" />
                            <asp:LinkButton ID="LinkButtonDelete" Visible='<%# GetEditButton(Item.AspNetUser.UserName)%>' Text="Delete" CommandName="Delete" runat="server" />
                        </td>
                    </tr>
                </ItemTemplate>
                <InsertItemTemplate>
                    <tr>
                        <td>
                            <asp:TextBox TextMode="MultiLine" ID="TextBoxAnswerContent" Text='<%# BindItem.Content %>' runat="server" Rows="8" Width="320" />
                            <br />
                            <asp:LinkButton Text="Answer" ID="ButtonAddAnswer" runat="server" CommandName="Insert" />
                            <asp:LinkButton Text="Cancel" ID="ButtonCancelAnswer" runat="server" CommandName="Cancel" />
                        </td>
                    </tr>
                </InsertItemTemplate>
                <EditItemTemplate>
                    <tr>
                        <td>
                            <asp:TextBox TextMode="MultiLine" ID="TextBoxAnswerContent" Text='<%# BindItem.Content %>' runat="server" Rows="8" Width="320" />
                            <br />
                            <asp:LinkButton Text="Save" ID="ButtonUpdateAnswer" runat="server" CommandName="Update" />
                            <asp:LinkButton Text="Cancel" ID="ButtonCancelUpdateAnswer" runat="server" CommandName="Cancel" />
                        </td>
                    </tr>
                </EditItemTemplate>
                <EmptyItemTemplate>
                    <tr>
                        <td>
                            <asp:TextBox TextMode="MultiLine" ID="TextBoxAnswerContent" Text='<%# BindItem.Content %>' runat="server" Rows="8" Width="320" />
                            <br />
                            <asp:LinkButton Text="Answer" ID="ButtonAddAnswer" runat="server" CommandName="Insert" />
                            <asp:LinkButton Text="Cancel" ID="ButtonCancelAnswer" runat="server" CommandName="Cancel" />
                        </td>
                    </tr>
                </EmptyItemTemplate>
                <EmptyDataTemplate>
                    <table>
                        <thead>
                            <tr>
                                <th>
                                    <asp:LinkButton ID="LinkButtonAnswer" Visible='false' Text="Answer" OnClick="LinkButtonAnswer_Click" runat="server" />
                                </th>

                            </tr>
                        </thead>
                    </table>
                </EmptyDataTemplate>
            </asp:ListView>
<%--        </ContentTemplate>
    </asp:UpdatePanel>--%>
</asp:Content>
