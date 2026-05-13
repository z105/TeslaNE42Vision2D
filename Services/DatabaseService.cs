using FreeSql;
using System;
using System.Collections.Generic;
using TeslaNE42Vision2D.Entity;
using TeslaNE42Vision2D.Utils;

namespace TeslaNE42Vision2D.Services
{
    public class DatabaseService
    {
        private static readonly Lazy<DatabaseService> _lazy =
            new Lazy<DatabaseService>(() => new DatabaseService());

        public static DatabaseService Instance => _lazy.Value;

        private IFreeSql _fsql;

        private DatabaseService() { }

        public void Initialize(string connectionString)
        {
            try
            {
                _fsql = new FreeSqlBuilder()
                    .UseConnectionString(DataType.Sqlite, connectionString)
                    .UseAutoSyncStructure(true)
                    .Build();
            }
            catch (Exception ex)
            {
                LogHelper.Error("数据库初始化失败", ex);
            }
        }

        public void InsertRecord(DetectRecord record)
        {
            try
            {
                _fsql?.Insert(record).ExecuteAffrows();
            }
            catch (Exception ex)
            {
                LogHelper.Error("插入检测记录失败", ex);
            }
        }

        public List<DetectRecord> QueryRecords(DateTime date)
        {
            try
            {
                DateTime start = date.Date;
                DateTime end = start.AddDays(1);
                return _fsql?.Select<DetectRecord>()
                    .Where(r => r.DetectTime >= start && r.DetectTime < end)
                    .ToList() ?? new List<DetectRecord>();
            }
            catch (Exception ex)
            {
                LogHelper.Error("查询检测记录失败", ex);
                return new List<DetectRecord>();
            }
        }

        public void Dispose()
        {
            _fsql?.Dispose();
        }
    }
}
