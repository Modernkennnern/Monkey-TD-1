﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tower { 

    public class TackShooter : NovaTower {

        protected override void Start() {
            base.Start();
        }

        [SerializeField, Header("Tack Shooter Specifics:")]
        protected int tacksToFire;

        protected override void Shoot() {

            List<Projectile.StandardProjectile> shotProjectileList = CreateProjectiles(projectileToFire, transform.position, transform.rotation, transform.parent, tacksToFire);


            for(int i = 0; i < shotProjectileList.Count; i++) {

                Projectile.StandardProjectile shotProjectile = shotProjectileList[i];
                float rotationAdjustment = (360 / tacksToFire) * i;
                Quaternion newRot = Quaternion.Euler(0, 0, rotationAdjustment);
                shotProjectile.transform.rotation = newRot;
                shotProjectile.despawnDistance = firingRange * 1.2f;
            }

            Debug.Log("Tack Shooter Shot!");
            base.Shoot();
        }
    }

}