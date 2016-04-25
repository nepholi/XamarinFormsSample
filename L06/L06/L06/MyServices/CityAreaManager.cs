using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L05_2.MyServices
{
    public class CityAreaManager
    {
        //private string[] cityNames = { "基隆市", "台北市", "新北市", "桃園市", "新竹市", "新竹縣", "苗栗縣", "台中市", "彰化縣", "南投縣", "雲林縣", "嘉義市", "嘉義縣", "台南市", "高雄市", "屏東縣", "台東縣", "花蓮縣", "宜蘭縣", "金門縣", "澎湖縣" };
        private List<CityAreaDataBase> cadb_array;

        public CityAreaManager()
        {
            cadb_array = new List<CityAreaDataBase>();
            cadb_array.Add(new CityAreaDataBase() { city = "台北市", area = new List<string> { "中正區", "大同區", "中山區", "松山區", "大安區", "萬華區", "信義區", "士林區", "北投區", "內湖區", "南港區", "文山區" } });
            cadb_array.Add(new CityAreaDataBase() { city = "新北市", area = new List<string> { "金山區", "萬里區", "板橋區", "汐止區", "深坑區", "石碇區", "瑞芳區", "平溪區", "貢寮區", "新店區", "坪林區", "烏來區", "永和區", "中和區", "土城區", "三峽區", "樹林區", "鶯歌區", "三重區", "新莊區", "泰山區", "林口區", "蘆洲區", "五股區", "八里區", "淡水區", "三芝區", "石門區" } });
        }

        public List<string> GetCityList() {
            var city_list = new List<string>();
            foreach(var cadb in cadb_array) {
                city_list.Add(cadb.city);
            }
            return city_list;
        }

        public List<string> GetAreaList(string cityName) {
            foreach (var cadb in cadb_array) {
                if (cadb.city.Equals(cityName)) {
                    return cadb.area;
                }
            }
            return null;
        }
    }

    public class CityAreaDataBase
    {
        public string city;
        public List<string> area;
    }
}
