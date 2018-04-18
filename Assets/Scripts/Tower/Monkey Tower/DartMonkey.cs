﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tower {

    public class DartMonkey : MonkeyTower {

        protected override void Start() {
            base.Start();
        }

        protected override void Shoot() {
            List<Projectile.StandardProjectile> shotProjectileList = new List<Projectile.StandardProjectile>();

            for (int i = 0; i < projectileSpawnPoints.Length; i++) {
                shotProjectileList.Add(CreateProjectile(projectileToFire, projectileSpawnPoints[i].position, transform.rotation, transform.parent));
            }

            foreach (Projectile.StandardProjectile projectile in shotProjectileList) {
                projectile.despawnDistance = firingRange * 2.0f;
            }

            Debug.Log("Dart Monkey shot!");
            base.Shoot();
        }
    }
}