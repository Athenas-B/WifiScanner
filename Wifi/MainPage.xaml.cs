using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Controls.Maps;
using Windows.UI.Popups;
using Windows.Devices.Geolocation;
using Windows.UI;
using Windows.Storage;
using Windows.Storage.Streams;
using System.Runtime.Serialization;

// Dokumentaci k šabloně položky Prázdná stránka najdete na adrese https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x405

namespace Wifi
{
    /// <summary>
    /// Prázdná stránka, která se dá použít samostatně nebo v rámci objektu Frame
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private WifiScanner WifiScanner;
        private Geolocator geolocator;
        private WiFiSignalInfo SelectedWifi;
        private bool Recording = false;
        public MainPage()
        {
            this.InitializeComponent();
            WifiScanner = new WifiScanner();
            geolocator = new Geolocator();
            SetEviroment();
        }

        private async void SetEviroment() {
            try
            {
                await WifiScanner.InitializeFirstAdapter();
            }
            catch (Exception)
            {

                ShowError("WiFi service error. Please enable WiFi access and restart this app.");
                return;
            }
            WifiList.Items.Clear();
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 4);
            timer.Tick += NetworkScan;
            timer.Start();

            MapControl.MapServiceToken = "Ai-JrzIrH33ZLoj7rtSUdwLYliMcYfetOTp_cGmu85gWntcUSD6SOB1OmiSw3eB6";
            SetCurrentLocation();
        }
        private async void SetCurrentLocation() {
            var accessStatus = await Geolocator.RequestAccessAsync();
            switch (accessStatus)
            {
                case GeolocationAccessStatus.Allowed:

                    // Get the current location.
                    Geolocator geolocator = new Geolocator();
                    Geoposition pos = await geolocator.GetGeopositionAsync();
                    Geopoint myLocation = pos.Coordinate.Point;

                    // Set the map location.
                    MapControl.Center = myLocation;
                    MapControl.ZoomLevel = 20;
                    MapControl.LandmarksVisible = true;
                    break;

                case GeolocationAccessStatus.Denied:
                    // Handle the case  if access to location is denied.
                    ShowError("This app needs Location services enabled. Please enable them and restart this app.");
                    break;

                case GeolocationAccessStatus.Unspecified:
                    // Handle the case if  an unspecified error occurs.
                    ShowError("Location acces unspecified. This app needs Location services enabled. Please enable them and restart this app.");
                    break;
            }
        }

        private async void NetworkScan(object sender, object e)
        {
            
            Geoposition geoPosition = await geolocator.GetGeopositionAsync(); //maybe move somewhere erlier in code to reduce delay? (+ rethink await)
            await WifiScanner.ScanForNetworks();
            WifiList.Items.Clear();
            foreach (var item in WifiScanner.Wifi)
            {
                WifiList.Items.Add(item);

                if (item.Equals(SelectedWifi)) { //if there is selected network refresh its info
                    SelectedWifi.RssiInDecibelMilliwatts = item.RssiInDecibelMilliwatts;
                    SelectedWifi.SignalBars = item.SignalBars;
                    WifiDetail.Text = SelectedWifi.GetTextDetail();
                    WifiList.SelectedItem = item;

                    if (Recording) {



                        var locationData = new WiFiLocationtData
                        {
                            Latitude = geoPosition.Coordinate.Latitude,
                            Longitude = geoPosition.Coordinate.Longitude,
                            RssiInDecibelMilliwatts = item.RssiInDecibelMilliwatts,
                            SignalBars = item.SignalBars
                        };

                        SelectedWifi.LocationData.Add(locationData);
                        CreatePolygon(locationData);
                        
                    }
                }
            }

            if (WifiList.SelectedItem == null && SelectedWifi!=null)
            {
                SelectedWifi.SignalBars = 0;
                SelectedWifi.RssiInDecibelMilliwatts = 0;
                WifiDetail.Text = SelectedWifi.GetTextDetail();
            }


        }

        private void WifiList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (WifiList.SelectedItem != null && (SelectedWifi==null || !WifiList.SelectedItem.Equals(SelectedWifi) ) )
            {
                SelectedWifi = (WiFiSignalInfo)WifiList.SelectedItem;
            }
            WifiDetail.Text = SelectedWifi.GetTextDetail();
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            SelectedWifi.LocationData.Clear();
        }


        private async void Record_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedWifi == null)
            {
                ShowError("No selected network!");
                return;
            }
           

            Recording = !Recording;
            if (Recording)
            {
                Record.Content = "Stop";
            }
            else {
                Record.Content = "Record";
            }

          


        }

        private async void Load_Click(object sender, RoutedEventArgs e)
        {
            var openPicker = new Windows.Storage.Pickers.FileOpenPicker();
            openPicker.SuggestedStartLocation =
                Windows.Storage.Pickers.PickerLocationId.DocumentsLibrary;

            openPicker.FileTypeFilter.Add( ".wrec" );


            Windows.Storage.StorageFile file = await openPicker.PickSingleFileAsync();
            if (file != null)
            {

                // Prevent updates to the remote version of the file until
                // we finish making changes and call CompleteUpdatesAsync.
                Windows.Storage.CachedFileManager.DeferUpdates(file);

                
                var Serializer = new DataContractSerializer(typeof(WiFiSignalInfo));
                using (var stream = await file.OpenStreamForReadAsync())
                {
                    SelectedWifi = (WiFiSignalInfo)Serializer.ReadObject(stream);

                    foreach (var item in SelectedWifi.LocationData)
                    {
                        CreatePolygon(item);
                    }
                }
                


                // write to file
                //await Windows.Storage.FileIO.WriteTextAsync(file, file.Name);

                // Let Windows know that we're finished changing the file so
                // the other app can update the remote version of the file.
                // Completing updates may require Windows to ask for user input.
                Windows.Storage.Provider.FileUpdateStatus status =
                    await Windows.Storage.CachedFileManager.CompleteUpdatesAsync(file);
                if (status == Windows.Storage.Provider.FileUpdateStatus.Complete)
                {

                }
                else
                {
                    ShowError("File " + file.Name + " couldn't be saved.");
                }
            }
            else
            {
                //this.textBlock.Text = "Operation cancelled.";
            }
        }

        private async void Save_Click(object sender, RoutedEventArgs e)
        {
            var savePicker = new Windows.Storage.Pickers.FileSavePicker();
            savePicker.SuggestedStartLocation =
                Windows.Storage.Pickers.PickerLocationId.DocumentsLibrary;
            // Dropdown of file types the user can save the file as
            savePicker.FileTypeChoices.Add("Wifi Records", new List<string>() { ".wrec" });
            // Default file name if the user does not type one in or select a file to replace
            savePicker.SuggestedFileName = "New Record";

            Windows.Storage.StorageFile file = await savePicker.PickSaveFileAsync();
            if (file != null)
            {

                // Prevent updates to the remote version of the file until
                // we finish making changes and call CompleteUpdatesAsync.
                Windows.Storage.CachedFileManager.DeferUpdates(file);


                IRandomAccessStream raStream = await file.OpenAsync(FileAccessMode.ReadWrite);

                using (IOutputStream outStream = raStream.GetOutputStreamAt(0))
                {

                    // Serialize the Session State. 

                    DataContractSerializer serializer = new DataContractSerializer(typeof(WiFiSignalInfo));

                    serializer.WriteObject(outStream.AsStreamForWrite(), SelectedWifi);

                    await outStream.FlushAsync();
                    outStream.Dispose(); //  
                    raStream.Dispose();
                }

                
                // write to file
                //await Windows.Storage.FileIO.WriteTextAsync(file, file.Name);
               
                // Let Windows know that we're finished changing the file so
                // the other app can update the remote version of the file.
                // Completing updates may require Windows to ask for user input.
                Windows.Storage.Provider.FileUpdateStatus status =
                    await Windows.Storage.CachedFileManager.CompleteUpdatesAsync(file);
                if (status == Windows.Storage.Provider.FileUpdateStatus.Complete)
                {
                    
                }
                else
                {
                    ShowError("File " + file.Name + " couldn't be saved.");
                }
            }
            else
            {
                //this.textBlock.Text = "Operation cancelled.";
            }
        }

        private async void ShowError(string text)
        {
            // Create the message dialog and set its content
            var messageDialog = new MessageDialog(text);

            // Add commands and set their callbacks; both buttons use the same callback function instead of inline event handlers
            messageDialog.Commands.Add(new UICommand(
                "Close",
                new UICommandInvokedHandler(this.CommandInvokedHandler)));

            // Set the command that will be invoked by default
            messageDialog.DefaultCommandIndex = 0;

            // Set the command to be invoked when escape is pressed
            messageDialog.CancelCommandIndex = 0;

            // Show the message dialog
            await messageDialog.ShowAsync();
        }

        private void CommandInvokedHandler(IUICommand command)
        {
            // Display message showing the label of the command that was invoked
           // rootPage.NotifyUser("The '" + command.Label + "' command has been selected.",   NotifyType.StatusMessage);
        }

        private MapPolygon CreatePolygon(WiFiLocationtData data) {
            const double size = 0.000005;
            var mapPolygon = new MapPolygon
            {
                Path = new Geopath(new List<BasicGeoposition> {
                    new BasicGeoposition() {Latitude=data.Latitude +size, Longitude=data.Longitude-size },
                    new BasicGeoposition() {Latitude=data.Latitude-size, Longitude=data.Longitude-size },
                    new BasicGeoposition() {Latitude=data.Latitude-size, Longitude=data.Longitude+size },
                    new BasicGeoposition() {Latitude=data.Latitude+size, Longitude=data.Longitude+size },
                }),
                ZIndex = 1,
                StrokeThickness = 1,
                StrokeDashed = false,
                Visible = true
            };

            byte green = 255;
            if (true) {
                green = (byte)((255 / 4) * data.SignalBars);
            }

            mapPolygon.FillColor = Color.FromArgb(255, 255, green, 0);

            mapPolygon.StrokeColor = mapPolygon.FillColor;



            MapControl.MapElements.Add(mapPolygon);
            return mapPolygon;

        }
        public void HighlightArea()
        {
            // Add MapPolygon to a layer on the map control.
            var MyHighlights = new List<MapElement>();

            //MyHighlights.Add(mapPolygon);

            

            //MapControl.MapElements.Add
        }
    }
}
