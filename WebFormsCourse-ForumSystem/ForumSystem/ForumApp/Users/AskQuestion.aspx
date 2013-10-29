<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AskQuestion.aspx.cs" Inherits="ForumApp.AskQuestion" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h1>Ask a question</h1>
    <div>
       <p>The question in one sentence:</p>
        <asp:TextBox ID="TextBoxQuestionTitle" runat="server" /> <br />
        <asp:Literal Text="Choose Category" runat="server" />
        <asp:DropDownList 
            ID="DropDownListCategory" runat="server" DataTextField="Title" DataValueField="Id">
        </asp:DropDownList> <br />
        <asp:TextBox TextMode="MultiLine" ID="TextBoxContent" runat="server" Rows="8" Width="320"/> <br />
        <asp:Button Text="Ask The Question" ID="ButtonAddQuestion" runat="server" OnClick="ButtonAddQuestion_Click" />
    </div>
    <asp:Literal Id="Result" runat="server" />
</asp:Content>
