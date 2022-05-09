
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

using System.IO;

using System.Xml.Serialization;
using Swarms.Datatypes.Grids;
using Swarms.Entities;

namespace Swarms
{
    public class Heatmapper
    {


        public Heatmapper() { }

      /*   public int[][] countOccurences(GridLocation[][][] grids)
        {
            var mapped = new int[40][];
            var newmapped = fillArray(mapped);
            for (int i = 0; i < grids.Length; i++)
            {
                for (int j = 0; j < 40; j++)
                {
                    for (int k = 0; k < 24; k++)
                    {
                        if (grids[i][j][k].GetType() == typeof(Agent)) newmapped[j][k] += 1;
                    }
                }

            }

            return newmapped;
        } */

        public int[][] fillArray(int[][] array){
            for(int i = 0; i< array.Length; i++){
                array[i] = new int[24];
                for(int j=0; j< array[0].Length; j++){
                    array[i][j] = 0;
                }

                
            }
            return array;
        }
        public Color HeatMap(decimal value, decimal min, decimal max)
        {
            if(value == 0) return Color.WhiteSmoke;
            decimal val = (value - min) / (max - min);
            return new Color
            {
                A = 255,
                R = Convert.ToByte(255 * val),
                G = Convert.ToByte(255 * (1 - val)),
                B = 0
            };
        }
    }
} 