using System;
using System.Collections.Generic;
using System.Text;

namespace CurseForgeToMultiMC.Models
{
    public class mmcCachedRequire
    {

        public string suggests { get; set; }
        public string uid { get; set; }
        public string equals { get; set; }
    }

    public class mmcComponent
    {
        public string cachedName { get; set; }
        public string cachedVersion { get; set; }
        public bool cachedVolatile { get; set; }
        public bool dependencyOnly { get; set; }
        public string uid { get; set; }
        public string version { get; set; }
        public List<mmcCachedRequire> cachedRequires { get; set; }
        public bool? important { get; set; }
    }

    public class mmcPack
    {
        public List<mmcComponent> components { get; set; }
        public int formatVersion { get; set; }
    }
}
