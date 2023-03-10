using labMonitor.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Globalization;
using System.Web.UI.DataVisualization.Charting;
using System.Drawing;
using OfficeOpenXml;

namespace labMonitor
{
    public partial class Reports : System.Web.UI.Page
    {
        LogDAL factory = new LogDAL();
        User user;
        private string GetConnected()
        {
            return "Server= sql.neit.edu\\studentsqlserver,4500; Database=SE265_LabMonitorProj; User Id=SE265_LabMonitorProj;Password=FaridRyanSpencer;";
        }

        private string Filept()
        {
            string downloadsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string filePath = Path.Combine(downloadsPath, "myfile.csv");

            return filePath;
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            user = Session["User"] as labMonitor.Models.User;
            // Connection string for your database
            string connectionString = GetConnected();

            // SQL query to select data from table
            string query = "SELECT DISTINCT YEAR(timein) AS Year FROM Log ORDER BY Year ASC";
            string countQuery = "SELECT YEAR(timein) AS Year, COUNT(DISTINCT studentID) AS 'Student Count' FROM Log WHERE timein IS NOT NULL GROUP BY YEAR(timein)";

            // Create SQL connection and command objects
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);

                // Create SqlDataAdapter and DataTable
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable dataTable = new DataTable();

                // Fill DataTable with data from SQL
                adapter.Fill(dataTable);

                // Get the min and max year values
                int minYear = Convert.ToInt32(dataTable.Rows[0]["Year"]);
                int maxYear = Convert.ToInt32(dataTable.Rows[dataTable.Rows.Count - 1]["Year"]);

                // Create a new DataTable to hold the final results
                DataTable finalTable = new DataTable();
                finalTable.Columns.Add("Year", typeof(int));
                finalTable.Columns.Add("Student Count", typeof(int));

                // Create SQL command to get student count for each year
                command = new SqlCommand(countQuery, connection);
                adapter.SelectCommand = command;
                dataTable.Clear();
                adapter.Fill(dataTable);

                // Fill in the missing years with 0 counts
                for (int year = minYear; year <= maxYear; year++)
                {
                    DataRow[] rows = dataTable.Select("Year = " + year);
                    int count = 0;
                    if (rows.Length > 0)
                    {
                        count = Convert.ToInt32(rows[0]["Student Count"]);
                    }
                    finalTable.Rows.Add(year, count);
                }

                // Bind chart to DataTable
                Chart1.DataSource = finalTable;
                Chart1.DataBind();

                // Set ChartArea background color and border style
                Chart1.ChartAreas[0].BackColor = Color.White;
                Chart1.ChartAreas[0].BorderDashStyle = ChartDashStyle.Solid;
                Chart1.ChartAreas[0].BorderColor = Color.Black;

                // Set Series color, border width, and label format
                Chart1.Series[0].Color = Color.Red;
                Chart1.Series[0].BorderWidth = 5;
                Chart1.Series[0].LabelFormat = "#";

                // Set XValueMember and YValueMember for the series
                Chart1.Series[0].XValueMember = "Year";
                Chart1.Series[0].YValueMembers = "Student Count";

                // Format AxisX to show all years and display total number of people
                Chart1.ChartAreas[0].AxisX.Minimum = minYear;
                Chart1.ChartAreas[0].AxisX.Maximum = maxYear;
                Chart1.ChartAreas[0].AxisX.IntervalType = DateTimeIntervalType.Years;
                Chart1.ChartAreas[0].AxisX.LabelStyle.Format = "yyyy";
                Chart1.ChartAreas[0].AxisX.Interval = 1;
                Chart1.ChartAreas[0].AxisX.Title = "Year";
                Chart1.ChartAreas[0].AxisX.TitleFont = new Font("Arial", 18, FontStyle.Bold);
                Chart1.ChartAreas[0].AxisX.TitleForeColor = Color.Black;

                // Format AxisY to display number of people
                Chart1.ChartAreas[0].AxisY.Title = "Number of People";
                Chart1.ChartAreas[0].AxisY.TitleFont = new Font("Arial", 18, FontStyle.Bold);
                Chart1.ChartAreas[0].AxisY.TitleForeColor = Color.Black;




            }

        }







        protected void Chart1_Load(object sender, EventArgs e)
        {

        }

        protected void OnSelectedIndexChanged(object sender, EventArgs e)
        {

            switch (schsel.SelectedIndex)
            {
                case 0: // Day
                    dtpc.Visible = true;
                    wkpc.Visible = false;
                    mtpc.Visible = false;
                    stday.Visible = false;
                    endday.Visible = false;

                    break;
                case 1: // Week
                    dtpc.Visible = false;
                    wkpc.Visible = true;
                    mtpc.Visible = false;
                    stday.Visible = false;
                    endday.Visible = false;

                    break;
                case 2: // Month
                    dtpc.Visible = false;
                    wkpc.Visible = false;
                    mtpc.Visible = true;
                    stday.Visible = false;
                    endday.Visible = false;
                    break;
                case 3: // Term
                    dtpc.Visible = false;
                    wkpc.Visible = false;
                    mtpc.Visible = false;
                    stday.Visible = true;
                    endday.Visible = true;

                    break;
                case 4: // All
                    dtpc.Visible = false;
                    wkpc.Visible = false;
                    mtpc.Visible = false;
                    stday.Visible = false;
                    endday.Visible = false;
                    break;
                default:
                    dtpc.Visible = false;
                    wkpc.Visible = false;
                    mtpc.Visible = false;
                    stday.Visible = false;
                    endday.Visible = false;
                    break;
            }
        }

        protected void ExportData(object sender, EventArgs e)
        {
            List<Log> logs = new List<Log>();
            // File path for CSV file
            string filePath = Filept();
            // DT oject to store dates
            DateTime dt = new DateTime();
            switch (schsel.SelectedIndex)
            {
                // Day
                case 0:
                    // Get selected date from input
                    dt = DateTime.ParseExact(dtpc.Text, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                    logs = factory.GetLogsBetween(dt, dt.Date.AddDays(1), user.userDept);

                    break;
                // Week
                case 1:
                    // Set the parameters for the start and end of the week
                    String[] weekText = wkpc.Text.Split('-');
                    if (DateTime.TryParseExact(weekText[0], "yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dt))
                    {
                        int weeksToAdvance = Int32.Parse(weekText[1].Replace("W", ""));
                        logs = factory.GetLogsBetween(dt, dt.AddDays(weeksToAdvance * 7), user.userDept);
                    }
                    break;
                // Month
                case 2:
                    dt = DateTime.ParseExact(mtpc.Text, "yyyy-MM", CultureInfo.InvariantCulture);
                    logs = factory.GetLogsBetween(dt, dt.AddMonths(1).AddDays(-1), user.userDept);
                    break;
                // Term
                case 3:
                    dt = DateTime.ParseExact(stday.Text, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                    DateTime end = DateTime.ParseExact(endday.Text, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                    logs = factory.GetLogsBetween(dt, end, user.userDept);
                    break;
                // All
                default:
                    logs = factory.GetAllLogs(user.userDept);
                    break;
            }
            switch (dpfltp.SelectedIndex)
            {
                // PDF
                case 0:
                    break;
                // CSV
                case 1:
                    // Create new file and open for writing
                    using (StreamWriter writer = new StreamWriter(filePath))
                    {
                        // Write column headers to CSV file
                        writer.WriteLine("studentID,timein,timeout,itemsBorrowed");

                        for (int i = 0; i < logs.Count; i++)
                        {
                            Log log = logs[i];
                            // Write values to CSV file
                            writer.WriteLine("{0},{1},{2},{3}", log.studentID, log.timeIn.ToString("MM/dd/yyyy HH:mm:ss"), log.timeOut.ToString("MM/dd/yyyy HH:mm:ss"), log.itemsBorrowed);
                        }
                        // Close CSV file
                        writer.Close();
                    }

                    // Set response headers
                    Response.Clear();
                    Response.ContentType = "text/csv";
                    Response.AddHeader("content-disposition", "attachment;filename=MyTable.csv");

                    // Write CSV file contents to response stream
                    byte[] fileBytes = File.ReadAllBytes(filePath);
                    Response.BinaryWrite(fileBytes);
                    Response.Flush();
                    Response.End();
                    break;
                // XLXS
                case 2:
                    // See: https://epplussoftware.com/developers/licenseexception
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    // Create a new Excel package
                    using (var package = new ExcelPackage())
                    {
                        // Create a new worksheet
                        var worksheet = package.Workbook.Worksheets.Add("Sheet1");

                        // Write the headers
                        worksheet.Cells[1, 1].Value = "studentID";
                        worksheet.Cells[1, 2].Value = "Time In";
                        worksheet.Cells[1, 3].Value = "Time Out";
                        worksheet.Cells[1, 4].Value = "Items Borrowed";

                        // Write the data
                        for (int i = 0; i < logs.Count; i++)
                        {
                            worksheet.Cells[i + 2, 1].Value = logs[i].studentID;
                            worksheet.Cells[i + 2, 2].Value = logs[i].timeIn.ToString("MM/dd/yyyy HH:mm:ss");
                            worksheet.Cells[i + 2, 3].Value = logs[i].timeOut.ToString("MM/dd/yyyy HH:mm:ss");
                            worksheet.Cells[i + 2, 4].Value = logs[i].itemsBorrowed;
                        }

                        // Save the file
                        var stream = new MemoryStream();
                        package.SaveAs(stream);
                        byte[] data = stream.ToArray();

                        // Download the file
                        Response.Clear();
                        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        Response.AddHeader("Content-Disposition", "attachment; filename=report.xlsx");
                        Response.BinaryWrite(data);
                        Response.End();
                    }
                    break;
                default:
                    break;
            }

        }
    }
}