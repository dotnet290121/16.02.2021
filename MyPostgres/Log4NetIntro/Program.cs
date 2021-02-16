using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Log4NetIntro
{
    class Program
    {
        private static readonly log4net.ILog my_logger =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        
        static void Main(string[] args)
        {
            my_logger.Info("****************** System startup");


            my_logger.Info("Connecting to Db");
            List<Movie> movies = DbContext.Instance.GetAllMovies();
            movies.ForEach(m => Console.WriteLine(m));

            int result = DbContext.Instance.Run_sp_a_sp_max(709, 708);
            Console.WriteLine($"max 709 , 708? {result}");

            Console.WriteLine("************ mid ****************");
            List<Movie> mid_movies = DbContext.Instance.Run_a_sp_get_movies_mid();
            mid_movies.ForEach(m => Console.WriteLine(m));
            


            my_logger.Info("******************** System shutdown");


            Console.WriteLine("Press Enter");
            Console.ReadLine();
        }
    }
}
