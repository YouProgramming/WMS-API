using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WMS_Repository_Data_Layer.Data.Entities.Models;

namespace WMS_Repository_Data_Layer.Repository.IRepos
{
    public interface IIssuingRepo
    {
        public  Task<List<Issuing>> GetAllIssuings();

        public  Task<Issuing> GetIssuingById(int id);

        public  Task<int> InsertIssuing(Issuing issuing);

        public  Task<int> UpdateIssuing(Issuing issuing);

        public Task<int> DeleteIssuing(int id);
    }
}
