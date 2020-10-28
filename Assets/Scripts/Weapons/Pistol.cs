using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GroupFPS.Weapons;

namespace GroupFPS.Weapons
{
    public class Pistol : GeneralGun
    {
        public override void Reload()
        {
            if (currentClip < maxClip)
            {
                currentClip = maxClip;
                UpdateUI();
            }
        }
        public override void UpdateUI()
        {
            clipText.text = currentClip + " / " + maxClip;
            ammoPoolText.text = "";
        }

        protected override void Awake()
        {
            damage = 5;
            range = 20;
            maxClip = 10;
            currentClip = maxClip;
        }
    }
}