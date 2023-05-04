using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assignment1
{
    public class InputHandler : MonoBehaviour
    {
        private InputReceiver activeReceiver;

        private Dictionary<InputType, KeyCode> inputList;

        private float horizontalIP;
        private float verticalIP;

        public void Initialize()
        {
            //make list of input keys (useful for keybinding)
            inputList = new Dictionary<InputType, KeyCode>();
            inputList.Add(InputType.YES, KeyCode.Y);
            inputList.Add(InputType.NO, KeyCode.N);
        }

        public void SetInputReceiver(InputReceiver inputReceiver)
        {
            //set current input receiver (to control 1 thing at a time)
            activeReceiver = inputReceiver;
        }

        // Update is called once per frame
        void Update()
        {
            //detect key input every frame

            //TASK 1d: Get input keys
            //Without using Unity Input Manager, detect keyboard input for Yes and No responses
            //and call the DoYesAction and DoNoAction functions respectively in activeReceiver.
            //The input should be detected when the key is pressed down.
            //You may retrieve the keycodes assigned for Yes and No input from inputList.
            //TASK 1d START

            // If input YES is pressed
            if (Input.GetKeyDown(inputList[InputType.YES]))
            {
                activeReceiver.DoYesAction();
            }

            // if input NO is pressed
            if (Input.GetKeyDown(inputList[InputType.NO]))
            {
                activeReceiver.DoNoAction();
            }

            //TASK 1d END

            //TASK 1e: Yes and No Actions (open-ended question)
            //Answer the following questions within the comment below:
            //activeReceiver is an InputReceiver interface object. 
            //Apart from PlayerMovement, what other class is an InputReceiver?
            //What do DoYesAction and DoNoAction do in that class?
            //TASK 1e START

            /*Answer:  
             GameOverMenuScript is also a class that is an InputReciever interface object,
             DoYesAction restarts the game level, while DoNoAction quits the game. */

            //TASK 1e END
        }

        void FixedUpdate()
        {
            //TASK 1a: Get movement direction
            //Using Unity Input Manager (Edit -> Project Settings -> Input Manager), 
            //without changing default values in Input Manager,
            //TASK 1a START

            //get the values of the Horizontal and Vertical axes (WASD and arrow keys by default)
            horizontalIP = Input.GetAxis("Horizontal");
            verticalIP = Input.GetAxis("Vertical");

            //and set them to moveDir vector as x and y values.
            Vector2 moveDir = new Vector3(horizontalIP, verticalIP, 0f);
            
            //TASK 1a END

            //apply move
            activeReceiver.DoMoveDir(moveDir);
        }
    }

    //enum for input keys
    public enum InputType
    {
        YES,
        NO
    }

    //interface for all input receivers
    public interface InputReceiver
    {
        void DoMoveDir(Vector2 aDir); //for movement controls
        void DoYesAction(); //yes option
        void DoNoAction(); //no option
    }
}