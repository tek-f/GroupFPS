using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GroupFPS.Weapons;

namespace GroupFPS.Weapons
{
    public class AssultRifle : GeneralGun
    {
        protected override void Awake()
        {
            damage = 10;
            range = 25;
            maxClip = 30;
            base.Awake();
        }
    }
}