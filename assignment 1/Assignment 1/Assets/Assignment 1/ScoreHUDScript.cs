using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assignment1
{
    public class ScoreHUDScript : MonoBehaviour
    {
        private int gameOverThreshold;
        private Text scoreText;

        public void Initialize(int aThreshold)
        {
            //initialize score display
            scoreText = this.GetComponent<Text>();
            gameOverThreshold = aThreshold;
            ResetScoreDisplay();
        }

        public void ResetScoreDisplay()
        {
            //set display to 0
            UpdateScoreDisplay(0, 0, 0);
        }

        public void UpdateScoreDisplay(int success, int fail, float timer)
        {
            //format score display text
            scoreText.text = "Delivered: " + success;
            scoreText.text += "\nFailed: " + fail + "/" + gameOverThreshold;
            scoreText.text += "\nTime survived: " + Mathf.FloorToInt(timer);
        }
    }
}