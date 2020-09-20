namespace Model
{
    public class Airport
    {
        public string Name { get; set; }
        public Position Position { get; set; }
        public bool IsOperational { get; set; }
        public int MqttIp { get; set; }
        public int MqttPort { get; set; }
        public int MqttTopic { get; set; }
    }
}
