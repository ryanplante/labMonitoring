<%@ Page Language="C#" Title="Calendar View" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="Calendar.aspx.cs" Inherits="labMonitor.Calendar" %>



<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
<script type='text/javascript'>
    // Get all anchor elements with the class of "nav-link"
    var navLinks = document.querySelectorAll('.nav-link');
    if (isEdited) {
        // Attach a click event listener to each nav link
        navLinks.forEach(function (link) {
            link.addEventListener('click', function (event) {

                // Display a confirmation message to the user
                var confirmed = confirm("Are you sure you want to leave this page?");
                // If the user clicks "OK", continue with the link action
                if (confirmed) {
                    return true;
                }

                // Otherwise, prevent the link action from happening
                event.preventDefault();
                return false;
            });
        });
    }

</script>
<div>
    <%--Show when user is department head or admin --%>
    <input type="hidden" id="isEdited" runat="server" />
    <h1>Calendar</h1>
    <asp:DataGrid id="ScheduleGrid" runat="server" AutoGenerateColumns="false" AutoPostBack="false" OnItemCommand="OnSelectedCell" >
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
