using System.Runtime.CompilerServices;
using System.Globalization;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Swarms.Datatypes.Grids;

namespace Swarms.Entities
{
    public class Tree : Boardentity
    {

        public bool _isBurning {get; set;}

        public Tree(Vector2 location) : base(-1, false, location){
            _location = location;
            _temp = 60;
            _color = GetColor();
        }
        public Tree(){}

        
        public Color GetColor()
        {
            return _temp <= 80 ? Color.LawnGreen : Color.Red;
        }

        public void TickTemp(List<Tree> trees)
        {
            var adjacentSquares = getAdjacentSquares();
            var adjacentTrees = getAdjacentTrees(adjacentSquares, trees);
            
            foreach (var tree in adjacentTrees)
            {
                var temp = tree._temp;
                if (temp >= 60 && temp <= 100) _temp += 1;
                if (temp > 100) _temp += 2;
            }
            
            _color = GetColor();
        }

        private List<Tree> getAdjacentTrees(List<Vector2> adjacentSquares, List<Tree> trees)
        {
            var adjacentTrees = new List<Tree>();
            foreach (var loc in adjacentSquares)
            {
                foreach (var tree in trees)
                {
                    if (tree._location == loc)
                    {
                        adjacentTrees.Add(tree);
                    }
                }
            }

            return adjacentTrees;
        }

        private List<Vector2> getAdjacentSquares()
        {
            var adjacent = new List<Vector2>();
            for (float i = 1; i < 3; i++)
            {
                adjacent.Add(new Vector2(_location.X, _location.Y - i)); //up
                adjacent.Add(new Vector2(_location.X, _location.Y + i)); //down
                adjacent.Add(new Vector2(_location.X - i, _location.Y)); //left
                adjacent.Add(new Vector2(_location.X + i, _location.Y)); //right
                
                var topRight = new Vector2(_location.X +i, _location.Y+i);
                var topLeft = new Vector2(_location.X -i, _location.Y +i );
                var botLeft = new Vector2(_location.X +i, _location.Y -i);
                var botRight = new Vector2(_location.X-i, _location.Y -i);
                
                adjacent.Add(topRight);
                adjacent.Add(topLeft);
                adjacent.Add(botLeft);
                adjacent.Add(botRight);
            }
            return adjacent;
        }
    }
}