using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WMS_Repository_Data_Layer.Data.Entities.Models;

namespace WMS_Repository_Data_Layer.Repository.IRepos
{
    public interface IReceivingRepo
    {
        public Task<List<Receiving>> GetAllReceivings();
        public Task<Receiving> GetReceivingById(int id);
        public Task<int> InsertReceiving(Receiving receiving);
        public Task<int> UpdateReceiving(Receiving receiving);
        public Task<int> DeleteReceiving(int id);
    }
}
