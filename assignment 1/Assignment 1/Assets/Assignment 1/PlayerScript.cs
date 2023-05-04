using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assignment1
{
    //inherited class for all player scripts 
    public abstract class PlayerScript : MonoBehaviour
    {
        public abstract void Initialize(GameController gameController);
    }
}