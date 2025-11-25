using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientsYchet
{
    public class MasterManager: IDisposable
    {
        private IMasterRepository repository;
        private List<Master> masters;

        public MasterManager(IMasterRepository repository)
        {
            this.repository = repository;
            masters = new List<Master>();
        }

        public List<Master> LoadMasters()
        {
            try
            {
                masters = repository.GetAllMasters();
                return masters;
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка загрузки данных: {ex.Message}");
            }
        }

        public List<Master> GetMasters()
        {
            return masters;
        }

        public void RefreshMasters()
        {
            LoadMasters();
        }

        public void Dispose()
        {
            if (repository is IDisposable disposable)
                disposable.Dispose();
        }
    }
}
