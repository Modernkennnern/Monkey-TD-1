﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tower {

    public class MonkeyTower : StandardTower {

        protected Collider2D target;

        protected enum TargettingStates {
            First,
            Last,
            Closest,
            Toughest
        }
        [SerializeField]
        protected TargettingStates state;


        protected override void FixedUpdate() {

            if (GameControl.WaveSpawner.controllerObject.waveActive) {
                justShot -= Time.fixedDeltaTime * GameControl.GameController.controllerObject.currentGameSpeed;
                firingCooldown -= Time.fixedDeltaTime * GameControl.GameController.controllerObject.currentGameSpeed;
                aimCooldown -= Time.fixedDeltaTime * GameControl.GameController.controllerObject.currentGameSpeed;
                if (justShot < 0) {
                    if (aimCooldown < 0) {
                        targetting = true;

                        if (attackSpeed < GameControl.GameController.controllerObject.aimTimer)
                            aimCooldown = attackSpeed * 2 / 3;

                        else
                            aimCooldown = GameControl.GameController.controllerObject.aimTimer;

                        target = SearchForEnemiesInRange();

                        if (target != null) {
                            if (justShot <= 0)
                                AimAt(target.gameObject);
                        }
                    }
                }
                else targetting = false;
            }
            else {
                targetting = false;
                justShot = -1;
                firingCooldown = -1;
                aimCooldown = -1;
            }
        }


        protected Collider2D SearchForEnemiesInRange() {

            Collider2D[] allCollisions = GetEnemiesInRange();
            Collider2D _target = null;

            switch (state) {
                case TargettingStates.Closest:
                    _target = SearchForClosestEnemy(allCollisions);
                    break;
                case TargettingStates.First:
                    _target = SearchForFirstEnemy(allCollisions);
                    break;
                case TargettingStates.Last:
                    // _target = SearchForLastEnemy(allCollisions);
                    break;
                case TargettingStates.Toughest:
                    // _target = SearchForToughestEnemy(allCollisions);
                    break;
            }
            return _target;
        }

        protected virtual Collider2D SearchForClosestEnemy(Collider2D[] allCollisions) {
            Collider2D enemy = null;
            float closestEnemyDistance = float.MaxValue;

            foreach (Collider2D target in allCollisions) {
                if (target.tag == "Enemy") {

                    float absDist = Vector2.Distance(target.transform.position, transform.position);
                    if (absDist < closestEnemyDistance) {
                        enemy = target;
                        closestEnemyDistance = absDist;
                    }
                }
            }
            return enemy;
        }

        protected virtual Collider2D SearchForFirstEnemy(Collider2D[] allCollisions) {

            Collider2D enemy = null;
            float firstEnemyDistance = 0;
            int firstEnemyWaypoint = 0;

            foreach (Collider2D target in allCollisions) {
                if (target.tag == "Enemy") {

                    int currentWayPoint = target.GetComponent<WayPoints>().currentWayPoint;

                    float distanceToPreviousWaypoint = target.GetComponent<WayPoints>().distanceToPreviousWaypoint;

                    if (currentWayPoint > firstEnemyWaypoint) {
                        enemy = target;
                        firstEnemyWaypoint = currentWayPoint;
                        firstEnemyDistance = distanceToPreviousWaypoint;
                    }
                    else if (currentWayPoint == firstEnemyWaypoint) {
                        if (distanceToPreviousWaypoint > firstEnemyDistance) {
                            enemy = target;
                            firstEnemyWaypoint = currentWayPoint;
                            firstEnemyDistance = distanceToPreviousWaypoint;
                        }
                    }
                }
            }

            return enemy;
        }

        protected virtual Collider2D SearchForLastEnemy(Collider2D[] allCollisions) {

            Collider2D enemy = null;
            float firstEnemyDistance = 0;
            int firstEnemyWaypoint = 0;

            foreach (Collider2D target in allCollisions) {
                if (target.tag == "Enemy") {

                    int currentWayPoint = target.GetComponent<WayPoints>().currentWayPoint;

                    Vector2 currentWayPointPos = GameControl.PathController.controllerObject.wayPointList[currentWayPoint].position;

                    float absDist = Vector2.Distance(currentWayPointPos, transform.position);

                    if (currentWayPoint >= firstEnemyWaypoint) {
                        if (absDist >= firstEnemyDistance) {
                            firstEnemyWaypoint = currentWayPoint;
                            firstEnemyDistance = absDist;
                            enemy = target;
                        }
                    }
                }
            }

            return enemy;
        }

        protected virtual Collider2D SearchForToughestEnemy(Collider2D[] allCollisions) {

            Collider2D enemy = null;
            float firstEnemyDistance = 0;
            int firstEnemyWaypoint = 0;

            foreach (Collider2D target in allCollisions) {
                if (target.tag == "Enemy") {

                    int currentWayPoint = target.GetComponent<WayPoints>().currentWayPoint;

                    Vector2 currentWayPointPos = GameControl.PathController.controllerObject.wayPointList[currentWayPoint].position;

                    float absDist = Vector2.Distance(currentWayPointPos, transform.position);

                    if (currentWayPoint >= firstEnemyWaypoint) {
                        if (absDist >= firstEnemyDistance) {
                            firstEnemyWaypoint = currentWayPoint;
                            firstEnemyDistance = absDist;
                            enemy = target;
                        }
                    }
                }
            }

            return enemy;
        }

        protected virtual void AimAt(GameObject target) {

            var dir = target.transform.position - transform.position;
            var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + rotationOffset;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

            if (firingCooldown < 0) {
                Shoot();
            }
        }

        protected override void Shoot() {
            firingCooldown = attackSpeed;

            base.Shoot();

            if (attackSpeed < rotationDurationAfterShooting)
                justShot = attackSpeed * 1 / 3;
            else
                justShot = rotationDurationAfterShooting;
        }

    }

}