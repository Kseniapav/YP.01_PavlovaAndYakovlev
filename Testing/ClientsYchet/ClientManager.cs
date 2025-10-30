using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ClientsYchet
{
    public class ClientManager
    {
        private IClientRepository repository;
        private List<Client> clients;


        public ClientManager(IClientRepository repo)
        {
            repository = repo;
            clients = new List<Client>();
        }

        public string AddClient(Client client)
        {
            // Валидация ФИО
            if (string.IsNullOrWhiteSpace(client.FioClienta))
            {
                return "ФИО клиента не может быть пустым";
            }

            // Валидация даты рождения
            if (client.DateOfBirth == default || client.DateOfBirth > DateTime.Today)
            {
                return "Дата рождения указана некорректно";
            }

            // Валидация телефона
            if (!IsValidPhone(client.Phone))
            {
                return "Некорректный номер телефона";
            }

            // Валидация e-mail
            if (!IsValidEmail(client.Email))
            {
                return "Некорректный e-mail";
            }

            // Валидация даты регистрации
            if (client.DateOfReg == default || client.DateOfReg > DateTime.Today)
            {
                return "Дата регистрации указана некорректно";
            }           

            return string.Empty; // Успешное добавление
        }


        public string DeleteClient(string email, string phone)
        {

            // Валидация e-mail
            if (!IsValidEmail(email))
            {
                return "Некорректный e-mail";
            }

            // Валидация телефона
            if (!IsValidPhone(phone))
            {
                return "Некорректный номер телефона";
            }

            // Проверяем наличие клиента
            var client = repository.FindByEmailOrPhone(email, phone);
            if (client == null)
            {
                return "Клиент с указанными данными не найден";
            }

            // Пытаемся удалить
            bool deleted = repository.DeleteByEmailOrPhone(email, phone);
            return deleted ? "Клиент успешно удалён" : "Ошибка при удалении клиента";
        }

        private bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(email, pattern);
        }

        private bool IsValidPhone(string phone)
        {
            if (string.IsNullOrWhiteSpace(phone))
                return false;

            string pattern = @"^\+7\d{10}$"; // формат +7XXXXXXXXXX
            return Regex.IsMatch(phone, pattern);
        }
        public string UpdateClient(Client client)
        {
            try
            {
                bool updateResult = repository.UpdateClient(client);
                if (updateResult)
                {
                    return $"Данные клиента {client.FioClienta} успешно изменены";
                }
                else
                {
                    return "Упс! Ошибка изменения базы данных, обратитесь к администратору.";
                }
            }
            catch
            {
                return "Упс! Ошибка изменения базы данных, обратитесь к администратору.";
            }
        }

        public string ValideClientData(Client client)
        {

            // Проверка обязательных полей
            if (string.IsNullOrWhiteSpace(client.FioClienta))
                return "Упс! Поле ФИО не должно быть пустым!";
            if  (string.IsNullOrWhiteSpace(client.Phone))
                return "Упс! Номер телефона не должно быть пустым!";

            // Валидация ФИО (только кириллица)
            if (!Regex.IsMatch(client.FioClienta, @"^[А-Яа-яЁё\s]+$"))
                return "Упс! Проверьте заполненность данных ФИО!";

            // Валидация даты рождения
            if (client.DateOfBirth >= DateTime.Now)
                return "Упс! Дата рождения не может быть из будущего!";

            // Валидация телефона
            if (!Regex.IsMatch(client.Phone, @"^\+7\d{10}$"))
                return "Упс! Неправильный номер телефона!";

            // Валидация Email
            if (!Regex.IsMatch(client.Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                return "Упс! Неверный формат e-mail!";

            // Вызываем UpdateClient для сохранения в БД
            return UpdateClient(client);
        }
        public Client GetClientById(int id)
        {
            return repository.GetClientById(id);
        }
    }
}
