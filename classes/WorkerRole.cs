using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using System.IO;
using System.Net;
using System.Text;

namespace PathFinding
{

    /*
     * Worker role constantly runs on Azure server, 
     * ready to perform pathfinding once the specific room is entered
     */

    public class WorkerRole : RoleEntryPoint
    {

        private Graph my_graph;

        private string mobile_services_host = "[your mobile services host]";
        private string security_key = "[your key]";

        private string table_name = "offices";
        private string filter = "complete%20eq%20false%20and%20cancelled%20eq%20false";
        
        private int start_office = 0;
        private string node_host = "[your nodebot host]";

        private string container_name = "maps";

        ManualResetEvent CompletedEvent = new ManualResetEvent(false);


        public override void Run()
        {
            string sURL = "https://" + mobile_services_host + "/tables/" + table_name + "?$filter=(" + filter + ")";

            while (true)
            {
                Thread.Sleep(10000);

                List<Record> records = getNewRecords(sURL);

                foreach (Record record in records)
                { 
                    Path shortest_path = findPath(Convert.ToInt32(record.get("office")));
                    sendInstructions(shortest_path);
                    setRequestToComplete(record.get("id"));
                }
            }
        }

        /*
         * This function is for testing separately from web-appp and the database.
         * It creates the list of Record objects with given office Numbers
         * 
         */
        private List <Record> getDummieRecords(List <int> office_numbers)
        {
            List<Record> records = new List<Record>();
            foreach (int number in office_numbers)
            {
                records.Add(new Record("office:" + Convert.ToString(number)));
            }

            return records;
        }

        /*
         * Gets the list of new requests from the database
         */
        private List<Record> getNewRecords(string sURL)
        {
            List<Record> records = new List<Record>();

            HttpWebRequest wrGETURL;
            wrGETURL = (HttpWebRequest)WebRequest.Create(sURL);
            wrGETURL.Accept = "application/json";
            wrGETURL.Headers.Add("X-ZUMO-APPLICATION", security_key);
            wrGETURL.Host = mobile_services_host;

            Stream objStream;
            objStream = wrGETURL.GetResponse().GetResponseStream();

            StreamReader objReader = new StreamReader(objStream);

            string sLine = "";
            while (sLine != null)
            {
                sLine = objReader.ReadLine();
                if (sLine != null && sLine != "[]")
                {
                    string[] record_strings = sLine.Split(new Char[] { '{', '}' });
                    foreach (string record_info in record_strings)
                    {
                        if (record_info.Length > 2)
                        {
                            Record record = new Record(record_info);
                            records.Add(record);
                            setRequestToComplete(record.get("id"));
                        }

                    }

                }

            }
            objStream.Close();

            return records;
        }


        /*
         * Represents a record in a database 
         */
        private class Record
        {
            private Dictionary<string, string> properties;

            public Dictionary<string, string> Properties
            {
                get { return properties; }
                set { properties = value; }
            }

            public Record(string recordinfo)
            {
                properties = new Dictionary<string, string>();
                string[] values = recordinfo.Split(',');
                foreach (string value in values)
                {
                    string property = value.Replace("\"", "");
                    property = property.Replace("[", "");
                    property = property.Replace("[", "");
                    string[] parts = property.Split(':');
                    if (parts.Length == 2)
                    {
                        properties.Add(parts[0], parts[1]);
                    }
                }
            }

            public string get(string property_name)
            {
                return properties[property_name];
            }


        }

        /*
         * finds the path between start office and end office specified
         * @param end_office end office
         */
        private Path findPath(int end_office)
        {

            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
                    CloudConfigurationManager.GetSetting("StorageConnectionString"));
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer container = blobClient.GetContainerReference(container_name);

            Node start_node = my_graph.findNodeByOfficeNumber(start_office);
            Node end_node = my_graph.findNodeByOfficeNumber(end_office);
            MinCostPathFinder pathfinder = new MinCostPathFinder();
            Path shortest_path = pathfinder.findPath(start_node, end_node);

            return shortest_path;
        }

        /*
         * OPTIONAL:
         * this function is for testing the results of the serach. It graphically draws the path on a map
         */
        private void storeResultingPath(Path path_to_draw)
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
            CloudConfigurationManager.GetSetting("StorageConnectionString"));
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer container = blobClient.GetContainerReference(container_name);


            //upload
            CloudBlockBlob mapBlob = container.GetBlockBlobReference("map.svg");

            // Save blob contents to a file.
            using (var fileStream = System.IO.File.OpenWrite(@"map.svg"))
            {
                mapBlob.DownloadToStream(fileStream);
            }
            SVGcreator.drawPath("path.svg", "map.svg", path_to_draw);
            CloudBlockBlob graphblob = container.GetBlockBlobReference("path.svg");
            using (var fileStream = System.IO.File.OpenRead(@"path.svg"))
            {

                graphblob.UploadFromStream(fileStream);
            }
        }

        /*
         * Sends the list of directions to the robot, using the nodebot host specified 
         * @param path the path to be sent to the robot
         */
        private void sendInstructions(Path path)
        {

            string body = path.getJSONDirections(my_graph.Scale);
            string sURL = "http://" + node_host + "/robot/list";

            HttpWebRequest wrPOSTURL;
            wrPOSTURL = (HttpWebRequest)WebRequest.Create(sURL);
            wrPOSTURL.Method = "POST";
            wrPOSTURL.Accept = "application/json";
            wrPOSTURL.ContentType = "application/json";
            wrPOSTURL.Host = node_host;
            wrPOSTURL.ContentLength = body.Length;
            WebProxy myProxy = new WebProxy();
            myProxy.BypassProxyOnLocal = true;

            wrPOSTURL.Proxy = myProxy;


            Stream stream = wrPOSTURL.GetRequestStream();
            byte[] byteArray = Encoding.UTF8.GetBytes(body);
            stream.Write(byteArray, 0, byteArray.Length);

            //TODO: CHECK IF RESPONSE IS OK
            //WebResponse resp = wrPOSTURL.GetResponse();
            stream.Close();

        }

        /*
         * Marks the request with specified id as complete in a database 
         * @param id record id
         */
        private void setRequestToComplete(string id)
        {

            string body = "{\"complete\":true}";
            string sURL = "https://" + mobile_services_host + "/tables/" + table_name + "/" + id;

            HttpWebRequest wrPATCHURL;
            wrPATCHURL = (HttpWebRequest)WebRequest.Create(sURL);
            wrPATCHURL.Method = "PATCH";
            wrPATCHURL.Accept = "application/json";
            wrPATCHURL.ContentType = "application/json";
            wrPATCHURL.Headers.Add("X-ZUMO-APPLICATION", security_key);
            wrPATCHURL.Host = mobile_services_host;
            wrPATCHURL.ContentLength = body.Length;

            Stream stream = wrPATCHURL.GetRequestStream();
            byte[] byteArray = Encoding.UTF8.GetBytes(body);
            stream.Write(byteArray, 0, byteArray.Length);

            //TODO: CHECK IF RESPONSE IS OK
            WebResponse resp = wrPATCHURL.GetResponse();
            stream.Close();
        }


        public override bool OnStart()
        {
            // Set the maximum number of concurrent connections 
            ServicePointManager.DefaultConnectionLimit = 12;
            createGraph();

            return base.OnStart();
        }

        /*
         * Converts the map that is already in the storage into a graph object
         */
        private void createGraph()
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
            CloudConfigurationManager.GetSetting("StorageConnectionString"));
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer container = blobClient.GetContainerReference(container_name);


            //upload
            CloudBlockBlob mapBlob = container.GetBlockBlobReference("map.svg");

            // Save blob contents to a file.
            using (var fileStream = System.IO.File.OpenWrite(@"map.svg"))
            {
                mapBlob.DownloadToStream(fileStream);
            }

            string filePath = "map.svg";

            my_graph = Converter.convert(filePath);
        }

        public override void OnStop()
        {
            CompletedEvent.Set();
            base.OnStop();


        }
    }
}
