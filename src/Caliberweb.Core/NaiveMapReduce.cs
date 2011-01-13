using System.Collections.Generic;
using System.Linq;

namespace Caliberweb.Core
{
    public class NaiveMapReduce<K1, V1, K2, V2, V3>
    {
        public delegate IEnumerable<KeyValuePair<K2, V2>> MapFunction(K1 key, V1 value);

        public delegate IEnumerable<V3> ReduceFunction(K2 key, IEnumerable<V2> values);

        private readonly MapFunction map;
        private readonly ReduceFunction reduce;

        public NaiveMapReduce(MapFunction map, ReduceFunction reduce)
        {
            this.map = map;
            this.reduce = reduce;
        }

        private IEnumerable<KeyValuePair<K2, V2>> Map(IEnumerable<KeyValuePair<K1, V1>> input)
        {
            return input.SelectMany(pair => map(pair.Key, pair.Value));
        }

        private IEnumerable<KeyValuePair<K2, V3>> Reduce(IEnumerable<KeyValuePair<K2, V2>> intermediateValues)
        {
            // First, group intermediate values by key
            var groups = intermediateValues.GroupBy(pair => pair.Key, pair => pair.Value).Select(g => g);

            // Reduce on each group

            return groups.Select(g => new
            {
                g,
                k2 = g.Key
            }).SelectMany(t => reduce(t.k2, t.g), (t, reducedValue) => new KeyValuePair<K2, V3>(t.k2, reducedValue));
        }

        public IEnumerable<KeyValuePair<K2, V3>> Execute(IEnumerable<KeyValuePair<K1, V1>> input)
        {
            return Reduce(Map(input));
        }
    }
}