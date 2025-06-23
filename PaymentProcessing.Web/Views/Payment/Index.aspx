<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<PaymentProcessing.Web.Models.PaymentViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    <%= ViewData["Title"] %>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Process Payment</h2>
    
    <% if (ViewData["Error"] != null) { %>
        <div class="alert alert-error">
            <%= ViewData["Error"] %>
        </div>
    <% } %>
    
    <% using (Html.BeginForm("Process", "Payment", FormMethod.Post)) { %>
        
        <div class="form-group">
            <%= Html.LabelFor(m => m.Amount) %>
            <%= Html.TextBoxFor(m => m.Amount, new { placeholder = "0.00" }) %>
            <%= Html.ValidationMessageFor(m => m.Amount, "", new { @class = "error" }) %>
        </div>
        
        <div class="form-group">
            <%= Html.LabelFor(m => m.Currency) %>
            <%= Html.DropDownListFor(m => m.Currency, new SelectList(new[] { "USD", "EUR", "GBP", "CAD" }, Model.Currency)) %>
            <%= Html.ValidationMessageFor(m => m.Currency, "", new { @class = "error" }) %>
        </div>
        
        <div class="form-group">
            <%= Html.LabelFor(m => m.CardNumber, "Card Number") %>
            <%= Html.TextBoxFor(m => m.CardNumber, new { placeholder = "1234 5678 9012 3456", maxlength = "19" }) %>
            <%= Html.ValidationMessageFor(m => m.CardNumber, "", new { @class = "error" }) %>
        </div>
        
        <div class="form-group">
            <%= Html.LabelFor(m => m.CardHolderName, "Card Holder Name") %>
            <%= Html.TextBoxFor(m => m.CardHolderName, new { placeholder = "John Doe" }) %>
            <%= Html.ValidationMessageFor(m => m.CardHolderName, "", new { @class = "error" }) %>
        </div>
        
        <div style="display: flex; gap: 15px;">
            <div class="form-group" style="flex: 1;">
                <%= Html.LabelFor(m => m.ExpiryMonth, "Expiry Month") %>
                <%= Html.DropDownListFor(m => m.ExpiryMonth, 
                    new SelectList(Enumerable.Range(1, 12).Select(i => new { Value = i, Text = i.ToString("00") }), "Value", "Text", Model.ExpiryMonth),
                    "Select Month") %>
                <%= Html.ValidationMessageFor(m => m.ExpiryMonth, "", new { @class = "error" }) %>
            </div>
            
            <div class="form-group" style="flex: 1;">
                <%= Html.LabelFor(m => m.ExpiryYear, "Expiry Year") %>
                <%= Html.DropDownListFor(m => m.ExpiryYear, 
                    new SelectList(Enumerable.Range(DateTime.Now.Year, 11).Select(i => new { Value = i, Text = i.ToString() }), "Value", "Text", Model.ExpiryYear),
                    "Select Year") %>
                <%= Html.ValidationMessageFor(m => m.ExpiryYear, "", new { @class = "error" }) %>
            </div>
            
            <div class="form-group" style="flex: 1;">
                <%= Html.LabelFor(m => m.CVV) %>
                <%= Html.TextBoxFor(m => m.CVV, new { placeholder = "123", maxlength = "4" }) %>
                <%= Html.ValidationMessageFor(m => m.CVV, "", new { @class = "error" }) %>
            </div>
        </div>
        
        <div class="form-group">
            <%= Html.LabelFor(m => m.CustomerEmail, "Customer Email") %>
            <%= Html.TextBoxFor(m => m.CustomerEmail, new { placeholder = "customer@example.com" }) %>
            <%= Html.ValidationMessageFor(m => m.CustomerEmail, "", new { @class = "error" }) %>
        </div>
        
        <div class="form-group">
            <%= Html.LabelFor(m => m.Description) %>
            <%= Html.TextBoxFor(m => m.Description, new { placeholder = "Payment description (optional)" }) %>
            <%= Html.ValidationMessageFor(m => m.Description, "", new { @class = "error" }) %>
        </div>
        
        <div class="form-group">
            <%= Html.LabelFor(m => m.BillingAddress, "Billing Address") %>
            <%= Html.TextAreaFor(m => m.BillingAddress, new { placeholder = "123 Main St, City, State, ZIP", rows = "3" }) %>
            <%= Html.ValidationMessageFor(m => m.BillingAddress, "", new { @class = "error" }) %>
        </div>
        
        <div class="form-group">
            <%= Html.LabelFor(m => m.MerchantId, "Merchant ID") %>
            <%= Html.TextBoxFor(m => m.MerchantId, new { @readonly = "readonly" }) %>
            <%= Html.ValidationMessageFor(m => m.MerchantId, "", new { @class = "error" }) %>
        </div>
        
        <div style="margin-top: 20px;">
            <input type="submit" value="Process Payment" class="btn btn-success" />
            <%= Html.ActionLink("Cancel", "Index", "Home", null, new { @class = "btn" }) %>
        </div>
        
    <% } %>
    
    <div class="result-box" style="margin-top: 30px;">
        <h4>Test Information:</h4>
        <p>Use these test card numbers:</p>
        <ul>
            <li><strong>4111111111111111</strong> - Approved</li>
            <li><strong>4111111111111119</strong> - Declined</li>
            <li><strong>4111111111111110</strong> - Error</li>
        </ul>
    </div>
</asp:Content>
