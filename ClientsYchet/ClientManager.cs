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

        public ClientManager()
        {
            clients = new List<Client>();
        }

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

            // Проверка существования клиента в базе
            if (repository != null)
            {
                if (repository.ExistsByEmailOrPhone(client.Email, client.Phone))
                {
                    return "Клиент с такими контактными данными уже существует";
                }

                repository.AddClient(client);
            }

            return string.Empty; // Успешное добавление
        }


        public string DeleteClient(string email, string phone)
        {
            if (repository == null)
                return "Репозиторий недоступен";

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
    }
}
