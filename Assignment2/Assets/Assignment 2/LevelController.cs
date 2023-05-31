using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assignment2
{
    public class LevelController : GameSceneController
    {
        private Camera mainCamera;
        private PlayerScript player;
        private List<EnemyScript> enemyList;
        private List<CollectibleScript> collectibleList;
        private EndPointScript endPoint;
        private int collectedCount;

        private bool isStarted;

        public float outOfBoundsDuration = 3f;
        private float outOfBoundsTimer;

        public override void Initialize(GameController aController)
        {
            isStarted = false;

            base.Initialize(aController);

            mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();

            //clear auto destroy objects
            foreach (DamagerScript autoDestroy in FindObjectsOfType<DamagerScript>())
            {
                autoDestroy.DestroyDamager();
            }

            //initialize player script
            if (player == null) player = FindObjectOfType<PlayerScript>();
            if (player != null) player.Initialize(this);

            //initialize all guard scripts
            if (enemyList == null)
            {
                enemyList = new List<EnemyScript>();
                enemyList.AddRange(FindObjectsOfType<EnemyScript>());
            }

            foreach (EnemyScript enemyScript in enemyList)
            {
                enemyScript.Initialize(player);
            }

            //reset collectibles
            if (collectibleList == null)
            {
                collectibleList = new List<CollectibleScript>();
                collectibleList.AddRange(FindObjectsOfType<CollectibleScript>());
            }
            ResetCollectedCount();

            if (endPoint == null) endPoint = FindObjectOfType<EndPointScript>();
            endPoint.HideEndPoint();

            gameController.StartLevel(player);

            gameController.UpdateProgressHUD(collectedCount, collectibleList.Count);

            isStarted = true;
        }

        void Update()
        {
            if (player != null && !gameController.CheckGameOver())
            {
                Vector2 viewportPos = mainCamera.WorldToViewportPoint(player.transform.position);
                if (viewportPos.x > 1f || viewportPos.x < 0 || viewportPos.y > 1f || viewportPos.y < 0)
                {
                    //add out of bounds time
                    outOfBoundsTimer += Time.deltaTime;

                    gameController.ShowWarning(true, outOfBoundsDuration, outOfBoundsTimer);
                }
                else
                {
                    outOfBoundsTimer = 0;
                    gameController.ShowWarning(false, outOfBoundsDuration, outOfBoundsTimer);
                }

                //out of bounds game over
                if (outOfBoundsTimer >= outOfBoundsDuration)
                {
                    gameController.SetGameOver(true, false, collectedCount, collectibleList.Count);
                }
            }
            else
            {
                //no player or game not active
                outOfBoundsTimer = 0;
                gameController.ShowWarning(false, outOfBoundsDuration, outOfBoundsTimer);
            }
        }

        public void SetGameOver(bool aGameOver, bool isWin)
        {
            gameController.SetGameOver(aGameOver, isWin, collectedCount, collectibleList.Count);
        }

        public bool CheckGameOver()
        {
            //check if game over
            return gameController.CheckGameOver();
        }

        public void AddCollectedCount(int add)
        {
            collectedCount += add;
            gameController.UpdateProgressHUD(collectedCount, collectibleList.Count);

            if (collectedCount >= collectibleList.Count)
            {
                endPoint.ShowEndPoint();
            }
        }

        public void ResetCollectedCount()
        {
            foreach (CollectibleScript collectible in collectibleList)
            {
                collectible.DoReset();
            }

            collectedCount = 0;
            gameController.UpdateProgressHUD(collectedCount, collectibleList.Count);
        }

        public bool CheckIsStarted()
        {
            return isStarted;
        }
    }
}