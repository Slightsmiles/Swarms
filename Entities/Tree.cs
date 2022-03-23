using System.Runtime.CompilerServices;
using System.Globalization;
using System;
using Microsoft.Xna.Framework;
using Swarms.Datatypes.Grids;

namespace Swarms.Entities
{
    public class Tree : Boardentity
    {

        bool isBurning;

        public Tree(Vector2 location) : base(-1, false, location){
            _location = location;
            _temp = defaultTemp;
            _color = GetColor();
        }

        
        public new Color GetColor(){
            if (_temp <= 80) return Color.LawnGreen;
            else return Color.Red;
        }
    }
}