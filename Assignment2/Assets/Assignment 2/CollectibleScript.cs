using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assignment2
{
    public class CollectibleScript : MonoBehaviour
    {
        public void DoReset()
        {
            this.gameObject.SetActive(true);
        }

        public void DoCollect()
        {
            this.gameObject.SetActive(false);
        }
    }
}