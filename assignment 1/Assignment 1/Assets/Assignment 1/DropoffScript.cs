using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assignment1
{
    //script added to delivery objects, inherits DeliveryScript
    public class DropoffScript : DeliveryScript
    {
        // Update is called once per frame
        void Update()
        {
            //if inactive, do not update
            if (!isActive) return;

            //update timer
            timer -= Time.deltaTime;
            UpdateTimerFill();

            //if timer ended, dropoff failed
            if (timer <= 0)
            {
                //TASK 4a: Finish failed delivery
                //End the delivery as a failure.
                //You should use the FinishDelivery function in GameController.
                //TASK 4a START

                //TASK 4a END

                //set inactive
                isActive = false;

                //destroy object
                DestroyDelivery();
            }
        }

        //when trigger area enters
        private void OnTriggerEnter2D(Collider2D collision)
        {
            //check if entered trigger has PlayerDelivery script
            if (collision.GetComponent<PlayerDelivery>() != null)
            {
                //check if player is holding correct package
                PlayerDelivery delivery = collision.GetComponent<PlayerDelivery>();
                if (delivery.GetDeliveryIndex() == deliveryIndex)
                {
                    //TASK 4b: Finish successful delivery
                    //End the delivery as a success.
                    //You should use the FinishDelivery function in GameController.
                    //TASK 4b START

                    //TASK 4b END

                    //set this to inactive
                    isActive = false;

                    //destroy object
                    DestroyDelivery();
                }
            }
        }
    }
}