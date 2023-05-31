using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assignment2
{
    public abstract class AttackStrategy : MonoBehaviour
    {
        public float delay;
        public float distance;

        public abstract void DoAttack();
    }

    public class DamagerScript : MonoBehaviour
    {
        public void DestroyDamager()
        {
            Destroy(this.gameObject);
        }

        public virtual void DoOnHit()
        {
            //do nothing by default
        }
    }
}