using LiteDB;

namespace asgdotnet_lib
{
    public class Config
    {
        public int Id { get; set; }
        public string? Login { get; set; }
        public string? Password { get; set; }
        public int SimNumber{ get; set; }

    }
    public class Message
    {
        public int Id{ get; set; }
        public int BatchId{ get; set; }
        public string? MessageText{ get; set; }
    }

    public class Recipient
    {
        public int Id{ get; set; }
        public int BatchId{ get; set; }
        public string? Phone{ get; set; }
        public bool Sent{ get; set; }
        public string? Error{ get; set; }
    }

    public class Batch
    {
        public int Id{ get; set; }
        public string? Name{ get; set; }
        public DateTime? CreationDate{ get; set; }
        public DateTime? LastSentDate{ get; set; }
        public int SecondsBetweenMessages { get; set; } = 30;
        public bool Finished { get; set; } = false;
    }

}
