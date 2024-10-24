using asgdotnet_lib;

// This example reads phone numbers from a simple text file, one phone number per line.

string? dbPath = @"c:\temp\test.db";
string? phonePath = @"J:\My Drive\Aux Cousinzins\Communication\telephones.txt";

CommandLineParser parser = new CommandLineParser();

dbPath = parser.GetString("path", dbPath);

// Reset the database.
if (parser.GetBool("reset", false))
{
    if (File.Exists(dbPath))
        File.Delete(dbPath);
}

// In this bloc we create the database.
if (parser.GetBool("create", false))
{
    using (StreamReader sr = new StreamReader(phonePath))
    {
        //var login = parser.GetEnv("login", "login");
        //var password = parser.GetEnv("password", "password");
        var login = sr.ReadLine();
        var password = sr.ReadLine();
        Db.AddConfig(dbPath, new Config() { Id = 1, Login = login, Password = password, SimNumber = 2 });
        int batchId = Db.AddBatch(dbPath, new Batch());
        int messageId = Db.AddMessage(dbPath, new Message() { BatchId = batchId, MessageText = "My message" });
        string? line;
        while ((line = sr.ReadLine()) != null)
        {
            line = line.Trim();
            if (line[0] == '0')
            {
                line = "+33" + line.Substring(1);
            }
            Db.AddRecipient(dbPath, new Recipient() { BatchId = batchId, Phone = line });
        }
    }
}

// In this bloc we send the messages at 30 second intervals.
if (parser.GetBool("run", false))
{
    Rest rest = new Rest();
    while (await rest.ProcessBatchAsync(dbPath, 1))
        await Task.Delay(1000);
}

Console.WriteLine("Done.");