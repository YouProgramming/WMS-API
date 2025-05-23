using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WMS_Repository_Data_Layer.Data.Entities.Models;
using WMS_Repository_Data_Layer.Data;

namespace WMS_Repository_Data_Layer.Repository.IRepos
{
    public interface ILogRepo
    {
        public  Task<List<Log>> GetAllLogs();

        public  Task<Log> GetLogById(int id);

        public  Task<int> InsertLog(Log log);

        public  Task<int> UpdateLog(Log log);

        public  Task<int> DeleteLog(int id);
    }
}
