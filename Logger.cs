using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using Swarms.Datatypes.Grids;
using Swarms.Entities;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace Swarms
{
    public class Logger
    {

        public List<int[]> _logsMin { get; set; }

        public List<int[]> _logsMid { get; set; }

        public List<int[]> _logsMax { get; set; }
    
        public GridLocation[][] locations;
        public int _min { get; set; }
        public int _mid { get; set; }
        public int _max { get; set; }

        public Logger(int min, int mid, int max, SquareGrid grid)
        {
            _logsMin = new List<int[]>(grid._columnNums);
            _logsMid = new List<int[]>(grid._columnNums);
            _logsMax = new List<int[]>(grid._columnNums);
            _min = min;
            _mid = mid;
            _max = max;
            initCounterArrays(_logsMin, grid);
            initCounterArrays(_logsMid, grid);
            initCounterArrays(_logsMax, grid);

        }
       /*  public Logger(int min, int mid, int max, SquareGrid grid)
        {
            _logsMin = new int[grid._columnNums][];

            _logsMid = new int[grid._columnNums][];
            _logsMax = new int[grid._columnNums][];
            _min = min;
            _mid = mid;
            _max = max;

        } */



        public void initCounterArrays(List<int[]> counterArray, SquareGrid grid)
        {
            for (int i = 0; i < counterArray.Capacity; i++)
            {
               counterArray.Insert(i, new int[grid._rowNums]); 
            }
        }
        public void logLocations(int ticks, SquareGrid grid)
        {

            if (ticks == _min) increment(grid, _logsMin);
            if (ticks == _mid) increment(grid, _logsMid);
            if (ticks == _max) increment(grid, _logsMax);

            return;
        }

        private void increment(SquareGrid grid, List<int[]> counters)
        {
            foreach (var agent in grid._agentList)
            {
                counters[(int)agent._location.X][(int)agent._location.Y] += 1;
            }
        }

        public void serialize()
        {
            serializeMin();
            serializeMid();
            serializeMax();
        }
        Type[] types = { typeof(Boardentity), typeof(Agent), typeof(Obstacle), typeof(Tree) };
        public void serializeMin()
        {

            XmlSerializer serializer = new XmlSerializer(typeof(List<int[]>), types);

            StreamWriter writer = new StreamWriter("testData" + _min + ".xml");
            //need to serialize trees aswell and deserialize

            serializer.Serialize(writer, _logsMin);
            writer.Close();
        }

        public void serializeMid()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<int[]>), types);

            StreamWriter writer = new StreamWriter("testData" + _mid + ".xml");
            serializer.Serialize(writer, _logsMid);
            writer.Close();
        }

        public void serializeMax()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<int[]>), types);

            StreamWriter writer = new StreamWriter("testData" + _max + ".xml");
            serializer.Serialize(writer, _logsMax);
            writer.Close();
        }

    }
}