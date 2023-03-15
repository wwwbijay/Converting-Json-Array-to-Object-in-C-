using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace FreeSchema1._1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        public TestController()
        {
            
        }
        [HttpPost ("Datas")]
        public void Datas() {

            JObject data = JObject.Parse(@"{
            'id': '0001',
            'type': 'donut',
            'name': 'Cake',
            'ppu': 0.55,
            'batters': {
                'batter': [
                    { 'id': '1001', 'type': 'Regular' },
                    { 'id': '1002', 'type': 'Chocolate' },
                    { 'id': '1003', 'type': 'Blueberry' },
                    { 'id': '1004', 'type': 'Devil\'s Food' }
                ]
            },
            'topping': [
                { 'id': '5001', 'type': 'None' },
                { 'id': '5002', 'type': 'Glazed' },
                { 'id': '5005', 'type': 'Sugar' },
                { 'id': '5007', 'type': 'Powdered Sugar' },
                { 'id': '5006', 'type': 'Chocolate with Sprinkles' },
                { 'id': '5003', 'type': 'Chocolate' },
                { 'id': '5004', 'type': 'Maple' }
            ]
        }");

            Dictionary<string, object> obj = new Dictionary<string, object>();

            DataLooping(data, obj);

            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(obj));

        }

        static void DataLooping(JObject data, Dictionary<string, object> obj)
        {
            Dictionary<string, object> noObjects = new Dictionary<string, object>();
            Dictionary<string, object> objects = new Dictionary<string, object>();

            foreach (var property in data.Properties())
            {

                if (property.Value is not JObject)
                {
                    noObjects[property.Name] = property.Value;
                }
                else
                {
                    var x = new Dictionary<string, object>();
                    DataLooping(property.Value as JObject, x);

                    objects[property.Name] = x;
                }
            }

            foreach (var item in noObjects)
            {
                obj[item.Key] = item.Value;
            }

            foreach (var item in objects)
            {
                obj[item.Key] = item.Value;
            }
        }
    }
}
