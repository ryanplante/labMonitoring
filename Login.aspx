<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="labMonitor.WebForm1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
<form id="form1" runat="server">
  <div>
    <label for="txtUsername">Username:</label>
    <asp:TextBox ID="txtUsername" runat="server"></asp:TextBox>
  </div>
  <div>
    <label for="txtPassword">Password:</label>
    <asp:TextBox ID="txtPassword" runat="server" TextMode="Password"></asp:TextBox>
  </div>
  <div>
    <asp:Button ID="btnSubmit" runat="server" Text="Submit" OnClick="btnSubmit_Click" />
  </div>
</form>
</body>
</html>
