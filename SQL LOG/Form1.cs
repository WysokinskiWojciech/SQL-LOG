using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace SQL_LOG
{
    public partial class Form1 : Form
    {
        bool stan = false;
        public Form1()
        {
            
            InitializeComponent();

        }

        public void Connect_Click(object sender, EventArgs e)
        {
            try
            {
                SqlConnection  polaczenie = new SqlConnection(@"Data source=" + IP.Text + @"\" +Port.Text + ";" + "database=" + DBName.Text + ";" + "User id=" + User.Text + ";" + "Password=" + Password.Text+";");
                polaczenie.Open();
                MessageBox.Show("Połączono z serwerem!");
                stan = true;
                polaczenie.Close();
            }
            catch {
                MessageBox.Show("Nie połączono z serwerem! Sprawdź dane logowania");
                stan = false;
            }
        }



        private void button1_Click(object sender, EventArgs e)
        {
            string a;
            int b;
            string line = "";
            if (stan == true)
            {
                try
                {
                    SqlConnection connection = new SqlConnection(@"Data source=" + IP.Text + @"\" + Port.Text + ";" + "database=" + DBName.Text + ";" + "User id=" + User.Text + ";" + "Password=" + Password.Text + ";");
                    string query = "SELECT COUNT(*)  FROM INFORMATION_SCHEMA.COLUMNS WHERE table_catalog = '" + DBName.Text + "' AND table_name = '" + Table.Text + "'";
                    string queryString = "SELECT * FROM dbo." + Table.Text + ";";
                    string column = "SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME ='" + Table.Text + "'";
                    SqlCommand command2 = new SqlCommand(column, connection);
                    SqlCommand command1 = new SqlCommand(query, connection);
                    SqlCommand command = new SqlCommand(queryString, connection);
                    connection.Open();
                    SqlDataReader reader1 = command1.ExecuteReader();
                    reader1.Read();
                    a = reader1[0].ToString();
                    b = Convert.ToInt32(a);
                    reader1.Close();
                    SqlDataReader reader2 = command2.ExecuteReader();

                    using (System.IO.StreamWriter writer = new System.IO.StreamWriter(FileName.Text, false))
                    {
                        while (reader2.Read())
                        {                     
                            line += reader2[0].ToString();
                            line += ";";
                        }
                        writer.WriteLine(line);
                    
                    line = "";
                    reader2.Close();
                    SqlDataReader reader = command.ExecuteReader();

                    
                        while (reader.Read())
                        {
                            line = "";
                            for (int i = 0; i < b; i++)
                            {
                                line += reader[i].ToString();
                                line += ";";
                            }
                            writer.WriteLine(line);
                        }
                        reader.Close();

                    }
                                      
                    connection.Close();
                    MessageBox.Show("Plik zapisano!");
                }
                catch
                {
                    MessageBox.Show("Brak połączenia!. Sprawdź poprawność pola Table ");
                }
            }
            else {
                MessageBox.Show("Sprawdź połączenie!. Wciśnij Connection Test!");
            }
            
        }
    } 
}
