using UnityEngine;

namespace AbrahamDev
{
    public class HealthComponent : BaseComponent
    {
        protected override void Setup()
        {
            SetColor(new Color32(255, 0,0, 255));
        }
    }
}