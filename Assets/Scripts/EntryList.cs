using System.Collections.Generic;
using UnityEngine;

namespace AbrahamDev
{
    public class EntryList
    {
        public Transform TheParent;
        public List<EntryData> Datas = new List<EntryData>();

        public int X;
        public int Y;

        public bool HasOverlapOnThisZone(EntryData data)
        {
            for (var i = 0; i < Datas.Count; i++)
                if (Datas[i].Overlaps(data))
                    return true;

            return false;
        }


        public void CheckForNeibors(Dictionary<Vector2, EntryList> grid)
        {
            var right = new Vector2(X + 1, Y);
            var up = new Vector2(X, Y + 1);
            var upRight = new Vector2(X + 1, Y + 1);

            if (grid.ContainsKey(right)) Right = grid[right];

            if (grid.ContainsKey(up)) Up = grid[up];

            if (grid.ContainsKey(upRight)) UpRight = grid[upRight];
        }

        public EntryList Right, Up, UpRight;


        public int AllMyCollitions()
        {
            var total = 0;
            total += TotalOverlapsForNode(this);
            //if (Right != null)
            //{
            //    total += Right.TotalOverlapsForNode(this);
            //}

            //if (Up != null)
            //{
            //    total += Up.TotalOverlapsForNode(this);
            //}

            //if (UpRight != null)
            //{
            //    total += UpRight.TotalOverlapsForNode(this);
            //}

            return total;
        }

        public int TotalOverlapsForNode(EntryList data)
        {
            var total = 0;

            for (var i = 0; i < Datas.Count; i++)
            for (var j = 0; j < data.Datas.Count; j++)
                if (Datas[i].Overlaps(data.Datas[j]))
                    total++;

            return total;
        }
    }
}