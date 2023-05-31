using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assignment2
{
    public class StartMenuController : GameSceneController
    {
        public override void Initialize(GameController aController)
        {
            base.Initialize(aController);
        }

        public void StartLevel(string aScene)
        {
            gameController.LoadScene(aScene);
            gameController.RemoveScene(sceneName);
        }
    }
}