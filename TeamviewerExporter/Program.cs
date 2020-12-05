using CsvHelper;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace TeamviewerExporter
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Connecting to teamviewer web api ...");
                RestClient client = new RestClient(Properties.Settings.Default.BaseUrl);

                Console.Write("Getting device list...");
                RestRequest requestDevices = new RestRequest(Properties.Settings.Default.DevicesUrlPart, Method.GET);
                requestDevices.AddHeader("Authorization", string.Format("Bearer {0}", Properties.Settings.Default.AuthorizationToken));
                IRestResponse<TeamviewerExporter.Devices> responseDevices = client.Execute<TeamviewerExporter.Devices>(requestDevices);
                Console.WriteLine(responseDevices.ResponseStatus.ToString());

                if (!string.IsNullOrWhiteSpace(responseDevices.ErrorMessage))
                    Console.WriteLine(string.Format("Error Information: {0}", responseDevices.ErrorMessage));

                Console.Write("Getting groups list...");
                RestRequest requestGroups = new RestRequest(Properties.Settings.Default.GroupsUrlPart, Method.GET);
                requestGroups.AddHeader("Authorization", string.Format("Bearer {0}", Properties.Settings.Default.AuthorizationToken));
                IRestResponse<Groups> responseGroups = client.Execute<Groups>(requestGroups);
                Console.WriteLine(responseGroups.ResponseStatus.ToString());

                if (!string.IsNullOrWhiteSpace(responseGroups.ErrorMessage))
                    Console.WriteLine(string.Format("Error Information: {0}", responseGroups.ErrorMessage));

                Console.WriteLine(string.Format("Got {0} entries and {1} groups.", responseDevices?.Data?.devices?.Count ?? -1, responseGroups?.Data?.groups?.Count ?? -1));

                Console.WriteLine("Adding information to original entries...");

                for (int i = 0; i < responseDevices.Data.devices.Count; i++)
                {
                    // set cleaned teamviewer id
                    if(char.IsLetter(responseDevices.Data.devices[i].remotecontrol_id[0]))
                    {
                        responseDevices.Data.devices[i].remotecontrol_id_clean = responseDevices.Data.devices[i].remotecontrol_id.Substring(1);
                    }
                    else
                    {
                        responseDevices.Data.devices[i].remotecontrol_id_clean = responseDevices.Data.devices[i].remotecontrol_id;
                    }

                    // write group name
                    Group inGroup = responseGroups?.Data?.groups?.FirstOrDefault(w => w.id == responseDevices.Data.devices[i].groupid);
                    if(inGroup != null)
                    {
                        responseDevices.Data.devices[i].groupname = inGroup.name;
                    }
                }

                Console.WriteLine("Writing json object to csv file 'original.csv'");
                //TextWriter tw = File.CreateText("original.csv");

                TextWriter tw = new StreamWriter("original.csv", false, Encoding.UTF8);

                CsvWriter csv = new CsvWriter(tw);
                csv.WriteRecords(responseDevices.Data.devices);
                csv.Dispose();

                Console.WriteLine("Wrote original csv file - starting to write RDM import csv.");

                List<ExportToRdm> erdm = new List<ExportToRdm>();
                foreach (TeamviewerExporter.Device dev in responseDevices.Data.devices)
                {
                    ExportToRdm toAdd = new ExportToRdm();
                    toAdd.Description = dev.description;
                    toAdd.Name = dev.alias;
                    toAdd.TeamViewer_ID = dev.remotecontrol_id_clean;
                    toAdd.Host = toAdd.TeamViewer_ID;

                    if(dev.groupname != null)
                    {
                        toAdd.Group = dev.groupname;
                    }

                    erdm.Add(toAdd);
                }

                Console.WriteLine("Writing RDM compatible import file 'rdm.csv'");
                //xtWriter rtw = File.CreateText("rdm.csv");
                TextWriter rtw = new StreamWriter("rdm.csv", false, Encoding.UTF8);
                CsvWriter rcsv = new CsvWriter(rtw);
                rcsv.Configuration.RegisterClassMap(typeof(MapClass)); // add custom mapping for headlines
                rcsv.WriteRecords(erdm); // write complete object to disc as csv
                rcsv.Dispose();

                Console.WriteLine("Successfully wrote 'rdm.csv' - application finished successfully");
            }
            catch(Exception ex)
            {
                Console.WriteLine();
                Console.WriteLine(string.Format("SOMETHING HAPPENED{0}{1}", Environment.NewLine, ex.ToString()));
            }
            finally
            {
                Console.WriteLine("Press any enter to exit :-)");
                Console.ReadLine();
            }
        }
    }

    public class EncodingStringWriter : StringWriter
    {
        private readonly Encoding _encoding;

        public EncodingStringWriter(StringBuilder builder, Encoding encoding) : base(builder)
        {
            _encoding = encoding;
        }

        public override Encoding Encoding
        {
            get { return _encoding; }
        }
    }
}
