using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Comlib
{
    public class SysUser: Cust
    {
        [JsonIgnore]
        public string cuGenderName { get { return cuGender == "0" ? "女性" : cuGender == "1" ? "男性" : cuGender == "2" ? "其他" : ""; } }
        [JsonIgnore]
        public string VerifyCode { get; set; }
        [JsonIgnore]
        public bool SmsValid { get; set; }
        [JsonIgnore]
        public string QRPic { get; set; }
        [JsonIgnore]
        public string OldPwd { get; set; }
        [JsonIgnore]
        public string cuCityName
        {
            get
            {
                var temp = "";
                switch (cuCity)
                {
                    case 1: temp = "台北市";break;
                    case 2: temp = "基隆市"; break;
                    case 3: temp = "新北市";break;
                    case 4: temp = "宜蘭縣";break;
                    case 6: temp = "新竹縣";break;
                    case 7: temp = "桃園市";break;
                    case 8: temp = "苗栗縣"; break;
                    case 9: temp = "臺中市";break;
                    case 10: temp = "彰化縣";break;
                    case 11: temp = "南投縣";break;
                    case 12: temp = "嘉義縣";break;
                    case 13: temp = "雲林縣";break;
                    case 14: temp = "臺南市";break;
                    case 15: temp = "高雄市";break;
                    case 16: temp = "屏東縣";break;
                    case 17: temp = "臺東縣";break;
                    case 18: temp = "花蓮縣";break;
                    case 90: temp = "澎湖縣";break;
                    case 91: temp = "金門縣";break;
                    case 92: temp = "連江縣"; break;
                    case 93: temp = "南海諸島";break;
                    case 300:temp = "新竹市";break;
                    case 600:temp = "嘉義市";break;
                }
                return temp;
            }
        }
        [JsonIgnore]
        public string cuTownName
        {
            get
            {
                var temp = "";
                switch (cuTown)
                {
                    case 100: temp = "中正區"; break;
                    case 103: temp = "大同區"; break;
                    case 104: temp = "中山區"; break;
                    case 105: temp = "松山區"; break;
                    case 106: temp = "大安區"; break;
                    case 108: temp = "萬華區"; break;
                    case 110: temp = "信義區"; break;
                    case 111: temp = "士林區"; break;
                    case 112: temp = "北投區"; break;
                    case 114: temp = "內湖區"; break;
                    case 115: temp = "南港區"; break;
                    case 116: temp = "文山區"; break;
                    case 200: temp = "仁愛區"; break;
                    case 201: temp = "信義區"; break;
                    case 202: temp = "中正區"; break;
                    case 203: temp = "中山區"; break;
                    case 204: temp = "安樂區"; break;
                    case 205: temp = "暖暖區"; break;
                    case 206: temp = "七堵區"; break;
                    case 207: temp = "萬里區"; break;
                    case 208: temp = "金山區"; break;
                    case 209: temp = "南竿鄉"; break;
                    case 210: temp = "北竿鄉"; break;
                    case 211: temp = "莒光鄉"; break;
                    case 212: temp = "東引鄉"; break;
                    case 220: temp = "板橋區"; break;
                    case 221: temp = "汐止區"; break;
                    case 222: temp = "深坑區"; break;
                    case 223: temp = "石碇區"; break;
                    case 224: temp = "瑞芳區"; break;
                    case 226: temp = "平溪區"; break;
                    case 227: temp = "雙溪區"; break;
                    case 228: temp = "貢寮區"; break;
                    case 231: temp = "新店區"; break;
                    case 232: temp = "坪林區"; break;
                    case 233: temp = "烏來區"; break;
                    case 234: temp = "永和區"; break;
                    case 235: temp = "中和區"; break;
                    case 236: temp = "土城區"; break;
                    case 237: temp = "三峽區"; break;
                    case 238: temp = "樹林區"; break;
                    case 239: temp = "鶯歌區"; break;
                    case 241: temp = "三重區"; break;
                    case 242: temp = "新莊區"; break;
                    case 243: temp = "泰山區"; break;
                    case 244: temp = "林口區"; break;
                    case 247: temp = "蘆洲區"; break;
                    case 248: temp = "五股區"; break;
                    case 249: temp = "八里區"; break;
                    case 251: temp = "淡水區"; break;
                    case 252: temp = "三芝區"; break;
                    case 253: temp = "石門區"; break;
                    case 260: temp = "宜蘭市"; break;
                    case 261: temp = "頭城鎮"; break;
                    case 262: temp = "礁溪鄉"; break;
                    case 263: temp = "壯圍鄉"; break;
                    case 264: temp = "員山鄉"; break;
                    case 265: temp = "羅東鎮"; break;
                    case 266: temp = "三星鄉"; break;
                    case 267: temp = "大同鄉"; break;
                    case 268: temp = "五結鄉"; break;
                    case 269: temp = "冬山鄉"; break;
                    case 270: temp = "蘇澳鎮"; break;
                    case 272: temp = "南澳鎮"; break;
                    case 290: temp = "釣魚臺列嶼"; break;
                    case 302: temp = "竹北市"; break;
                    case 303: temp = "湖口鄉"; break;
                    case 304: temp = "新豐鄉"; break;
                    case 305: temp = "新埔鎮"; break;
                    case 306: temp = "關西鎮"; break;
                    case 307: temp = "芎林鄉"; break;
                    case 308: temp = "寶山鄉"; break;
                    case 310: temp = "竹東鎮"; break;
                    case 311: temp = "五峰鄉"; break;
                    case 312: temp = "橫山鄉"; break;
                    case 313: temp = "尖石鄉"; break;
                    case 314: temp = "北埔鄉"; break;
                    case 315: temp = "峨眉鄉"; break;
                    case 320: temp = "中壢區"; break;
                    case 324: temp = "平鎮區"; break;
                    case 325: temp = "龍潭區"; break;
                    case 326: temp = "楊梅區"; break;
                    case 327: temp = "新屋區"; break;
                    case 328: temp = "觀音區"; break;
                    case 330: temp = "桃園區"; break;
                    case 333: temp = "龜山區"; break;
                    case 334: temp = "八德區"; break;
                    case 335: temp = "大溪區"; break;
                    case 336: temp = "復興區"; break;
                    case 337: temp = "大園區"; break;
                    case 338: temp = "蘆竹區"; break;
                    case 350: temp = "竹南鎮"; break;
                    case 351: temp = "頭份市"; break;
                    case 352: temp = "三灣鄉"; break;
                    case 353: temp = "南庄鄉"; break;
                    case 354: temp = "獅潭鄉"; break;
                    case 356: temp = "後龍鎮"; break;
                    case 357: temp = "通霄鎮"; break;
                    case 358: temp = "苑裡鄉"; break;
                    case 360: temp = "苗栗市"; break;
                    case 361: temp = "造橋鄉"; break;
                    case 362: temp = "頭屋鄉"; break;
                    case 363: temp = "公館鄉"; break;
                    case 364: temp = "大湖鄉"; break;
                    case 365: temp = "泰安鄉"; break;
                    case 366: temp = "銅鑼鄉"; break;
                    case 367: temp = "三義鄉"; break;
                    case 368: temp = "西湖鄉"; break;
                    case 369: temp = "卓蘭鄉"; break;
                    case 400: temp = "中 區"; break;
                    case 401: temp = "東 區"; break;
                    case 402: temp = "南 區"; break;
                    case 403: temp = "西 區"; break;
                    case 404: temp = "北 區"; break;
                    case 406: temp = "北屯區"; break;
                    case 407: temp = "西屯區"; break;
                    case 408: temp = "南屯區"; break;
                    case 411: temp = "太平區"; break;
                    case 412: temp = "大里區"; break;
                    case 413: temp = "霧峰區"; break;
                    case 414: temp = "烏日區"; break;
                    case 420: temp = "豐原區"; break;
                    case 421: temp = "后里區"; break;
                    case 422: temp = "石岡區"; break;
                    case 423: temp = "東勢區"; break;
                    case 424: temp = "和平區"; break;
                    case 426: temp = "新社區"; break;
                    case 427: temp = "潭子區"; break;
                    case 428: temp = "大雅區"; break;
                    case 429: temp = "神岡區"; break;
                    case 432: temp = "大肚區"; break;
                    case 433: temp = "沙鹿區"; break;
                    case 434: temp = "龍井區"; break;
                    case 435: temp = "梧棲區"; break;
                    case 436: temp = "清水區"; break;
                    case 437: temp = "大甲區"; break;
                    case 438: temp = "外埔區"; break;
                    case 439: temp = "大安區"; break;
                    case 500: temp = "彰化市"; break;
                    case 502: temp = "芬園鄉"; break;
                    case 503: temp = "花壇鄉"; break;
                    case 504: temp = "秀水鄉"; break;
                    case 505: temp = "鹿港鎮"; break;
                    case 506: temp = "福興鄉"; break;
                    case 507: temp = "線西鄉"; break;
                    case 508: temp = "和美鎮"; break;
                    case 509: temp = "伸港鄉"; break;
                    case 510: temp = "員林市"; break;
                    case 511: temp = "社頭鄉"; break;
                    case 512: temp = "永靖鄉"; break;
                    case 513: temp = "埔心鄉"; break;
                    case 514: temp = "溪湖鎮"; break;
                    case 515: temp = "大村鄉"; break;
                    case 516: temp = "埔鹽鄉"; break;
                    case 520: temp = "田中鎮"; break;
                    case 521: temp = "北斗鎮"; break;
                    case 522: temp = "田尾鄉"; break;
                    case 523: temp = "埤頭鄉"; break;
                    case 524: temp = "溪州鄉"; break;
                    case 525: temp = "竹塘鄉"; break;
                    case 526: temp = "二林鎮"; break;
                    case 527: temp = "大城鄉"; break;
                    case 528: temp = "芳苑鄉"; break;
                    case 530: temp = "二水鄉"; break;
                    case 540: temp = "南投市"; break;
                    case 541: temp = "中寮鄉"; break;
                    case 542: temp = "草屯鎮"; break;
                    case 544: temp = "國姓鄉"; break;
                    case 545: temp = "埔里鎮"; break;
                    case 546: temp = "仁愛鄉"; break;
                    case 551: temp = "名間鄉"; break;
                    case 552: temp = "集集鎮"; break;
                    case 553: temp = "水里鄉"; break;
                    case 555: temp = "魚池鄉"; break;
                    case 556: temp = "信義鄉"; break;
                    case 557: temp = "竹山鎮"; break;
                    case 558: temp = "鹿谷鎮"; break;
                    case 602: temp = "番路鄉"; break;
                    case 603: temp = "梅山鄉"; break;
                    case 604: temp = "竹崎鄉"; break;
                    case 605: temp = "阿里山鄉"; break;
                    case 606: temp = "中埔鄉"; break;
                    case 607: temp = "大埔鄉"; break;
                    case 608: temp = "水上鄉"; break;
                    case 611: temp = "鹿草鄉"; break;
                    case 612: temp = "太保市"; break;
                    case 613: temp = "朴子市"; break;
                    case 614: temp = "東石鄉"; break;
                    case 615: temp = "六腳鄉"; break;
                    case 616: temp = "新港鄉"; break;
                    case 621: temp = "民雄鄉"; break;
                    case 622: temp = "大林鎮"; break;
                    case 623: temp = "溪口鄉"; break;
                    case 624: temp = "義竹鄉"; break;
                    case 625: temp = "布袋鎮"; break;
                    case 630: temp = "斗南鎮"; break;
                    case 631: temp = "大埤鄉"; break;
                    case 632: temp = "虎尾鎮"; break;
                    case 633: temp = "土庫鎮"; break;
                    case 634: temp = "褒忠鄉"; break;
                    case 635: temp = "東勢鄉"; break;
                    case 636: temp = "臺西鄉"; break;
                    case 637: temp = "崙背鄉"; break;
                    case 638: temp = "麥寮鄉"; break;
                    case 640: temp = "斗六市"; break;
                    case 643: temp = "林內鄉"; break;
                    case 646: temp = "古坑鄉"; break;
                    case 647: temp = "莿桐鄉"; break;
                    case 648: temp = "西螺鎮"; break;
                    case 649: temp = "二崙鄉"; break;
                    case 651: temp = "北港鎮"; break;
                    case 652: temp = "水林鄉"; break;
                    case 653: temp = "口湖鄉"; break;
                    case 654: temp = "四湖鄉"; break;
                    case 655: temp = "元長鄉"; break;
                    case 700: temp = "中西區"; break;
                    case 701: temp = "東 區"; break;
                    case 702: temp = "南 區"; break;
                    case 704: temp = "北 區"; break;
                    case 708: temp = "安平區"; break;
                    case 709: temp = "安南區"; break;
                    case 710: temp = "永康區"; break;
                    case 711: temp = "歸仁區"; break;
                    case 712: temp = "新化區"; break;
                    case 713: temp = "左鎮區"; break;
                    case 714: temp = "玉井區"; break;
                    case 715: temp = "楠西區"; break;
                    case 716: temp = "南化區"; break;
                    case 717: temp = "仁德區"; break;
                    case 718: temp = "關廟區"; break;
                    case 719: temp = "龍崎區"; break;
                    case 720: temp = "官田區"; break;
                    case 721: temp = "麻豆區"; break;
                    case 722: temp = "佳里區"; break;
                    case 723: temp = "西港區"; break;
                    case 724: temp = "七股區"; break;
                    case 725: temp = "將軍區"; break;
                    case 726: temp = "學甲區"; break;
                    case 727: temp = "北門區"; break;
                    case 730: temp = "新營區"; break;
                    case 731: temp = "後壁區"; break;
                    case 732: temp = "白河區"; break;
                    case 733: temp = "東山區"; break;
                    case 734: temp = "六甲區"; break;
                    case 735: temp = "下營區"; break;
                    case 736: temp = "柳營區"; break;
                    case 737: temp = "鹽水區"; break;
                    case 741: temp = "善化區"; break;
                    case 742: temp = "大內區"; break;
                    case 743: temp = "山上區"; break;
                    case 744: temp = "新市區"; break;
                    case 745: temp = "安定區"; break;
                    case 800: temp = "新興區"; break;
                    case 801: temp = "前金區"; break;
                    case 802: temp = "苓雅區"; break;
                    case 803: temp = "鹽埕區"; break;
                    case 804: temp = "鼓山區"; break;
                    case 805: temp = "旗津區"; break;
                    case 806: temp = "前鎮區"; break;
                    case 807: temp = "三民區"; break;
                    case 811: temp = "楠梓區"; break;
                    case 812: temp = "小港區"; break;
                    case 813: temp = "左營區"; break;
                    case 814: temp = "仁武區"; break;
                    case 815: temp = "大社區"; break;
                    case 817: temp = "東沙群島"; break;
                    case 819: temp = "南沙群島"; break;
                    case 820: temp = "岡山區"; break;
                    case 821: temp = "路竹區"; break;
                    case 822: temp = "阿蓮區"; break;
                    case 823: temp = "田寮區"; break;
                    case 824: temp = "燕巢區"; break;
                    case 825: temp = "橋頭區"; break;
                    case 826: temp = "梓官區"; break;
                    case 827: temp = "彌陀區"; break;
                    case 828: temp = "永安區"; break;
                    case 829: temp = "湖內區"; break;
                    case 830: temp = "鳳山區"; break;
                    case 831: temp = "大寮區"; break;
                    case 832: temp = "林園區"; break;
                    case 833: temp = "鳥松區"; break;
                    case 840: temp = "大樹區"; break;
                    case 842: temp = "旗山區"; break;
                    case 843: temp = "美濃區"; break;
                    case 844: temp = "六龜區"; break;
                    case 845: temp = "內門區"; break;
                    case 846: temp = "杉林區"; break;
                    case 847: temp = "甲仙區"; break;
                    case 848: temp = "桃源區"; break;
                    case 849: temp = "那瑪夏區"; break;
                    case 851: temp = "茂林區"; break;
                    case 852: temp = "茄萣區"; break;
                    case 880: temp = "馬公市"; break;
                    case 881: temp = "西嶼鄉"; break;
                    case 882: temp = "望安鄉"; break;
                    case 883: temp = "七美鄉"; break;
                    case 884: temp = "白沙鄉"; break;
                    case 885: temp = "湖西鄉"; break;
                    case 890: temp = "金沙鎮"; break;
                    case 891: temp = "金湖鎮"; break;
                    case 892: temp = "金寧鄉"; break;
                    case 893: temp = "金城鎮"; break;
                    case 894: temp = "烈嶼鄉"; break;
                    case 896: temp = "烏坵鄉"; break;
                    case 900: temp = "屏東市"; break;
                    case 901: temp = "三地門鄉"; break;
                    case 902: temp = "霧臺鄉"; break;
                    case 903: temp = "瑪家鄉"; break;
                    case 904: temp = "九如鄉"; break;
                    case 905: temp = "里港鄉"; break;
                    case 906: temp = "高樹鄉"; break;
                    case 907: temp = "鹽埔鄉"; break;
                    case 908: temp = "長治鄉"; break;
                    case 909: temp = "麟洛鄉"; break;
                    case 911: temp = "竹田鄉"; break;
                    case 912: temp = "內埔鄉"; break;
                    case 913: temp = "萬丹鄉"; break;
                    case 920: temp = "潮州鎮"; break;
                    case 921: temp = "泰武鄉"; break;
                    case 922: temp = "來義鄉"; break;
                    case 923: temp = "萬巒鄉"; break;
                    case 924: temp = "崁頂鄉"; break;
                    case 925: temp = "新埤鄉"; break;
                    case 926: temp = "南州鄉"; break;
                    case 927: temp = "林邊鄉"; break;
                    case 928: temp = "東港鎮"; break;
                    case 929: temp = "琉球鄉"; break;
                    case 931: temp = "佳冬鄉"; break;
                    case 932: temp = "新園鄉"; break;
                    case 940: temp = "枋寮鄉"; break;
                    case 941: temp = "枋山鄉"; break;
                    case 942: temp = "春日鄉"; break;
                    case 943: temp = "獅子鄉"; break;
                    case 944: temp = "車城鄉"; break;
                    case 945: temp = "牡丹鄉"; break;
                    case 946: temp = "恆春鎮"; break;
                    case 947: temp = "滿州鄉"; break;
                    case 950: temp = "臺東市"; break;
                    case 951: temp = "綠島鄉"; break;
                    case 952: temp = "蘭嶼鄉"; break;
                    case 953: temp = "延平鄉"; break;
                    case 954: temp = "卑南鄉"; break;
                    case 955: temp = "鹿野鄉"; break;
                    case 956: temp = "關山鄉"; break;
                    case 957: temp = "海端鄉"; break;
                    case 958: temp = "池上鄉"; break;
                    case 959: temp = "東河鄉"; break;
                    case 961: temp = "成功鎮"; break;
                    case 962: temp = "長濱鄉"; break;
                    case 963: temp = "太麻里鄉"; break;
                    case 964: temp = "金峰鄉"; break;
                    case 965: temp = "大武鄉"; break;
                    case 966: temp = "達仁鄉"; break;
                    case 970: temp = "花蓮市"; break;
                    case 971: temp = "新城鄉"; break;
                    case 972: temp = "秀林鄉"; break;
                    case 973: temp = "吉安鄉"; break;
                    case 974: temp = "壽豐鄉"; break;
                    case 975: temp = "鳳林鎮"; break;
                    case 976: temp = "光復鄉"; break;
                    case 977: temp = "豐濱鄉"; break;
                    case 978: temp = "瑞穗鄉"; break;
                    case 979: temp = "萬榮鄉"; break;
                    case 981: temp = "玉里鎮"; break;
                    case 982: temp = "卓溪鄉"; break;
                    case 983: temp = "富里鄉"; break;
                }
                return temp;
            }
        }
        [JsonIgnore]
        public DateTime VerfiyTime { get; set; }
    }

    public class Cust
    {
        /// <summary>
        /// 編號
        /// </summary>
        public string cuID { get; set; }
        /// <summary>
        /// 電話
        /// </summary>
        public string cuTel { get; set; }
        /// <summary>
        /// 密碼
        /// </summary>
        public string cuPawd { get; set; }
        /// <summary>
        /// 名稱
        /// </summary>
        public string cuName { get; set; }
        /// <summary>
        /// 電子郵件
        /// </summary>
        public string cuMail { get; set; }
        /// <summary>
        /// 城市
        /// </summary>
        public int cuCity { get; set; }
        /// <summary>
        /// 區域
        /// </summary>
        public int cuTown { get; set; }
        /// <summary>
        /// 區域
        /// </summary>
        public string cuArea { get; set; }
        public int cuStat { get; set; }
        public int cuType { get; set; }
        public int cuDots { get; set; }
        public int cuAmt { get; set; }
        public string cuBad { get; set; }
        public string cuEnd { get; set; }
        /// <summary>
        /// 累計消費分數
        /// </summary>
        public float cuTolT { get; set; }
        public float cuTolA { get; set; }
        public string cuLastD { get; set; }
        public float cuLastM { get; set; }
        public float cuLastT { get; set; }
        /// <summary>
        /// 認證號
        /// </summary>
        public string token { get; set; }
        /// <summary>
        /// PIN CODE
        /// </summary>
        public string cuPin { get; set; }
        /// <summary>
        /// 二為碼
        /// </summary>
        public string cuQR { get; set; }
        /// <summary>
        /// 性別
        /// </summary>
        public string cuGender { get; set; }
        /// <summary>
        /// 生日
        /// </summary>
        public string cuBirthday { get; set; }
        public string createDate { get; set; }
        public string modifyDate { get; set; }
        public string modifyStatus { get; set; }
    }
}
