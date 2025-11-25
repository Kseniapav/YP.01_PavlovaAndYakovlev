using System;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace ClientsYchetsForms
{
    public partial class EditClientForm : Form
    {
        private int clientId;
        MySqlConnection connection = new MySqlConnection("Server=localhost;Database=clientsychet;Uid=root;Pwd=vertrigo;");

        public EditClientForm(int ClientID, string FullName, string BirthDate, string PhoneNumber, string Email, string RegistrationDate)
        {
            InitializeComponent();
            clientId = ClientID;
            textBoxFio.Text = FullName;
            textBoxData.Text = BirthDate;
            textBoxNumber.Text = PhoneNumber;
            textBoxEmail.Text = Email;
            textBoxregistration.Text = RegistrationDate;
        }

        private void EditClientForm_Load(object sender, EventArgs e)
        {

        }

        private void ButtonSave_Click(object sender, EventArgs e)
        {
            // Проверка обязательных полей
            if (string.IsNullOrWhiteSpace(textBoxFio.Text))
            {
                MessageBox.Show("Упс! Поле ФИО не может быть пустым!");
                return;
            }
            if (string.IsNullOrWhiteSpace(textBoxNumber.Text))
            {
                MessageBox.Show("Упс! Поле с номером телефона не может быть пустым!");
                return;
            }
            if (string.IsNullOrWhiteSpace(textBoxData.Text))
            {
                MessageBox.Show("Упс! Поле с датой рождения не может быть пустым!");
                return;
            }

            // Валидация ФИО (только кириллица)
            if (!Regex.IsMatch(textBoxFio.Text, @"^[А-Яа-яЁё\s]+$"))
            {
                MessageBox.Show("ФИО должно содержать только кириллицу!");
                return;
            }

            // Валидация даты рождения
            if (!DateTime.TryParse(textBoxData.Text, out DateTime date) || date >= DateTime.Now)
            {
                MessageBox.Show("Дата рождения должна быть корректной датой в прошлом!");
                return;
            }

            // Валидация телефона
            if (!Regex.IsMatch(textBoxNumber.Text, @"^\+7\d{10}$"))
            {
                MessageBox.Show("Телефон должен быть в формате +7XXXXXXXXXX!");
                return;
            }

            // Валидация Email
            if (!Regex.IsMatch(textBoxEmail.Text, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                MessageBox.Show("Некорректный формат Email!");
                return;
            }

            try
            {
                connection.Open();
                string query = "UPDATE clients SET ФИО клиента=@name, Дата рождения=@birth_date, Номер телефона=@phone, Email=@mail, Дата регистрации=@reg WHERE ID=@id";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@name", textBoxFio.Text);
                cmd.Parameters.AddWithValue("@birth_date", date.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@phone", textBoxNumber.Text);
                cmd.Parameters.AddWithValue("@mail", textBoxEmail.Text);
                cmd.Parameters.AddWithValue("@reg", date.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@id", clientId);

                int result = cmd.ExecuteNonQuery();

                if (result > 0)
                {
                    MessageBox.Show($"Данные клиента {textBoxFio.Text} успешно изменены.");
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Упс! Ошибка изменения базы данных, обратитесь к администратору.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Упс! Ошибка изменения базы данных, обратитесь к администратору.\n" + ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }

        private void buttonCansel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
