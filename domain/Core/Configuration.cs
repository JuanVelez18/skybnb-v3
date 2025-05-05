namespace domain.Core
{
    public class Configuration
    {
        public static string ConexionString { get; } = "";
        public static string Key { get; } = "";

        static Configuration()
        {
            string JsonPath = "../secrets.json";
            if (!File.Exists(JsonPath))
            {
                Console.Error.WriteLine("The \"secrets.json\" is missed");
                return;
            }

            StreamReader jsonStream = File.OpenText(JsonPath);
            var json = jsonStream.ReadToEnd();
            var datos = JsonConversor.ToObject<Dictionary<string, string>>(json)!;

            ConexionString = datos["ConexionString"]!;
            Key = datos["Key"]!;
        }
    }
}
