using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GunBall.Weapons;

namespace GunBall.Weapons
{
    public class AssultRifle : GeneralGun
    {
        protected override void Awake()
        {
            damage = 10;
            range = 25;
            maxClip = 30;
            //set the guns player location
            base.Awake();
        }
    }
}