using Newtonsoft.Json;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Log4NetIntro
{
    public class DbContext
    {
        private static readonly log4net.ILog my_logger =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static object key = new object();
        private static bool init_succeed = false;
        private static DbContext m_Instance;
        private const string conn_string = "Host=localhost;Username=postgres;Password=admin;Database=postgres";

        private bool TestDbConnection(String conn)
        {
            try
            {
                using (var my_conn = new NpgsqlConnection(conn))
                {
                    my_conn.Open();
                    return true;
                }
            }
            catch (Exception ex)
            {
                my_logger.Fatal($"Cannot connect to Db. Error: {ex}");
                return false;
            }
        }
        private void Init(string conn)
        {
            init_succeed = TestDbConnection(conn);
        }
        private DbContext()
        {
            Init(conn_string);
        }

        public static DbContext Instance
        {
            get
            {
                // to avoid waiting for lock when instance already exist
                if (m_Instance == null)
                {
                    lock (key)
                    {
                        // need to check again since after the first creation
                        // we want to avoid another creation of the instance
                        // in the sequential thread
                        if (m_Instance == null)
                        {
                            m_Instance = new DbContext();
                            if (!init_succeed)
                            {
                                m_Instance = null;
                            }
                        }
                    }
                }
                return m_Instance;
            }
        }

        public List<Movie> GetAllMovies()
        {
            List<Movie> result = new List<Movie>();
            using (var conn = new NpgsqlConnection(conn_string))
            {
                conn.Open();
                string query = "SELECT * From movies m join country on m.country_id = country.id";
                var command = new NpgsqlCommand(query, conn);
                command.CommandType = System.Data.CommandType.Text;

                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Movie m = new Movie
                    {
                        Id = (long)reader["id"],
                        Title = (string)reader["title"],
                        CountryId = (long)reader["country_id"],
                        Price = (double)reader["price"],
                        ReleaseDate = (DateTime)reader["release_date"],
                        CountryName = (string)reader["name"]
                    };
                    result.Add(m);
                }
            }
            return result;
        }

        public int Run_sp_a_sp_max(int _x, int _y)
        {
            int result = 0;
            using (var conn = new NpgsqlConnection(conn_string))
            {
                conn.Open();
                string sp_name = "a_sp_max";
                var command = new NpgsqlCommand(sp_name, conn);
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.Parameters.AddRange(new NpgsqlParameter[]
                {
                    new NpgsqlParameter("x", _x),
                    new NpgsqlParameter("y", _y)
                });

                var reader = command.ExecuteReader();
                if (reader.Read())
                {
                    result = (int)reader["a_sp_max"];
                }
            }
            return result;
        }

        public List<Movie> Run_a_sp_get_movies_mid()
        {
            List<Movie> result = new List<Movie>();
            using (var conn = new NpgsqlConnection(conn_string))
            {
                conn.Open();
                string sp_name = "a_sp_get_movies_mid";
                var command = new NpgsqlCommand(sp_name, conn);
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.Parameters.AddRange(new NpgsqlParameter[]
                {
                });

                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Movie m = new Movie
                    {
                        Id = (long)reader["id"],
                        Title = (string)reader["title"],
                        Price = (double)reader["price"],
                        ReleaseDate = (DateTime)reader["release_date"],
                        CountryName = (string)reader["country_name"]
                    };
                    result.Add(m);
                }
            }
            return result;
        }

    }
}
