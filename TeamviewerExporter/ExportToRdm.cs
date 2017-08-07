using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TeamviewerExporter
{
    public class ExportToRdm
    {
        public ExportToRdm()
        {
            ConnectionType = "TeamViewer";
            //OpenEmbedded = false;
            //UserName = Environment.UserName;
            
            if(!string.IsNullOrWhiteSpace(Properties.Settings.Default.ImportDefaultPassword))
            {
                TeamViewer_Password = Properties.Settings.Default.ImportDefaultPassword;
            }
        }

        public string Host { get; set; }
        public string Name { get; set; }
        public string ConnectionType { get; set; }
        public string Group { get; set; }
        public string Description { get; set; }
        //public bool OpenEmbedded { get; set; }
        //public string UserName { get; set; }
        //public string Domain { get; set; }
        public string TeamViewer_Password { get; set; }
        public string TeamViewer_ID { get; set; }
    }

    public sealed class MapClass : CsvClassMap<ExportToRdm>
    {
        /// <summary>
        /// this mapping class is needed to overwrite header names to the needed ones from remotedesktopmanager
        /// </summary>
        public MapClass()
        {
            base.AutoMap();
            Map(m => m.TeamViewer_ID).Name(@"TeamViewer\ID");
            Map(m => m.TeamViewer_Password).Name(@"TeamViewer\Password");
        }
    }
}
