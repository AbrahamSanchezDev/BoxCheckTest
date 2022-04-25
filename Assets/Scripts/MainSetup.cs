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
            var thePath = Root() + "entityblueprints.txt";

            var reader = new StreamReader(thePath);

            _totalCom = 0;
            while (!reader.EndOfStream)
            {
                var curLine = reader.ReadLine();
                //Debug.Log(curLine);
                var parts = curLine.Split(' ');
                if (parts.Length <= 1)
                {
                    Log("No part to split : " + curLine);
                }
                else if (parts.Length == 4 || parts[4] == "")
                {
                    //Log("Obj does not have component to it : " + curLine);
                    var data = new EntryData(parts, _totalCom);
                    AddToList(data);
                }
                else if (parts.Length >= 5)
                {
                    var data = new EntryData(parts, _totalCom);
                    AddToList(data);
                }
            }

            Debug.Log("Total with components : " + _totalCom);

            reader.Close();

            var overlap = Data[0].Overlaps(Data[2797]);
            Debug.Log(overlap);
            Debug.Log(Data[0].Overlaps(Data[817]));
            Debug.Log(Data[0].Overlaps(Data[1529]));

            var curGrid = new Vector2();
            //TODO stuck at detecting collitions
            for (int i = _minX; i <= _maxX; i++)
            {
                for (int j = _minY; j <= _maxY; j++)
                {
                    curGrid.x = i;
                    curGrid.y = j;
                    var cur = Grid[curGrid];
                    Debug.Log(curGrid + "  :  " + cur.Datas.Count);

                    if (i == -9 && j == -9)
                    {
                        var total = cur.AllMyCollitions();
                        Debug.Log("Total " + total);
                        return;
                    }

                    //Debug.Log(curGrid);
                }
            }
        }

        private int _gridSize = 100;

        public int _minX, _minY, _maxX, _maxY;

        private void CheckMinMax(int x, int y)
        {
            if (x < _minX)
            {
                _minX = x;
            }

            if (x > _maxX)
            {
                _maxX = x;
            }

            //Check min max Y
            if (y < _minY)
            {
                _minY = y;
            }

            if (y > _maxY)
            {
                _maxY = y;
            }
        }

        private void AddToList(EntryData data)
        {
            var x = (int) (data.X / _gridSize);
            var y = (int) (data.Y / _gridSize);

            CheckMinMax(x, y);

            var position = new Vector2(x, y);

            if (Grid.ContainsKey(position))
            {
                var gridArea = Grid[position];

                data.VisualGo.transform.SetParent(gridArea.TheParent, true);
                gridArea.Datas.Add(data);
            }
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

            Data.Add(data);
            _totalCom++;
        }

        private void Log(string theText)
        {
            Debug.Log(theText);
        }

        private string Root()
        {
            var path = Application.streamingAssetsPath;

            if (!Directory.Exists(path)) Directory.CreateDirectory(path);

            return path + "/";
        }
    }
}