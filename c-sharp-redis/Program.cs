using System;
using System.Configuration;

namespace c_sharp_redis
{
    class Program
    {
        static string Connection {
            get {
                return ConfigurationSettings.AppSettings["REDIS_CONNECTION_STRING"].ToString();
            }
        }
        static void Main(string[] args)
        {
            try
            {
                NoSqlManager noSql = new NoSqlManager(Connection);

                // Check key is exist 
                var isExist = noSql.Exist("860585005101885");
                Console.WriteLine(isExist);

                // Save 
                //var saveResult = noSql.Save(new RecordEvent {
                //    Serial = "860585005101885",
                //    IsMoving = true,
                //    Speed = 100,
                //    EventCode = 10017, 
                //    EventTime = DateTime.Now,
                //});

                // Delete by key
                // var delResult = noSql.Delete("860585005101885");

            }
            catch (Exception error)
            {
                Console.WriteLine(error);
            }
        }
    }
}
