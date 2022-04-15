using System;

namespace Swarms
{
    public static class Program
    {
        [STAThread]
        static void Main(String[] args)
        {
            var x = 40;
            var y = 24;
            var screenHeight =  480;
            var screenWidth = 800;
            var logging = false;
            var lower = 40;
            var mid = 80;
            var high = 120;
            
            if(args.Length != 0)
            { 
            
                logging = bool.Parse(args[0]);
                lower = Int32.Parse(args[1]);
                mid = Int32.Parse(args[2]);
                high = Int32.Parse(args[3]);


                if (args.Length > 4)
                {
                    x = Int32.Parse(args[4]);
                    y = Int32.Parse(args[5]);
                }

                if(args.Length > 6) {
                    screenHeight = Int32.Parse(args[6]);
                    screenWidth = Int32.Parse(args[7]);
                }

               
              
            }

            using (var game = new Game1(x, y, screenHeight, screenWidth, logging, lower, mid, high))
                game.Run();
        }
    }
}
