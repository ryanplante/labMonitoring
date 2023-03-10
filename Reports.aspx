<%@ Page Language="C#" Title="Reports" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="Reports.aspx.cs" Inherits="labMonitor.Reports" %>


<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
<div>
    <h1>Report</h1>
    <h2 runat="server" id="welcome"></h2>
    
    <%--Show when user is department head or admin --%>
    <div id="head" runat="server">
        <div class="search" id="search" runat="server">
            <div class="nextto" id="selectbox" >
                <asp:DropDownList id="schsel" runat="server" AutoPostBack = "true"  OnSelectedIndexChanged = "OnSelectedIndexChanged">
                    <asp:listitem Text="Day"     Value="0">Day</asp:listitem>
                    <asp:listitem Text="Week"    Value="1">Week</asp:listitem>
                    <asp:listitem Text="Month"   Value="2">Month</asp:listitem>
                    <asp:listitem Text="Term"    Value="3">Term</asp:listitem>
                    <asp:listitem Text="All"     Value="4">All</asp:listitem>
                </asp:DropDownList>
            </div>

            <div class="nextto" id="datebox"  >
                    <asp:TextBox id="dtpc"   runat="server" class="dtpc"    Value="0"    Type="Date"   Textmode="Date"     ReadOnly = "false" Visible="true"  placeholder="mm/dd/yyyy"></asp:TextBox>  <%-- Day --%> 
                    <asp:TextBox id="wkpc"   runat="server" class="wkpc"    Value="1"      Type="Date"   Textmode="Week"     ReadOnly = "false" Visible="false"></asp:TextBox> <%--Week--%>
                    <asp:TextBox id="mtpc"   runat="server" class="mtpc"    Value="2"      Type="Date"   Textmode="Month"    ReadOnly = "false" Visible="false"></asp:TextBox> <%--Month--%>
                    <asp:TextBox id="stday"  runat="server" class="stday"   Value="3"     Type="Date"   Textmode="Date"     ReadOnly = "false" Visible="false">Start Day</asp:TextBox> <%-- Start of Term  --%>
                    <asp:TextBox id="endday" runat="server" class="endday"  Value="4"    Type="Date"   Textmode="Date"     ReadOnly = "false" Visible="false">End Day</asp:TextBox> <%-- End of Term  --%>
                </div>


            <div class="nextto" id="filetbox">
                <asp:DropDownList id="dpfltp" runat="server">
                    <asp:listitem class="myfile" id="pdf">PDF</asp:listitem>
                    <asp:listitem class="myfile" id="csv">CSV</asp:listitem>
                    <asp:listitem class="myfile" id="xlxs">XLXS</asp:listitem>
                </asp:DropDownList>
            </div>

            <div>
                <asp:Button runat="server" class="button" Text="Download"      Visible="true" OnClick="ExportData" />

            </div>

            <asp:Label ID="lblError" runat="server" Text=""></asp:Label>
        </div>


<asp:Chart ID="Chart1" runat="server" Width="1300px" Height="600px" BackColor="WhiteSmoke" BorderlineDashStyle="Solid" BorderlineColor="#C6C6C6" BorderlineWidth="3">
    <Titles>
        <asp:Title Font="Tahoma, 24pt, style=Bold" ForeColor="#333333" Text="Student Check In/Out Report"></asp:Title>
    </Titles>
    <Series>
        <asp:Series Name="Series1" ChartType="Column"></asp:Series>
    </Series>
    <ChartAreas>
        <asp:ChartArea Name="ChartArea1">
            <AxisX Title="Time"></AxisX>
            <AxisY Title="Count"></AxisY>
        </asp:ChartArea>
    </ChartAreas>
</asp:Chart>




    </div>

 
</div>

</asp:Content>

