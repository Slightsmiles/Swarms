using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Xml.Serialization;
using Swarms.Datatypes.Grids;
using Swarms.Entities;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace Swarms
{
    public class Logger
    {
        
        public List<Tuple<Vector2[][], int>> _logs = new List<Tuple<Vector2[][], int>>();
        
        public Logger()
        {
            
        }


        public Vector2[][] logLocations(int ticks, SquareGrid grid)
        {
            Vector2[][] locations = new Vector2[grid._columnNums][];
            for(int i = 0; i < grid._columnNums; i++){
                
                for(int j = 0; j < grid._rowNums ; j++)
                {
                    var entity = grid._slots[i][j];
                    if (grid._slots[i][j].GetType() == typeof(Agent)) locations[i][j] = entity._location;
                }
            }
            
            _logs.Add(new Tuple<Vector2[][], int>(locations,ticks));
            serialize(_logs, ticks);
            return locations;
        }


        public void serialize(List<Tuple<Vector2[][], int>> logs, int ticks)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<Tuple<Vector2[][], int>>));

            StreamWriter writer = new StreamWriter("simData" + ticks + ".xml");
            // If file already exists, deserialize, add to collection and then serialize.
            // else just serialize
            // how to check if file already exists?????

            if (true)
            {
                deserialize(ticks);
                serializer.Serialize(writer, _logs);
            }
            else
            {
                serializer.Serialize(writer, logs);
            }
            
            writer.Close();
        }

        public void deserialize(int ticks)
        {
            var mySerializer = new XmlSerializer(typeof(List<Tuple<Vector2[][], int>>));
            using var myFileStream = new FileStream("simData" + ticks +".xml", FileMode.Open);
            
            var serializedLog = (List<Tuple<Vector2[][], int>>) mySerializer.Deserialize(myFileStream);
          
            
            _logs.AddRange(serializedLog);
            
            _logs = serializedLog;
        }

    }
}