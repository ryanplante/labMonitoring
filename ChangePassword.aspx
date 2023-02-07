<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ChangePassword.aspx.cs" Inherits="labMonitor.ChangePassword" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <h1>Change Password</h1>
    <form id="form1" runat="server">
      <div>
        <label for="txtPassword">New Password:</label>
        <asp:TextBox ID="txtPassword" runat="server" TextMode="Password"></asp:TextBox>
      </div>
      <div>
        <label for="txtConfirm">Confirm Password:</label>
        <asp:TextBox ID="txtConfirm" runat="server" TextMode="Password"></asp:TextBox>
      </div>
      <div>
        <asp:Button ID="btnSubmit" runat="server" Text="Change" OnClick="btnSubmit_Click" />
      </div>
    </form>
</body>
</html>
