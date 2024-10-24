using sms_gate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asgdotnet_lib
{
    public class Db
    {
        static public void AddConfig(string? litedb_path, Config config)
        {
            using (var db = new LiteDB.LiteDatabase(litedb_path))
            {
                var col = db.GetCollection<Config>("config");
                col.Insert(config);
            }
        }

        static public int AddMessage(string? litedb_path, Message message)
        {
            using (var db = new LiteDB.LiteDatabase(litedb_path))
            {
                var col = db.GetCollection<Message>("message");
                col.Insert(message);
                return message.Id;
            }
        }

        static public int AddRecipient(string? litedb_path, Recipient recipient)
        {
            using (var db = new LiteDB.LiteDatabase(litedb_path))
            {
                var existing = db.GetCollection<Recipient>("recipient").Find(x => x.BatchId == recipient.BatchId && x.Phone == recipient.Phone).FirstOrDefault();
                if (existing != null)
                    return existing.Id;
                var col = db.GetCollection<Recipient>("recipient");
                col.Insert(recipient);
                return recipient.Id;
            }
        }

        static public int AddBatch(string? litedb_path, Batch batch)
        {
            if (batch.CreationDate == null)
                batch.CreationDate = DateTime.Now;

            using (var db = new LiteDB.LiteDatabase(litedb_path))
            {
                var col = db.GetCollection<Batch>("batch");
                col.Insert(batch);
                return batch.Id;
            }

        }
    }
}
