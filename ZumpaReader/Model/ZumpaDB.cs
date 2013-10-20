using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;

namespace ZumpaReader.Model
{
    public class ZumpaDB : DataContext
    {
        public const string DBConnectionString = "Data Source=isostore:/ZumpaDB.sdf";

        static ZumpaDB()
        {
            _instance = CreateDatabaseIfNecessary();
        }

        public ZumpaDB(string connectionString = DBConnectionString) : base(connectionString) { }

        public Table<ImageRecord> Images;

        private static ZumpaDB _instance;
        public static ZumpaDB Instance
        {
            get
            {
                return _instance;
            }
        }


        private static ZumpaDB CreateDatabaseIfNecessary()
        {
            ZumpaDB db = new ZumpaDB(DBConnectionString);
            if (!db.DatabaseExists())
            {
                db.CreateDatabase();
            }
            return db;
        }
    }
}
