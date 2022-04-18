using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Swarms.Utility
{
    public class NoiseUtil
    {
        public double tempFactor;
        public NoiseUtil(){

        }

        public double withNoise(double temp){
            var random = new Random();
            var factor = temp/10;
            if(random.Next(1) == 1) return temp += factor;
            else return temp -= factor;
        }
        
    }
}