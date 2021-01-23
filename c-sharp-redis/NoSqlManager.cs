using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace c_sharp_redis
{
    public class NoSqlManager
    {   

        private string connectionString;

        public NoSqlManager(string config)
        {
            connectionString = config;
        }

        // Check if key "device:860585005101885" exists
        public bool Exist(string key = "860585005101885")
        {
            try
            {
                ConnectionMultiplexer redis = ConnectionMultiplexer.Connect(connectionString);
                IDatabase db = redis.GetDatabase(0);
                var isExist = db.KeyExists("device:" + key);                
                if (redis != null) redis.Close();
                if (isExist) return true;
                return false;
            }
            catch (Exception err)
            {
                return false;
            }
        }

        // Save or Update
        public int Save(RecordEvent e)
        {
            try
            {
                ConnectionMultiplexer redis = ConnectionMultiplexer.Connect(connectionString);
                IDatabase db = redis.GetDatabase(0);

                db.HashSet("device:" + e.Serial, new[] {
                        new HashEntry("serial", e.Serial),
                        new HashEntry("is_moving", e.IsMoving),
                        new HashEntry("speed", e.Speed),
                        new HashEntry("event_code", e.EventCode),
                        new HashEntry("event_time", e.EventTime.ToUniversalTime().ToString()),
                 });

                redis.Close();
                return 1;
            }
            catch (Exception err)
            {
                return -1;
            }
        }

        // Delete
        public bool Delete(string key)
        {
            try
            {
                ConnectionMultiplexer redis = ConnectionMultiplexer.Connect(connectionString);
                IDatabase db = redis.GetDatabase(0);
                var result = db.KeyDelete("device:" + key);
                redis.Close();
                return result;
            }
            catch (Exception err)
            {
                return false;
            }
        }
    }
}
