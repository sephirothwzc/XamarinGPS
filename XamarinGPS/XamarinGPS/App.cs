using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace XamarinGPS
{
    public class App : Application
    {
        public ILocationManager il { get; set; }


        public Geocoder geoCoder { get; set; }
        public Map map { get; set; }

        public Label lab { get; set; }

        public Label labaddress { get; set; }

        public App()
        {
            geoCoder = new Geocoder();
  

            //地图
            map = new Map(
            MapSpan.FromCenterAndRadius(
                  new Position(37, -122), Distance.FromMiles(0.3)))
            {
                IsShowingUser = true,
                HeightRequest = 100,
                WidthRequest = 960,
                VerticalOptions = LayoutOptions.FillAndExpand,
                MapType = MapType.Street
            };
            var stack = new StackLayout { Spacing = 0 };
            stack.Children.Add(map);
            //滑块
            var slider = new Slider(1, 18, 1);
            slider.ValueChanged += (sender, e) =>
            {
                var zoomLevel = e.NewValue; // between 1 and 18
                var latlongdegrees = 360 / (Math.Pow(2, zoomLevel));
                map.MoveToRegion(new MapSpan(map.VisibleRegion.Center, latlongdegrees, latlongdegrees));
            };
            stack.Children.Add(slider);

            lab = new Label();
            stack.Children.Add(lab);
            labaddress = new Label();
            stack.Children.Add(labaddress);

            Button btn = new Button()
            {
                Text = "获取位置"
            };
            btn.Clicked += (send, e) => 
            {
                var ld = il.GetLocation();
                map.MoveToRegion(
    MapSpan.FromCenterAndRadius(
        new Position(ld[0], ld[1]), Distance.FromMiles(1)));

                ReverseGeocodePosition(ld[0], ld[1]);
            };
            stack.Children.Add(btn);
            // The root page of your application
            MainPage = new ContentPage
            {
                Content = stack
            };

            //this.GeocodeAnAddress();
            //il.LocationChang((Latitude, Longitude) => 
            //{
            //    ReverseGeocodePosition(Latitude, Longitude);
            //});
            

        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }

        public async void GeocodeAnAddress(string xamarinAddress)
        {
            //var xamarinAddress = "上海嘉定区秦安路305号";
            var approximateLocations = await geoCoder.GetPositionsForAddressAsync(xamarinAddress);
            foreach (var p in approximateLocations)
            {
                this.lab.Text += p.Latitude + ", " + p.Longitude + "\n";

                var position = new Position(p.Latitude, p.Longitude); // Latitude, Longitude
                var pin = new Pin
                {
                    Type = PinType.Place,
                    Position = position,
                    Label = "我的位置",
                    Address = xamarinAddress
                };
                map.Pins.Add(pin);

            }
        }

        public async void ReverseGeocodePosition(double Latitude,double Longitude)
        {
            var fortMasonPosition = new Position(Latitude, Longitude);
            var possibleAddresses = await geoCoder.GetAddressesForPositionAsync(fortMasonPosition);
            foreach (var a in possibleAddresses)
            {
                labaddress.Text += a + "\n";
            }

            var pin = new Pin
            {
                Type = PinType.Place,
                Position = fortMasonPosition,
                Label = "我的位置",
                Address = labaddress.Text
            };
            map.Pins.Add(pin);
        }
    }
}
