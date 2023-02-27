<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="labMonitor._Default" %>
<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"  
    Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %> 



<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

<div>
    <h2 runat="server" id="welcome"></h2>
    <%--Show when user is department head or admin --%>
    <div id="head" runat="server">
       <asp:Chart ID="Chart1" runat="server" BackColor="128, 64, 0" BackGradientStyle="LeftRight"  
        BorderlineWidth="0" Height="340px" Palette="None" PaletteCustomColors="64, 0, 0"  
        Width="360px" BorderlineColor="192, 64, 0">  
        <Series>  
            <asp:Series Name="Series1" YValuesPerPoint="6">
            </asp:Series>  
        </Series>  
        <ChartAreas>  
            <asp:ChartArea Name="ChartArea1">  
            </asp:ChartArea>  
        </ChartAreas>  
    </asp:Chart> 
    </div>
    <div id="monitor" runat="server">

    </div>

            <%-- Head View--%>
           <div class="headview" >
               <h1></h1>

           </div>
           
           <%-- Admin View--%>
           <div class="adminview">
               <h1></h1>

           </div>

           <%-- Monitor View--%>
           <div class="monitorview" runat="server" visible="false">
                <h3>Unlogged Students</h3>

           </div>

               <%-- Student View--%>
           <div class="studentview" runat="server" id="studentview" visible="false">
               <h1>Dashboard</h1>
            <%labMonitor.Models.LabDAL labFactory = new labMonitor.Models.LabDAL(); %>
            <%labMonitor.Models.ScheduleDAL scheduleFactory = new labMonitor.Models.ScheduleDAL(); %>
            <% List<labMonitor.Models.Lab> labs = labFactory.GetAllLabs(); %>
            <% foreach (labMonitor.Models.Lab lab in labs) { %>
                <div class="labcard" id="<%= lab.labRoom %>">
                    <div class="htags">
                        <h3 class="lbn"><%= lab.labName %></h3>
                        <h3 class="rn"><%= lab.labRoom %></h3>
                    </div>

                    <div class="cardcontent" runat="server">
                        <asp:Literal ID="scheduleDiv" runat="server"></asp:Literal>

                        <div class="imgbk">
                            <img src="images/image 39.png" />
                        </div>
                    </div>
                </div>

                <% string schedule = scheduleFactory.GetDeptSchedule(lab.deptID); %>
                <% Literal scheduleDiv = (Literal)Page.FindControl("scheduleDiv"); %>
                <% if (scheduleDiv != null) { scheduleDiv.Text = schedule; } %>
            <% } %>
               </div>
               </div>





    </div>
</asp:Content>
