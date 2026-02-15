using System.IO;

namespace Unity.Services.LevelPlay.Editor
{
    interface IXmlDocument
    {
        void Load(string filename);
        void Load(Stream stream);
        void LoadXml(string xml);
        #nullable enable
        System.Xml.XmlNode ? SelectSingleNode(string xpath);
        #nullable disable

        System.Xml.XmlNodeList GetElementsByTagName(string name);
        void Save(string filename);
        System.Xml.XmlElement CreateElement(string tagName);
    }

    interface IXmlDocumentFactory
    {
        IXmlDocument CreateXmlDocument();
    }
}
