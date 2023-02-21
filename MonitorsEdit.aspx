<%@ Page Language="C#" Title="Lab Monitors" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="MonitorsEdit.aspx.cs" Inherits="labMonitor.MonitorsEdit" %>




<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

<div>
    <%--Show when user is department head or admin --%>
    <h2 runat="server" id="welcome">Lab Monitors</h2>
    <h3>Current Monitors</h3>
    <div class="monitor" runat="server">
        <asp:GridView ID="DGLabMonitors" runat="server" AutoGenerateColumns="false">
        <Columns>
            <asp:BoundField DataField="userID" HeaderText="Student ID" />
            <asp:BoundField DataField="userFName" HeaderText="First Name" />
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:HyperLink ImageUrl="~/images/x.png" HeaderText="X Icon" 
                                   DataNavigateUrlFields="userID"
                                   DataNavigateUrlFormatString="~/RemoveUser.aspx?userID={0}" 
                                   runat="server">
                    </asp:HyperLink>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
    </div>
    <div class="MonitorForm" runat="server">
        <h3>Add Lab Monitor</h3>
        <label for="txtStudentID">Student ID</label>
        <br />
        <asp:textbox runat="server" id="txtAutoComplete" cssclass="autosuggest"></asp:textbox>
        <br />
        <label for="txtStudentFirst">Student First Name</label>
        <br />
        <asp:TextBox ID="txtStudentFirst" runat="server"></asp:TextBox>
        <br />
        <label for="txtStudentLast">Student Last Name</label>
        <br />
        <asp:TextBox ID="txtStudentLast" runat="server"></asp:TextBox>
        <br />
        <asp:Button BackColor="Yellow" OnClick="Search_Users" Text="Search" runat="server" style="border-radius: 20%;"/>
        <asp:GridView ID="GridResults" runat="server" AutoGenerateColumns="false" Visible="false">
                <Columns>
            <asp:BoundField DataField="userID" HeaderText="Student ID" />
            <asp:BoundField DataField="userFName" HeaderText="First Name" />
            <asp:BoundField DataField="userLName" HeaderText="Last Name" />
            </Columns>
        </asp:GridView>
    </div>
    <asp:Button BackColor="Yellow" OnClick="Unnamed_Click" Text="Add new Monitor" runat="server" style="border-radius: 20%;"/>
</div>

</asp:Content>