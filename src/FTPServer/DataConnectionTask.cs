namespace FTPServer
{
    public class DataConnectionTask
    {
        public DataConnectionTaskType Type { get; set; } = DataConnectionTaskType.Unknown;

        public string Arguments { get; set; } = null;
    }

    public enum DataConnectionTaskType
    {
        // Unknown option
        Unknown = -1,
        // List option
        List = 0,
        // Retrieve option
        Download = 1,
        // Upload option
        Upload = 2,
    }
}
