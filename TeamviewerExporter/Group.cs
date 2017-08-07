using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TeamviewerExporter
{
    public class Group
    {
        public string id { get; set; }
        public string name { get; set; }
        public string permissions { get; set; }
    }

    public class Groups
    {
        public List<Group> groups { get; set; }
    }
}
