using Game.Client.Messages;

namespace Game
{
    public class JSONSerializer
    {
        public static string Serialize(IMessage message)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(message);
        }

        public static T Deserialize<T>(string message)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(message);
        }
    }
}
