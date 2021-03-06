using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Swarms.Entities;
using Microsoft.Xna.Framework;
namespace Swarms.Utility
{
    public class NoiseUtil
    {
        public double tempFactor {get; set;} = 20;
        static Random random = new Random();
        public NoiseUtil()
        {

        }

        public double withNoise(double temp)
        {
            var tempDiff = temp / tempFactor;
            if (random.Next(2) == 1) return temp += tempDiff;
            else return temp -= tempDiff;
        }

        public Vector2 withNoise(Vector2 from){

            if(from.X != -1 && from.Y != -1 && from.Y != 0 && from.X != 0)
            switch (random.Next(40)){
                case 10:
                    return new Vector2(from.X+1,from.Y);
                case 20:
                    return new Vector2(from.X-1,from.Y);
                case 30:
                    return new Vector2(from.X,from.Y+1);
                case 40:
                    return new Vector2(from.X+1,from.Y-1);
            }
            return from;
            

        }

    }
}