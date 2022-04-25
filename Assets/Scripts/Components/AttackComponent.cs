using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AbrahamDev
{
    public class AttackComponent : BaseComponent
    {
        protected override void Setup()
        {
            SetColor(new Color32(0, 255, 0, 255));
        }
    }
}