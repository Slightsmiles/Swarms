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
        public Agent(Vector2 location) : base(-1, false, location){
            _location = location;
            _temp = defaultTemp;
            _color = Color.Black;
        }
        public Agent(){}
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

            if(burningTree == null) {
                var newPos = randomDirection(adjacent, grid);
                var from = _location;
                _location = newPos;
                _prevLocation = from;
            }

            var adjAgents = locateAgents(adjacent, grid);
            broadcastMessage(adjAgents, grid);

        }

        private List<Agent> locateAgents(List<Vector2> locs, GridLocation[][] grid)
        {
            var nearbyAgents = new List<Agent>();
            foreach (var loc in locs)
            {
                if (grid[(int)loc.X][(int)loc.Y].GetType() == typeof(Agent))
                {
                    nearbyAgents.Add((Agent)grid[(int)loc.X][(int)loc.Y]);
                }
            }

            return nearbyAgents;
        }

        // This is where the magic happens
        private Vector2 randomDirection(List<Vector2> adjacent, GridLocation[][] grid) {
            List<Vector2> availableMoves = checkAvailableMoves(adjacent, grid);
            
            var randomize = new Random().Next(0, availableMoves.Count);

                                                 

            var direction = availableMoves[randomize];
            
            if(availableMoves.Count == 0) return this._location;
            else return direction;
        }

        private List<Vector2> checkAvailableMoves(List<Vector2> adjacent, GridLocation[][] grid)
        {
            var available = adjacent.Where(position =>
                                                isTraversable(position, grid)
                                                && !(position.X - _location.X < -1 
                                                    || position.Y - _location.Y < -1
                                                    || position.X - _location.X > 1
                                                    || position.Y - _location.Y > 1)).ToList();

            return available;
        }

        private bool isTraversable(Vector2 location, GridLocation[][] grid) {
            return grid[(int) location.X][(int) location.Y]._traversable;
        }

        private Tree locateTree(List<Vector2> adjacent, GridLocation[][] grid) {
            var trees = adjacent
                .Where(postion => grid[(int)postion.X][(int)postion.Y].GetType() == typeof(Tree))
                .Select(position => (Tree) grid[(int)position.X][(int)position.Y]).ToList();

            Tree muchBurningSuchTree = null;

            foreach(var tree in trees) {
                if(muchBurningSuchTree == null) muchBurningSuchTree = tree;
                if(tree._temp > muchBurningSuchTree._temp && muchBurningSuchTree._isBurning) muchBurningSuchTree = tree;
            }
            return muchBurningSuchTree;
        }
    }
}