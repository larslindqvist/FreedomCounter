using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Media;

namespace FreedomCounter
{
    public class DatabaseHandler
    {
        public DatabaseHandler()
        {
            using (var cmd = new SQLiteCommand(GetDbConnection()))
            {
                cmd.CommandText = @"CREATE TABLE IF NOT EXISTS logs(id INTEGER PRIMARY KEY, name TEXT, created TEXT, workhours TEXT)";
                cmd.ExecuteNonQuery();
            }
        }

        public SQLiteConnection GetDbConnection()
        {
            var dir = Directory.GetCurrentDirectory(); //TODO: Put this dir in settings maybe, or a better path.
            string url = dir + @"\freedom.db3";
            var con = new SQLiteConnection();
            con.ConnectionString = $"Data Source = {url} ; Version = 3";
            con.Open();
            return con;
        }
        public void InsertTime(string name, DateTime created)
        {
            using (var cmd = new SQLiteCommand(GetDbConnection()))
            {
                cmd.CommandText = "INSERT INTO logs(name, created) VALUES (@name, @date)";
                cmd.Parameters.AddWithValue("@name", name);
                cmd.Parameters.AddWithValue("@date", created);
                cmd.ExecuteNonQuery();
            }
        }

        public void UpdateWorkhours(string name, DateTime workhours)
        {
            using (var cmd = new SQLiteCommand(GetDbConnection()))
            {
                cmd.CommandText = "UPDATE logs SET workhours = @workhours WHERE name = @name";
                cmd.Parameters.AddWithValue("@name", name);
                cmd.Parameters.AddWithValue("@workhours", workhours);
                cmd.ExecuteNonQuery();
            }
        }

        public bool IsFirstStartToday()
        {
            using (var cmd = new SQLiteCommand(GetDbConnection()))
            {
                cmd.CommandText = @"SELECT created FROM logs WHERE created >= date('now', 'start of day')";
                SQLiteDataReader r = cmd.ExecuteReader();
                if (r.HasRows)
                    return false;
                return true;
            }
            
        }

        public DateTime GetFirstStartToday()
        {
            using (var cmd = new SQLiteCommand(GetDbConnection()))
            {
                List<DateTime> dates = new List<DateTime>();
                cmd.CommandText = @"SELECT created FROM logs WHERE created >= date('now', 'start of day')";
                cmd.CommandType = CommandType.Text;
                SQLiteDataReader r = cmd.ExecuteReader();
                while (r.Read())
                {
                    DateTime date = Convert.ToDateTime(r["created"]);
                    dates.Add(date);
                }
                return dates.Min();
            }
        }
    }
}
