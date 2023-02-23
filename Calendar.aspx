<%@ Page Language="C#" Title="Calendar View" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="Calendar.aspx.cs" Inherits="labMonitor.Calendar" %>



<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

<script type="text/javascript">
    function getCellIndex(linkButton) {
        var cell = linkButton.parentNode;
        var row = cell.parentNode;
        var rowIndex = row.rowIndex - 1; //subtract header row
        var colIndex = cell.cellIndex;
        var cellIndex = rowIndex + "," + colIndex;
        alert(cellIndex);
        linkButton.setAttributeNS(null, "CommandArgument", cellIndex);
    }
</script>
<div>
    <%--Show when user is department head or admin --%>
    <h1>Calendar</h1>
    <asp:DataGrid id="ScheduleGrid" runat="server" AutoGenerateColumns="false" AutoPostBack="false" OnItemCommand="DataGrid1_ItemCommand">
        <Columns>
            <asp:BoundColumn HeaderText="Student Name" DataField="studentName" />
        <asp:TemplateColumn HeaderText="Sunday" >
            <ItemTemplate>
                <asp:LinkButton runat="server" Text='<%# Eval("Sunday") %>' CommandName="GetCellValue" CommandArgument='<%# Container.ItemIndex + ",0"%>' />
            </ItemTemplate>
        </asp:TemplateColumn>
        <asp:TemplateColumn HeaderText="Monday" >
            <ItemTemplate>
                <asp:LinkButton runat="server" Text='<%# Eval("Monday") %>' CommandName="GetCellValue" CommandArgument='<%# Container.ItemIndex + ",1"%>' />
            </ItemTemplate>
        </asp:TemplateColumn>
        <asp:TemplateColumn HeaderText="Tuesday" >
            <ItemTemplate>
                <asp:LinkButton runat="server" Text='<%# Eval("Tuesday") %>' CommandName="GetCellValue" CommandArgument='<%# Container.ItemIndex + ",2"%>' />
            </ItemTemplate>
        </asp:TemplateColumn>
        <asp:TemplateColumn HeaderText="Wednesday" >
            <ItemTemplate>
                <asp:LinkButton runat="server" Text='<%# Eval("Wednesday") %>' CommandName="GetCellValue" CommandArgument='<%# Container.ItemIndex + ",3"%>' />
            </ItemTemplate>
        </asp:TemplateColumn>
        <asp:TemplateColumn HeaderText="Thursday" >
            <ItemTemplate>
                <asp:LinkButton runat="server" Text='<%# Eval("Thursday") %>' CommandName="GetCellValue" CommandArgument='<%# Container.ItemIndex + ",4"%>' />
            </ItemTemplate>
        </asp:TemplateColumn>
        <asp:TemplateColumn HeaderText="Friday" >
            <ItemTemplate>
                <asp:LinkButton runat="server" Text='<%# Eval("Friday") %>' CommandName="GetCellValue" CommandArgument='<%# Container.ItemIndex + ",5"%>' />
            </ItemTemplate>
        </asp:TemplateColumn>
        <asp:TemplateColumn HeaderText="Saturday" >
            <ItemTemplate>
                <asp:LinkButton runat="server" Text='<%# Eval("Saturday") %>' CommandName="GetCellValue" CommandArgument='<%# Container.ItemIndex + ",6"%>' />
            </ItemTemplate>
        </asp:TemplateColumn>
        </Columns>
       </asp:DataGrid>
        <asp:Button BackColor="Yellow" OnClick="Publish" Text="Publish" runat="server" style="border-radius: 5%;"/>
       <div class="MonitorForm" runat="server" id="ScheduleForm" visible="false">
        <h2>Edit Schedule</h2>
        <asp:HiddenField ID="coords" runat="server" />
        <h3 id="lblStudent" runat="server">FirstName LastName</h3>
        <h3 id="lblDay" runat="server">Schedule for:</h3>
        <label for="start">Start time:</label>
        <input type="time" id="start" name="start"
               required runat="server">
        <br />
        <label for="end">End time:</label>
        <input type="time" id="end" name="end"
               required runat="server">
        <br />
        <asp:Label ID="lblWarning" CssClass="warning" runat="server" Visible="false"></asp:Label>
        <br />
        <asp:Button BackColor="Yellow" OnClick="Submit" Text="Submit" runat="server" style="border-radius: 5%;"/>
        <asp:Button BackColor="Yellow" OnClick="Remove" Text="Remove" runat="server" style="border-radius: 5%;"/>
    </div>
    
</div>

</asp:Content>
