using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientsYchet
{
    public interface IClientRepository
    {
        Client AddClient(Client client);
        bool ExistsByEmailOrPhone(string email, string phone); //Проверяет, есть ли клиент с указанным email или телефоном
        Client FindByEmailOrPhone(string email, string phone); //Возвращает клиента по контактным данным
        bool DeleteByEmailOrPhone(string email, string phone); //Удаляет клиента по email или телефону
    }
}
