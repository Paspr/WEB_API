using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Script.Serialization;
using System.Text.RegularExpressions;

namespace WeatherTime.Controllers
{
    public class WeatherTimeController : ApiController
    {
        public string GetWeather(int zip) 
        {
            // check input parameter
            if (!Regex.IsMatch(zip.ToString(), "^[0-9]{5}(?:-[0-9]{4})?$"))
            {
                return "Invalid ZIP-code";
            }
            else
            {
                string openWeatherAPIkey = "fa1bfd3f8508b13e38ac367cc6decd2b";
                string googleTimezoneAPIkey = "AIzaSyBPjXzityr_uQDoA1OtZ9JK9FisnG4h9lg";
                // prepare for Open Weather request
                string weatherURL = $"http://api.openweathermap.org/data/2.5/weather?zip={zip}&units=metric&appid={openWeatherAPIkey}";
                WebClient client = new WebClient();
                string jsonWeather;
                // handle the error "city not found" from the server
                try
                { 
                    jsonWeather = client.DownloadString(weatherURL);
                }
                catch (Exception e)
                {
                    return "ERROR " + e;
                }
                Models.WeatherTime.OpenWeather Weather = (new JavaScriptSerializer()).Deserialize<Models.WeatherTime.OpenWeather>(jsonWeather);
                var cityName = Weather.name;
                var currentTemperature = Weather.main.temp;
                // getting coordinates and timestamp for GoogleTimezone request, formatting floats in coordinates to dot separation
                var latitude = Weather.coord.lat.ToString(System.Globalization.CultureInfo.InvariantCulture);
                var longtitude = Weather.coord.lon.ToString(System.Globalization.CultureInfo.InvariantCulture);
                var timestamp = Weather.dt;
                // prepare for Google Timezone request
                string timezoneURL = $"https://maps.googleapis.com/maps/api/timezone/json?location={latitude},{longtitude}&timestamp={timestamp}&key={googleTimezoneAPIkey}";
                string jsonTimezone = client.DownloadString(timezoneURL);
                Models.WeatherTime.GoogleTimezone Timezone = (new JavaScriptSerializer()).Deserialize<Models.WeatherTime.GoogleTimezone>(jsonTimezone);
                var timezone = Timezone.timeZoneId;
                // additional check for the presence of a timezone value
                if (timezone!=null)
                {
                    return $"At the location {cityName}, the temperature is {currentTemperature}°С, and the timezone is {timezone}";
                    
                }
                else
                {   
                    return $"At the location {cityName}, the temperature is {currentTemperature}°С. Google Timezone API requests have exceeded the daily quota.";
                }
            }
        }
    }
}
