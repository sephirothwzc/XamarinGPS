using CoreLocation;
using System;
using System.Collections.Generic;
using System.Text;
using UIKit;

namespace XamarinGPS
{
    /// <summary>
    /// 获取坐标类
    /// </summary>
    public class LocationManager : ILocationManager
    {
        public LocationManager()
        {
            LocMgr = new CLLocationManager();

            LocMgr.RequestAlwaysAuthorization(); //to access user's location in the background
            LocMgr.RequestWhenInUseAuthorization(); //to access user's location when the app is in use.

            //LocMgr.PausesLocationUpdatesAutomatically = false;

            //// iOS 8 has additional permissions requirements
            //if (UIDevice.CurrentDevice.CheckSystemVersion(8, 0))
            //{
            //    LocMgr.RequestAlwaysAuthorization(); // works in background
            //                                         //locMgr.RequestWhenInUseAuthorization (); // only in foreground
            //}

            if (UIDevice.CurrentDevice.CheckSystemVersion(9, 0))
            {
                LocMgr.AllowsBackgroundLocationUpdates = true;
            }

            //开启后台获取数据
            if (CLLocationManager.LocationServicesEnabled)
            {
                LocMgr.StartMonitoringSignificantLocationChanges();
            }
            else {
                Console.WriteLine("Location services not enabled, please enable this in your Settings");
            }
            //this.LocationUpd(LocMgr.Location.Coordinate.Latitude, LocMgr.Location.Coordinate.Longitude); 

            //LocMgr.LocationsUpdated += (o, e) =>
            //{
            //    this.LocationUpd(e.Locations[0].Coordinate.Latitude, e.Locations[0].Coordinate.Longitude);
            //};

            //停止监听位置
            //LocMgr.StopMonitoringSignificantLocationChanges（）;
        }



        public CLLocationManager LocMgr
        {
            get;
            set;
        }
        
        private Action<double, double> LocationUpd { get; set; }

        List<double> ILocationManager.GetLocation()
        {
            return new List<double> {
                LocMgr.Location.Coordinate.Latitude,
                LocMgr.Location.Coordinate.Longitude };
        }

        void ILocationManager.LocationChang(Action<double, double> GetLocation)
        {
            this.LocationUpd = GetLocation;
            LocMgr.LocationsUpdated += (o, e) =>
            {
                this.LocationUpd(e.Locations[0].Coordinate.Latitude, e.Locations[0].Coordinate.Longitude);
            };
        }

        void ILocationManager.StartMonitoringSignificantLocationChanges()
        {
            if (CLLocationManager.LocationServicesEnabled)
            {
                LocMgr.StartMonitoringSignificantLocationChanges();
            }
            else {
                Console.WriteLine("Location services not enabled, please enable this in your Settings");
            }

            //this.LocationUpd(LocMgr.Location.Coordinate.Latitude, LocMgr.Location.Coordinate.Longitude);
        }

        void ILocationManager.StopMonitoringSignificantLocationChanges()
        {
            LocMgr.StopMonitoringSignificantLocationChanges();
        }
    }
}
