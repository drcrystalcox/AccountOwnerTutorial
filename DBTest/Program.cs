using System;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace DBTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            int x;
            x = 10;
            x = x+2;
            Console.WriteLine(x);

            string connStr="server=localhost;user=root;database=accountowner;port=3306;password=cscipass";
            MySqlConnection conn = new MySqlConnection(connStr);
            try {
                conn.Open();
                string query = "Select * from owner";
                MySqlCommand cmd = new MySqlCommand(query,conn);

                MySqlDataReader rdr = cmd.ExecuteReader();

                while(rdr.Read()) {
                    Console.WriteLine(rdr[0]+"--"+rdr[1]+"--"+rdr[2]+"--"+rdr[3]);
                }
                rdr.Close();
                string insert = "INSERT INTO Owner (OwnerId, Name, DateOfBirth,Address) VALUES ('24fd81f8-d58a-4bcc-9f35-dc6cd5641901','Claire Ellington','1990-04-01','123 Main St')";
                MySqlCommand cmd2 = new MySqlCommand(insert,conn);
                cmd2.ExecuteNonQuery();


            }
            catch(Exception e) { Console.WriteLine(e.ToString());}
            conn.Close();

        }
    }
}
