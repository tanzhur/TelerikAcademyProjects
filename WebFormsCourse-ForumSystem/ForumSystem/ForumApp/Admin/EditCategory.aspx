<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="EditCategory.aspx.cs" Inherits="ForumApp.Admin.EditCategory" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
     <div class="row">
        <div class="span3">
            <asp:TextBox ID="CatNameTb" runat="server" />
        </div>
        <div class="span1">
            <asp:Button OnClick="Save_Click" Text="Save" runat="server" CssClass="btn" />
        </div>
        <div class="span1">
            <asp:HyperLink NavigateUrl="~/Categories.aspx" Text="Cancel" runat="server"  CssClass="btn"/>
        </div>
        <div class="span5"></div>
    </div>
</asp:Content>
