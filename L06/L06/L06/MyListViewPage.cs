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
        //private string[] cityNames = { "基隆市", "台北市", "新北市", "桃園市", "新竹市", "新竹縣", "苗栗縣", "台中市", "彰化縣", "南投縣", "雲林縣", "嘉義市", "嘉義縣", "台南市", "高雄市", "屏東縣", "台東縣", "花蓮縣", "宜蘭縣", "金門縣", "澎湖縣" };
        private Picker cityPicker;
        private string cityUserChoose;
        private Picker areaPicker;
        private string areaUserChoose;

        private Button searchButton;
        private Entry areaEntry;

        
        private List<FamilyStore> myStoreDataList;
        private readonly WebApiServices myWebApiService;
        private CityAreaManager myCityAreaManager;

        public MyListViewPage(string title)
        {
            Title = title;

            myStoreDataList = new List<FamilyStore>();
            myWebApiService = new WebApiServices();
            myCityAreaManager = new CityAreaManager();

            searchButton = new Button { Text = "Search" };
            areaEntry = new Entry{ Placeholder = "請輸入行政區域"};

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
                areaUserChoose = cityPicker.Items[areaPicker.SelectedIndex];
            };

            var listView = new ListView
            {
                IsPullToRefreshEnabled = true,
                RowHeight = 80,
                ItemsSource = new[] { new StoreData {} },
                ItemTemplate = new DataTemplate(typeof(MyListViewCell))
            };

            searchButton.Clicked += async (sender, e) =>
            {
                if(cityUserChoose != null && cityUserChoose != empty) 

                var resultData = await myWebApiService.GetDataAsync(cityUserChoose, areaUserChoose);
                myStoreDataList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<FamilyStore>>(resultData);

                Debug.WriteLine(myStoreDataList.Count);
            };
            
            listView.ItemTapped += (sender, e) =>
            {
                var baseUrl = "https://www.google.com.tw/maps/place/";
                var storeData = e.Item as StoreData;
               
                if (storeData != null)
                    Device.OpenUri(new Uri( $"{baseUrl}{storeData.Address}"));

                ((ListView)sender).SelectedItem = null;
            };

            Padding = new Thickness(0, 20, 0, 0);
            Content = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                Children =
                {
                    cityPicker,
                    areaEntry,
                    searchButton,
                    new Label
                    {
                        HorizontalTextAlignment= TextAlignment.Center,
                        Text = Title,
                        FontSize = 30
                    },
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
