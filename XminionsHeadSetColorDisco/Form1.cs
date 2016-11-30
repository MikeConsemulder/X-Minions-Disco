//assigning all the things we need in this program
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.IO;
using System.Web.Script.Serialization;
using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Threading;
using System.Timers;

//opening the project
namespace XminionsHeadSetColorDisco
{

    public partial class Form1 : Form
    {
        //setup for some important stuff
        //
        //handleId so we can keep track of the last setting
        public int handleId;
        //colorJson for later comparising with server
        public string colorJson;
        //setup for the hotkeys
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vk);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);
        //setup for the gameObject class
        GameObject gameObject = new GameObject();
        //modifieres for the hotkeys, so we can easily call them
        enum KeyModifier
        {

            None = 0,
            Alt = 1,
            Control = 2,
            Shift = 4,
            WinKey = 8
        }

        public Form1()
        {

            this.WindowState = FormWindowState.Minimized;
            this.ShowInTaskbar = false;
            //setup the application
            InitializeComponent();
            //write the default color file
            writeColorJson();
            //register the hotkeys
            RegisterHotKey(this.Handle, 0, (int)KeyModifier.Shift, Keys.F1.GetHashCode());
            RegisterHotKey(this.Handle, 1, (int)KeyModifier.Shift, Keys.F2.GetHashCode());
            RegisterHotKey(this.Handle, 2, (int)KeyModifier.Shift, Keys.F3.GetHashCode());
            RegisterHotKey(this.Handle, 3, (int)KeyModifier.Shift, Keys.F4.GetHashCode());
            RegisterHotKey(this.Handle, 4, (int)KeyModifier.Shift, Keys.F5.GetHashCode());
            RegisterHotKey(this.Handle, 5, (int)KeyModifier.Shift, Keys.F6.GetHashCode());
            RegisterHotKey(this.Handle, 6, (int)KeyModifier.Shift, Keys.F7.GetHashCode());
            RegisterHotKey(this.Handle, 7, (int)KeyModifier.Shift, Keys.F8.GetHashCode());
            RegisterHotKey(this.Handle, 8, (int)KeyModifier.Shift, Keys.F9.GetHashCode());
            RegisterHotKey(this.Handle, 9, (int)KeyModifier.Shift, Keys.F10.GetHashCode());
            RegisterHotKey(this.Handle, 10, (int)KeyModifier.Shift, Keys.F11.GetHashCode());
            RegisterHotKey(this.Handle, 11, (int)KeyModifier.Shift, Keys.F11.GetHashCode());
            //register the game event, (just a 0 for the reason that there has to be something there :P)
            SubmitData(getIpFromJson, "0", "bind_game_event");
        }

        /*
        * See if a hotkey is pressed,
        * If it's pressed, search for id
        * Execute the game_event function with the value of the id + 1 to match the data required for the color event
        */
        protected override void WndProc(ref Message m)
        {

            base.WndProc(ref m);
            if (m.Msg == 0x0312)
            {
                Keys key = (Keys)(((int)m.LParam >> 16) & 0xFFFF);
                KeyModifier modifier = (KeyModifier)((int)m.LParam & 0xFFFF);
                int id = m.WParam.ToInt32();
                //assign the id to the global handleId
                handleId = id;
                //execute the game event
                SubmitData(getIpFromJson, (id).ToString(), "game_event");
            }
        }

        //when the form closses, remove the hotkeys from the hotkeylist
        private void ExampleForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            UnregisterHotKey(this.Handle, 0);
        }

        //create the button method that will delete the game from the gamesence software
        private void button1_Click(object sender, EventArgs e)
        {
            SubmitData(getIpFromJson, "50", "remove_game");
        }

        public string getIpFromJson
        {
            get
            {
                //get the json where the URL is saved with the system variable (%programdata%)
                string jsonIpUrl = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "/SteelSeries/SteelSeries Engine 3/coreProps.json";
                //make a dictionary so we can acces later by searching for the "address" key instead of just [0] or [1]
                var dictionary = new Dictionary<string, object>();
                //assign variable
                string json;
                //open and read the json
                using (StreamReader r = new StreamReader(jsonIpUrl))
                {
                    json = r.ReadToEnd();
                }
                //put brackets around the json so c# won't be an assehole
                json = "[" + json + "]";
                //again c# is an assehole so we have to put a frigglethingy infront and after a comma-sign
                json = json.Replace(",", "},{");
                //Parse the json into an array so we can cycle trough it
                JArray a = JArray.Parse(json);
                //Cycle through the array
                foreach (JObject o in a.Children<JObject>())
                {
                    //look in each subobjec to find the values and put them into the librabry
                    foreach (JProperty p in o.Properties())
                    {
                        string name = p.Name;
                        string value = (string)p.Value;
                        dictionary.Add(name, value);
                    }
                }
                //look for the address object and assign it to a string
                object ipAddress = dictionary["address"];
                ipAddress = ipAddress.ToString();
                //return the whole address included "Http://"
                return "http://" + ipAddress.ToString();
            }
        }

        //the main object where we're gonna put some important stuff
        public class GameObject
        {
            public string gameName;
            public string gameEvent;
            public string method;
            public string jsonString;
            public string colorValue;
            public void setJsonString()
            {
                switch (method)
                {
                    case "bind_game_event":
                        jsonString = "{\"game\":\"" + gameName + "\"";
                        jsonString += ",\"event\":\"" + gameEvent + "\"";
                        jsonString += ",\"min_value\":0";
                        jsonString += ",\"max_value\":100";
                        jsonString += ",\"handlers\":[{";
                        jsonString += "\"device-type\":\"headset\"";
                        jsonString += ",\"zone\":\"earcups\"";
                        jsonString += ",\"mode\":\"color\"";
                        jsonString += ",\"color\":[";
                        jsonString += generateColorString();
                        jsonString += "]}]}";
                        break;
                    case "game_event":
                        jsonString = "{\"game\":\"" + gameName + "\",\"event\":\"" + gameEvent + "\",\"data\": {\"value\": " + colorValue + "}}";
                        break;
                    case "remove_game":
                        jsonString = "{\"game\":\"" + gameName + "\",\"event\":\"" + gameEvent + "\"}";
                        break;
                    default:
                        jsonString = "{\"game\":\"" + gameName + "\"}";
                        break;
                }
            }
            private string generateColorString()
            {
                //read the color.txt from the programdata folder
                StreamReader sr = new StreamReader(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "/SteelSeries/SteelSeries Engine 3/color.txt");
                String line = sr.ReadToEnd();
                sr.Dispose();
                //return the json
                return line;
            }
        }

        private void writeColorJson()
        {
            string userName = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            userName = userName.Substring(userName.LastIndexOf('\\') + 1);
            Console.WriteLine(userName);
            //
            //see if the color.txt file exist. otherwise make it
            string curFile = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "/SteelSeries/SteelSeries Engine 3/color.txt";
            if (!File.Exists(curFile))
            {
                //Meh.. c# isn't letting me make a nice object to json string... meeehhhhh :(
                string tempJson;
                tempJson = "{\"low\":0,\"high\":0,\"color\":{\"red\":255,\"green\":0,\"blue\":0}}";
                tempJson += ",{\"low\":1,\"high\":1,\"color\":{\"red\":0,\"green\":255,\"blue\":0}}";
                tempJson += ",{\"low\":2,\"high\":2,\"color\":{\"red\":0,\"green\":0,\"blue\":255}}";
                tempJson += ",{\"low\":3,\"high\":3,\"color\":{\"red\":255,\"green\":0,\"blue\":255}}";
                tempJson += ",{\"low\":4,\"high\":4,\"color\":{\"red\":255,\"green\":255,\"blue\":0}}";
                tempJson += ",{\"low\":5,\"high\":5,\"color\":{\"red\":255,\"green\":130,\"blue\":0}}";
                tempJson += ",{\"low\":6,\"high\":6,\"color\":{\"red\":0,\"green\":255,\"blue\":255}}";
                tempJson += ",{\"low\":7,\"high\":7,\"color\":{\"red\":130,\"green\":0,\"blue\":130}}";
                tempJson += ",{\"low\":8,\"high\":8,\"color\":{\"red\":0,\"green\":255,\"blue\":130}}";
                tempJson += ",{\"low\":9,\"high\":9,\"color\":{\"red\":255,\"green\":115,\"blue\":189}}";
                tempJson += ",{\"low\":10,\"high\":10,\"color\":{\"red\":255,\"green\":127,\"blue\":84}}";
                string[] lines = { tempJson };
                //send the default values to the file
                writeColorToFile(lines);
            }
        }

        private void writeColorToFile(string[] lineData)
        {
            //Write a text file to the programdata folder so we can store some settings about the colors there later on.
            System.IO.File.WriteAllLines(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "/SteelSeries/SteelSeries Engine 3/color.txt", lineData);
            SubmitData(getIpFromJson, "0", "bind_game_event");
            SubmitData(getIpFromJson, handleId.ToString(), "game_event");
        }

        private void SubmitData(string ipUrl, string colorValueFromHotkey, string method)
        {
            try
            {
                //assign some stuff to the object (remove this later please Mr future Mike)
                gameObject.gameName = "X_MINIONS";
                gameObject.gameEvent = "COLORDISCO";
                gameObject.method = method;
                gameObject.colorValue = colorValueFromHotkey;
                gameObject.setJsonString();
                //Serialize the json so we can send it to the server
                JavaScriptSerializer js = new JavaScriptSerializer();
                ASCIIEncoding encoding = new ASCIIEncoding();
                string postData = gameObject.jsonString;
                byte[] data = encoding.GetBytes(postData);
                //Prepare the server so it can receive the data
                WebRequest request = WebRequest.Create(ipUrl + "/" + gameObject.method);
                request.Method = "POST";
                request.ContentType = "application/json";
                request.ContentLength = data.Length;
                //Send the data to the server
                Stream stream = request.GetRequestStream();
                stream.Write(data, 0, data.Length);
                stream.Close();
                //Get the response from the server
                WebResponse response = request.GetResponse();
                stream = response.GetResponseStream();
                //Analyse the response and make it into an readable string so we can print it in the console
                StreamReader sr = new StreamReader(stream);
                Console.WriteLine(sr.ReadToEnd());
                //close the request
                sr.Close();
                stream.Close();
            }
            catch (Exception ex)
            {
                //write the error message if something goes wrong
                Console.WriteLine("Error : " + ex.Message);
            }

        }

        private void heartbeatTimer_Tick(object sender, EventArgs e)
        {
            //keep the game alive so the color won't change back to it's original
            SubmitData(getIpFromJson, "50", "game_heartbeat");
        }

        public string readLocalColorJson()
        {
            //read the color.txt from the programdata folder
            StreamReader sr = new StreamReader(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "/SteelSeries/SteelSeries Engine 3/color.txt");
            string line = sr.ReadToEnd();
            sr.Dispose();
            //return the json
            colorJson = line;
            return line;
        }
    }
}
