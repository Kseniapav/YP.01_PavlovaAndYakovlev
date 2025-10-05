using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientsYchet
{
    public class Client
    {
        public int IdClientd { get; set; } //уникальный идентификатор клиента
        public string FioClienta { get; set; } //ФИО клиента
        public DateTime DateOfBirth { get; set; } //дата рождения клиента
        public string Phone { get; set; } //номер телефона клиента
        public string Email { get; set; } //эл почта клиента
        public DateTime DateOfReg { get; set; } //дата занесения клиента в бд
    }
}
