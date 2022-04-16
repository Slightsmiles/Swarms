using System.Collections.Concurrent;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using Swarms.Datatypes.Grids;
using Swarms.Entities;

namespace Swarms.Entities
{
    public class Obstacle : Boardentity
    {
        public Obstacle(Vector2 loc) : base(-1, false, loc) {
            _location = loc;
            _color = Color.Aqua;
        }

        public Obstacle(){}
    }
}