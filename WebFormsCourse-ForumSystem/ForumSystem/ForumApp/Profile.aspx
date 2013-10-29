<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Profile.aspx.cs" Inherits="ForumApp.Profile" %>
<asp:Content ID="ContentProfileInformation" ContentPlaceHolderID="MainContent" runat="server">
    <asp:FormView ID="FormViewProfileInformation" runat="server"
        DataKeyNames="Id"
        ItemType="ForumApp.Models.ApplicationUser"
        SelectMethod="FormViewProfileInformation_GetItem"
        UpdateMethod="FormViewProfileInformation_UpdateItem">
        <ItemTemplate>
            <table>
                <tr>
                    <td>
                        <asp:LinkButton ID="LinkButton" CommandName="Edit" Text="Edit" runat="server"></asp:LinkButton>
                    </td>
                </tr>
                <tr>
                    <td>
                        Picture: <asp:Image   ImageUrl="<%# Item.ProfilePicturePath %>" runat="server" Width="50" Height="50" />
                    </td>
                </tr>
                <tr>
                    <td>
                        Username: <%#: Item.UserName %>
                    </td>
                </tr>
                <tr>
                    <td>
                        Answers: <%#: GetAnswers() %>
                    </td>
                </tr>
                <tr>
                    <td>
                        Questions: <%#: GetQuestions() %>
                    </td>
                </tr>
            </table>
        </ItemTemplate>
        <EditItemTemplate>
            <table>
                <tr>
                    <td>
                        <asp:TextBox ID="TextBox" runat="server" Text='<%# BindItem.UserName %>' />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:LinkButton ID="LinkButtonUpdate" CommandName="Update"  Text="Save"  runat="server"/>
                        <asp:LinkButton ID="LinkButtonCancel" CommandName="Cancel"  Text="Cancel"  runat="server" />
                    </td>
                </tr>
            </table>
        </EditItemTemplate>
    </asp:FormView>

    
     <asp:ListView runat="server" ID="ListViewQuestionsByUser" InsertItemPosition="None" SelectMethod="ListViewQuestionsByUser_GetData"
                  ItemType="ForumApp.Models.Question">
        <LayoutTemplate>
            <table >
                <thead>
                    <tr>
                        <th>
                            <h2>    Recent activity by <asp:Literal Text="AuthorId" runat="server" /> </h2>
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
                    <h3> No user activity</h3>
                </div>
            </div>
        </EmptyDataTemplate>
    </asp:ListView>
</asp:Content>
