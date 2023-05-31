using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Assignment2
{
    public class AttackShoot : AttackStrategy
    {
        public Object bulletObj;
        public float bulletSpeed;

        public override void DoAttack()
        {
            StartCoroutine(DoAttackSequence());
        }

        private IEnumerator DoAttackSequence()
        {
            yield return new WaitForSeconds(delay);
            GameObject bullet = Instantiate(bulletObj, this.transform.position, Quaternion.identity) as GameObject;
            bullet.GetComponent<BulletScript>().Initialize(bulletSpeed, distance, transform.up);
        }
    }
}