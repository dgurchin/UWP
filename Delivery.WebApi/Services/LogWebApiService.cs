using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Inventory.Data;
using Inventory.Models;
using Inventory.Services;

namespace Delivery.WebApi.Services
{
    public class LogWebApiService : ILogService
    {
        private static NLog.Logger _logger = NLog.Web.NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();

        public async Task WriteAsync(LogType type, string source, string action, Exception ex)
        {
            await WriteAsync(LogType.Error, source, action, ex.Message, ex.ToString());
            Exception deepException = ex.InnerException;
            while (deepException != null)
            {
                await WriteAsync(LogType.Error, source, action, deepException.Message, deepException.ToString());
                deepException = deepException.InnerException;
            }
        }

        public async Task WriteAsync(LogType type, string source, string action, string message, string description)
        {
            var appLog = new AppLog()
            {
                User = "WebApi",
                Type = type,
                Source = source,
                Action = action,
                Message = message,
                Description = description,
                DateTime = DateTime.UtcNow,
                Name = ""
            };
            SaveLog(appLog);
            await Task.CompletedTask;
        }

        public async void LogInformation(string source, string action, string message, string description)
        {
            await WriteAsync(LogType.Information, source, action, message, description);
        }

        public async void LogWarning(string source, string action, string message, string description)
        {
            await WriteAsync(LogType.Warning, source, action, message, description);
        }

        public void LogException(string source, string action, Exception exception)
        {
            LogError(source, action, exception.Message, exception.ToString());
        }
        public async void LogError(string source, string action, string message, string description)
        {
            await WriteAsync(LogType.Error, source, action, message, description);
        }

        public Task<AppLogModel> GetLogAsync(long id)
        {
            throw new NotImplementedException();
        }

        public Task<IList<AppLogModel>> GetLogsAsync(DataRequest<AppLog> request)
        {
            throw new NotImplementedException();
        }

        public Task<IList<AppLogModel>> GetLogsAsync(int skip, int take, DataRequest<AppLog> request)
        {
            throw new NotImplementedException();
        }

        public Task<int> GetLogsCountAsync(DataRequest<AppLog> request)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteLogAsync(AppLogModel model)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteLogRangeAsync(int index, int length, DataRequest<AppLog> request)
        {
            throw new NotImplementedException();
        }

        public Task MarkAllAsReadAsync()
        {
            throw new NotImplementedException();
        }

        private AppLogDbSQLServer CreateDataSource()
        {
            return new AppLogDbSQLServer();
        }

        private void SaveLog(AppLog appLog)
        {
            string logString = $"{appLog.Source}|{appLog.Action}|{appLog.Message}|{appLog.Description}";
            switch (appLog.Type)
            {
                case LogType.Error:
                    _logger.Error(logString);
                    break;
                case LogType.Warning:
                    _logger.Warn(logString);
                    break;
                case LogType.Information:
                    _logger.Info(logString);
                    break;
            }
        }
    }
}
