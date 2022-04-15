using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Swarms.Datatypes.Grids;
using Swarms.Utility;

namespace Swarms.Entities
{
    public class Agent : Boardentity
    {
        public Vector2 _prevLocation { get; set; }
        public Tree _target {get; private set;}
        public List<Tree> availableTargets { get; set; }

        //these are our tweakable bias parameters.
        //Lessening alpha will lessen the bias of target quality.
        //Lessening beta will lessen the bias of cost to target
        // alpha, beta > 0; a,b in real numbers
        double alpha = 1.00000;
        double beta = 1.00000;
        
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
<<<<<<< HEAD
           // Console.WriteLine(message);
=======
            //Console.WriteLine(message);
>>>>>>> 64b396503d7735d1143409742064f8b68edfeac3
        }

        public void move(GridLocation[][] grid)
        {
            var adjacent = getAdjacent(grid);

            Tree burningTree = weightedDecision(adjacent, grid);

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
            var nearbyAgents = locs.Where(pos => grid[(int)pos.X][(int)pos.Y].GetType() == typeof(Agent))
                                   .Select(pos => (Agent)grid[(int)pos.X][(int)pos.Y]).ToList();

            return nearbyAgents;
        }

        // This is where the magic happens
        private Vector2 randomDirection(List<Vector2> adjacent, GridLocation[][] grid)
        {
            List<Vector2> availableMoves = checkAvailableMoves(adjacent, grid);
            
            if(availableMoves.FirstOrDefault() == null) return _location;

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

            return available;
        }

        private bool isTraversable(Vector2 location, GridLocation[][] grid)
        {
            return grid[(int) location.X][(int) location.Y]._traversable;
        }

        private bool isSquareOccupied(GridLocation[][] grid, Vector2 position)
        {
            var gLType = grid[(int) position.X][(int) position.Y].GetType();
            return gLType == typeof(Agent)
                   || gLType == typeof(Tree)
                   || gLType == typeof(Obstacle);
        }      
        
        //this is formula(4)
        private double fromEuclidToReciprocral(Double dist)
        {
            return 1 / dist;
        }
        
        
        //This is Qi in formula(5)
        private double getQuality(Tree target)
        {
            //Here we want to decide how an agent checks the quality of a target. That is we need to figure out how we simulate the sensoral input
            
            return 1.0;
        }
        
        //This is qi in formula(5), or the finished formula(5)
        private double allQualities(Tree target)
        {
            var current = getQuality(target);
            var sum = 0.0;
            foreach (var tree in availableTargets)
            {
               // if (!tree.Equals(target)) sum += getQuality(tree); //this line might be wrong
                sum += getQuality(tree); // i believe this to be correct.
            }

            return current / sum;
        }
        
        //this is formula (6), or computation of utility

        public double computeFinalProbability(Tree target)
        {
            
            double qi = Math.Pow(allQualities(target), alpha);
            double ni = Math.Pow(fromEuclidToReciprocral(getEuclidianDistance(target._location, this._location)), beta);
            double sum = 0.0;
            
            foreach (var tree in availableTargets)
            {
                
                sum += Math.Pow(allQualities(tree),alpha) * Math.Pow(fromEuclidToReciprocral(getEuclidianDistance(tree._location, _location)), beta);
            }

            var probability = (qi * ni) / sum;
          
            return probability;
        }

        private Tree weightedDecision(List<Vector2> adjacent, GridLocation[][] grid) {

            var trees = adjacent
                .Where(postion => grid[(int) postion.X][(int) postion.Y].GetType() == typeof(Tree))
                .Select(position => (Tree) grid[(int) position.X][(int) position.Y]).ToList();

            availableTargets = trees;

            var weightedRandomBag = new WeightedRandomBag<Tree>();

            foreach(var tree in availableTargets) {
                if(!isBurning(tree)) continue;   
                
                var probability = computeFinalProbability(tree);
                weightedRandomBag.add(tree, probability);
            }
            return weightedRandomBag.getRandom();
        }

        private bool isBurning(Tree tree) {
            return tree._temp >= 80;
        }
    }
}