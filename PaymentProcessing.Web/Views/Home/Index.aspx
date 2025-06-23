<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    <%= ViewData["Title"] %>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2><%= ViewData["Message"] %></h2>
    
    <div class="result-box">
        <h3>Welcome to the Payment Processing System</h3>
        <p>This is a legacy .NET 3.5 Framework application demonstrating payment processing capabilities.</p>
        
        <h4>Features:</h4>
        <ul>
            <li>Credit card payment processing</li>
            <li>Payment validation and verification</li>
            <li>Transaction status tracking</li>
            <li>Refund processing</li>
            <li>MVC Web interface</li>
            <li>Web API endpoints</li>
            <li>Console application for batch processing</li>
        </ul>
        
        <h4>Getting Started:</h4>
        <p>Click on "Process Payment" to begin processing a payment transaction.</p>
        
        <div style="margin-top: 20px;">
            <%= Html.ActionLink("Process Payment", "Index", "Payment", null, new { @class = "btn btn-success" }) %>
        </div>
    </div>
    
    <div class="result-box">
        <h4>Test Card Numbers:</h4>
        <p>Use these test card numbers to simulate different payment scenarios:</p>
        <ul>
            <li><strong>4111111111111111</strong> - Approved transaction</li>
            <li><strong>4111111111111119</strong> - Declined transaction</li>
            <li><strong>4111111111111110</strong> - Processing error</li>
        </ul>
        <p><em>Note: Any valid Luhn algorithm card number ending in 1-8 will be approved, 9 will be declined, and 0 will generate an error.</em></p>
    </div>
</asp:Content>
