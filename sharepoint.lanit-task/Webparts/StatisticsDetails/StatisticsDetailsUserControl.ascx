<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="StatisticsDetailsUserControl.ascx.cs" Inherits="sharepoint.lanit_task.Webparts.StatisticsDetails.StatisticsDetailsUserControl" %>

<link href="/assets/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
<script src="/assets/js/bootstrap.min.js" type="text/javascript"></script>

<style>
    .btn-primary {
        margin-left: 0px !important;
    }

    #statistic-details {
        padding: 20px;
        border-radius: 15px;
    }
</style>

<script type="text/javascript">
    function showReponse() {
        let form = document.querySelector('#statistic-details');
        if (!form) return;

        let responsePlaceholder = form.querySelector('#response');
        if (!responsePlaceholder) return;

        if (isSuccessed) {
            responsePlaceholder.classList.add('alert-success');
        } else {
            responsePlaceholder.classList.add('alert-danger');
        }

        let node = document.createTextNode(message);
        responsePlaceholder.appendChild(node);
        return;
    }

    document.addEventListener('DOMContentLoaded', showReponse, false);
</script>

<div class="container alert-dark" id="statistic-details">


<h3>Создание нового элемента статистики</h3>
    <div class="form-group">
        <div class="form-row">
            <label for="number">Укажите число:</label>
            <asp:TextBox ID="txtNumber" runat="server" CssClass="form-control"></asp:TextBox>
        </div>
        <div class="form-row">
            <asp:CompareValidator ID="numberCompareValidator" runat="server" ControlToValidate="txtNumber" Type="Integer" Operator="DataTypeCheck" ErrorMessage="Указано неверное число" CssClass="invalid-feedback" Display="Dynamic" ></asp:CompareValidator>
            <asp:RequiredFieldValidator ID="numberRequireValidator" runat="server" ControlToValidate="txtNumber" ErrorMessage="Укажите число" CssClass="invalid-feedback" Display="Dynamic"></asp:RequiredFieldValidator>
        </div>
    </div>

    <div class="form-group">
        <div class="form-row">
            <label for="date">Укажите дату:</label>
        </div>
        <div class="form-row">
            <SharePoint:DateTimeControl ID="dtcDate" runat="server" CssClassTextBox="form-control" CssClassDescription="form-control" />
        </div>
        <div class="form-row">
            <asp:CompareValidator ID="dateCompareValidator" runat="server" ControlToValidate="dtcDate$dtcDateDate" Type="Date" Operator="DataTypeCheck"  ErrorMessage="Указана неверная дата" CssClass="invalid-feedback" Display="Dynamic"></asp:CompareValidator>
            <asp:RequiredFieldValidator ID="dateRequireValidator" runat="server" ControlToValidate="dtcDate$dtcDateDate" ErrorMessage="Укажите дату" CssClass="invalid-feedback" Display="Dynamic"></asp:RequiredFieldValidator>
        </div>
    </div>

    <div class="form-group">
        <div class="form-row">
            <% if (Request.QueryString["success"] == "1")
                { %>

            <div class="alert alert-success">

            <% }
    else
    { %>
            <div class="alert alert-error">

            <% } %>

                 <%= Request.QueryString["message"] %>
               
            </div>
        </div>
    </div>

    <div class="form-row">
        <asp:Button ID="btnSave" CssClass="btn btn-primary"  runat="server" Text="Сохранить" OnClick="btnSave_Click" />
    </div>
</div>


