using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace asgdotnet_lib
{
    public class Rest
    {
        public async Task<bool> ProcessBatchAsync(string? litedb_path, int configId)
        {
            int count = 0;
            using (var db = new LiteDB.LiteDatabase(litedb_path))
            {
                Config config = db.GetCollection<Config>("config").FindById(configId);
                if (config != null)
                {
                    var batches = db.GetCollection<Batch>("batch").Find(_ => !_.Finished).ToList();
                    if (batches.Count > 0)
                    {
                        foreach (Batch batch in batches)
                        {
                            List<Recipient> recipients = db.GetCollection<Recipient>("recipient").Find(x => x.BatchId == batch.Id && !x.Sent).ToList();
                            if (recipients.Count > 0)
                            {
                                count++;
                                if (batch.LastSentDate == null || batch.LastSentDate.Value.AddSeconds(batch.SecondsBetweenMessages) < DateTime.Now)
                                {
                                    Message? message = db.GetCollection<Message>("message").Find(_ => _.BatchId == batch.Id).FirstOrDefault();
                                    if (message != null)
                                    {
                                        // Send messages to recipients in batch
                                        HttpClient http = new HttpClient();
                                        // Basic authentication
                                        var byteArray = Encoding.ASCII.GetBytes(config.Login + ":" + config.Password);
                                        http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

                                        sms_gate.swaggerClient client = new(http);
                                        var state = await client.MessagePOSTAsync(true, new sms_gate.Message()
                                        {
                                            Id = Guid.NewGuid().ToString(),
                                            IsEncrypted = false,
                                            Message1 = message.MessageText,
                                            PhoneNumbers = new List<string>() { recipients[0].Phone ?? string.Empty },
                                            SimNumber = config.SimNumber,
                                        });

                                        Console.WriteLine("Sent to " + recipients[0].Phone);

                                        recipients[0].Sent = true;
                                        db.GetCollection<Recipient>("recipient").Update(recipients[0]);
                                        batch.LastSentDate = DateTime.Now;
                                        db.GetCollection<Batch>("batch").Update(batch);
                                    }
                                }
                            }
                            else
                            {
                                Log.Info($"No more recipients for batchId={batch.Id}.");
                                batch.Finished = true;
                                db.GetCollection<Batch>("batch").Update(batch);
                            }
                        }
                    }
                }
                else
                    Log.Info($"Can't find config {configId}.");
            }
            return count > 0;
        }
    }
}
