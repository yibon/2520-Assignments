using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assignment1
{
    //script to manage player deliveries
    public class PlayerDelivery : PlayerScript
    {
        public GameObject packageIcon;
        private GameController gameController;
        private int deliveryIndex = 0;

        public override void Initialize(GameController aController)
        {
            //set game controller reference
            gameController = aController;

            //initialize delivery index
            deliveryIndex = 0;
            
            //update target icon display
            packageIcon.SetActive(deliveryIndex > 0);
            gameController.SetCurrentDelivery(deliveryIndex);
        }

        public bool CheckHasDelivery()
        {
            return deliveryIndex != 0;
        }

        public int GetDeliveryIndex()
        {
            return deliveryIndex;
        }

        public void SetDeliveryIndex(int aIndex)
        {
            //set current delivery
            deliveryIndex = aIndex;

            //update target icon display
            packageIcon.SetActive(deliveryIndex > 0);
            gameController.SetCurrentDelivery(deliveryIndex);
        }

        public void ClearDeliveryIndex(int aIndex)
        {
            //clear delivery index if index is the current delivery
            if (deliveryIndex == aIndex)
            {
                deliveryIndex = 0;

                //update target icon display
                packageIcon.SetActive(deliveryIndex > 0);
                gameController.SetCurrentDelivery(deliveryIndex);
            }
        }
    }
}