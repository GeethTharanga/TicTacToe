using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlServerCe;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicTacToe.Stats;

namespace T3DBStatRepository 
{
    public class DBRecordRepository: IGameRecordRepository
    {
        SqlCeConnection conn;
        private string DatabasePath
        {
            get
            {
                return "game_history.sdf";
            }
        }

        private string ConnectionString
        {
            get
            {
                return string.Format("DataSource=\"{0}\";Max Database Size=3000;", DatabasePath);
            }
        }

        public DBRecordRepository()
        {
            InitConnection();
        }

        private void InitConnection()
        {
            bool createTables = false;
            if(!File.Exists(DatabasePath))
            {
                SqlCeEngine en = new SqlCeEngine(ConnectionString);
                en.CreateDatabase();
                en.Dispose();
                createTables = true;
            }
            conn = new SqlCeConnection(ConnectionString);
            conn.Open();

            if(createTables)
            {
                using (SqlCeCommand comm = conn.CreateCommand())
                { 
                    comm.CommandText = "CREATE TABLE History (GameTime DateTime PRIMARY KEY, Opponent NTEXT NOT NULL, Result Integer NOT NULL)";
                    comm.ExecuteNonQuery();
                }
            }


        }

        public void SaveRecord(GamePlayRecord record)
        {
            using (SqlCeCommand comm = conn.CreateCommand())
            {
                comm.CommandText = "INSERT INTO History(GameTime, Opponent, Result) VALUES (@time, @opponent, @result)";
                comm.Parameters.Add(new SqlCeParameter("@time", SqlDbType.DateTime));
                comm.Parameters.Add(new SqlCeParameter("@opponent", SqlDbType.NText));
                comm.Parameters.Add(new SqlCeParameter("@result", SqlDbType.Int));

                comm.Prepare();

                comm.Parameters[0].Value = record.Time;
                comm.Parameters[1].Value = record.Opponent;
                comm.Parameters[2].Value = (int)record.Result;

                comm.ExecuteNonQuery();
            }
        }

        public IEnumerable<GamePlayRecord> GetLastRecords(int maxCount)
        {
            List<GamePlayRecord> results = new List<GamePlayRecord>();
            using (SqlCeCommand comm = conn.CreateCommand())
            {
                string text = @"SELECT TOP ( {0} ) GameTime, Opponent, Result FROM History
                            ORDER BY GameTime DESC";
                comm.CommandText = string.Format(text, maxCount);
                var reader = comm.ExecuteReader();

                while (reader.Read())
                {
                    GamePlayRecord rec = new GamePlayRecord();
                    rec.Time = reader.GetDateTime(0);
                    rec.Opponent = reader.GetString(1);
                    rec.Result = (GamePlayResult)reader.GetInt32(2);
                    results.Add(rec);
                }
            }

            return results;
        }

        public IDictionary<string, GamePlayStatistics> GetStatistics()
        {
            //initialize empty
            Dictionary<string, GamePlayStatistics> results = new Dictionary<string, GamePlayStatistics>();


            using (SqlCeCommand comm = conn.CreateCommand())
            {
                comm.CommandText = @"SELECT Opponent, Result, COUNT(*) FROM History
                            GROUP BY Opponent, Result";

                var reader = comm.ExecuteReader();
                while (reader.Read())
                {
                    string opp = reader.GetString(0);
                    GamePlayResult res = (GamePlayResult)reader.GetInt32(1);
                    int count = reader.GetInt32(2);
                    switch (res)
                    {
                        case GamePlayResult.Tied:
                            results[opp].Tied = count;
                            break;
                        case GamePlayResult.Won:
                            results[opp].Wins = count;
                            break;
                        case GamePlayResult.Loss:
                            results[opp].Losses = count;
                            break;
                    }
                }
            }

            return results;
        }

        public void ClearHistory()
        {
            using (SqlCeCommand comm = conn.CreateCommand())
            {
                comm.CommandText = "DELETE FROM History" ;
                comm.ExecuteNonQuery();
            } 
        }

        public void Dispose()
        {
            conn.Dispose();
        }
    }
}
