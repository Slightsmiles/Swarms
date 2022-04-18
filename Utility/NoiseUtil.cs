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
        public double tempFactor;
        static Random random = new Random();
        public NoiseUtil()
        {

        }

        public double withNoise(double temp)
        {
            var factor = temp / 10;
            if (random.Next(2) == 1) return temp += factor;
            else return temp -= factor;
        }

        public Vector2 withNoise(Vector2 from){

            //

            return from;
            

        }

    }
}