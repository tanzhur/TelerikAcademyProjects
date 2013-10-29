<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Categories.aspx.cs" Inherits="ForumApp.Categories" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h1>Browse categories</h1>
    <asp:Repeater runat="server" ID="RepeaterCategories" ItemType="ForumApp.Models.Category">
        <ItemTemplate>
            <p>
                <asp:HyperLink 
                    Text="Edit"
                    runat="server" 
                    Visible="<%# IsAdminInRole() %>" 
                    NavigateUrl='<%# String.Format("~/Admin/EditCategory.aspx?id={0}",Item.Id)  %>'/>
                <b>
                    <asp:Literal runat="server" Text="<%#: Item.Title %>" /></b> -
                    <asp:HyperLink
                        runat="server"
                    Text='<%#: String.Format("{0} Questions", Item.Questions.Count.ToString()) %>' 
                        NavigateUrl='<%#: String.Format("~/category?categoryId={0}", Item.Id) %>' />
            </p>
        </ItemTemplate>
    </asp:Repeater>

    <asp:Panel runat="server" ID="AddCategoryPanel" CssClass="well">
        <div class="row">
            <div class="span3">
                <asp:TextBox runat="server" ID="NewCatTextBox" />
            </div>
            <div class="span2">
                <asp:LinkButton ID="LinkButtonAdd" Text="Add New Category" runat="server" OnClick="LinkButtonAdd_Click" CssClass="btn" />
            </div>
        </div>
    </asp:Panel>

<%--        <asp:ListView
        ID="ListViewCategories"
        runat="server"
        SelectMethod="ListViewCategories_GetData"
        InsertMethod="ListViewCategories_InsertItem"
        ItemType="ForumApp.Models.Category">

        <LayoutTemplate>
            <div id="itemPlaceholder" runat="server"></div>

                    <asp:TextBox runat="server" ID="NewCatTextBox"  Visible="<%# IsAdminInRole()%>" />
                    <asp:Button CommandName="Insert" Text="Add New Category" runat="server" CssClass="btn"  Visible="<%# IsAdminInRole()%>" />
        </LayoutTemplate>

        <itemtemplate>
            <p>
                <asp:HyperLink Text="Edit" 
                    NavigateUrl='<%# String.Format("~/EditCategory.aspx?id={0}",Item.Id)  %>' 
                    Visible="<%# IsAdminInRole() %>" 
                    runat="server" />
                <b>
                    <asp:Literal runat="server" Text="<%#: Item.Title %>" /></b> -
                    <asp:HyperLink
                        runat="server"
                        Text='<%#: String.Format("{0} Questions", Item.Questions.Count.ToString()) %>'
                        NavigateUrl='<%#: String.Format("~/category?categoryId={0}", Item.Id) %>' />
            </p>
        </itemtemplate>

    </asp:ListView>--%>

</asp:Content>
