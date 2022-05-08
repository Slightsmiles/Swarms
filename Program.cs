using System;

namespace Swarms
{
    public static class Program
    {
        [STAThread]
        static void Main(String[] args)
        {
            var _x = 40;
            var _y = 24;
            var _screenHeight =  480;
            var _screenWidth = 800;
            var _logging = false;
            var _lower = 10;
            var _mid = 80;
            var _high = 120;
            var _isMapping = false;
            
            if(args.Length != 0)
            { 
                for (int i = 0; i < args.Length; i++)
                {
                    var arg = args[i].Split(":");
                    
                    switch (arg[0])
                    {
                        case "x":
                            _x = Int32.Parse(arg[1]);
                            break;
                        case "y":
                            _y = Int32.Parse(arg[1]);
                            break;
                        case "sw": 
                            _screenWidth = Int32.Parse(arg[1]);
                            break;
                        case "sh": 
                            _screenHeight = Int32.Parse(arg[1]);
                            break;
                        case "log":
                            _logging = bool.Parse(arg[1]);
                            break;
                        case "low":
                            _lower = Int32.Parse(arg[1]);
                            break;
                        case "mid":
                            _mid = Int32.Parse(arg[1]);
                            break;
                        case "high":
                            _high = Int32.Parse(arg[1]);
                            break;
                    }
                }
            }

            using (var game = new Game1(_x, _y, _screenHeight, _screenWidth, _logging, _isMapping, _lower, _mid, _high))
                game.Run();
        }
    }
}
