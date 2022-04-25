using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace AbrahamDev
{
    public class MainSetup : MonoBehaviour
    {
        public static MainSetup Instance;
        public List<EntryData> Data = new List<EntryData>();
        public GameObject TheVisualPrefab;

        protected void Awake()
        {
            Instance = this;
            Setup();
        }

        public Dictionary<Vector2, EntryList> Grid = new Dictionary<Vector2, EntryList>();

        private int _totalCom;

        protected void Setup()
        {
            //Read text file 
            var thePath = Root() + "entityblueprints.txt";

            var reader = new StreamReader(thePath);

            _totalCom = 0;
            //Loop through all the lines
            while (!reader.EndOfStream)
            {
                //Check the current line
                var curLine = reader.ReadLine();
                //Slip the line to get values
                var parts = curLine.Split(' ');
                if (parts.Length <= 1)
                {
                    Debug.Log("No part to split : " + curLine);
                }
                //Check for x,y,width,height,(components if has any)
                else if (parts.Length <= 5)
                {
                    var data = new EntryData(parts, _totalCom);
                    AddToList(data);
                }
            }

            reader.Close();

            Debug.Log("Total with components : " + _totalCom);
            //Test if the overlapping works
            var overlap = Data[0].Overlaps(Data[2797]);
            Debug.Log(overlap);
            Debug.Log(Data[0].Overlaps(Data[817]));
            Debug.Log(Data[0].Overlaps(Data[1529]));

            //Create testing coordinate 
            var curGrid = new Vector2();
            //TODO stuck at detecting collitions
            for (var i = _minX; i <= _maxX; i++)
            for (var j = _minY; j <= _maxY; j++)
            {
                curGrid.x = i;
                curGrid.y = j;
                var cur = Grid[curGrid];
                Debug.Log(curGrid + "  :  " + cur.Datas.Count);

                //Test only the first in the loop
                if (i == -9 && j == -9)
                {
                    var total = cur.AllMyCollitions();
                    Debug.Log("Total " + total);
                    return;
                }
            }
        }

        private int _gridSize = 100;

        public int _minX, _minY, _maxX, _maxY;

        //Check for the min max values in the data
        private void CheckMinMax(int x, int y)
        {
            if (x < _minX) _minX = x;

            if (x > _maxX) _maxX = x;

            //Check min max Y
            if (y < _minY) _minY = y;

            if (y > _maxY) _maxY = y;
        }

        //Add entry to the list and the grids
        private void AddToList(EntryData data)
        {
            var x = (int) (data.X / _gridSize);
            var y = (int) (data.Y / _gridSize);

            CheckMinMax(x, y);

            var position = new Vector2(x, y);
            //Check for the parent and add the data to it
            if (Grid.ContainsKey(position))
            {
                var gridArea = Grid[position];

                data.VisualGo.transform.SetParent(gridArea.TheParent, true);
                gridArea.Datas.Add(data);
            }
            //Create a new parent for the current data
            else
            {
                var go = new GameObject(position.ToString());
                go.transform.position = position * _gridSize;
                data.VisualGo.transform.SetParent(go.transform, true);

                var dataList = new EntryList
                {
                    TheParent = go.transform,
                    X = x,
                    Y = y
                };
                dataList.Datas.Add(data);

                Grid.Add(position, dataList);
            }

            //Add it to the global data
            Data.Add(data);
            _totalCom++;
        }

        //Check for the file root path
        private string Root()
        {
            var path = Application.streamingAssetsPath;

            if (!Directory.Exists(path)) Directory.CreateDirectory(path);

            return path + "/";
        }
    }
}