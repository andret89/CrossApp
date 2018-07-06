using CrossApp.Models;
using Newtonsoft.Json;
using PCLStorage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CrossApp
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            PCLStorageSample();
        }
        public MainPage(string jsonStr)
        {
            InitializeComponent();
            var objDes = GetJson(jsonStr);
            var interventi = new InterventiModel();
            Parser(objDes, interventi);
            BindingContext = interventi;

        }


        /*
        string path = @"/storage/emulated/0/Android/data/de.testo.ias2015app/files/reports/";
        var fileName = "2018-07-06_22-04-23.tjf";
        var task = FileManager.MyRead(path, fileName);
        */

        private Channel getChannelElem(JsonClassModel root, string key)
        {
            Channel ret = null;
            var channels = root.channels;
            foreach (Channel channel in channels)
            {
                if (channel.type.description.Equals(key))
                {
                    ret = channel;
                }
            }
            return ret;
        }

        private JsonClassModel GetJson(string jsonStr)
        {
            JsonClassModel jsonClass = JsonConvert.DeserializeObject<JsonClassModel>(jsonStr);
            return jsonClass;
        }

        private void Parser(JsonClassModel objRoot, InterventiModel interventi)
        {

            var TF = getChannelElem(objRoot, "TF").values.First().description;
            var TA = getChannelElem(objRoot, "TA").values.First().description;
            var O2 = getChannelElem(objRoot, "O₂").values.First().description;
            var CO = getChannelElem(objRoot, "CO").values.First().description;
            var CO2 = getChannelElem(objRoot, "CO₂").values.First().description;
            var RC = getChannelElem(objRoot, "Rend").values.First().description;

            interventi.INT_SENS_TEMP_FUMI = Convert.ToDouble(TF);
            interventi.INT_SENS_TEMP_ARIA = Convert.ToDouble(TA);
            interventi.INT_SENS_O2 = Convert.ToDouble(O2);
            interventi.INT_SENS_CO2 = Convert.ToDouble(CO2);
            interventi.INT_SENS_CO_CORRETTO = Convert.ToDouble(CO);
            interventi.INT_SENS_REND_COMB = Convert.ToDouble(RC);
            //interventi.INT_SENS_REND_MIN = Convert.ToDouble(TF);
            //interventi.INT_MOD_TERM = Convert.ToDouble(TF);
        }

        async void Handle_Clicked(object sender, System.EventArgs e)
        {
            await Navigation.PopModalAsync(true);
        }

        private void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            ISenderService storeService = DependencyService.Get<ISenderService>();
            storeService.sendRequest();

        }

        private string jsonText =
                @"{
  ""additionalMeasurementInformation"": [
  ],
  ""channels"": [
      {
          ""type"": {
              ""description"": ""TF"",
              ""name"": ""FT"",
              ""xmlid"": ""T_Flue""
          },
          ""unit"": {
              ""name"": ""°C"",
              ""xmlid"": ""UNIT_DEG_C""
          },
          ""values"": [
              {
                  ""additionalData"": [
                      {
                          ""name"": ""dilutionFactor"",
                          ""value"": 1
                      },
                      {
                          ""name"": ""noxflag"",
                          ""value"": false
                      }
                  ],
                  ""description"": ""292,4"",
                  ""exponent"": 1,
                  ""timeStamp"": ""2018-07-06T13:04:06"",
                  ""value"": 292.35125732421875
              }
          ]
      },
      {
          ""type"": {
              ""description"": ""TA"",
              ""name"": ""AT"",
              ""xmlid"": ""T_Air""
          },
          ""unit"": {
              ""name"": ""°C"",
              ""xmlid"": ""UNIT_DEG_C""
          },
          ""values"": [
              {
                  ""additionalData"": [
                      {
                          ""name"": ""dilutionFactor"",
                          ""value"": 1
                      },
                      {
                          ""name"": ""noxflag"",
                          ""value"": false
                      }
                  ],
                  ""description"": ""40,2"",
                  ""exponent"": 1,
                  ""timeStamp"": ""2018-07-06T13:04:06"",
                  ""value"": 40.18928527832031
              }
          ]
      },
      {
          ""type"": {
              ""description"": ""O₂"",
              ""name"": ""O2"",
              ""xmlid"": ""O2""
          },
          ""unit"": {
              ""name"": ""%"",
              ""xmlid"": ""UNIT_PERCENT""
          },
          ""values"": [
              {
                  ""additionalData"": [
                      {
                          ""name"": ""dilutionFactor"",
                          ""value"": 1
                      },
                      {
                          ""name"": ""noxflag"",
                          ""value"": false
                      }
                  ],
                  ""description"": ""164,0"",
                  ""exponent"": 1,
                  ""timeStamp"": ""2018-07-06T13:04:06"",
                  ""value"": 164.03233337402344
              }
          ]
      },
      {
          ""type"": {
              ""description"": ""CO"",
              ""name"": ""CO"",
              ""xmlid"": ""CO_Dil""
          },
          ""unit"": {
              ""name"": ""ppm"",
              ""xmlid"": ""UNIT_PPM""
          },
          ""values"": [
              {
                  ""additionalData"": [
                      {
                          ""name"": ""dilutionFactor"",
                          ""value"": 0.31157228350639343
                      },
                      {
                          ""name"": ""noxflag"",
                          ""value"": false
                      }
                  ],
                  ""description"": ""163"",
                  ""exponent"": 0,
                  ""timeStamp"": ""2018-07-06T13:04:06"",
                  ""value"": 162.97592163085938
              }
          ]
      },
      {
          ""type"": {
              ""description"": ""λ"",
              ""name"": ""λ"",
              ""xmlid"": ""Lambda""
          },
          ""unit"": {
              ""name"": """",
              ""xmlid"": ""UNIT_NONE""
          },
          ""values"": [
              {
                  ""additionalData"": [
                      {
                          ""name"": ""dilutionFactor"",
                          ""value"": 1
                      },
                      {
                          ""name"": ""noxflag"",
                          ""value"": false
                      }
                  ],
                  ""description"": ""14,49"",
                  ""exponent"": 2,
                  ""timeStamp"": ""2018-07-06T13:04:06"",
                  ""value"": 14.494154930114746
              }
          ]
      },
      {
          ""type"": {
              ""description"": ""CO₂"",
              ""name"": ""CO2"",
              ""xmlid"": ""CO2""
          },
          ""unit"": {
              ""name"": ""%"",
              ""xmlid"": ""UNIT_PERCENT""
          },
          ""values"": [
              {
                  ""additionalData"": [
                      {
                          ""name"": ""dilutionFactor"",
                          ""value"": 1
                      },
                      {
                          ""name"": ""noxflag"",
                          ""value"": false
                      }
                  ],
                  ""description"": ""133,1"",
                  ""exponent"": 1,
                  ""timeStamp"": ""2018-07-06T13:04:06"",
                  ""value"": 133.08682250976562
              }
          ]
      },
      {
          ""type"": {
              ""description"": ""Tiraggio"",
              ""name"": ""Draught"",
              ""xmlid"": ""P_Draft""
          },
          ""unit"": {
              ""name"": ""mbar"",
              ""xmlid"": ""UNIT_MBAR""
          },
          ""values"": [
              {
                  ""additionalData"": [
                      {
                          ""name"": ""dilutionFactor"",
                          ""value"": 1
                      },
                      {
                          ""name"": ""noxflag"",
                          ""value"": false
                      }
                  ],
                  ""description"": ""263,5"",
                  ""exponent"": 1,
                  ""timeStamp"": ""2018-07-06T13:04:06"",
                  ""value"": 263.5469055175781
              }
          ]
      },
      {
          ""type"": {
              ""description"": ""uCO"",
              ""name"": ""uCO"",
              ""xmlid"": ""CO_Norm""
          },
          ""unit"": {
              ""name"": ""ppm"",
              ""xmlid"": ""UNIT_PPM""
          },
          ""values"": [
              {
                  ""additionalData"": [
                      {
                          ""name"": ""dilutionFactor"",
                          ""value"": 1
                      },
                      {
                          ""name"": ""noxflag"",
                          ""value"": false
                      }
                  ],
                  ""description"": ""387"",
                  ""exponent"": 0,
                  ""timeStamp"": ""2018-07-06T13:04:06"",
                  ""value"": 387.10235595703125
              }
          ]
      },
      {
          ""type"": {
              ""description"": ""Qs"",
              ""name"": ""qAnet"",
              ""xmlid"": ""QAnet""
          },
          ""unit"": {
              ""name"": ""%"",
              ""xmlid"": ""UNIT_PERCENT""
          },
          ""values"": [
              {
                  ""additionalData"": [
                      {
                          ""name"": ""dilutionFactor"",
                          ""value"": 1
                      },
                      {
                          ""name"": ""noxflag"",
                          ""value"": false
                      }
                  ],
                  ""description"": ""284,3"",
                  ""exponent"": 1,
                  ""timeStamp"": ""2018-07-06T13:04:06"",
                  ""value"": 284.29583740234375
              }
          ]
      },
      {
          ""type"": {
              ""description"": ""Rend"",
              ""name"": ""Effg"",
              ""xmlid"": ""Effg""
          },
          ""unit"": {
              ""name"": ""%"",
              ""xmlid"": ""UNIT_PERCENT""
          },
          ""values"": [
              {
                  ""additionalData"": [
                      {
                          ""name"": ""dilutionFactor"",
                          ""value"": 1
                      },
                      {
                          ""name"": ""noxflag"",
                          ""value"": false
                      }
                  ],
                  ""description"": ""2,4"",
                  ""exponent"": 1,
                  ""timeStamp"": ""2018-07-06T13:04:06"",
                  ""value"": 2.4468767642974854
              }
          ]
      },
      {
          ""type"": {
              ""description"": ""No"",
              ""name"": ""NO"",
              ""xmlid"": ""NO_Dil""
          },
          ""unit"": {
              ""name"": ""ppm"",
              ""xmlid"": ""UNIT_PPM""
          },
          ""values"": [
              {
                  ""additionalData"": [
                      {
                          ""name"": ""dilutionFactor"",
                          ""value"": 1
                      },
                      {
                          ""name"": ""noxflag"",
                          ""value"": false
                      }
                  ],
                  ""description"": ""61"",
                  ""exponent"": 0,
                  ""timeStamp"": ""2018-07-06T13:04:06"",
                  ""value"": 61.09447479248047
              }
          ]
      },
      {
          ""type"": {
              ""description"": ""NOx"",
              ""name"": ""NOx"",
              ""xmlid"": ""NOx_Dil""
          },
          ""unit"": {
              ""name"": ""ppm"",
              ""xmlid"": ""UNIT_PPM""
          },
          ""values"": [
              {
                  ""additionalData"": [
                      {
                          ""name"": ""dilutionFactor"",
                          ""value"": 1
                      },
                      {
                          ""name"": ""noxflag"",
                          ""value"": false
                      }
                  ],
                  ""description"": ""385,5"",
                  ""exponent"": 1,
                  ""timeStamp"": ""2018-07-06T13:04:06"",
                  ""value"": 385.48193359375
              }
          ]
      },
      {
          ""type"": {
              ""description"": ""cNOx"",
              ""name"": ""cNOx"",
              ""xmlid"": ""NOxRed""
          },
          ""unit"": {
              ""name"": ""ppm"",
              ""xmlid"": ""UNIT_PPM""
          },
          ""values"": [
              {
                  ""additionalData"": [
                      {
                          ""name"": ""dilutionFactor"",
                          ""value"": 1
                      },
                      {
                          ""name"": ""noxflag"",
                          ""value"": false
                      }
                  ],
                  ""description"": ""63,8"",
                  ""exponent"": 1,
                  ""timeStamp"": ""2018-07-06T13:04:06"",
                  ""value"": 63.83122253417969
              }
          ]
      },
      {
          ""type"": {
              ""description"": ""tdp"",
              ""name"": ""Dew Pt"",
              ""xmlid"": ""T_Dew""
          },
          ""unit"": {
              ""name"": ""°C"",
              ""xmlid"": ""UNIT_DEG_C""
          },
          ""values"": [
              {
                  ""additionalData"": [
                      {
                          ""name"": ""dilutionFactor"",
                          ""value"": 1
                      },
                      {
                          ""name"": ""noxflag"",
                          ""value"": false
                      }
                  ],
                  ""description"": ""375,9"",
                  ""exponent"": 1,
                  ""timeStamp"": ""2018-07-06T13:04:06"",
                  ""value"": 375.9461669921875
              }
          ]
      },
      {
          ""type"": {
              ""description"": ""ET"",
              ""name"": ""ET"",
              ""xmlid"": ""ET""
          },
          ""unit"": {
              ""name"": ""%"",
              ""xmlid"": ""UNIT_PERCENT""
          },
          ""values"": [
              {
                  ""additionalData"": [
                      {
                          ""name"": ""dilutionFactor"",
                          ""value"": 1
                      },
                      {
                          ""name"": ""noxflag"",
                          ""value"": false
                      }
                  ],
                  ""description"": ""221,6"",
                  ""exponent"": 1,
                  ""timeStamp"": ""2018-07-06T13:04:06"",
                  ""value"": 221.62892150878906
              }
          ]
      }
  ],
  ""device"": {
      ""deviceIdentifier"": ""TT3502922177NEU0815"",
      ""lastServiceDate"": ""01.01.16"",
      ""name"": ""demo"",
      ""serial"": 2922177
  },
  ""properties"": [
      {
          ""description"": ""Gas Naturale"",
          ""id"": 6000,
          ""name"": ""Fuel"",
          ""values"": [
              {
                  ""description"": ""3%"",
                  ""name"": ""O₂ Riferimento"",
                  ""value"": 3
              }
          ]
      }
  ],
  ""schemaVersion"": ""1.0.4"",
  ""timeStamp"": ""2018-07-06T13:04:06"",
  ""type"": {
      ""name"": ""Analisi gas""
  }
}";
    }

}

