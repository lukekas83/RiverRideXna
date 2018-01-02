using System;

namespace RiverRideGame
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (RiverRide game = new RiverRide())
            {
                game.Run();
            }
        }
    }
}

