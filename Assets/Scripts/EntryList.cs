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

        //Check for entry list neighbors
        public List<EntryList> CheckForNeighbors(Dictionary<Vector2, EntryList> grid)
        {
            var left = new Vector2(X - 1, Y);
            var right = new Vector2(X + 1, Y);

            var down = new Vector2(X, Y - 1);
            var up = new Vector2(X, Y + 1);

            var upRight = new Vector2(X + 1, Y + 1);
            var upLeft = new Vector2(X - 1, Y + 1);

            var downRight = new Vector2(X + 1, Y - 1);
            var downLeft = new Vector2(X - 1, Y - 1);

            var listOfAreas = new List<EntryList>();
            //TODO maybe map the position and only check the edges?
            var theList = new Dictionary<int, List<EntryData>>();

            listOfAreas.Add(this);
            //all sides
            if (grid.ContainsKey(left)) listOfAreas.Add(grid[left]);
            if (grid.ContainsKey(right)) listOfAreas.Add(grid[right]);
            if (grid.ContainsKey(up)) listOfAreas.Add(grid[up]);
            if (grid.ContainsKey(down)) listOfAreas.Add(grid[down]);
            //Upper edges
            if (grid.ContainsKey(upRight)) listOfAreas.Add(grid[upRight]);
            if (grid.ContainsKey(upLeft)) listOfAreas.Add(grid[upLeft]);
            //Lower edges
            if (grid.ContainsKey(downRight)) listOfAreas.Add(grid[downRight]);
            if (grid.ContainsKey(downLeft)) listOfAreas.Add(grid[downLeft]);

            return listOfAreas;
        }

        //Check all the overlaps with the given entry list
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