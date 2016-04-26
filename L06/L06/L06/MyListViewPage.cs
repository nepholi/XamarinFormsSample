using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using L05_2.MyServices;
using Xamarin.Forms;

namespace L05_2
{
    public class MyListViewPage : ContentPage
    {
        private Picker cityPicker;
        private string cityUserChoose;
        private Picker areaPicker;
        private string areaUserChoose;

        private Button searchButton;

        
        private List<FamilyStore> myStoreDataList;
        private readonly WebApiServices myWebApiService;
        private CityAreaManager myCityAreaManager;

        public MyListViewPage(string title)
        {
            Title = title;
            myStoreDataList = new List<FamilyStore>();
            myWebApiService = new WebApiServices();
            myCityAreaManager = new CityAreaManager();

            //for CityPicker
            cityPicker = new Picker {
                Title = "請輸入城市名稱"
            };

            var CityNames = myCityAreaManager.GetCityList();
            foreach (var cn in CityNames) {
                cityPicker.Items.Add(cn);
            };

            cityPicker.SelectedIndexChanged += (sender, args) => {
                cityUserChoose = cityPicker.Items[cityPicker.SelectedIndex];

                areaPicker.IsEnabled = true;
                List<string> AreaNames = myCityAreaManager.GetAreaList(cityUserChoose);
                foreach (var an in AreaNames)
                {
                    areaPicker.Items.Add(an);
                };
            };

            //for AreaPicker
            areaPicker = new Picker {
                Title = "請輸入區域名稱",
                IsEnabled = false
            };

            areaPicker.SelectedIndexChanged += (sender, args) => {
                areaUserChoose = areaPicker.Items[areaPicker.SelectedIndex];
                searchButton.IsEnabled = true;
            };

            //for listview
            var listView = new ListView
            {
                IsPullToRefreshEnabled = true,
                RowHeight = 80,
                ItemsSource = null,
                ItemTemplate = new DataTemplate(typeof(MyListViewCell))
            };

            listView.ItemTapped += (sender, e) =>
            {
                var baseUrl = "https://www.google.com.tw/maps/place/";
                var storeData = e.Item as StoreData;

                if (storeData != null)
                    Device.OpenUri(new Uri($"{baseUrl}{storeData.Address}"));

                ((ListView)sender).SelectedItem = null;
            };

            //for searchButton
            searchButton = new Button { Text = "Search" };
            searchButton.IsEnabled = false;
            searchButton.Clicked += async (sender, e) =>
            {
                var resultData = await myWebApiService.GetDataAsync(cityUserChoose, areaUserChoose);
                myStoreDataList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<FamilyStore>>(resultData);

                var newdata = new List<StoreData>();

                foreach (var fs in myStoreDataList)
                {
                    newdata.Add(new StoreData { Name = fs.NAME, Address = fs.addr, Tel = fs.TEL });
                }

                
                listView.ItemsSource = null;
                listView.ItemsSource = newdata;

                searchButton.IsEnabled = false;

                Debug.WriteLine("Store count:" + myStoreDataList.Count);
            };


            Padding = new Thickness(0, 20, 0, 0);
            Content = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                Children =
                {
                    new Label
                    {
                        HorizontalTextAlignment= TextAlignment.Center,
                        Text = Title,
                        FontSize = 30
                    },
                    cityPicker,
                    areaPicker,
                    searchButton,
                    listView
                }
            };
        }
    }

    

    public class StoreData
    {
        public string Name { get; set; }
        public string Tel { get; set; }
        public string Address { get; set; }
    }

    public class FamilyStore
    {
        public string NAME { get; set; }
        public string TEL { get; set; }
        public string POSTel { get; set; }
        public double px { get; set; }
        public double py { get; set; }
        public string addr { get; set; }
        public double SERID { get; set; }
        public string pkey { get; set; }
        public string oldpkey { get; set; }
        public string post { get; set; }
        public string all { get; set; }
        public string road { get; set; }
        public object twoice { get; set; }
    }
}
