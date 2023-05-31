using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assignment2
{
    public class EndPointScript : MonoBehaviour
    {
        public void ShowEndPoint()
        {
            this.gameObject.SetActive(true);
        }

        public void HideEndPoint()
        {
            this.gameObject.SetActive(false);
        }
    }
}