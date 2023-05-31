using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assignment2
{
    public class BulletScript : DamagerScript
    {
        private bool isFlying = false;
        private float speed, distance;
        private Vector2 direction;

        private float flightDist;

        public void Initialize(float speed, float distance, Vector2 direction)
        {
            isFlying = true;
            this.speed = speed;
            this.distance = distance;
            this.direction = direction;

            flightDist = 0;
        }

        void FixedUpdate()
        {
            if (isFlying)
            {
                Vector3 movement = (Vector3)direction.normalized * speed * Time.fixedDeltaTime;
                this.transform.position += movement;

                flightDist += movement.magnitude;

                if (flightDist >= distance)
                {
                    DestroyDamager();
                }
            }
        }

        public override void DoOnHit()
        {
            DestroyDamager();
        }
    }
}