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
        public Tree _target { get; set; }

        public HashSet<Tree> _possibleTargets { get; set; } = new HashSet<Tree>();

        public Vector2 _destination { get; set; }

        //these are our tweakable bias parameters.
        //Lessening alpha will lessen the bias of target quality.
        //Lessening beta will lessen the bias of cost to target
        // alpha, beta > 0; a,b in real numbers
        double alpha = 1.00000;
        double beta = 1.00000;
        NoiseUtil noise;
        public static int MAXAGENTSPERTARGET = 2;
        public static int EXTINGUISHABLEDISTANCE = 2;
        public Agent(Vector2 location) : base(-1, false, location)
        {
            noise = new NoiseUtil();

            _location = location;
            _temp = defaultTemp;
            _color = Color.Black;
            _destination = new Vector2(-1, -1);
        }

        public Agent()
        {

        }

        /*   public void move(SquareGrid squareGrid)
          {
              var grid = squareGrid._slots;
              var adjacent = getAdjacent(grid);

              var adjAgents = locateAgents(adjacent, grid);

              if (_target == null)
              {
                  _target = weightedDecision(adjacent, grid);
                  Vector2 newPos = _location;

                  Console.WriteLine($"I: {_location} want to go here: {_destination}");
                  if (_destination.X == -1 || _destination.Y == -1) newPos = randomDirection(adjacent, grid);
                  var from = _location;
                  _location = newPos;
                  _prevLocation = from;

                  grid[(int)_prevLocation.X][(int)_prevLocation.Y] = new Boardentity(1, true, _prevLocation);
                  grid[(int)_location.X][(int)_location.Y] = this;

              }

              else sendMessage(squareGrid._agentList);
          } */



        private List<Agent> locateAgents(List<Vector2> locs, GridLocation[][] grid)
        {
            var nearbyAgents = locs.Where(pos => grid[(int)pos.X][(int)pos.Y].GetType() == typeof(Agent))
                                   .Select(pos => (Agent)grid[(int)pos.X][(int)pos.Y]).ToList();

            return nearbyAgents;
        }

        private List<Tree> locateTrees(List<Vector2> adjacent, GridLocation[][] grid)
        {

            var trees = adjacent
                .Where(postion => grid[(int)postion.X][(int)postion.Y].GetType() == typeof(Tree))
                .Select(position => (Tree)grid[(int)position.X][(int)position.Y]).ToList();


            return trees;
        }

        // This is where the magic happens
        private Vector2 randomDirection(List<Vector2> adjacent, GridLocation[][] grid)
        {
            List<Vector2> availableMoves = checkAvailableMoves(adjacent, grid);

            if (!availableMoves.Any() || availableMoves.Count == 0) return _location;

            var randomize = new Random().Next(0, availableMoves.Count);

            var direction = availableMoves[randomize];

            return direction;
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
            return grid[(int)location.X][(int)location.Y]._traversable;
        }

        private bool isSquareOccupied(GridLocation[][] grid, Vector2 position)
        {
            var gLType = grid[(int)position.X][(int)position.Y].GetType();
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
            var noisedTemp = noise.withNoise(target._temp);
            return noisedTemp / 100;

        }

        //This is qi in formula(5), or the finished formula(5)
        private double allQualities(Tree target)
        {
            var current = getQuality(target);
            var sum = 0.0;
            foreach (var tree in _possibleTargets)
            {
                if (!tree.Equals(target)) sum += getQuality(tree); //this line might be wrong
               // sum += getQuality(tree); // i believe this to be correct.
            }

            return current / sum;
        }

        //this is formula (6), or computation of utility

        public double computeFinalProbability(Tree target)
        {

            double qi = Math.Pow(allQualities(target), alpha);
            double ni = Math.Pow(fromEuclidToReciprocral(getEuclidianDistance(target._location, this._location)), beta);
            double sum = 0.0;

            foreach (var tree in _possibleTargets)
            {

                sum += Math.Pow(allQualities(tree), alpha) * Math.Pow(fromEuclidToReciprocral(getEuclidianDistance(tree._location, _location)), beta);
            }

            var probability = (qi * ni) / sum;

            return probability;
        }

        private Tree weightedDecision()
        {

            var weightedRandomBag = new WeightedRandomBag<Tree>();

            foreach (var tree in _possibleTargets)
            {
                if (!isBurning(tree)) continue;

                var probability = computeFinalProbability(tree);
                weightedRandomBag.add(tree, probability);
            }
            return weightedRandomBag.getRandom();
        }

        private bool isBurning(Tree tree)
        {
            var noisedTemp = noise.withNoise(tree._temp);
            return noisedTemp >= 80;
        }

        //=====================================================================================================================================
        //=====================================================Messaging stuff=================================================================
        //=====================================================================================================================================
        static Random rand = new Random();
        public void receiveMessage(Agent sender, Agent receiver)
        {
            if (rand.Next(100) != 1) _possibleTargets.UnionWith(sender._possibleTargets);

        }
        public void sendMessage(List<Agent> receivers)
        {



            foreach (var receiver in receivers.OrderBy(a => rand.Next(receivers.Count())))
            {
                receiver.receiveMessage(this, receiver);
            }
        }


        // WEIRD STUFF LNMAO
        public void toRuleThemAll(SquareGrid squareGrid)
        {

            var grid = squareGrid._slots;
            var adjacentSquares = getAdjacent(grid);
            var adjacentTrees = locateTrees(adjacentSquares, grid);

            //adds all nearby trees to available targets
            foreach (var tree in adjacentTrees)
            {
                _possibleTargets.Add(tree);
            }
            if (_target != null)
            {
                Console.WriteLine(this._location + " " + _target._location);
                Extinguish();
            }
            else if (_target == null && _possibleTargets.Any())
            {
                var tree = weightedDecision();
                if (tree == null)
                {
                    move(grid, randomDirection(adjacentSquares, grid));
                    sendMessage(squareGrid._agentList);
                    return;
                }
                //Here we check if any agents already have this tree as a target, we allow 2 agents per tree
                var sameTargetCounter = 0;
                foreach (var agent in squareGrid._agentList)
                {
                    if (agent._target == null) continue;
                    else if (agent._target._location == tree._location && agent._location != _location) sameTargetCounter++;
                }
                if (getEuclidianDistance(tree._location, _location) <= EXTINGUISHABLEDISTANCE && sameTargetCounter < MAXAGENTSPERTARGET)
                {
                    _target = tree;
                }         //THIS IS MAGIC NUMBERING IN TERMS OF DISTANCE
                _destination = tree._location;
                move(grid, roamTowardsTree(adjacentSquares, grid));

            }
            //If agent isn't "working" on a tree, and has no nearby trees it roams randomly for 1 tick
            else if (_target == null && !_possibleTargets.Any())
            {
                move(grid, randomDirection(adjacentSquares, grid));
            }

            sendMessage(squareGrid._agentList);

        }

        public void Extinguish()
        {

            var targetTemp = _target.getTemp();
            var noisedTemp = noise.withNoise(targetTemp);
            if (noisedTemp < 75)
            {
                _target = null;
                return;
            }

            _target.extinguishTick();


        }
        private Vector2 roamTowardsTree(List<Vector2> adjacent, GridLocation[][] grid)
        {
            var weightedMoves = new WeightedRandomBag<Vector2>();
            var moves = new List<Vector2>();
            rand = new Random();
            if (_destination.X - _location.X < 0 && _destination.Y - _location.Y < 0) moves.Add(new Vector2(_location.X - 1, _location.Y - 1));
            if (_destination.X - _location.X < 0 && _destination.Y - _location.Y > 1) moves.Add(new Vector2(_location.X - 1, _location.Y + 1));
            if (_destination.X - _location.X > 1 && _destination.Y - _location.Y < 0) moves.Add(new Vector2(_location.X + 1, _location.Y - 1));
            if (_destination.X - _location.X > 1 && _destination.Y - _location.Y > 1) moves.Add(new Vector2(_location.X + 1, _location.Y + 1));
            if (_destination.X - _location.X < 0) moves.Add(new Vector2(_location.X - 1, _location.Y));
            if (_destination.X - _location.X > 1) moves.Add(new Vector2(_location.X + 1, _location.Y));
            if (_destination.Y - _location.Y < 0) moves.Add(new Vector2(_location.X, _location.Y - 1));
            if (_destination.Y - _location.Y > 1) moves.Add(new Vector2(_location.X, _location.Y + 1));

            moves = moves.Where(pos => !isPathObstructed(pos, grid)).ToList();

            if (!moves.Any()){
                var allMoves = checkAvailableMoves(adjacent, grid);
                if (allMoves.Count == 0) return _location;
                return allMoves[rand.Next(allMoves.Count)];
            }


            /* .Where(pos => !moves.Contains(pos)) */;

            return moves[rand.Next(moves.Count)];
        }

        private void move(GridLocation[][] grid, Vector2 newPos)
        {
            var from = _location;
            _location = newPos;
            _prevLocation = from;

            grid[(int)_prevLocation.X][(int)_prevLocation.Y] = new Boardentity(1, true, _prevLocation);
            grid[(int)_location.X][(int)_location.Y] = this;
        }

        private bool isPathObstructed(Vector2 pos, GridLocation[][] grid)
        {
            return isSquareOccupied(grid, pos) || !isTraversable(pos, grid);
        }
    }
}