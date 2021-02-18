using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace c_sharp_socket_server_with_queue
{
    public static class Extensions
    {
        public static string ToJSON<T>(this T obj)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            serializer.MaxJsonLength = 5000000;
            try
            {
                return serializer.Serialize(obj);
            }
            catch
            {
                return string.Empty;
            }
        }
    }
}
