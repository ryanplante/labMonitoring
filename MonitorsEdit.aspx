<%@ Page Language="C#" Title="Lab Monitors" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="MonitorsEdit.aspx.cs" Inherits="labMonitor.MonitorsEdit" %>




<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

<div>
    <%--Show when user is department head or admin --%>
    <h2 runat="server" id="welcome">Lab Monitors</h2>
    <h3>Current Monitors</h3>
    <div class="monitor" runat="server">
        <asp:GridView ID="DGLabMonitors" runat="server" AutoGenerateColumns="false" OnRowCommand="Remove_User">
        <Columns>
            <asp:BoundField DataField="userID" HeaderText="Student ID" />
            <asp:BoundField DataField="userFName" HeaderText="First Name" />
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:LinkButton ID="btnRemove" runat="server" CommandName="RemoveUser" CommandArgument='<%#Eval("userID")%>'>
                        <asp:Image ID="X" runat="server" ImageUrl="/images/x.png" style="border-width: 0px;" />
                    </asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
    </div>
    <div class="MonitorForm" runat="server">
        <h3>Add Lab Monitor</h3>
        <label for="txtStudentID">Student ID</label>
        <br />
        <asp:TextBox ID="txtStudentID" runat="server"></asp:TextBox>
        <asp:RegularExpressionValidator ID="RegularExpressionValidator1"
            ControlToValidate="txtStudentID" runat="server"
            ErrorMessage="Not a valid student ID"
            ValidationExpression="\d+">
        </asp:RegularExpressionValidator>
        <br />
        <label for="txtStudentFirst">Student First Name</label>
        <br />
        <asp:TextBox ID="txtStudentFirst" runat="server"></asp:TextBox>
        <br />
        <label for="txtStudentLast">Student Last Name</label>
        <br />
        <asp:TextBox ID="txtStudentLast" runat="server"></asp:TextBox>
        <br />
        <asp:Label ID="lblWarning" CssClass="warning" runat="server" Visible="false"></asp:Label>
        <br />
        <asp:Button BackColor="Yellow" OnClick="Search_Users" Text="Search" runat="server" style="border-radius: 20%;"/>
        <asp:Button BackColor="Yellow" OnClick="Add_Monitor" Text="Add" runat="server" style="border-radius: 20%;"/>

        <asp:GridView ID="GridResults" runat="server" AutoGenerateColumns="false" Visible="false" AutoGenerateSelectButton="true" OnSelectedIndexChanged="Populate_User">
                <Columns>
            <asp:BoundField DataField="userID" HeaderText="Student ID" />
            <asp:BoundField DataField="userFName" HeaderText="First Name" />
            <asp:BoundField DataField="userLName" HeaderText="Last Name" />
            </Columns>
        </asp:GridView>
    </div>
    <asp:Button BackColor="Yellow" Text="Add new Monitor" runat="server" style="border-radius: 20%;"/>
    
</div>

</asp:Content>