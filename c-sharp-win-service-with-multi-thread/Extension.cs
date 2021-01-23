using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace c_sharp_win_service_with_multi_thread
{
    public static class Extensions
    {
        /// <summary>
        /// List To JSON (string)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        //public static string ToJSON<T>(this List<T> list)
        //{
        //    if (list.Count > 0)
        //    {
        //        JavaScriptSerializer serializer = new JavaScriptSerializer();
        //        serializer.MaxJsonLength = 5000000;                
        //        try
        //        {
        //            return serializer.Serialize(list);
        //        }
        //        catch
        //        {
        //            return string.Empty;
        //        }
        //    }
        //    return "";
        //}

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
