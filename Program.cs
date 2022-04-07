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

            if(args.Length != 0) {
                x = Int32.Parse(args[0]);
                y = Int32.Parse(args[1]);
            }

            using (var game = new Game1(x, y))
                game.Run();
        }
    }
}
