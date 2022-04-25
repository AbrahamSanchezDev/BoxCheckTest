using UnityEngine;

namespace AbrahamDev
{
    public abstract class BaseComponent : MonoBehaviour
    {
        protected void Awake()
        {
            Setup();
        }

        protected abstract void Setup();

        protected void SetColor(Color32 theColor)
        {
            var theMat = GetComponentInChildren<MeshRenderer>();
            theMat.material.color = theColor;
        }
    }
}