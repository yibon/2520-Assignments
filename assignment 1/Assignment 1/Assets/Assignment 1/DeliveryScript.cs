using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Assignment1
{
    //base script for package and dropoff object scripts
    public class DeliveryScript : MonoBehaviour
    {
        protected GameController gameController;
        protected float timer;
        protected float timerMax;
        protected bool isActive;
        protected int deliveryIndex;

        public GameObject targetIcon;
        public Transform timerFill;

        private SpriteRenderer targetSR;

        //initialize delivery
        public virtual void Initialize(GameController aController, float aTimer, int aIndex)
        {
            //set reference
            gameController = aController;

            //subscribe to target icon update event
            gameController.DeliveryUpdate += UpdateTargetIcon;

            //set delivery index
            deliveryIndex = aIndex;

            //set delivery timers
            timerMax = aTimer;
            timer = timerMax;
            UpdateTimerFill();

            //set delivery object active
            isActive = true;
            this.gameObject.SetActive(true);

            targetSR = targetIcon.GetComponent<SpriteRenderer>(); 

        }

        //TASK 3a: Mouse input detection
        //GameController has SetDeliveryHint and ClearDeliveryHint functions.
        //Write function(s) within this class to detect mouse input 
        //and call the 2 functions as appropriate.
        //SetDeliveryHint should pass deliveryIndex to gameController when
        //the player hovers the mouse over a package or dropoff object.
        //ClearDeliveryHint should pass deliveryIndex to gameController when
        //the player stops hovering the mouse over the object.
        //This script is inherited by PackageScript and DropoffScript, 
        //which are added to the package and dropoff objects respectively.
        //TASK 3a START

        private void OnMouseOver()
        {
            gameController.SetCurrentDelivery(deliveryIndex);
        }

        private void OnMouseExit()
        {
            gameController.ClearDeliveryHint(deliveryIndex);
        }





        //TASK 3a END

        //update target icon display on current object
        public void UpdateTargetIcon(int currentDelivery, int currentHint)
        {
            //TASK 3b: Update Icon
            //Complete the function such that targetIcon is set active and set to a yellow colour 
            //when the current object has deliveryIndex equal to the current delivery. 
            //If the current object is not indicated as the delivery dropoff point, 
            //check if the object has deliveryIndex equal to the current hint. 
            //If yes, show the target icon and set its colour to white, otherwise hide the icon.
            //TASK 3b START

            if (currentDelivery == deliveryIndex)
            {
                targetIcon.SetActive(true);
                targetSR.color = Color.yellow;
            }

            else
            {
                if (currentDelivery == currentHint)
                {
                    targetIcon.SetActive(true);
                    targetSR.color = Color.white;
                }

                else
                {
                    targetIcon.SetActive(false);
                }
            }


            //TASK 3b END
        }



        //delivery ended (success or fail)
        public void DestroyDelivery()
        {
            //remove from target icon update event
            gameController.DeliveryUpdate -= UpdateTargetIcon;

            //destroy game object using game controller function
            gameController.DestroyPrefab(this.gameObject);

            //disable game object
            this.gameObject.SetActive(false);
        }

        //update timer bar
        protected void UpdateTimerFill()
        {
            //change bar scale according to time left
            timerFill.localScale = new Vector3(1f, Mathf.Lerp(0, 1f, timer / timerMax), 1f);
        }
    }
}