using System;
using System.Collections.Generic;
using Swarms.Entities;

namespace Swarms.Utility
{
    public class WeightedRandomBag<T> where T : class 
    {
        private class Entry
        {
            public double _accumulatedWeight {get; set;}
            public T _object {get; set;}
            public Entry(double accumulatedWeight, T obj) {
                _accumulatedWeight = accumulatedWeight;
                _object = obj;
            }
        }

        private List<Entry> _entries = new List<Entry>();
        private double _accumulatedWeight = 0.0;
        private Random random = new Random();

        public void add(T t, double weight) {
            _accumulatedWeight += weight;
            var entry = new Entry(_accumulatedWeight, t);

            _entries.Add(entry);
        }
        public T getRandom() {
            double r = random.NextDouble() * _accumulatedWeight;

            foreach (var entry in _entries)
            {
                if(entry._accumulatedWeight >= r) return entry._object;
            }
            return default(T);  
        }
    }
}