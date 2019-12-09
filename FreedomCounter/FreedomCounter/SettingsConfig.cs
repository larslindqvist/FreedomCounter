using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace FreedomCounter
{
    public class SettingsConfig
    {
        private XDocument doc;
        public event EventHandler ValueChanged;
        public SettingsConfig()
        {
            if (!File.Exists(Directory.GetCurrentDirectory() + "/settings.config"))
            {
                var file = File.Create(Directory.GetCurrentDirectory() + "/settings.config");
                file.Close();
                doc = new XDocument();
                doc.Add(GetDefaultXmlElements());
                doc.Save(Directory.GetCurrentDirectory() + "/settings.config");
            }
            doc = XDocument.Load(Directory.GetCurrentDirectory() + "/settings.config");
        }

        protected virtual void OnValueChanged()
        {
            ValueChanged?.Invoke(this, EventArgs.Empty);
        }

        public int Workday
        {
            get { return Convert.ToInt32(GetSetting("Workday")); }
            set
            {
                SetSetting("Workday", value.ToString()); 
                OnValueChanged();
            }
        }

        public int Lunch
        {
            get { return Convert.ToInt32(GetSetting("Lunch")); }
            set
            {
                SetSetting("Lunch", value.ToString()); 
                OnValueChanged();
            }
        }

        public void SetSetting(string key, string value)
        {
            XElement setting = doc.Element("configuration")
                .Elements("setting")
                .Single(e => e.Attribute("key").Value.Equals(key));
            setting.Attribute("value").Value = value;
            doc.Save(Directory.GetCurrentDirectory() + "/settings.config");
        }

        public object GetSetting(string key)
        {
            var settings = doc.Elements("configuration").Elements("setting");
            foreach (var setting in settings)
            {
                if (setting.Attribute("key").Value.Equals(key))
                    return setting.Attribute("value").Value;
            }
            return null;
        }

        private XElement GetDefaultXmlElements()
        {
            XElement config = new XElement("configuration",
                new XElement("setting",
                    new XAttribute("key", "Workday"),
                    new XAttribute("value", "8")),
                 new XElement("setting",
                     new XAttribute("key", "Lunch"),
                     new XAttribute("value", "30")));
            return config;
        }
    }
}