using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WMS_Repository_Data_Layer.Data.Entities.Models;
using WMS_Repository_Data_Layer.Data;
using WMS_Repository_Data_Layer.Repository.IRepos;
using Microsoft.EntityFrameworkCore;

namespace WMS_Repository_Data_Layer.Repository.Repos
{
    public class LogRepo(AppDbContext appDbContext) : ILogRepo
    {
        private readonly AppDbContext _appDbContext = appDbContext;

        public async Task<List<Log>> GetAllLogs()
        {
            return await _appDbContext.Logs.ToListAsync();
        }

        public async Task<Log> GetLogById(int id)
        {
            var log = await _appDbContext.Logs.SingleOrDefaultAsync(l => l.LogId == id);
            return log is null ? throw new KeyNotFoundException($"Log with ID {id} was not found.") : log;
        }

        public async Task<int> InsertLog(Log log)
        {
            await _appDbContext.Logs.AddAsync(log);
            await _appDbContext.SaveChangesAsync();
            return log.LogId;
        }

        public async Task<int> UpdateLog(Log log)
        {
            var existing = await _appDbContext.Logs.FindAsync(log.LogId)
                ?? throw new KeyNotFoundException($"Log with ID {log.LogId} was not found.");

            _appDbContext.Entry(existing).CurrentValues.SetValues(log);
            return await _appDbContext.SaveChangesAsync();
        }

        public async Task<int> DeleteLog(int id)
        {
            var log = await _appDbContext.Logs.FindAsync(id)
                ?? throw new KeyNotFoundException($"Log with ID {id} was not found.");

            _appDbContext.Logs.Remove(log);
            return await _appDbContext.SaveChangesAsync();
        }

        //public async Task<List<Log>> GetAllLogs()
        //{
        //    List<Log> logs = [];

        //    try
        //    {
        //        logs = await _appDbContext.Logs.ToListAsync();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }

        //    return logs;
        //}

        //public async Task<Log> GetLogById(int id)
        //{
        //    Log? log;
        //    try
        //    {
        //        log = await _appDbContext.Logs.SingleOrDefaultAsync(l => l.LogId == id);
        //        if (log == null)
        //            throw new Exception("Not Found");
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }

        //    return log;
        //}

        //public async Task<int> InsertLog(Log log)
        //{
        //    int ID = -1;
        //    try
        //    {
        //        await _appDbContext.Logs.AddAsync(log);
        //        await _appDbContext.SaveChangesAsync();
        //        ID = log.LogId;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }

        //    return ID;
        //}

        //public async Task<int> UpdateLog(Log log)
        //{
        //    int Result = -1;
        //    try
        //    {
        //        var existing = await _appDbContext.Logs.FindAsync(log.LogId)
        //            ?? throw new Exception("Not Found");

        //        _appDbContext.Entry(existing).CurrentValues.SetValues(log);
        //        Result = await _appDbContext.SaveChangesAsync();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }

        //    return Result;
        //}

        //public async Task<int> DeleteLog(int id)
        //{
        //    int Result = -1;
        //    try
        //    {
        //        var log = await _appDbContext.Logs.FindAsync(id)
        //            ?? throw new Exception("Not Found");

        //        _appDbContext.Logs.Remove(log);
        //        Result = await _appDbContext.SaveChangesAsync();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }

        //    return Result;
        //}
    }
}
