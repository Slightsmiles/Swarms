using System.Runtime.CompilerServices;
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

        public bool _isBurning {get; set;}

        public Tree(Vector2 location, int temp = 30) : base(-1, false, location){
            _location = location;
            _temp = temp;
            _isBurning = temp >= 80; //Maybe do the computation in agent instead of global variable?
            _color = GetColor();
            
            Console.WriteLine($"tree temp: {_temp}");
            Console.WriteLine($"tree isBurning: {_isBurning}");
        }

        public Tree()
        {
            
        }
        
        public Color GetColor()
        {
            return _temp >= 80 ? Color.Red : Color.LawnGreen;
        }

        public void tickTemp(GridLocation[][] grid, List<Tree> trees)
        {
            var adjacentSquares = getAdjacent(grid);
            var adjacentTrees = getAdjacentTrees(grid, adjacentSquares, trees);
            
            foreach (var tree in adjacentTrees)
            {
                if(tree._temp >= 160) _temp += 4;
                else if(tree._temp >= 80 && tree._temp < 160) _temp += 2;
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
    }
}