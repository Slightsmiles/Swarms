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
        
        public List<GridLocation[][]> _logsMin {get; set;}
        
        public List<GridLocation[][]> _logsMid {get; set;}

        public List<GridLocation[][]> _logsMax {get; set;}
        public GridLocation[][] locations;
        public int _min {get; set;}
        public int _mid {get; set;}
        public int _max {get; set;}


        public Logger(int min, int mid, int max)
        {
            _logsMin = new List<GridLocation[][]>();
            _logsMid = new List<GridLocation[][]>();
            _logsMax = new List<GridLocation[][]>();
            _min = min;
            _mid = mid;
            _max = max;
            
        }



        public void initStuff(){

        }
        public GridLocation[][] logLocations(int ticks, SquareGrid grid)
        {
            locations = grid._slots;
            
            if(ticks == _min) _logsMin.Add(locations);
            if(ticks == _mid) _logsMid.Add(locations);
            if(ticks == _max) _logsMax.Add(locations);                
            
            return locations;
        }

        public void serialize(){
            serializeMin();
            serializeMid();
            serializeMax();
        }
        Type[] types = {typeof(Boardentity), typeof(Agent), typeof(Obstacle),typeof(Tree)};
        public void serializeMin(){
            
            XmlSerializer serializer = new XmlSerializer(typeof(List<GridLocation[][]>), types);

            StreamWriter writer = new StreamWriter("testData"+ _min + ".xml");
            //need to serialize trees aswell and deserialize
        
            serializer.Serialize(writer, _logsMin);
            writer.Close();
        }

        public void serializeMid(){
            XmlSerializer serializer = new XmlSerializer(typeof(List<GridLocation[][]>), types);

            StreamWriter writer = new StreamWriter("testData"+ _mid + ".xml");
            serializer.Serialize(writer, _logsMid);
            writer.Close();
        }

        public void serializeMax(){
            XmlSerializer serializer = new XmlSerializer(typeof(List<GridLocation[][]>), types);

            StreamWriter writer = new StreamWriter("testData"+ _max + ".xml");
            serializer.Serialize(writer, _logsMax);
            writer.Close();
        }

    }
}