using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TeamviewerExporter
{
    public class Device
    {
        public string remotecontrol_id { get; set; }
        public string device_id { get; set; }
        public string alias { get; set; }
        public string groupid { get; set; }
        public string online_state { get; set; }
        public bool assigned_to { get; set; }
        public string supported_features { get; set; }
        public string description { get; set; }

        public string remotecontrol_id_clean { get; set; }
        public string groupname { get; set; }
    }

    public class Devices
    {
        public List<Device> devices { get; set; }
    }
}
