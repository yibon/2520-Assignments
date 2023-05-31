using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Assignment2
{
    public class GameController : MonoBehaviour
    {
        private PlayerScript player;
        private GameSceneController currentSceneController;

        public string initialScene = "A2StartScreen";
        public GameObject gameOverScreen;
        public GameObject pauseScreen;
        public GameObject warningText;
        public GameObject progressHUD;

        private bool isGameOver;
        private bool isPaused;

        void Start()
        {
            //set initial state
            SetGameOver(false, false, 0, 0);
            SetPause(false);

            //load initial scene
            LoadScene(initialScene);
        }

        void Update()
        {
            if (player != null && !isGameOver && Input.GetKeyDown(KeyCode.Escape))
            {
                TogglePause();
            }
        }

        void FixedUpdate()
        {

            if (player != null && !isGameOver && !isPaused)
            {
                Vector2 moveDir = Vector2.zero;
                if (Input.GetKey(KeyCode.W)) moveDir += Vector2.up;
                if (Input.GetKey(KeyCode.S)) moveDir += Vector2.down;
                if (Input.GetKey(KeyCode.A)) moveDir += Vector2.left;
                if (Input.GetKey(KeyCode.D)) moveDir += Vector2.right;

                //move player position
                player.MovePlayer(moveDir.normalized * Time.fixedDeltaTime);
            }
        }

        public void ShowWarning(bool aShow, float outOfBoundsDuration, float outOfBoundsTimer)
        {
            //countdown if player is offscreen
            warningText.SetActive(aShow);
            warningText.GetComponent<Text>().text = "Out of Bounds!\nGame Over in " + Mathf.CeilToInt(outOfBoundsDuration - outOfBoundsTimer) + "s";
        }

        public void LoadScene(string aScene)
        {
            AsyncOperation loadSceneOp = SceneManager.LoadSceneAsync(aScene, LoadSceneMode.Additive);
            loadSceneOp.completed += (result) =>
            {
                //Task 1a: Initialize Scene Controller
                //Get the newly-loaded scene using GetSceneByName in SceneManager. 
                //Get root game objects in the scene using GetRootGameObjects function.
                //Loop through the root game objects and set currentSceneController to any GameSceneController found.
                //You can use GetComponentInChild for this. The scene should contain only 1 GameSceneController.
                //Run Initialize function on the GameSceneController found.
                //Task 1a START

                Scene newlyLoaded = SceneManager.GetSceneByName(aScene);
                GameObject[] rootObjs = newlyLoaded.GetRootGameObjects();

                for (int i = 0; i < rootObjs.Length; i++)
                {
                    currentSceneController = rootObjs[i].GetComponentInChildren<GameSceneController>();
                }

                currentSceneController.Initialize(this);

                //Task 1a END
            };
        }

        public void RemoveScene(string aScene)
        {
            //Task 1b: Remove Scene
            //Unload the scene of the name given by aScene.
            //Use the UnloadSceneAsync function in SceneManager.
            //Task 1b START

            SceneManager.UnloadSceneAsync(aScene);

            //Task 1b END
        }

        public void RestartLevel()
        {
            if (currentSceneController != null) currentSceneController.Initialize(this);
        }

        public void StartLevel(PlayerScript playerScript)
        {
            player = playerScript;

            //set game ongoing
            SetGameOver(false, false, 0, 0);
            SetPause(false);
        }

        public void GoToLevelSelect()
        {
            SetGameOver(false, false, 0, 0);
            SetPause(false);

            if (currentSceneController != null) RemoveScene(currentSceneController.sceneName);
            LoadScene("A2StartScreen");
        }

        public void TogglePause()
        {
            SetPause(!isPaused);
        }

        public void SetPause(bool aPause)
        {
            //Task 6a: Set Paused State
            //Set the boolean isPaused according to the input aPause.
            //Set Time.timeScale to 0 if paused, and 1f if unpaused.
            //Set pauseScreen gameObject to active if paused and inactive if unpaused.
            //Task 6a START

            //Task 6a END
        }

        public void SetGameOver(bool aGameOver, bool aWin, int numCollected, int numTotal)
        {
            //set game over state
            isGameOver = aGameOver;

            ShowWarning(false, 0, 0);
            if (isGameOver) Time.timeScale = 0;

            //show game over screen if game over
            if (!aWin)
            {
                if (numTotal > 0)
                {
                    gameOverScreen.GetComponentInChildren<Text>().text = "Game Over\nProgress: " + numCollected + "/" + numTotal;
                }
                else
                {
                    gameOverScreen.GetComponentInChildren<Text>().text = "Game Over\nOut of Bounds";
                }
            }
            else
                gameOverScreen.GetComponentInChildren<Text>().text = "Level Completed!";

            gameOverScreen.SetActive(isGameOver);
        }

        public bool CheckGameOver()
        {
            //check if game over
            return isGameOver;
        }

        public void UpdateProgressHUD(int numCollected, int numTotal)
        {
            progressHUD.GetComponent<Text>().text = "Progress: " + numCollected + "/" + numTotal;

            if (numCollected >= numTotal)
            {
                progressHUD.GetComponent<Text>().text += " DONE!\nHead to Exit";
            }
        }
    }
}