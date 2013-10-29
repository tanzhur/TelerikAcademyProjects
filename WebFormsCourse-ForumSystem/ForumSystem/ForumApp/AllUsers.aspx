<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AllUsers.aspx.cs" Inherits="ForumApp.AllUsers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <h1>All users</h1>

    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <asp:ListView runat="server" ID="ListViewUsers" 
                          SelectMethod="ListViewUsers_GetData"
                          ItemType="ForumApp.Models.ApplicationUser">
                <LayoutTemplate>
                    <div class="container">
                        <div class="span6">
                            <asp:PlaceHolder runat="server" ID="itemPlaceholder"></asp:PlaceHolder>
                        </div>
                    </div>
                    <div class="container">
                        <div class="span6">
                            <asp:DataPager ID="DataPagerCustomers" runat="server" PageSize="4">
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
                        </div>
                    </div>
                </LayoutTemplate>
                <ItemTemplate>
                    <div class="span2 well">
                        <asp:Image   ImageUrl="<%# Item.ProfilePicturePath %>" runat="server" Width="30" Height="30" />
                        <asp:HyperLink NavigateUrl='<%# string.Format("~/Profile?userId={0}",Item.Id )%>'
                                       runat="server" Text="<%#: Item.UserName %>" />
                    </div>
                </ItemTemplate>
            </asp:ListView>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
