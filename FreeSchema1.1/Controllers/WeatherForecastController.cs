using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Xml.Linq;

namespace FreeSchema1._1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
    
        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpPost("GetData")]
        public void GetData([FromBody] dynamic forecast) {

            Console.WriteLine(forecast.GetType());

            JsonElement d = forecast as dynamic;
            JsonValueKind jsonValueKind = d.ValueKind;
            Dictionary<string, dynamic> initialData = new Dictionary<string, dynamic>();

            if (jsonValueKind == JsonValueKind.Array)
            {
                Console.WriteLine("Ok");

                var list = JsonConvert.DeserializeObject<IEnumerable<KeyValuePair<string, string>>>(forecast.GetRawText());

                Console.WriteLine("Please entery JSOn data instead of array");
            }
            else
            {
                // Console.WriteLine(JsonConvert.SerializeObject(finalData));
               initialData = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(forecast.GetRawText());

                dynamic test = dataLooping(initialData);
                Console.WriteLine(JsonConvert.SerializeObject(test));
            }

            
           
           
          
        }

        private Dictionary<string, dynamic> dataLooping(Dictionary<string, dynamic> initialData)
        {

            Dictionary<string, dynamic> noObjects = new();
            Dictionary<string, dynamic> objects = new();

            Dictionary<string, dynamic> finalData = new Dictionary<string, dynamic>();

            foreach (KeyValuePair<string, dynamic> kvp in initialData)
            {
                dynamic ValueData = kvp.Value;


                if (ValueData.GetType() == typeof(JArray) )
                {
                    var i = 0;

                   List<dynamic> temp = new List<dynamic>();
                    Dictionary<string, dynamic> x = new Dictionary<string, dynamic>();
                    List<Dictionary<string, dynamic>> abc = new List<Dictionary<string, dynamic>>();

                    foreach (dynamic item in ValueData)
                    {
                        if (item.GetType() == typeof(JObject))
                        {
                            Dictionary<string, dynamic> dItem = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(item.ToString());
                            abc.Add(dataLooping(dItem));
                        }
                        else
                        {
                            temp.Add(item);
                        }
                

                      /*  if (item.GetType() == typeof(JObject))
                        {
                            temp.Add(i.ToString(), dataLooping(dItem));
                        }
                        else
                        {
                            temp.Add(i.ToString(), item);
                        }*/
                      
                        i++;
                    }

                    if (abc.Count > 0)
                    {
                         x.Add(kvp.Key, DicListToDictionary(abc));
                    }
                    else
                    {
                          x.Add(kvp.Key, StringListToDictionary(temp));
                    }

                    

                    objects = mergeDictionary(objects, x);

                    // data = JsonConvert.SerializeObject(temp);

                } else if (ValueData.GetType() == typeof(JObject))
                {
                    Dictionary<string, dynamic> x = new Dictionary<string, dynamic>();
                    Dictionary<string, dynamic> dData = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(ValueData.ToString());
                    Console.WriteLine(dData);
                   
                    Dictionary<string, dynamic> y = dataLooping(dData);
                    x.Add(kvp.Key, y);

                    objects = mergeDictionary(objects, x);

                    Console.WriteLine(objects);
                }
                else
                {
                    noObjects.Add(kvp.Key, ValueData);
                }

                // Console.WriteLine(data);
             }

            finalData = mergeDictionary(objects, noObjects);

            return finalData;

        }

        private Dictionary<string, dynamic> DicListToDictionary(List<Dictionary<string, dynamic>> myList)
        {
            int count = 0;
            Dictionary<string, dynamic> myDict = new Dictionary<string, dynamic>();

            foreach (Dictionary<string, dynamic> myString in myList)
            {
                myDict.Add(count.ToString(), myString);
                count++;
            }

            return myDict;
        }

        private Dictionary<string, dynamic> StringListToDictionary(List<dynamic> myList)
        {
            int count = 0;
            Dictionary<string, dynamic> myDict = new Dictionary<string, dynamic>();

            foreach (dynamic myString in myList)
            {
                myDict.Add(count.ToString(), myString);
                count++;
            }

            return myDict;
        }

        private Dictionary<string, dynamic> listToDictionary(List<dynamic> myList)
        {
            int count = 0;
            Dictionary<string, dynamic> myDict = new Dictionary<string, dynamic>();

            foreach (string myString in myList)
            {
                myDict.Add(count.ToString(), myString);
                count++;
            }

            return myDict;
        }

        private Dictionary<string, dynamic> mergeDictionary(Dictionary<string, dynamic> d1, Dictionary<string, dynamic> d2)
        {
            foreach (KeyValuePair<string, dynamic> kvp in d1)
            {
                d2.Add(kvp.Key, kvp.Value);
            }
            return d2;
        }

        /*  private void getChild(dynamic data)
          {

              var dictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(data.GetRawText());
              //Console.WriteLine(dictionary);

              Dictionary<string, dynamic> finalData = new Dictionary<string, dynamic>();


              //Console.WriteLine(item);
              foreach (KeyValuePair<string, object> kvp in dictionary)
              {

                  dynamic json = kvp.Value;

                  if (json.GetType() == typeof(JArray))
                  {
                      // Console.WriteLine(json);
                      var i = 0;
                      Dictionary<string, dynamic> temp = new Dictionary<string, dynamic>();
                      foreach (JObject item in json)
                      {

                          temp.Add(i.ToString(), item);

                          i++;
                      }

                      data = JsonConvert.SerializeObject(temp);

                      foreach (KeyValuePair<string, object> kvp_data in data)
                      {
                          if (kvp_data.GetType() == typeof(JArray))
                          {
                              getChild(kvp_data.Value);
                          }

                      }



                  }



              }


          }*/
    }
}