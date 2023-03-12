<%@ Page Language="C#" Title="Calendar View" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="Calendar.aspx.cs" Inherits="labMonitor.Calendar" %>



<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
<div>
    <%--Show when user is department head or admin --%>
    <p runat="server" style="display:none;" id="permission"></p>
    <h1>Calendar</h1>
    <asp:HiddenField ID="isEdited" runat="server"/>
    <asp:DataGrid id="ScheduleGrid" runat="server" AutoGenerateColumns="false" AutoPostBack="false" OnItemCommand="OnSelectedCell" >
        <Columns>
            <asp:BoundColumn HeaderText="Student Name" DataField="studentName" />
        <asp:TemplateColumn HeaderText="Sunday" >
            <ItemTemplate>
                <asp:LinkButton runat="server" Text='<%# Eval("Sunday") %>' CommandName="GetCellValue" CommandArgument='<%# Container.ItemIndex + ",0"%>' />
            </ItemTemplate>
        </asp:TemplateColumn>
        <asp:TemplateColumn HeaderText="Monday">
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
        <label for="checkRepeat">Repeat schedule for work week</label>
           <asp:CheckBox runat="server" ID="checkRepeat" />
           <br />
        <asp:Button BackColor="Yellow" OnClick="Submit" Text="Submit" runat="server" style="border-radius: 5%;"/>
        <asp:Button BackColor="Yellow" OnClick="Remove" Text="Remove" runat="server" style="border-radius: 5%;"/>
    </div>
    
</div>
    <script type='text/javascript'>
        window.onload = function () {
            setTdBackgroundColor();
        }

        var privLevel = Number(document.querySelector('#MainContent_permission').innerText)

        if (privLevel < 2) {
            tblLinks = document.querySelectorAll(`td a`)
            for (let i = 0; i < tblLinks.length; i++) {
                tblLinks[i].outerHTML = tblLinks[i].outerHTML.replace("a", "p")
            }
        }

        // Get all anchor elements with the class of "nav-link"
        // Get all anchor elements with the class of "nav-link"
        var navLinks = document.querySelectorAll('li');
        let isEdited = document.getElementById('<%= isEdited.ClientID %>');
        if (isEdited && isEdited.value === 'true') {
            // Attach a click event listener to each nav link
            navLinks.forEach(function (link) {
                link.addEventListener('click', function (event) {
                    // Display a confirmation message to the user
                    var confirmed = confirm("Are you sure you want to leave this page?\nUnsaved changes will be lost.");
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


        function setTdBackgroundColor() {
            const tds = document.querySelectorAll('td');
            tds.forEach(td => {
                if (td.querySelector('a') && td.querySelector('a').textContent === 'off') {
                    td.style.backgroundColor = '#D3D3D3';
                }
                else if (td.querySelector('p') && td.querySelector('p').textContent === 'off') {
                    td.style.backgroundColor = '#D3D3D3';

                }
            });
        }
    </script>
</asp:Content>
