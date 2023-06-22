using System.Net;
using System.IO;
using static Weather.Form1;

using System.Text.Json.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using Newtonsoft.Json;

namespace Weather
{
    public partial class Form1 : Form
    {
        double tickTime;
        public Form1()
        {
            TopMost = true;
            InitializeComponent();
            this.BackColor = Color.Gray;
            this.TransparencyKey = Color.Gray;
            this.StartPosition = FormStartPosition.Manual;
            //Верхний левый угол экрана
            Point pt = Screen.PrimaryScreen.WorkingArea.Location;
            //Перенос в нижний правый угол экрана без панели задач
            pt.Offset(Screen.PrimaryScreen.WorkingArea.Width, Screen.PrimaryScreen.WorkingArea.Height);
            //Перенос в местоположение верхнего левого угла формы, чтобы её правый нижний угол попал в правый нижний угол экрана
            pt.Offset(-this.Width, -this.Height);
            //Новое положение формы
            this.Location = pt;
            this.FormBorderStyle = FormBorderStyle.None;    

        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            _timer.Tick += onTimerTick;
            _timer.Start();

        }

        private void onTimerTick(object? sender, EventArgs e)
        {
            var now = DateTime.Now;
            label1.Text = $"{now.ToLongTimeString()}";
            //============================================
            DateTime nowTime = DateTime.Now;
            DateTime specificTime = new DateTime(nowTime.Year, nowTime.Month, nowTime.Day, 18, 0, 0, 0);
            if (nowTime > specificTime)
                label3.Text = "";
            else
            {
                tickTime = (specificTime - nowTime).TotalMilliseconds;
                int i = (int)tickTime;
                int h = 0;
                int m = 0;
                int s = 0;
                h = i / 3600000;
                m = (i - (h * 3600000)) / 60000;
                s = ((i - ((h * 3600000) + (m * 60000))) / 1000) + 1;
                --i;
                label3.Text = $"{(h).ToString()}:{(m).ToString()}:{(s).ToString()}";
            }

        }

        System.Windows.Forms.Timer _timer = new System.Windows.Forms.Timer { Interval = 1000 };

        private void Form1_Load(object sender, EventArgs e)
        {
            WebRequest request = HttpWebRequest.Create("http://api.weatherapi.com/v1/forecast.json?key=db427efe758a4ad38fd75818231906&q=Dnipropetrovsk&days=1&aqi=yes&alerts=yes\r\n");
            WebResponse response = request.GetResponse();
            StreamReader reader = new StreamReader(response.GetResponseStream());
            string Current_JSON = reader.ReadToEnd();
            Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(Current_JSON);
            labelNow.Text = $"Now is {myDeserializedClass.current.temp_c.ToString()}°";
            label4.Text = $"Preasure is {(myDeserializedClass.current.pressure_mb * 0.75).ToString()}";
            Image image = GetImageFromPicPath(myDeserializedClass.current.condition.icon);
            pictureBox1.Image = image;
            timer2.Start();
        }

        public static Image GetImageFromPicPath(string strUrl)
        {
            strUrl = "https:" + strUrl;
            using (WebResponse wrFileResponse = WebRequest.Create(strUrl).GetResponse())
            using (Stream objWebStream = wrFileResponse.GetResponseStream())
            {
                MemoryStream ms = new MemoryStream();
                objWebStream.CopyTo(ms, 8192);
                return System.Drawing.Image.FromStream(ms);
            }
        }

        private void onTimerTick2(object sender, EventArgs e)
        {
            WebRequest request = HttpWebRequest.Create("http://api.weatherapi.com/v1/forecast.json?key=db427efe758a4ad38fd75818231906&q=Dnipropetrovsk&days=1&aqi=yes&alerts=yes\r\n");
            WebResponse response = request.GetResponse();
            StreamReader reader = new StreamReader(response.GetResponseStream());
            string Current_JSON = reader.ReadToEnd();
            Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(Current_JSON);
            labelNow.Text = $"Now is {myDeserializedClass.current.temp_c.ToString()}°";
            label4.Text = $"Preasure is {(myDeserializedClass.current.pressure_mb * 0.75).ToString()}";
            Image image = GetImageFromPicPath(myDeserializedClass.current.condition.icon);
            pictureBox1.Image = image;

        }
    }
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class AirQuality
    {
        public double co { get; set; }
        public double no2 { get; set; }
        public double o3 { get; set; }
        public double so2 { get; set; }
        public double pm2_5 { get; set; }
        public double pm10 { get; set; }

        [JsonProperty("us-epa-index")]
        public int usepaindex { get; set; }

        [JsonProperty("gb-defra-index")]
        public int gbdefraindex { get; set; }
    }

    public class Alerts
    {
        public List<object> alert { get; set; }
    }

    public class Astro
    {
        public string sunrise { get; set; }
        public string sunset { get; set; }
        public string moonrise { get; set; }
        public string moonset { get; set; }
        public string moon_phase { get; set; }
        public string moon_illumination { get; set; }
        public int is_moon_up { get; set; }
        public int is_sun_up { get; set; }
    }

    public class Condition
    {
        public string text { get; set; }
        public string icon { get; set; }
        public int code { get; set; }
    }

    public class Current
    {
        public int last_updated_epoch { get; set; }
        public string last_updated { get; set; }
        public double temp_c { get; set; }
        public double temp_f { get; set; }
        public int is_day { get; set; }
        public Condition condition { get; set; }
        public double wind_mph { get; set; }
        public double wind_kph { get; set; }
        public int wind_degree { get; set; }
        public string wind_dir { get; set; }
        public double pressure_mb { get; set; }
        public double pressure_in { get; set; }
        public double precip_mm { get; set; }
        public double precip_in { get; set; }
        public int humidity { get; set; }
        public int cloud { get; set; }
        public double feelslike_c { get; set; }
        public double feelslike_f { get; set; }
        public double vis_km { get; set; }
        public double vis_miles { get; set; }
        public double uv { get; set; }
        public double gust_mph { get; set; }
        public double gust_kph { get; set; }
        public AirQuality air_quality { get; set; }
    }

    public class Day
    {
        public double maxtemp_c { get; set; }
        public double maxtemp_f { get; set; }
        public double mintemp_c { get; set; }
        public double mintemp_f { get; set; }
        public double avgtemp_c { get; set; }
        public double avgtemp_f { get; set; }
        public double maxwind_mph { get; set; }
        public double maxwind_kph { get; set; }
        public double totalprecip_mm { get; set; }
        public double totalprecip_in { get; set; }
        public double totalsnow_cm { get; set; }
        public double avgvis_km { get; set; }
        public double avgvis_miles { get; set; }
        public double avghumidity { get; set; }
        public int daily_will_it_rain { get; set; }
        public int daily_chance_of_rain { get; set; }
        public int daily_will_it_snow { get; set; }
        public int daily_chance_of_snow { get; set; }
        public Condition condition { get; set; }
        public double uv { get; set; }
        public AirQuality air_quality { get; set; }
    }

    public class Forecast
    {
        public List<Forecastday> forecastday { get; set; }
    }

    public class Forecastday
    {
        public string date { get; set; }
        public int date_epoch { get; set; }
        public Day day { get; set; }
        public Astro astro { get; set; }
        public List<Hour> hour { get; set; }
    }

    public class Hour
    {
        public int time_epoch { get; set; }
        public string time { get; set; }
        public double temp_c { get; set; }
        public double temp_f { get; set; }
        public int is_day { get; set; }
        public Condition condition { get; set; }
        public double wind_mph { get; set; }
        public double wind_kph { get; set; }
        public int wind_degree { get; set; }
        public string wind_dir { get; set; }
        public double pressure_mb { get; set; }
        public double pressure_in { get; set; }
        public double precip_mm { get; set; }
        public double precip_in { get; set; }
        public int humidity { get; set; }
        public int cloud { get; set; }
        public double feelslike_c { get; set; }
        public double feelslike_f { get; set; }
        public double windchill_c { get; set; }
        public double windchill_f { get; set; }
        public double heatindex_c { get; set; }
        public double heatindex_f { get; set; }
        public double dewpoint_c { get; set; }
        public double dewpoint_f { get; set; }
        public int will_it_rain { get; set; }
        public int chance_of_rain { get; set; }
        public int will_it_snow { get; set; }
        public int chance_of_snow { get; set; }
        public double vis_km { get; set; }
        public double vis_miles { get; set; }
        public double gust_mph { get; set; }
        public double gust_kph { get; set; }
        public double uv { get; set; }
        public AirQuality air_quality { get; set; }
    }

    public class Location
    {
        public string name { get; set; }
        public string region { get; set; }
        public string country { get; set; }
        public double lat { get; set; }
        public double lon { get; set; }
        public string tz_id { get; set; }
        public int localtime_epoch { get; set; }
        public string localtime { get; set; }
    }

    public class Root
    {
        public Location location { get; set; }
        public Current current { get; set; }
        public Forecast forecast { get; set; }
        public Alerts alerts { get; set; }
    }

}