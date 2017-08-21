using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Metro_Navigation
{
    public class Serializer
    {
        public static void SerializeAll(object obj,string fileName)
        {
            var xmlSerializer = new XmlSerializer(obj.GetType());
            using (FileStream fs = new FileStream(fileName, FileMode.Create))
            {
                xmlSerializer.Serialize(fs, obj);
            }


        }

        public static T DeserializeAll<T>(string fileName)
        {
            var xmlSerializer = new XmlSerializer(typeof(T));
            using (FileStream fs = new FileStream(fileName, FileMode.Open))
                return (T)xmlSerializer.Deserialize(fs);
        }
    }
}
