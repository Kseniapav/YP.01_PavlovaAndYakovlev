using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace ClientsYchetsForms
{
    public partial class MainForm: Form
    {
        MySqlConnection connection = new MySqlConnection("Server = localhost; Database=clientsychet;Uid=root;Pwd=vertrigo;");
        public MainForm()
        {
            InitializeComponent();
            LoadClients();
        }
        private void LoadClients()
        {
            try
            {
                connection.Open();

                string query = "SELECT * FROM clients";

                MySqlDataAdapter adapter = new MySqlDataAdapter(query, connection);
                DataTable table = new DataTable();
                adapter.Fill(table);

                dataGridView1.DataSource = table;
                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dataGridView1.ReadOnly = true;
                dataGridView1.AllowUserToAddRows = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка подключения или загрузки данных: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {

        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
