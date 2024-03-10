using System;
using System.Globalization;

namespace labMonitor
{
    public static class Helpers
    {
        public static string GetConnected()
        {
            return "Server=sql.neit.edu\\studentsqlserver,4500; Database=SE265_LabMonitorProj; User Id=SE265_LabMonitorProj;Password=FaridRyanSpencer;";
        }

        public static string FormatOperatingHours(string operatingHours)
        {
            if (operatingHours == "off")
                return "off";
            string[] hours = operatingHours.Split('-');
            DateTime startTime = DateTime.ParseExact(hours[0], "HH:mm", CultureInfo.InvariantCulture);
            DateTime endTime = DateTime.ParseExact(hours[1], "HH:mm", CultureInfo.InvariantCulture);
            string formattedStartTime = startTime.ToString("h:mm tt", CultureInfo.InvariantCulture);
            string formattedEndTime = endTime.ToString("h:mm tt", CultureInfo.InvariantCulture);
            return $"{formattedStartTime}-{formattedEndTime}";
        }

        public static void LogError(Exception e)
        {
            // It would be better to log the error to a database but this is sufficent enough as the admin can just view the console.
            Console.WriteLine($"{DateTime.Now}: Error: {e.Message}");
            throw e;
        }
    }

}

