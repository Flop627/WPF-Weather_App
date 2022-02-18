using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;

namespace WpfApp3_01_02_2022
{


    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DetectIP detect;
        public MainWindow()
        {
            InitializeComponent();
            LeftMenu.Click += BtnOpen_Click;
            DisplayCurrentTime();
            GetCurrentCity();
            GetWeather();
        }

        void DisplayCurrentTime()
        {
            thisTime.Text = DateTime.Now.ToShortTimeString();

        }

        void GetCurrentCity()
        {
            string url = "https://extreme-ip-lookup.com/json/?key=OlK1dbL3W2JOEVZKgzce";
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();

            string response;
            using (StreamReader sr = new StreamReader(httpWebResponse.GetResponseStream()))
            {
                response = sr.ReadToEnd();
            }
            //MessageBox.Show(response);
            detect = JsonConvert.DeserializeObject<DetectIP>(response);

            

            thisCity.Text = detect.city;
            // MessageBox.Show(detect.lat + " " + detect.lon);                        
        }

        void GetWeather()
        {
            string url = $"https://api.openweathermap.org/data/2.5/onecall?lat={detect.lat}&lon={detect.lon}&units=metric&exclude=minutely,alerts&appid=090b7bf1b45d62326bce7b8a8095c193";

            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();

            string response;
            using (StreamReader sr = new StreamReader(httpWebResponse.GetResponseStream()))
            {
                response = sr.ReadToEnd();
            }

            MasterWeather master = JsonConvert.DeserializeObject<MasterWeather>(response);


            // Подставляем картинки и данные соответствующие погоде по часам
            for (int i = 0; i < Parent_For_Hourly.Children.Count; i++)
            {

                StackPanel sp = Parent_For_Hourly.Children[i] as StackPanel;
                TextBlock timeblock = sp.Children[0] as TextBlock;
                System.Windows.Controls.Image Dayly_image = sp.Children[1] as System.Windows.Controls.Image;
                TextBlock timeWeather = sp.Children[2] as TextBlock;


                timeWeather.Text = Convert.ToInt32((float)Convert.ToDouble(master.hourly[i].temp.Replace('.', ','))).ToString() + "°";

                if (master.hourly[i].weather[0].main == "Snow")
                {
                    Dayly_image.Source = snow_weather.Source;
                }
                else if (master.hourly[i].weather[0].main == "Rain")
                {
                    Dayly_image.Source = rain_weather.Source;
                }
                else if (master.hourly[i].weather[0].main == "Clouds")
                {
                    Dayly_image.Source = cloudy_weather.Source;
                }
                else if (master.hourly[i].weather[0].main == "Sunny")
                {
                    Dayly_image.Source = sunny_weather.Source;
                }

                DateTime time = new DateTime();
                time = time.AddSeconds(Convert.ToDouble(master.hourly[i].dt)).ToLocalTime();
                timeblock.Text = time.ToShortTimeString();
            }

          
            // Подставляем картинки и данные соответствующие погоде в этот день
            for (int i = 0; i < Weather_week.Children.Count; i++)
            {               
                
                StackPanel sp = Weather_week.Children[i] as StackPanel;
                System.Windows.Controls.Image Dayly_image = sp.Children[1] as System.Windows.Controls.Image;
                TextBlock DaylyWeatherDay = sp.Children[2] as TextBlock;
                TextBlock DaylyWeatherNight = sp.Children[3] as TextBlock;

                //MessageBox.Show(master.daily[i].weather[0].main.ToString()); Статус погоды для картинки

                if (master.daily[i].weather[0].main == "Snow")
                {
                    Dayly_image.Source = snow_weather.Source;
                }
                else if (master.daily[i].weather[0].main == "Rain")
                {
                    Dayly_image.Source = rain_weather.Source;
                }
                else if (master.daily[i].weather[0].main == "Clouds")
                {
                    Dayly_image.Source = cloudy_weather.Source;
                }
                else if (master.daily[i].weather[0].main == "Sunny")
                {
                    Dayly_image.Source = sunny_weather.Source;
                }


                DaylyWeatherDay.Text = Convert.ToInt32((float)Convert.ToDouble(master.daily[i].temp.day.Replace('.', ','))).ToString() + "°";
                DaylyWeatherNight.Text = Convert.ToInt32((float)Convert.ToDouble(master.daily[i].temp.night.Replace('.', ','))).ToString() + "°";
            }



            CurrentTemperature.Text = master.current.temp.Split('.')[0]+"°";
            Feels_like.Text = "It feels like " + master.current.feels_like.Split('.')[0] + "°";
        }


        //Перегрузка для получения погоды по другому городу
        void GetWeather(DetectIP detect)
        {
            string url = $"https://api.openweathermap.org/data/2.5/onecall?lat={detect.lat}&lon={detect.lon}&units=metric&exclude=minutely,alerts&appid=090b7bf1b45d62326bce7b8a8095c193";

            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();

            string response;
            using (StreamReader sr = new StreamReader(httpWebResponse.GetResponseStream()))
            {
                response = sr.ReadToEnd();
            }

            MasterWeather master = JsonConvert.DeserializeObject<MasterWeather>(response);

            // Подставляем картинки и данные соответствующие погоде по часам
            for (int i = 0; i < Parent_For_Hourly.Children.Count; i++)
            {
                //MessageBox.Show(Parent_For_Hourly.Children[i].ToString());

                StackPanel sp = Parent_For_Hourly.Children[i] as StackPanel;
                TextBlock timeblock = sp.Children[0] as TextBlock;
                System.Windows.Controls.Image Dayly_image = sp.Children[1] as System.Windows.Controls.Image;
                TextBlock timeWeather = sp.Children[2] as TextBlock;


                timeWeather.Text = Convert.ToInt32((float)Convert.ToDouble(master.hourly[i].temp.Replace('.', ','))).ToString() + "°";

                if (master.hourly[i].weather[0].main == "Snow")
                {
                    Dayly_image.Source = snow_weather.Source;
                }
                else if (master.hourly[i].weather[0].main == "Rain")
                {
                    Dayly_image.Source = rain_weather.Source;
                }
                else if (master.hourly[i].weather[0].main == "Clouds")
                {
                    Dayly_image.Source = cloudy_weather.Source;
                }
                else if (master.hourly[i].weather[0].main == "Sunny")
                {
                    Dayly_image.Source = sunny_weather.Source;
                }

                DateTime time = new DateTime();
                time = time.AddSeconds(Convert.ToDouble(master.hourly[i].dt)).ToLocalTime();
                timeblock.Text = time.ToShortTimeString();
            }


            // Подставляем картинки и данные соответствующие погоде в этот день
            for (int i = 0; i < Weather_week.Children.Count; i++)
            {
                StackPanel sp = Weather_week.Children[i] as StackPanel;
                System.Windows.Controls.Image Dayly_image = sp.Children[1] as System.Windows.Controls.Image;
                TextBlock DaylyWeatherDay = sp.Children[2] as TextBlock;
                TextBlock DaylyWeatherNight = sp.Children[3] as TextBlock;

                //MessageBox.Show(master.daily[i].weather[0].main.ToString()); проверяем статус погоды для картинки

                if (master.daily[i].weather[0].main == "Snow")
                {
                    Dayly_image.Source = snow_weather.Source;
                }
                else if (master.daily[i].weather[0].main == "Rain")
                {
                    Dayly_image.Source = rain_weather.Source;
                }
                else if (master.daily[i].weather[0].main == "Clouds")
                {
                    Dayly_image.Source = cloudy_weather.Source;
                }
                else if (master.daily[i].weather[0].main == "Sunny")
                {
                    Dayly_image.Source = sunny_weather.Source;
                }


                DaylyWeatherDay.Text = Convert.ToInt32((float)Convert.ToDouble(master.daily[i].temp.day.Replace('.', ','))).ToString() + "°";
                DaylyWeatherNight.Text = Convert.ToInt32((float)Convert.ToDouble(master.daily[i].temp.night.Replace('.', ','))).ToString() + "°";
            }



            CurrentTemperature.Text = master.current.temp.Split('.')[0] + "°";
            Feels_like.Text = "It feels like " + master.current.feels_like.Split('.')[0] + "°";
        }




        private void BtnOpen_Click(object sender, RoutedEventArgs e)
        { 
            Storyboard sr = Resources["OpenMenu"] as Storyboard;
            sr.Begin(Menu);
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            StoryboardClose();
        }

        private void StoryboardClose()
        {
            Storyboard left = Resources["CloseMenu"] as Storyboard;
            left.Begin(Menu);
        }


        private void Search_Click(object sender, MouseButtonEventArgs e)
        {
            SearchCity();
        }

        // Поиск по городам
        private void SearchCity()
        {
            if (string.IsNullOrEmpty(cityToSearch.Text))
            {
                MessageBox.Show("Строка пуста");
                return;
            }

            try
            {
                string url = $"https://eu1.locationiq.com/v1/search.php?key=pk.c3f6087c389649f6b1b01a3fc541215a&q={cityToSearch.Text}&format=json&country=Russia";
                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                string response;
                using (StreamReader sr = new StreamReader(httpWebResponse.GetResponseStream()))
                {
                    response = sr.ReadToEnd();
                }

                List<MasterSearchCityCoord> searchMaster = new List<MasterSearchCityCoord>();
                var data = JsonConvert.DeserializeObject<List<MasterSearchCityCoord>>(response);

                DetectIP searchDetect = new DetectIP();
                searchDetect.lat = data[0].lat.ToString();
                searchDetect.lon = data[0].lon.ToString();
                searchDetect.city = data[0].display_name.Split(',')[0];
                thisCity.Text = searchDetect.city;
                cityToSearch.Text = null;
                GetWeather(searchDetect);

                StoryboardClose();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private class MasterWeather
        {
            public string lat;
            public string lon;
            public string timezone;
            public string timezone_offset;
            public CurrentWeather current;
            public HourlyWeather[] hourly;
            public DailyWeather[] daily;

        }
        private class DailyWeather
        {
            public Temp temp { get; set; }
            public List<Weather> weather { get; set; }

        }
        private class Temp
        {
            public string day;
            public string night;
        }

        private class Weather
        {
            public string main { get; set; }
        }

        private class CurrentWeather
        {
            public string temp;
            public string feels_like;
        }
        private class HourlyWeather
        {
            public string temp;
            public string dt;
            public List<Weather> weather { get; set; }
        }

        private class DetectIP
        {
            public string city;
            public string lon;
            public string lat;
        }

        private class MasterSearchCityCoord
        {
            public double lat;
            public double lon;
            public string display_name;
        }
    }
}
