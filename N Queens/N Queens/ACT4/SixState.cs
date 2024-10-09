using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ACT4
{
    class SixState
    {
        public int[] Y = new int[6];

        public SixState() { }
        public SixState(int a)
        {
            Y[0] = Y[1] = Y[2] = Y[3] = Y[4] = Y[5] = a;
        }
        public SixState(int a, int b, int c, int d, int e, int f)
        {
            Y[0] = a;
            Y[1] = b;
            Y[2] = c;
            Y[3] = d;
            Y[4] = e;
            Y[5] = f;
        }
        public SixState(SixState f)
        {
            Y[0] = f.Y[0];
            Y[1] = f.Y[1];
            Y[2] = f.Y[2];
            Y[3] = f.Y[3];
            Y[4] = f.Y[4];
            Y[5] = f.Y[5];
        }
    }

    class PriorityQueue
    {
        private List<Tuple<int, SixState>> elements = new List<Tuple<int, SixState>>();

        public void Enqueue(SixState state, int priority)
        {
            elements.Add(Tuple.Create(priority, state));
            elements = elements.OrderBy(x => x.Item1).ToList();
        }

        public SixState Dequeue()
        {
            var bestItem = elements[0];
            elements.RemoveAt(0);
            return bestItem.Item2;
        }

        public bool IsEmpty()
        {
            return elements.Count == 0;
        }
    }
}
