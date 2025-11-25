using System;
using System.Data;
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
            btnEdit.Enabled = false;
            dataGridView1.SelectionChanged += DataGridView1_SelectionChanged;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
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
        private void DataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            bool isRowSelected = dataGridView1.SelectedRows.Count == 1;
            btnEdit.Enabled = isRowSelected;
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

       

        private void btnEdit_Click_1(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 1)
            {
                var row = dataGridView1.SelectedRows[0];

                int id = Convert.ToInt32(row.Cells["ID"].Value);
                string name = row.Cells["ФИО клиента"].Value.ToString();
                string birthDate = row.Cells["Дата рождения"].Value.ToString();
                string phone = row.Cells["Номер телефона"].Value.ToString();
                string email = row.Cells["Email"].Value.ToString();
                string dataRegistration = row.Cells["Дата регистрации"].Value.ToString();

                EditClientForm editForm = new EditClientForm(id, name, birthDate, phone, email, dataRegistration);

                if (editForm.ShowDialog() == DialogResult.OK)
                {
                    LoadClients(); // обновляем таблицу после сохранения
                }
            }
            else
            {
                MessageBox.Show("Выберите клиента для редактирования!");
            }

        }
    }
}
