using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Swarms.Datatypes.Grids;

namespace Swarms.Entities
{
    public class Agent : Boardentity
    {
        public Vector2 _prevLocation { get; set; }
        private List<Tree> availableTargets { get; set; }

        public Agent(Vector2 location) : base(-1, false, location)
        {
            _location = location;
            _temp = defaultTemp;
            _color = Color.Black;
            availableTargets = new List<Tree>();
        }

        public Agent()
        {
            
        }
        // -------Mulig Optimering-------
        // Måske en IEnumerable<Vector2> eller andet her for memory reasons

        // private List<Vector2> getAdjacent() {
        //     var adjacent = new List<Vector2>();

        //     adjacent.Add(new Vector2(_location.X, _location.Y - 1)); //up by a factor of i
        //     adjacent.Add(new Vector2(_location.X, _location.Y + 1)); //down by a factor of i
        //     adjacent.Add(new Vector2(_location.X - 1, _location.Y)); //left by a factor of i
        //     adjacent.Add(new Vector2(_location.X + 1, _location.Y)); //right by a factor of 

        //     return adjacent;
        // }


        private void broadcastMessage(List<Agent> agents, GridLocation[][] grid)
        {
            var message = "hey dude";
            foreach (var agent in agents)
            {
                agent.receiveMessage(message);
            }
        }

        private void receiveMessage(String message)
        {
            Console.WriteLine(message);
        }

        public void move(GridLocation[][] grid)
        {
            var adjacent = getAdjacent(grid);

            Tree burningTree = locateTree(adjacent, grid);

            if (burningTree == null)
            {
                var newPos = randomDirection(adjacent, grid);
                var from = _location;
                _location = newPos;
                _prevLocation = from;

                grid[(int) _prevLocation.X][(int) _prevLocation.Y] = new Boardentity(1, true, _prevLocation);
                grid[(int) _location.X][(int) _location.Y] = this;
            }

            var adjAgents = locateAgents(adjacent, grid);
            broadcastMessage(adjAgents, grid);
        }

        private List<Agent> locateAgents(List<Vector2> locs, GridLocation[][] grid)
        {
            var nearbyAgents = new List<Agent>();
            foreach (var loc in locs)
            {
                if (grid[(int) loc.X][(int) loc.Y].GetType() == typeof(Agent))
                {
                    nearbyAgents.Add((Agent) grid[(int) loc.X][(int) loc.Y]);
                }
            }

            return nearbyAgents;
        }

        // This is where the magic happens
        private Vector2 randomDirection(List<Vector2> adjacent, GridLocation[][] grid)
        {
            List<Vector2> availableMoves = checkAvailableMoves(adjacent, grid);

            var randomize = new Random().Next(0, availableMoves.Count);

            var direction = availableMoves[randomize];

            if (availableMoves.Count == 0) return this._location;
            else return direction;
        }

        private List<Vector2> checkAvailableMoves(List<Vector2> adjacent, GridLocation[][] grid)
        {
            var available = adjacent.Where(position =>
                isTraversable(position, grid)
                && !(position.X - _location.X < -1
                     || position.Y - _location.Y < -1
                     || position.X - _location.X > 1
                     || position.Y - _location.Y > 1)
                && !isSquareOccupied(grid, position)).ToList();
            foreach (var loc in available) Console.WriteLine($"x: {loc.X}, y: {loc.Y}");

            return available;
        }

        private bool isTraversable(Vector2 location, GridLocation[][] grid)
        {
            return grid[(int) location.X][(int) location.Y]._traversable;
        }

        private Tree locateTree(List<Vector2> adjacent, GridLocation[][] grid)
        {
            var trees = adjacent
                .Where(postion => grid[(int) postion.X][(int) postion.Y].GetType() == typeof(Tree))
                .Select(position => (Tree) grid[(int) position.X][(int) position.Y]).ToList();

            Tree muchBurningSuchTree = null;

            foreach (var tree in trees)
            {
                availableTargets.Add(tree);
                if (muchBurningSuchTree == null) muchBurningSuchTree = tree;
                if (tree._temp > muchBurningSuchTree._temp && muchBurningSuchTree._isBurning)
                    muchBurningSuchTree = tree;
            }

            return muchBurningSuchTree;
        }

        public bool isSquareOccupied(GridLocation[][] grid, Vector2 position)
        {
            var gLType = grid[(int) position.X][(int) position.Y].GetType();
            return gLType == typeof(Agent)
                   || gLType == typeof(Tree)
                   || gLType == typeof(Obstacle);
        }
        
        
      
            
            
        //this is formula(3)    
        public double getEuclidianDistance(Vector2 target, Vector2 agent)
        {
            var dist = Math.Sqrt(Math.Pow(target.X - agent.X, 2) + Math.Pow(target.Y - agent.Y, 2));
             
            return dist;
        }
        //this is formula(4)
        public double fromEuclidToReciprocral(Double dist)
        {
            return 1 / dist;
        }
        
        
        //This is Qi in formula(5)
        private double getQuality(Tree target)
        {
            //Here we want to decide how an agent checks the quality of a target. That is we need to figure out how we simulate the sensoral input
            
            return 0.0;
        }
        
        //This is qi in formula(5), or the finished formula(5)
        private double allQualities(Tree target)
        {
            var current = getQuality(target);
            var sum = 0.0;
            foreach (var tree in availableTargets)
            {
                if (!tree.Equals(target)) sum += getQuality(tree); //this line might be wrong
            }

            return current / sum;
        }
        
        //this is formula (6), or act of computing ultility
        

         
        
    }
}