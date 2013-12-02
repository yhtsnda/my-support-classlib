using Avalon.HttpClient;
using Avalon.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters;
using System.Runtime.Serialization.Formatters.Soap;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Avalon.UCenter
{
    internal abstract class AbstractPassport91XmlData
    {
        public string AppName { get; set; }

        public string AppPassword { get; set; }

        public string AppCheckCode { get; set; }

        public string CheckCode { get; set; }

        protected Dictionary<string, string> InvokeApi<T>(string pathName, string url, string body, out T code) where T : struct
        {
            using (TimeoutWebClient client = new TimeoutWebClient(30000))
            {
                client.Headers.Add("Content-Type", "text/xml; charset=utf-8");
                client.Headers.Add("SOAPAction", "\"http://tempuri.org/" + pathName + "\"");//该行可以不添加

                var content = client.UploadString(url, body);
                string soap = ExtractSoapbody(content);

                Dictionary<string, string> dict = ParseXML(soap, pathName);

                string codeString;
                if (dict.TryGetValue(pathName + "Result", out codeString))
                {
                    if (Enum.TryParse<T>(codeString, out code))
                    {
                        return dict;
                    }
                }
            }
            throw new Exception("接口调用发生错误");
        }

        object Parse(string soap)
        {
            XmlRootAttribute xRoot = new XmlRootAttribute();
            xRoot.Namespace = "http://schemas.xmlsoap.org/soap/envelope/";

            using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(soap)))
            {
                SoapFormatter formatter = new SoapFormatter();
                var v = formatter.Deserialize(ms);
                if (v is SoapFault)
                {
                    var sf = (SoapFault)v;
                    throw new Exception(String.Format("调用API错误。FaultCode:{0}, FaultMessage:{1}", sf.FaultCode, sf.FaultString));
                }
                return v;
            }
        }




        public class LionObjBinder : System.Runtime.Serialization.SerializationBinder
        {
            public override Type BindToType(string assemblyName, string typeName)
            {
                Type result = null;

                return result;
            }
        }


        string ExtractSoapbody(string soapMessage)
        {
            Parse(soapMessage);

            //check soap message
            if (soapMessage == null || soapMessage.Length <= 0)
                throw new Exception("Soap message not valid");

            //declare some local variable

            //load the Soap Message
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(soapMessage);

            //search for the "http://schemas.xmlsoap.org/soap/envelope/" URI prefix
            string prefix = "";
            for (int i = 0; i < doc.ChildNodes.Count; i++)
            {
                XmlNode soapNode = doc.ChildNodes[i];
                prefix = soapNode.GetPrefixOfNamespace("http://schemas.xmlsoap.org/soap/envelope/");

                if (prefix.Length > 0)
                    break;
            }

            //prefix not founded. 
            if (prefix == null || prefix.Length <= 0)
                throw new Exception("Can't found the soap envelope prefix");

            //find soap body start index
            int iSoapBodyElementStartFrom = soapMessage.IndexOf("<" + prefix + ":Body");
            int iSoapBodyElementStartEnd = soapMessage.IndexOf(">", iSoapBodyElementStartFrom);
            int iSoapBodyStartIndex = iSoapBodyElementStartEnd + 1;

            //find soap body end index
            int iSoapBodyEndIndex = soapMessage.IndexOf("</" + prefix + ":Body>") - 1;

            //get soap body (xml data)
            return soapMessage.Substring(iSoapBodyStartIndex, iSoapBodyEndIndex - iSoapBodyStartIndex + 1);
        }

        Dictionary<string, string> ParseXML(string xml, string pathName)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);

            string strXPath = "//ns:" + pathName + "Response//*";
            XmlNamespaceManager namespaceManager = new XmlNamespaceManager(doc.NameTable); //namespace 
            namespaceManager.AddNamespace("ns", "http://tempuri.org/"); //类似xmlns=\"\"的文件前缀可以任意命名

            Dictionary<string, string> dict = new Dictionary<string, string>();

            XmlNodeList list = doc.SelectNodes(strXPath, namespaceManager);
            if (list != null)
            {
                foreach (XmlNode n in list)
                {
                    dict.Add(n.LocalName, n.InnerText);
                }
            }
            return dict;
        }

        [Serializable]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://tempuri.org/")]
        public class RegisterUserInfo_Cop_591UP_WithRegTypeResponse
        {
            public int RegisterUserInfo_Cop_591UP_WithRegTypeResult { get; set; }

            public long return_userid { get; set; }
        }
    }
}
