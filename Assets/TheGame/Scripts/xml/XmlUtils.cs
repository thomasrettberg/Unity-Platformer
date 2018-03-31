
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using UnityEngine;

public class XmlUtils
{

    /// <summary>
    /// Creates a representive XML-String from serializble Data-Object.
    /// Encoding in UTF-8.
    /// </summary>
    /// <param name="data">Object to save.</param>
    /// <returns>Representive XML-String.</returns>
    public static string SaveXmlString(object data)
    {
        using (MemoryStream ms = new MemoryStream())
        {
            XmlSerializer serializer = new XmlSerializer(data.GetType());
            using (StreamWriter stream = new StreamWriter(ms, Encoding.GetEncoding("UTF-8")))
            {
                using (XmlWriter xmlWriter = XmlWriter.Create(stream, new XmlWriterSettings { Indent = false }))
                {
                    serializer.Serialize(xmlWriter, data);
                }
                ms.Position = 0;
                return new StreamReader(ms).ReadToEnd();
            }
        }
    }

    /// <summary>
    /// Loads an object from a string in UTF-8 encoding.
    /// </summary>
    /// <param name="xml">XML sourcecode.</param>
    /// <param name="dataType">Class that should be instanced. (You may use typeof(Class) or obj.getType() get the required type.)</param>
    /// <returns>The new object or null in case of problems.</returns>
    public static object Load(string xml, System.Type dataType)
    {
        using (MemoryStream ms = new MemoryStream(1024))
        {
            ms.Write(System.Text.Encoding.UTF8.GetBytes(xml), 0, System.Text.Encoding.UTF8.GetBytes(xml).Length);
            ms.Seek(0, SeekOrigin.Begin);
            XmlSerializer serializer = new XmlSerializer(dataType);
            using (StreamReader stream = new StreamReader(ms, Encoding.GetEncoding("UTF-8"))) 
            {
                ms.Position = 0;
                object o;
                try
                {
                    o = serializer.Deserialize(stream);
                }
                catch (System.Exception ex) { Debug.LogException(ex); o = null; }
                return o;
            }
        }

    }

    /// <summary>
    /// Loads an object from a string in UTF-8 encoding.
    /// Same as <see cref="Load(string, System.Type)"/> 
    /// with typeparam notation.
    /// </summary>
    /// <typeparam name="T">Deserialization type, class that should be instanced.</typeparam>
    /// <param name="xml">XML sourcecode.</param>
    /// <returns>The new object, deserialized from xml.</returns>
    public static T Load<T>(string xml)
    {
        return (T)System.Convert.ChangeType(Load(xml, typeof(T)), typeof(T));
    }
}
