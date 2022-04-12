using System;
using System.Collections.Generic;

namespace Swarms.Utility
{
    public class WeightedRandomBag<T> where T : class 
    {
        private class Entry
        {
            public double _accumulatedWeight {get; set;}
            public T _object {get; set;}
        }

        private List<Entry> _entries = new List<Entry>();
        private double _accumulatedWeight = 0.0;
        private Random random = new Random();

        public void add(T t, double weight) {
            _accumulatedWeight += weight;
            var entry = new Entry();
            entry._accumulatedWeight = _accumulatedWeight;
            entry._object = t;
            _entries.Add(entry);
        }
        public T getRandom() {
            double r = random.NextDouble() * _accumulatedWeight;

            foreach (var entry in _entries)
            {
                if(_accumulatedWeight >= r) return entry._object;
            }
            return null;  
        }
    }
}