using Mono.Cecil;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Assignment1
{
    public class GameController : MonoBehaviour
    {
        public GameObject playerObj;
        public float packageTimeMin, packageTimeMax;
        public Object packageObj, dropoffObj;
        public InputHandler inputHandler;
        public GameOverMenuScript gameOverMenu;
        public ScoreHUDScript scoreHUD;

        public float difficultyScale; //how long before reaching min spawn interval
        public float spawnIntervalMin, spawnIntervalMax;
        public float minTravelDistance;
        public int maxCount;

        public int gameOverThreshold;

        private float gameTimer;
        private float spawnTimer;
        private int deliveryIndex;
        private int deliveryHint;
        private int deliveryCurrent;

        private Vector2 boundaryMin, boundaryMax;

        private int currentCount = 0;
        private int deliverySuccessCount = 0;
        private int deliveryFailCount = 0;

        public delegate void DeliveryNotify(int currentDelivery, int currentHint);
        public event DeliveryNotify DeliveryUpdate;

        // Start is called before the first frame update
        void Start()
        {
            //set game boundary according to camera boundary (for fixed camera)
            Camera mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
            boundaryMin = mainCamera.ViewportToWorldPoint(new Vector2(0.1f, 0.1f));
            boundaryMax = mainCamera.ViewportToWorldPoint(new Vector2(0.9f, 0.9f));

            //initialize input handler
            inputHandler.Initialize();

            //start game function
            StartGame();
        }

        // Update is called once per frame
        void Update()
        {
            //proceed game timers
            gameTimer += Time.deltaTime;
            spawnTimer -= Time.deltaTime;

            if (spawnTimer <= 0 && currentCount < maxCount)
            {
                //spawn new package
                deliveryIndex += 1;
                SpawnNewPackage(deliveryIndex);

                //reset spawn timer
                spawnTimer = GetNextSpawnTime();

                //increase current number of deliveries
                currentCount++;
            }

            //update score HUD
            scoreHUD.UpdateScoreDisplay(deliverySuccessCount, deliveryFailCount, gameTimer);
        }

        //Initialize all game values
        public void StartGame()
        {
            //set player initial position
            playerObj.transform.position = Vector2.zero;

            //clear existing prefabs (for replay)
            foreach (DeliveryScript delivery in GameObject.FindObjectsOfType<DeliveryScript>())
            {
                delivery.DestroyDelivery();
            }

            //reset all timers
            gameTimer = 0;
            spawnTimer = 0;

            //reset all delivery trackers
            deliveryIndex = 0;
            deliveryHint = 0;
            deliveryCurrent = 0;

            //reset all counters
            currentCount = 0;
            deliverySuccessCount = 0;
            deliveryFailCount = 0;

            //initialize player scripts (movement and delivery)
            foreach (PlayerScript playerScript in playerObj.GetComponents<PlayerScript>())
            {
                playerScript.Initialize(this);
            }

            //initialize score hud
            scoreHUD.Initialize(gameOverThreshold);

            //set input handler to player movement script
            inputHandler.SetInputReceiver(playerObj.GetComponent<PlayerMovement>());

            //hide game over menu
            gameOverMenu.gameObject.SetActive(false);

            //unpause game
            Time.timeScale = 1f;
        }

        #region Delivery

        //spawn new package
        private void SpawnNewPackage(int aIndex)
        {
            //get package prefab
            GameObject package = GetPrefab(packageObj);

            //end function if GetPrefab returns null
            if (package == null) return;

            //rename package object
            package.name = packageObj.name + "_" + aIndex;

            //randomize and set package position
            Vector2 packagePos = GetRandomPos(playerObj.transform.transform.position);
            package.transform.position = packagePos;

            //randomize package delivery time
            float packageTime = GetPackageTime();

            //initialize package script with details
            package.GetComponent<PackageScript>().Initialize(this, packageTime, aIndex);

            //spawn dropoff location
            SpawnNewDropoff(deliveryIndex, packageTime, packagePos);

            //update all delivery displays
            DeliveryUpdate?.Invoke(deliveryCurrent, deliveryHint);
        }

        private void SpawnNewDropoff(int aIndex, float packageTime, Vector2 packagePos)
        {
            //get dropoff prefab
            GameObject dropoff = GetPrefab(dropoffObj);

            //end function if GetPrefab returns null
            if (dropoff == null) return;

            //rename dropoff object
            dropoff.name = dropoffObj.name + "_" + aIndex;

            //randomize and set package position
            dropoff.transform.position = GetRandomPos(packagePos);

            //initialize dropoff script with details
            dropoff.GetComponent<DropoffScript>().Initialize(this, packageTime, aIndex);
        }

        //randomize position a minimum distance away from a given point
        private Vector2 GetRandomPos(Vector2 avoidPos)
        {
            Vector2 randomPos = avoidPos;

            //loop while randomPos is too close to avoidPos
            while (Vector2.Distance(randomPos, avoidPos) <= minTravelDistance)
            {
                //randomize a position within game boundary
                randomPos = new Vector2(Random.Range(boundaryMin.x, boundaryMax.x), Random.Range(boundaryMin.y, boundaryMax.y));
            }

            return randomPos;
        }

        public void FinishDelivery(bool isSuccess, int finishIndex)
        {
            if (isSuccess) deliverySuccessCount += 1;
            else deliveryFailCount += 1;

            //clear delivery index on player (if holding package)
            playerObj.GetComponent<PlayerDelivery>().ClearDeliveryIndex(finishIndex);

            //update score display
            scoreHUD.UpdateScoreDisplay(deliverySuccessCount, deliveryFailCount, gameTimer);

            //track current delivery count
            currentCount--;

            //if failed delivery reaches game over threshold
            if (deliveryFailCount >= gameOverThreshold)
            {
                //is game over, pause game
                Time.timeScale = 0;

                //set input handler to game over menu script
                inputHandler.SetInputReceiver(gameOverMenu);

                //show game over menu
                gameOverMenu.ShowGameOver(deliverySuccessCount, deliveryFailCount, gameTimer, this);
                gameOverMenu.gameObject.SetActive(true);
            }
        }

        //get time interval before next delivery spawns
        private float GetNextSpawnTime()
        {
            //spawn time scales from max to min as game timer proceeds towards difficulty scale threshold
            return Mathf.Lerp(spawnIntervalMax, spawnIntervalMin, gameTimer / difficultyScale);
        }

        //get random time duration for delivery to be completed
        private float GetPackageTime()
        {
            //randomize between min and max time
            return Random.Range(packageTimeMin, packageTimeMax);
        }

        #endregion Delivery

        #region Hint Icon

        public void SetDeliveryHint(int aHint)
        {
            //show hint icon for given index
            deliveryHint = aHint;

            //update all delivery displays
            DeliveryUpdate?.Invoke(deliveryCurrent, deliveryHint);
        }

        public void ClearDeliveryHint(int aHint)
        {
            //clear hint icon for given index
            if (deliveryHint == aHint) deliveryHint = 0;

            //update all delivery displays
            DeliveryUpdate?.Invoke(deliveryCurrent, deliveryHint);
        }

        public void SetCurrentDelivery(int aCurrent)
        {
            //set current package index
            deliveryCurrent = aCurrent;

            //update all delivery displays
            DeliveryUpdate?.Invoke(deliveryCurrent, deliveryHint);
        }

        #endregion Hint Icon

        #region Prefabs

        //for object pooling
        private Dictionary<string, List<GameObject>> objectPool = new Dictionary<string, List<GameObject>>();
        private const int POOL_MAX = 20;

        public GameObject GetPrefab(Object prefab)
        {
            //TASK 2a: Create GameObject from prefab
            //Create a new GameObject from given prefab object.
            //Return the newly-created GameObject.
            //TASK 2b: Get GameObject from object pool (Bonus)
            //Convert the object creation code to use object pooling.
            //Use Dictionary<string, List<GameObject>> objectPool to store the lists of GameObjects.
            //This function should remove a GameObject from the pool, if available, 
            //and return that instead of creating a new GameObject.
            //TASK 2a/b START

            //TASK 2a

            GameObject newobj = new GameObject ();

            newobj = Instantiate((GameObject)prefab);

            return newobj; 


            //TASK 2a/b END
        }

        public void DestroyPrefab(GameObject aObj)
        {
            //TASK 2c: Destroy Game object
            //Destroy the given GameObject.
            //TASK 2d: Add GameObject to object pool (Bonus)
            //Convert the object destruction code to use object pooling.
            //Use Dictionary<string, List<GameObject>> objectPool to store the lists of GameObjects.
            //This function should add the GameObject to the pool instead of destroying it, 
            //if the pool contains fewer than POOL_MAX items.
            //More details given in the assignment document.
            //TASK 2c/d START

            Destroy(aObj);

            //TASK 2c/d END
        }

        #endregion Prefabs
    }
}