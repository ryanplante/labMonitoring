<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="Admin.aspx.cs" Inherits="labMonitor.Admin"  %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div>

    </div>
    <h1>Admin</h1>
        <div class="labs-header">
          <h2>Current Labs</h2>
          <asp:Button BackColor="Orange" OnClick="ShowAddForm" Text="Add new lab" runat="server" style="border-radius: 5%;" />
        </div>
            <asp:GridView ID="DGLabs" runat="server" AutoGenerateColumns="false" OnRowCommand="Lab_Command" class="stTable" border="0">
        <Columns>
            <asp:BoundField DataField="labID" HeaderText="Lab ID" Visible="false" />
            <asp:BoundField DataField="labName" HeaderText="Lab Name" />
            <asp:BoundField DataField="labRoom" HeaderText="Room #" />
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:LinkButton ID="btnEdit" runat="server" CommandName="EditLab" CommandArgument='<%#Eval("labID")%>'>
                        <asp:Image ID="Pencil" runat="server" ImageUrl="/images/pencil.png" class="xbutton" style="border-width: 0px;" />
                    </asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:LinkButton ID="btnRemove" runat="server" CommandName="RemoveLab" CommandArgument='<%#Eval("labID")%>'
                                    OnClientClick="return confirm('Are you sure you want to remove this lab?');">
                        <asp:Image ID="X" runat="server" ImageUrl="/images/x.png" class="xbutton" style="border-width: 0px;" />
                    </asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
         <div class="MonitorForm" runat="server" id="labForm" visible="false">
             <asp:HiddenField ID="action" runat="server" />
             <asp:HiddenField ID="selectedID" runat="server" />
             <asp:HiddenField ID="labID" runat="server" />
            <h2 id="formHeader" runat="server">Lab</h2>
            <label for="txtLabName">Lab Name</label>
            <br />
             <asp:TextBox ID="txtLabName" runat="server" class="formfield"></asp:TextBox>
             <br />
            <asp:Label ID="lblNameWarning" CssClass="warning" runat="server" Visible="false"></asp:Label>
            <label for="txtRoom">Room #</label>
            <br />
             <asp:TextBox ID="txtRoom" runat="server" class="formfield"></asp:TextBox>
             <br />
            <asp:Label ID="lblRoomWarning" CssClass="warning" runat="server" Visible="false"></asp:Label>
            <label for="txtDept">Department Head</label>
            <br />
             <asp:TextBox ID="txtDept" runat="server" class="formfield"></asp:TextBox>
             <asp:ImageButton runat="server" ID="searchButton" ImageUrl="~/images/search.png" Height="37" Width="37" OnClick="SearchUsers" />
             <br />
            <asp:Label ID="lblHeadWarning" CssClass="warning" runat="server" Visible="false"></asp:Label>
            <asp:GridView ID="GridResults" runat="server" AutoGenerateColumns="false" Visible="false" AutoGenerateSelectButton="true" OnSelectedIndexChanged="Populate_User">
                    <Columns>
                <asp:BoundField DataField="userID" HeaderText="Employee ID" />
                <asp:BoundField DataField="userFName" HeaderText="First Name" />
                <asp:BoundField DataField="userLName" HeaderText="Last Name" />
                </Columns>
            </asp:GridView>
              <label for="comboDept">Department</label>
             <br />
             <asp:DropDownList runat="server" ID="comboDept" EnableViewState="true" ></asp:DropDownList>
             <br />
             <asp:Label ID="lblDeptWarning" CssClass="warning" runat="server" Visible="false"></asp:Label>
             <asp:Button runat="server" id="actionButton" Text='<%#action.Value.ToString() %>' BackColor="Orange" OnClick="actionButton_Click" />
        </div>

    </asp:Content>