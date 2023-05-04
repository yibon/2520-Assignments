using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assignment1
{
    public class PlayerMovement : PlayerScript, InputReceiver
    {
        public Camera mainCamera;
        public float moveSpeed;

        private Rigidbody2D playerrb2D;
        private Vector2 moveDir;

        public override void Initialize(GameController gameController)
        {
            //not moving
            moveDir = Vector2.zero;
        }

        private void Start()
        {
            playerrb2D = GetComponent<Rigidbody2D>();
        }

        #region Input handling

        public void DoMoveDir(Vector2 aDir)
        {
            //set movement direction
            moveDir = aDir;

            //if moving, clamp movement vector magnitude
            if (moveDir.magnitude > 1f) moveDir.Normalize();

            //TASK 1b: Move player object
            //Vector moveDir has been received from InputHandler.
            //Use moveDir to move the player in the input direction according to moveSpeed.
            //You are encouraged to use the MovePosition function in Rigidbody2D for this.
            //If your calculation results in unusually slow or fast movement, 
            //you may specify a new moveSpeed in a clear comment within the script
            //(eg. //recommended moveSpeed = 10), and set your moveSpeed in the Player object in scene.
            //If no new moveSpeed is specified, default speed of 10 will be used.
            //TASK 1b START

            //Debug.Log(moveDir);
            moveSpeed = 7.5f;

            //                      v Getting the initial position 
            playerrb2D.MovePosition(playerrb2D.position + (moveDir * moveSpeed * Time.fixedDeltaTime));

            //TASK 1b END

            //TASK 1c: Face direction
            //TASK 1c START 

            //This direction only needs to be set when the player is moving.
            // Checking if player is moving
            if (moveDir.x != 0 || moveDir.y != 0 || moveDir.magnitude != 0)
            {
                //Rotate the player object such that the dot points in the direction the player is moving.
                transform.up = moveDir;
            }

            //TASK 1c END
        }

        public void DoYesAction()
        {
            //do nothing
        }

        public void DoNoAction()
        {
            //do nothing
        }

        #endregion Input handling


    }
}