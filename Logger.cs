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
        
        public List<Vector2[][]> _logsMin {get; set;}
        
        public List<Vector2[][]> _logsMid {get; set;}

        public List<Vector2[][]> _logsMax {get; set;}
        public Vector2[][] locations;
        public int _min {get; set;}
        public int _mid {get; set;}
        public int _max {get; set;}


        public Logger(int min, int mid, int max)
        {
            _logsMin = new List<Vector2[][]>();
            _logsMid = new List<Vector2[][]>();
            _logsMax = new List<Vector2[][]>();
            _min = min;
            _mid = mid;
            _max = max;
            
        }

  
        public Vector2[][] logLocations(int ticks, SquareGrid grid)
        {
            locations = new Vector2[grid._columnNums][];
            
            for(int i = 0; i < grid._columnNums; i++){
                locations[i] = new Vector2[grid._rowNums];
                for(int j = 0; j < grid._rowNums ; j++)
                { 
                     
                    var entity = grid._slots[i][j];
               
                    if (entity.GetType() == typeof(Agent)){
                        Console.WriteLine("found a thiung"); 
                        locations[i][j] = entity._location;
                }
                }
                
            }
            
            if(ticks == _min) _logsMin.Add(locations);
            if(ticks == _mid) _logsMid.Add(locations);
            if(ticks == _max) _logsMax.Add(locations);                
            
            //serialize(_logs, ticks);
            return locations;
        }

        public void serialize(){
            serializeMin();
            serializeMid();
            serializeMax();
        }

        public void serializeMin(){
            XmlSerializer serializer = new XmlSerializer(typeof(List<Vector2[][]>));

            StreamWriter writer = new StreamWriter("testData"+ _min + ".xml");
            serializer.Serialize(writer, _logsMin);
            writer.Close();
        }

        public void serializeMid(){
            XmlSerializer serializer = new XmlSerializer(typeof(List<Vector2[][]>));

            StreamWriter writer = new StreamWriter("testData"+ _mid + ".xml");
            serializer.Serialize(writer, _logsMid);
            writer.Close();
        }

        public void serializeMax(){
            XmlSerializer serializer = new XmlSerializer(typeof(List<Vector2[][]>));

            StreamWriter writer = new StreamWriter("testData"+ _max + ".xml");
            serializer.Serialize(writer, _logsMax);
            writer.Close();
        }


      /*  public void serialize(List<Vector2[][]> logs, int ticks)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<Vector2[][]>));

            StreamWriter writer = new StreamWriter("test" + ticks + ".xml");
            // If file already exists, deserialize, add to collection and then serialize.
            // else just serialize
            // how to check if file already exists?????

            if (File.Exists("test"+ ticks + ".xml"))
            {
                writer.Close();
            
                
                deserialize(ticks);
                serializer.Serialize(writer, _logs);
            }
            else
            {
                
                serializer.Serialize(writer, logs);
            }
            
           
        }
*/
    

    }
}