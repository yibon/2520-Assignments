using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assignment1
{
    //script added to package objects, inherits DeliveryScript
    public class PackageScript : DeliveryScript
    {
        // Update is called once per frame
        void Update()
        {
            //if inactive, do not update
            if (!isActive) return;

            //update timer
            timer -= Time.deltaTime;
            UpdateTimerFill();

            //if timer ended, pickup failed
            if (timer <= 0)
            {
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
                //check if player is empty-handed
                PlayerDelivery delivery = collision.GetComponent<PlayerDelivery>();
                if (!delivery.CheckHasDelivery())
                {
                    //automatically pick up package
                    delivery.SetDeliveryIndex(deliveryIndex);

                    //set this to inactive
                    isActive = false;

                    //destroy object
                    DestroyDelivery();
                }
            }
        }
    }
}