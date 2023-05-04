using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assignment1
{
    public class GameOverMenuScript : MonoBehaviour, InputReceiver
    {
        private GameController gameController;

        //set game over display
        public void ShowGameOver(int success, int fail, float timer, GameController aController)
        {
            //get reference to game controller
            gameController = aController;

            //format game over text display
            Text gameOverText = this.GetComponentInChildren<Text>();
            gameOverText.text = "GAME OVER\n";
            gameOverText.text += "\nDelivered: " + success;
            gameOverText.text += "\nFailed: " + fail;
            gameOverText.text += "\n\nTime survived: " + Mathf.FloorToInt(timer) + "s";
            gameOverText.text += "\n\nAgain? Y/N";
        }
        public void DoMoveDir(Vector2 aDir)
        {
            //do nothing
        }

        public void DoYesAction()
        {
            //start game again
            gameController.StartGame();
        }

        public void DoNoAction()
        {
#if UNITY_EDITOR
            //if in unity editor, stop playing
            UnityEditor.EditorApplication.isPlaying = false;
#else
            //if not in unity editor, quit application
            Application.Quit();
#endif
        }
    }
}