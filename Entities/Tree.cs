using System.Globalization;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Swarms.Datatypes.Grids;
using System.Linq;

namespace Swarms.Entities
{
    public class Tree : Boardentity
    {

        public int _id {get;set;}
        public bool _isBurning {get; set;}

        public static int _spreadRange = 3;

        public Tree(Vector2 location, double temp = 30, int  id = 0) : base(-1, false, location){
  
            _location = location;
            _temp = temp;
            _id = id;
            _isBurning = _temp >= 80; //Maybe do the computation in agent instead of global variable?
            _color = GetColor();
        }

        public Tree()
        {
            
        }
        
        public Color GetColor()
        {
            return _temp >= 80 ? Color.Red : Color.LawnGreen;
        }

        public double getTemp(){
            return _temp;
        }

        public void tickTemp(GridLocation[][] grid, List<Tree> trees)
        {
            var adjacentSquares = getAdjacent(grid, _spreadRange);
            var adjacentTrees = getAdjacentTrees(grid, adjacentSquares, trees);
            
            foreach (var tree in adjacentTrees)
            {
                var euclidianDistance = getEuclidianDistance(tree._location, _location);

                var tempFactor = Math.Pow(0.5, Math.Floor(euclidianDistance)); // The further out the smaller the temperature increase
                 
                if(_temp >= 300) {
                    _color = GetColor();
                    return;
                }
                if(tree._temp >= 160) _temp += 4 * tempFactor;
                else if(tree._temp >= 80 && tree._temp < 160) _temp += 2 * tempFactor;

                
            }

            _color = GetColor();
        }
        private List<Tree> getAdjacentTrees(GridLocation[][] grid, List<Vector2> adjacentSquares, List<Tree> trees)
        {
            var adjacentTrees = adjacentSquares
                .Where(position => grid[(int)position.X][(int)position.Y].GetType() == typeof(Tree))
                .Select(position => (Tree) grid[(int)position.X][(int)position.Y])
                .ToList();

            return adjacentTrees;
        }

        public void extinguishTick(){
             if(_temp < 75){
                
                return;
            }
            _temp -= 9;
        }

    }
}