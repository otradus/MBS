using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Net.Mail;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace MBS
{
    public static class App
    {
        public static void DoubleBuffered(this DataGridView dgv, bool setting)
        {
            Type dgvType = dgv.GetType();
            PropertyInfo pi = dgvType.GetProperty("DoubleBuffered",
                  BindingFlags.Instance | BindingFlags.NonPublic);
            pi.SetValue(dgv, setting, null);
        }

        public static string getConnectionString()
        {
            MySqlConnectionStringBuilder connstring = new MySqlConnectionStringBuilder();
            connstring.Server = Args.host;
            connstring.UserID = Args.username;
            connstring.Password = Args.password;
            connstring.Database = Args.database;

            return connstring.ToString();
        }


        public static DataTable executeReader(string query)

        {
            DataTable results = new DataTable("Results");
            using (MySqlConnection connection = new MySqlConnection(App.getConnectionString()))
            {
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Connection.Open();
                    command.ExecuteNonQuery();

                    using (MySqlDataReader reader = command.ExecuteReader())
                        results.Load(reader);
                }
            }
            return results;
        }

        public static void executeNonQuery(string query)
        {
            MySqlConnection conn = new MySqlConnection(getConnectionString());
            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            conn.Close();
        }


        public static object executeScalar(string query)
        {
            MySqlConnection conn = new MySqlConnection(getConnectionString());
            object result;
            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(query, conn);
                result = cmd.ExecuteScalar();
                //if (result != null){int r = Convert.ToInt32(result);Console.WriteLine("Number of countries in the World database is: " + r);}

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                result = null;
            }

            conn.Close();
            return result;
        }

        //        public static MySqlConnection conn = new MySqlConnection(getConnectionString());


        public static void loadTable(DataGridView dtv, string search)
        {
            dtv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
            dtv.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;

            MySqlConnection conn = new MySqlConnection(getConnectionString());
            MySqlCommand command1 = new MySqlCommand(search, conn);
            MySqlDataAdapter adapter = new MySqlDataAdapter(command1);
            DataTable dt = new DataTable();
            adapter.Fill(dt);

            //    foreach (DataRow dr in dt.Rows)
            //  {
            //    dr["Harga"] = ;
            //}

            dtv.DataSource = dt;
            if (dtv.Columns["Harga"] != null) { dtv.Columns["Harga"].DefaultCellStyle.Format = "c"; }
            if (dtv.Columns["HargaBeli"] != null) { dtv.Columns["HargaBeli"].DefaultCellStyle.Format = "c"; }
            if (dtv.Columns["HargaJual"] != null) { dtv.Columns["HargaJual"].DefaultCellStyle.Format = "c"; }
            if (dtv.Columns["Subtotal"] != null) { dtv.Columns["Subtotal"].DefaultCellStyle.Format = "c"; }
            //           dtv.Columns["Harga"].DefaultCellStyle.Format = "c";
            //         dtv.Columns["HargaBeli"].DefaultCellStyle.Format = "c";

            //dtv.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);

            dtv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dtv.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
        }

        public static void loadComboBox(ComboBox cmbx, string search)
        {
            MySqlConnection conn = new MySqlConnection(getConnectionString());
            MySqlCommand command1 = new MySqlCommand(search, conn);
            MySqlDataAdapter adapter = new MySqlDataAdapter(command1);
            DataTable dt = new DataTable();
            adapter.Fill(dt);

            foreach (DataRow dr in dt.Rows)
            {
                cmbx.Items.Add(dr["Nama"]);
            }

            //            cmbx.DataSource = dt;
        }

        public static string mysqlcurrency(string str)
        {
            return "CONCAT('Rp', FORMAT(" + str + ", 0))";
        }

        public static string strtomoney(string str)
        {
            //return "Rp" + str.Replace(",", ".");
            try
            {
                return String.Format("{0:C0}", Convert.ToInt32(str));

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return "";
            }
        }

        public static double moneytodouble(string str)
        {
            try
            {
                str = str.Replace("Rp", "");
                str = str.Replace(".", "");
                return Convert.ToDouble(str);
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.ToString());
                return 0;
            }
        }

        public static string doubletomoney(object amount)
        {
            string money = Convert.ToString(amount);
            return strtomoney(money);
        }

        public static decimal moneytodecimal(string str)
        {
            try
            {
                str = str.Replace("Rp", "");
                str = str.Replace(".", "");
                return Convert.ToDecimal(str);
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.ToString());
                return 0;
            }
        }

        public static string decimaltomoney(object amount)
        {
            string money = Convert.ToString(amount);
            return strtomoney(money);
        }


        public static void formatDataGridView(DataGridView dgv)
        {
            dgv.MultiSelect = false;
            dgv.AllowUserToAddRows = false;
            dgv.AllowUserToDeleteRows = false;
            dgv.AllowUserToOrderColumns = false;
            dgv.AllowUserToResizeColumns = false;
            dgv.AllowUserToResizeRows = false;
            //dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            //dgv.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dgv.ReadOnly = true;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.RowHeadersVisible = false;
        }

        public static void autoSizeDataGridView(DataGridView dgv)
        {
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            if (dgv.Columns.Count > 0)
            {
                dgv.Columns[dgv.Columns.Count - 1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }
        }

        public static int cInt(object obj)
        {
            return Convert.ToInt32(obj);
        }

        public static double cDouble(object obj)
        {
            return Convert.ToDouble(obj);
        }

        public static string stripMoney(string money)
        {
            money = money.Replace("Rp", "");
            money = money.Replace(".", "");
            return money;
        }

        public static void shellCommand(string cmdtext)
        {
            var proc1 = new System.Diagnostics.ProcessStartInfo();
            proc1.UseShellExecute = true;

            proc1.WorkingDirectory = @"C:\Windows\System32";

            proc1.FileName = @"C:\Windows\System32\cmd.exe";
            //proc1.Verb = "runas";
            proc1.Arguments = "/c " + cmdtext;
            proc1.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            System.Diagnostics.Process.Start(proc1);
        }


        public static void printPenjualan(string faktur, string sales)
        {
            DateTime tgl = DateTime.Now;
            DataTable rs = executeReader("SELECT * FROM penjualan WHERE Faktur = '" + faktur + "'");

            //PRINT INVOICE
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(Convert.ToChar(27) + "a1" + Convert.ToChar(27) + "!4" + "Maju Baby Shop");
            sb.AppendLine("Tasikmalaya");
            sb.AppendLine(Convert.ToChar(27) + "@");
            sb.AppendLine("Faktur: " + faktur + " Sales: " + sales);
            sb.AppendLine("Tanggal: " + tgl.ToShortDateString() + " Jam: " + tgl.ToShortTimeString());
            sb.AppendLine("");
            sb.AppendLine("========================================");

            double total = 0;
            int qty = 0;
            int i = 1;
            foreach (DataRow row in rs.Rows)
            {
                sb.AppendLine(Left((i.ToString() + ". " + row[3].ToString()), 40));
                sb.AppendLine("   " + strtomoney(row[5].ToString()) + Convert.ToChar(9) + "x" + Convert.ToChar(9) + row[4].ToString() + Convert.ToChar(9) + strtomoney(row[6].ToString()));
                total += Convert.ToDouble(row[6]);
                qty += Convert.ToInt32(row[4]);
                i += 1;
            }

            sb.AppendLine("----------------------------------------");
            //TODO: Total money space length
            sb.AppendLine("   Qty: " + qty.ToString() + Convert.ToChar(9) + "       TOTAL: " + strtomoney(total.ToString()));
            sb.AppendLine("");

            sb.AppendLine(Convert.ToChar(29) + "VA0");


            System.IO.File.WriteAllText(@"C:\test\invoicembs.txt", sb.ToString());

            shellCommand("copy c:\\test\\invoicembs.txt " + Args.printer);

        }

        public static void printPembelian(string faktur, string user)
        {
            DateTime tgl = DateTime.Now;
            DataTable rs = executeReader("SELECT Kode, Nama, Jumlah FROM pembelian WHERE Faktur = '" + faktur + "'");

            //PRINT INVOICE
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(Convert.ToChar(27) + "a1" + Convert.ToChar(27) + "!4" + "PEMBELIAN [BABY]");
            sb.AppendLine(Convert.ToChar(27) + "@");
            sb.AppendLine("Faktur: " + faktur + " User: " + user);
            sb.AppendLine("Tanggal: " + tgl.ToShortDateString() + " Jam: " + tgl.ToShortTimeString());
            sb.AppendLine("");
            sb.AppendLine("========================================");

            int qty = 0;
            foreach (DataRow row in rs.Rows)
            {
                sb.AppendLine(row[0].ToString() + Convert.ToChar(9) + row[1].ToString() + Convert.ToChar(9) + row[2].ToString());
                qty += Convert.ToInt32(row[2]);
            }

            sb.AppendLine("-----------------------------------------");
            sb.AppendLine("   " + Convert.ToChar(9) + Convert.ToChar(9) + Convert.ToChar(9) + "Qty: " + qty.ToString());
            sb.AppendLine("");

            sb.AppendLine(Convert.ToChar(29) + "VA0");


            System.IO.File.WriteAllText(@"C:\test\invoicepembelian.txt", sb.ToString());

            shellCommand("copy c:\\test\\invoicepembelian.txt " + Args.printer);

        }

        public static void printBarcode(string kode, string nama, string harga, string queue, string printerbarcode)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine(Convert.ToChar(2) + "KI71");
            sb.AppendLine(Convert.ToChar(2) + "L");
            sb.AppendLine("A2");
            sb.AppendLine("D12");


            if (kode.Length <= 8)
            {
                sb.AppendLine("1e2202500300015A" + kode);
                sb.AppendLine("111100000150020" + nama);
                sb.AppendLine("112100000000020" + harga);

                sb.AppendLine("1e2202500300155A" + kode);
                sb.AppendLine("111100000150160" + nama);
                sb.AppendLine("112100000000160" + harga);

                sb.AppendLine("1e2202500300295A" + kode);
                sb.AppendLine("111100000150300" + nama);
                sb.AppendLine("112100000000300" + harga);
            }
            else
            {
                sb.AppendLine("1e1102500300015A" + kode);
                sb.AppendLine("111100000150020" + nama);
                sb.AppendLine("112100000000020" + harga);

                sb.AppendLine("1e1102500300155A" + kode);
                sb.AppendLine("111100000150160" + nama);
                sb.AppendLine("112100000000160" + harga);

                sb.AppendLine("1e1102500300295A" + kode);
                sb.AppendLine("111100000150300" + nama);
                sb.AppendLine("112100000000300" + harga);
            }

            sb.AppendLine("Q" + queue);
            sb.AppendLine("E");
            sb.AppendLine(Convert.ToChar(2) + "Q");

            System.IO.File.WriteAllText(@"C:\test\barcode.txt", sb.ToString());

            shellCommand("copy c:\\test\\barcode.txt " + printerbarcode);

        }

        public static string Left(string value, int maxLength)
        {
            if (string.IsNullOrEmpty(value)) return value;
            maxLength = Math.Abs(maxLength);

            return (value.Length <= maxLength
                   ? value
                   : value.Substring(0, maxLength)
                   );
        }

        public static string getFaktur(DateTime tgl)
        {
            string tanggal = tgl.Day.ToString();
            string bulan = tgl.Month.ToString();
            string tahun = tgl.Year.ToString();

            if (tanggal.Length < 2)
            {
                tanggal = "0" + tanggal;
            }

            if (bulan.Length < 2)
            {
                bulan = "0" + bulan;
            }

            tahun = tahun.Substring(2);

            MySqlConnection conn = new MySqlConnection(App.getConnectionString());
            object result;
            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("Select COUNT(*) FROM penjualancompact WHERE Tanggal = '" + tgl.ToShortDateString() + "'", conn);
                result = cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                result = null;
            }
            conn.Close();

            string nomor = "";
            string urut;

            if (Convert.ToString(result) != "0")
            {
                urut = Convert.ToString(Convert.ToUInt32(result) + 1);
            }
            else
            {
                urut = "1";
            }

            if (urut.Length == 1)
            {
                nomor = "000" + urut;
            }
            else if (urut.Length == 2)
            {
                nomor = "00" + urut;
            }
            else if (urut.Length == 3)
            {
                nomor = "0" + urut;
            }
            else if (urut.Length == 4)
            {
                nomor = urut;
            }

            return tanggal + bulan + tahun + nomor;
        }


        public static void sendEmail(string subject, string mailbody)
        {
            // Command line argument must the the SMTP host.
            SmtpClient client = new SmtpClient();
            client.Port = 587;
            client.Host = "smtp.gmail.com";
            client.EnableSsl = true;
            client.Timeout = 10000;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            client.Credentials = new System.Net.NetworkCredential(Args.emailusername, Args.emailpassword);

            MailMessage mm = new MailMessage(Args.emailusername, Args.emailrecipient, subject, mailbody);
            mm.BodyEncoding = UTF8Encoding.UTF8;
            mm.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;

            client.Send(mm);
        }

    }

}
