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

        public char[] EntryType;
        public GameObject VisualGo;

        public bool Overlaps(EntryData other)
        {
            return OverlapX(other) && OverlapY(other);
        }

        private bool OverlapX(EntryData other)
        {
            return valueInRange(X, other.X, other.Right()) || valueInRange(other.X, X, Right());
        }

        private bool OverlapY(EntryData other)
        {
            return valueInRange(Y, other.Y, other.Top()) || valueInRange(other.Y, Y, Top());
        }

        bool valueInRange(float value, float min, float max)
        {
            return (value >= min) && (value <= max);
        }

        public float Top()
        {
            return Y + Height;
        }

        public float Right()
        {
            return X + Width;
        }


        public EntryData(string[] data, int index)
        {
            X = float.Parse(data[0]);
            Y = float.Parse(data[1]);
            Width = float.Parse(data[2]);
            Height = float.Parse(data[3]);
            if (!string.IsNullOrEmpty(data[4])) EntryType = data[4].ToCharArray();

            BuildVisual(index);
        }

        public void BuildVisual(int index)
        {
            var go = Object.Instantiate(MainSetup.Instance.TheVisualPrefab);
            go.transform.position = new Vector3(X + Width / 2, Y + Height / 2, 0);
            go.transform.localScale = new Vector3(Width, Height, 1);
            go.name = "Data : " + index;
            VisualGo = go;
            if (EntryType != null)
                for (int i = 0; i < EntryType.Length; i++)
                {
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
        }

        private void AddCom<T>(GameObject go) where T : BaseComponent
        {
            var hasComponent = go.GetComponent<T>();
            if (hasComponent == null)
            {
                go.AddComponent<T>();
            }
        }
    }
}