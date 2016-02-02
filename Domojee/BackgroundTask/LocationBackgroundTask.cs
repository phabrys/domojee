﻿using System;
using System.Threading;
using Windows.ApplicationModel.Background;
using Windows.Storage;
using Windows.Devices.Geolocation;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace BackgroundTask
{
    public sealed class LocationBackgroundTask : IBackgroundTask
    {
        private CancellationTokenSource _cts = null;

        async void IBackgroundTask.Run(IBackgroundTaskInstance taskInstance)
        {
            BackgroundTaskDeferral deferral = taskInstance.GetDeferral();

            try
            {
                // Associate a cancellation handler with the background task.
                taskInstance.Canceled += OnCanceled;

                // Get cancellation token
                if (_cts == null)
                {
                    _cts = new CancellationTokenSource();
                }
                CancellationToken token = _cts.Token;

                // Create geolocator object
                Geolocator geolocator = new Geolocator();

                // Make the request for the current position
                Geoposition pos = await geolocator.GetGeopositionAsync().AsTask(token);

                DateTime currentTime = DateTime.Now;

                WriteStatusToAppData("Time: " + currentTime.ToString());
                WriteGeolocToAppData(pos);
            }
            catch (UnauthorizedAccessException)
            {
                WriteStatusToAppData("Disabled");
                WipeGeolocDataFromAppData();
            }
            catch (Exception ex)
            {
                WriteStatusToAppData(ex.ToString());
                WipeGeolocDataFromAppData();
            }
            finally
            {
                _cts = null;
                deferral.Complete();
            }
        }

        private void WriteGeolocToAppData(Geoposition pos)
        {
            var settings = ApplicationData.Current.LocalSettings;
            settings.Values["Latitude"] = pos.Coordinate.Point.Position.Latitude.ToString();
            settings.Values["Longitude"] = pos.Coordinate.Point.Position.Longitude.ToString();
            settings.Values["Accuracy"] = pos.Coordinate.Accuracy.ToString();
            Position(pos.Coordinate.Point.Position.Latitude.ToString() + ',' + pos.Coordinate.Point.Position.Longitude.ToString());
        }

        async private void Position(string position)
        {
            ApplicationDataContainer RoamingSettings = ApplicationData.Current.RoamingSettings;
            ApplicationDataContainer LocalSettings = ApplicationData.Current.LocalSettings;
            var _apikey="";
            var _address="";
            var _GeolocObjectId = (LocalSettings.Values["GeolocObjectId"] == null) ? "" : LocalSettings.Values["GeolocObjectId"].ToString();


            if (RoamingSettings.Values["addressSetting"] != null)
            {
                _address = RoamingSettings.Values["addressSetting"] as string;
                if (RoamingSettings.Values["apikeySetting"] != null)
                {
                   _apikey = RoamingSettings.Values["apikeySetting"] as string;
                }
            }
            try
            {
                 HttpClient httpclient = new HttpClient();
                httpclient.BaseAddress = new Uri(_address + "/core/api/");
                HttpContent content=null;
                var response = await httpclient.PostAsync("jeeApi.php?api=" + _apikey + "& type=geoloc&id=" + _GeolocObjectId + "&value=" + position, content);
                httpclient.Dispose();
             }
             catch (Exception)
             {
             }
        }
        private void WipeGeolocDataFromAppData()
        {
            var settings = ApplicationData.Current.LocalSettings;
            settings.Values["Latitude"] = "";
            settings.Values["Longitude"] = "";
            settings.Values["Accuracy"] = "";
        }

        private void WriteStatusToAppData(string status)
        {
            var settings = ApplicationData.Current.LocalSettings;
            settings.Values["Status"] = status;
        }

        private void OnCanceled(IBackgroundTaskInstance sender, BackgroundTaskCancellationReason reason)
        {
            if (_cts != null)
            {
                _cts.Cancel();
                _cts = null;
            }
        }
    }
}
