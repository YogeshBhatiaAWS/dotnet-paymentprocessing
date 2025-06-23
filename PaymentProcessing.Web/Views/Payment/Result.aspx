<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<PaymentProcessing.Web.Models.PaymentResultViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    <%= ViewData["Title"] %>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Payment Result</h2>
    
    <div class="result-box">
        <% if (Model.IsSuccessful) { %>
            <div class="success">
                <h3>✓ Payment Successful!</h3>
            </div>
        <% } else { %>
            <div class="alert alert-error">
                <h3>✗ Payment Failed</h3>
            </div>
        <% } %>
        
        <table style="width: 100%; border-collapse: collapse; margin-top: 15px;">
            <tr style="border-bottom: 1px solid #ddd;">
                <td style="padding: 8px; font-weight: bold;">Transaction ID:</td>
                <td style="padding: 8px;"><%= Model.TransactionId %></td>
            </tr>
            <tr style="border-bottom: 1px solid #ddd;">
                <td style="padding: 8px; font-weight: bold;">Status:</td>
                <td style="padding: 8px;">
                    <span style="color: <%= Model.IsSuccessful ? "#27ae60" : "#e74c3c" %>;">
                        <%= Model.Status %>
                    </span>
                </td>
            </tr>
            <tr style="border-bottom: 1px solid #ddd;">
                <td style="padding: 8px; font-weight: bold;">Amount:</td>
                <td style="padding: 8px;">$<%= Model.ProcessedAmount.ToString("F2") %></td>
            </tr>
            <tr style="border-bottom: 1px solid #ddd;">
                <td style="padding: 8px; font-weight: bold;">Processed Date:</td>
                <td style="padding: 8px;"><%= Model.ProcessedDateTime.ToString("yyyy-MM-dd HH:mm:ss") %></td>
            </tr>
            <% if (!string.IsNullOrEmpty(Model.AuthorizationCode)) { %>
            <tr style="border-bottom: 1px solid #ddd;">
                <td style="padding: 8px; font-weight: bold;">Authorization Code:</td>
                <td style="padding: 8px;"><%= Model.AuthorizationCode %></td>
            </tr>
            <% } %>
            <tr style="border-bottom: 1px solid #ddd;">
                <td style="padding: 8px; font-weight: bold;">Response Message:</td>
                <td style="padding: 8px;"><%= Model.ResponseMessage %></td>
            </tr>
            <% if (!string.IsNullOrEmpty(Model.ErrorMessage)) { %>
            <tr style="border-bottom: 1px solid #ddd;">
                <td style="padding: 8px; font-weight: bold;">Error Details:</td>
                <td style="padding: 8px; color: #e74c3c;"><%= Model.ErrorMessage %></td>
            </tr>
            <% } %>
        </table>
    </div>
    
    <div style="margin-top: 20px;">
        <%= Html.ActionLink("Process Another Payment", "Index", "Payment", null, new { @class = "btn btn-success" }) %>
        <%= Html.ActionLink("Home", "Index", "Home", null, new { @class = "btn" }) %>
        
        <% if (Model.IsSuccessful && Model.Status.ToString() == "Approved") { %>
            <div style="margin-top: 15px;">
                <h4>Refund Options:</h4>
                <% using (Html.BeginForm("Refund", "Payment", FormMethod.Post)) { %>
                    <%= Html.Hidden("transactionId", Model.TransactionId) %>
                    <input type="number" name="amount" step="0.01" max="<%= Model.ProcessedAmount %>" placeholder="Refund amount" style="margin-right: 10px;" />
                    <input type="submit" value="Process Refund" class="btn btn-danger" onclick="return confirm('Are you sure you want to process this refund?');" />
                <% } %>
            </div>
        <% } %>
    </div>
    
    <div class="result-box" style="margin-top: 30px;">
        <h4>Transaction Status Check:</h4>
        <p>You can check the status of this transaction later using the Transaction ID: <strong><%= Model.TransactionId %></strong></p>
        <% using (Html.BeginForm("Status", "Payment", FormMethod.Get)) { %>
            <input type="text" name="transactionId" value="<%= Model.TransactionId %>" style="margin-right: 10px;" />
            <input type="submit" value="Check Status" class="btn" />
        <% } %>
    </div>
</asp:Content>
