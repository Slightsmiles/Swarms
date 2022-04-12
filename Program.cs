﻿using System;

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
            
            if(args.Length != 0) {
                x = Int32.Parse(args[0]);
                y = Int32.Parse(args[1]);
                 
                if(args.Length > 2) {
                    screenHeight = Int32.Parse(args[2]);
                    screenWidth = Int32.Parse(args[3]);
                }

               
                if (args.Length > 4)
                {
                    logging = bool.Parse(args[4]);
                }
            }

            using (var game = new Game1(x, y, screenHeight, screenWidth, logging))
                game.Run();
        }
    }
}
