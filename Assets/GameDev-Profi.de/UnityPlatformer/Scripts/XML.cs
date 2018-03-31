using UnityEngine;

using System.IO;
using System.Text; //required for encoding
using System.Xml;
using System.Xml.Serialization;

/// <summary>
/// License:
/// The MIT License (MIT)
/// 
/// Copyright (c) 2017 René Bühling, www.gamedev-profi.de
/// 
/// Permission is hereby granted, free of charge, to any person obtaining a copy
/// of this software and associated documentation files (the "Software"), to deal
/// in the Software without restriction, including without limitation the rights
/// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
/// copies of the Software, and to permit persons to whom the Software is
/// furnished to do so, subject to the following conditions:
/// 
/// The above copyright notice and this permission notice shall be included in all
/// copies or substantial portions of the Software.
/// 
/// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
/// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
/// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
/// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
/// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
/// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
/// SOFTWARE.
/// </summary>
namespace GameDevProfi.Utils
{

    /// <summary>
    /// Utilities related to XML processing.
    /// </summary>
    public class XML
    {
        /// <summary>
        /// Serializes the given object into a string in UTF-8 encoding.
        /// </summary>
        /// <param name="data">Object to save.</param>
        /// <returns>Serialization string.</returns>
        /// <see cref="http://tech.pro/tutorial/798/csharp-tutorial-xml-serialization"/>
        /// <see cref="http://wiki.unity3d.com/index.php?title=Saving_and_Loading_Data:_XmlSerializer"/>
        public static string Save(object data)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                //fixed for encoding:
                XmlSerializer serializer = new XmlSerializer(data.GetType());
                //using (StreamWriter stream = new StreamWriter(placedFilename, false, Encoding.GetEncoding("UTF-8"))) //using fires stream.close
                using (StreamWriter stream = new StreamWriter(ms, Encoding.GetEncoding("UTF-8"))) //using fires stream.close
                {
                    using (XmlWriter xmlWriter = XmlWriter.Create(stream, new XmlWriterSettings { Indent = false }))
                    {
                        serializer.Serialize(xmlWriter, data);
                    }
                    //to string:    (!) must be called here as closing StreamWriter (via /using) will dispose ms for some reason
                    ms.Position = 0;
                    //Debug.Log("xml="+new StreamReader(ms).ReadToEnd());
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
        /// <see cref="http://tech.pro/tutorial/798/csharp-tutorial-xml-serialization"/>
        /// <see cref="http://wiki.unity3d.com/index.php?title=Saving_and_Loading_Data:_XmlSerializer"/>
        /// <see cref="Load{T}(string)"/>
        public static object Load(string xml, System.Type dataType)
        {
            Debug.Log("read: " + xml);
            using (MemoryStream ms = new MemoryStream(1024))
            {
                ms.Write(System.Text.Encoding.UTF8.GetBytes(xml), 0, System.Text.Encoding.UTF8.GetBytes(xml).Length);
                ms.Seek(0, SeekOrigin.Begin);

                //fix for encoding:
                XmlSerializer serializer = new XmlSerializer(dataType);
                using (StreamReader stream = new StreamReader(ms, Encoding.GetEncoding("UTF-8"))) //using fires stream.close
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
            return (T)System.Convert.ChangeType(Load(xml,typeof(T)), typeof(T));
        }
    }
}
