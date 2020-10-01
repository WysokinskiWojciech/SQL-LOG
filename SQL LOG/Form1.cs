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
            try//definiowanie połączenia
            {
                SqlConnection  polaczenie = new SqlConnection(@"Data source=" + IP.Text + @"\" +Port.Text + ";" + "database=" + DBName.Text + ";" + "User id=" + User.Text + ";" + "Password=" + Password.Text+";");//inicjalizacja połączenia z bazą danych
                polaczenie.Open();//
                MessageBox.Show("Połączono z serwerem!");
                stan = true;//stan połączenia testowego
                polaczenie.Close();//zamknięcie połączenia
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
            if (stan == true)//jeżeli test połączenia się udał
            {
                try
                {
                    SqlConnection connection = new SqlConnection(@"Data source=" + IP.Text + @"\" + Port.Text + ";" + "database=" + DBName.Text + ";" + "User id=" + User.Text + ";" + "Password=" + Password.Text + ";");//definiowanie połączenia
                    string query = "SELECT COUNT(*)  FROM INFORMATION_SCHEMA.COLUMNS WHERE table_catalog = '" + DBName.Text + "' AND table_name = '" + Table.Text + "'";//zapytanie sql do bazy danych by wzróciła liczbę kolumn
                    string queryString = "SELECT * FROM dbo." + Table.Text + ";";//zapytanie sql do bazy danych by wzróciła wszystkie dane z tabeli
                    string column = "SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME ='" + Table.Text + "'";//zapytanie sql do bazy danych by wzróciła nazwę kolumn
                    
                    SqlCommand command2 = new SqlCommand(column, connection);//definiowanie zapytania 
                    SqlCommand command1 = new SqlCommand(query, connection);//
                    SqlCommand command = new SqlCommand(queryString, connection);//

                    connection.Open();//otwarcie połączenia

                    SqlDataReader reader1 = command1.ExecuteReader();//wykonanie zapytania
                    reader1.Read();//odczyt danych wzróconych z zapytania
                    a = reader1[0].ToString();//odczyt liczby kolumn
                    b = Convert.ToInt32(a);
                    reader1.Close();//zamknięcie zapytania

                    SqlDataReader reader2 = command2.ExecuteReader();//wykonanie zapytania dot. nazwy kolumn

                    using (System.IO.StreamWriter writer = new System.IO.StreamWriter(FileName.Text, false))
                    {
                        while (reader2.Read())
                        {                     
                            line += reader2[0].ToString();//odczyt nazwy kolumn
                            line += ";";
                        }
                        writer.WriteLine(line);//zapis do pliku
                    
                    line = "";
                    reader2.Close();//zamknięcie odczytu
                    SqlDataReader reader = command.ExecuteReader();//odczyt danych z tabeli bazy

                    
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
                        reader.Close();//zamknięcie odczytu

                    }
                                      
                    connection.Close();//zamknięcie połączenia
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
