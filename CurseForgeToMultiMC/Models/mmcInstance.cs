using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CurseForgeToMultiMC.Models
{
    public class mmcInstance {
        public string InstanceType { get; set; }
        public bool JoinServerOnLaunch { get; set; }
        public bool OverrideCommands { get; set; }
        public bool OverrideConsole { get; set; }
        public bool OverrideGameTime { get; set; }
        public bool OverrideJavaArgs { get; set; }
        public bool OverrideJavaLocation { get; set; }
        public bool OverrideMemory { get; set; }
        public bool OverrideNativeWorkArounds { get; set; }
        public bool OverrideWindow { get; set; }
        public string iconKey { get; set; }
        public string name { get; set; } 
        public string notes { get; set; }

        public string serializeInstance() {
            string serializedInstance = "";
            foreach (PropertyInfo propertyInfo in this.GetType().GetProperties()) {
                var currentLine = propertyInfo.Name + "=" + propertyInfo.GetValue(this);
                serializedInstance += currentLine + Environment.NewLine;
            }
            return serializedInstance;
        }
    }
}
