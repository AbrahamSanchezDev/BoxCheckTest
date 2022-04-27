using UnityEngine;

namespace AbrahamDev
{
    [System.Serializable]
    public class EntryData
    {
        public float X;
        public float Y;
        public float Width;
        public float Height;

        public int UniqueIndex;

        public char[] EntryType;

        public GameObject VisualGo;

        //Check other entry overlaps with this one
        public bool Overlaps(EntryData other)
        {
            return OverlapX(other) && OverlapY(other);
        }

        //Check for overlap on the X
        private bool OverlapX(EntryData other)
        {
            return ValueInRange(X, other.X, other.Right()) || ValueInRange(other.X, X, Right());
        }

        //Check for overlap on the Y
        private bool OverlapY(EntryData other)
        {
            return ValueInRange(Y, other.Y, other.Top()) || ValueInRange(other.Y, Y, Top());
        }

        //Check if the values overlaps
        private bool ValueInRange(float value, float min, float max)
        {
            return value >= min && value <= max;
        }

        public float Top()
        {
            return Y + Height;
        }

        public float Right()
        {
            return X + Width;
        }

        //Constructor for the entry data
        public EntryData(string[] data, int index)
        {
            X = float.Parse(data[0]);
            Y = float.Parse(data[1]);
            Width = float.Parse(data[2]);
            Height = float.Parse(data[3]);
            UniqueIndex = index;
            if (!string.IsNullOrEmpty(data[4])) EntryType = data[4].ToCharArray();

            BuildVisual(index);
        }

        //Create the visual object and add it's components
        public void BuildVisual(int index)
        {
            var go = Object.Instantiate(MainSetup.Instance.TheVisualPrefab);
            go.transform.position = new Vector3(X + Width / 2, Y + Height / 2, 0);
            go.transform.localScale = new Vector3(Width, Height, 1);
            go.name = "Data : " + index;
            VisualGo = go;
            if (EntryType != null)
                for (var i = 0; i < EntryType.Length; i++)
                    switch (EntryType[i])
                    {
                        case 'H':
                            // This Entity has a HealthComponent.
                            AddCom<HealthComponent>(go);
                            break;
                        case 'A':
                            // This Entity has an AttackComponent.
                            AddCom<AttackComponent>(go);
                            break;
                        case 'M':
                            // This Entity has a MovementComponent.
                            AddCom<MovementComponent>(go);
                            break;
                        default:
                            Debug.Log("Unknown Component type: ");
                            break;
                    }
        }

        //Add a component if it doesn't have it
        private void AddCom<T>(GameObject go) where T : BaseComponent
        {
            var hasComponent = go.GetComponent<T>();
            if (hasComponent == null) go.AddComponent<T>();
        }
    }
}