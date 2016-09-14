namespace CloudMagick_Client_Gui.JSONstuff
{
    class Result
    {
        public int ExecutionTime { get; set; }
        public int ConversionTime { get; set; }
        public int SendingTime { get; set; }

        public override string ToString()
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(this);
        }
    }
}
