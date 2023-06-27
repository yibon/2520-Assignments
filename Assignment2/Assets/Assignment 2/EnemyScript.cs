using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assignment2
{
    public class EnemyScript : MonoBehaviour
    {
        public int initialWaypoint = 0;

        private PlayerScript player;

        public List<Transform> waypointList;
        private int currentWaypoint = 0;

        public float sightRange = 5f;
        public float angleRange = 10f;
        public float attackRange = 2.5f;
        public float moveSpeed = 3f;
        public float chaseSpeed = 5f;
        public float lookAroundSpeed = 180f;

        public float susRate = 1f;
        private float suspicion = 0;

        public Object attackPrefab;
        public float attackInterval = 1.5f;
        private float attackTimer = 0;

        public float idleTime = 1.5f;
        public int idleInterval = 3;
        private int idleCount = 0;

        private EnemyState currentState;

        public SpriteRenderer suspicionCircle;
        public SpriteRenderer eyeCircle;

        private Vector2 playerLastSeenPos;

        void FixedUpdate()
        {
            //Task 2c: Update Guard action
            //Update the position, action and state of the guard using the DoActionUpdate function of the current State.
            //As this is done in FixedUpdate, the delta time to be passed as input parameter will be Time.fixedDeltaTime.
            //Task 2c START

            currentState.DoActionUpdate(Time.fixedDeltaTime);

            //Task 2c END
        }

        public void SetCurrentState(EnemyState nextState)
        {
            //Task 2a: Set current state
            //Set the current state of the Guard to the input nextState.
            //Task 2a START

            currentState = nextState;

            //Task 2a END
        }

        void Update()
        {
            attackTimer += Time.deltaTime;
        }

        public void Initialize(PlayerScript aPlayer)
        {
            player = aPlayer;

            //set to initial position
            this.transform.position = waypointList[initialWaypoint].position;

            //set to first waypoint and initial state
            currentWaypoint = initialWaypoint;
            idleCount = 0;
            attackTimer = attackInterval;

            //Task 2b: Set initial state
            //Set the initial state of the Guard by setting currentState to a new EnemyStatePatrol state.
            //You will need to pass in this EnemyScript as an input parameter to the constructor.
            //Task 2b START

            currentState = new EnemyStatePatrol(this);

            //Task 2b END
        }

        #region movement

        public Vector2 GetPlayerLastSeenPos()
        {
            //get player last seen position
            return playerLastSeenPos;
        }

        public void SetPlayerLastSeenPos(Vector2 lastPos)
        {
            //set player last seen position
            playerLastSeenPos = lastPos;
        }

        public Vector2 GetPlayerPosition()
        {
            //get player position
            return player.transform.position;
        }

        public Vector2 GetSelfPosition()
        {
            //get self position
            return this.transform.position;
        }

        public Vector2 GetNextWaypoint()
        {
            //get target waypoint
            return waypointList[currentWaypoint].position;
        }

        public void SetToNextWaypoint()
        {
            //go to next waypoint
            currentWaypoint += 1;

            //cycle back to first waypoint
            if (currentWaypoint >= waypointList.Count) currentWaypoint = 0;
        }

        public void FaceTowards(Vector2 targetPos)
        {
            //set guard face direction
            Vector2 direction = targetPos;
            direction.x -= this.transform.position.x;
            direction.y -= this.transform.position.y;
            this.transform.up = direction.normalized;
        }

        public void Rotate(float rotateSpeed)
        {
            //rotate guard face direction
            Debug.Log("Rotate " + rotateSpeed);
            this.transform.Rotate(new Vector3(0, 0, rotateSpeed));
        }

        public void MoveTowards(float dTime, Vector2 targetPos, float moveSpeed)
        {
            FaceTowards(targetPos);

            //move towards target position
            Vector2 direction = targetPos;
            direction.x -= this.transform.position.x;
            direction.y -= this.transform.position.y;
            Vector2 moveVector = direction.normalized * dTime * moveSpeed;
            Vector2 finalPos = moveVector;
            finalPos.x += this.transform.position.x;
            finalPos.y += this.transform.position.y;
            this.GetComponent<Rigidbody2D>().MovePosition(finalPos);

            //if close enough to target, count as reached
            if (Vector2.Distance(targetPos, finalPos) < 0.2f)
            {
                currentState?.ReachTarget();
            }
        }

        public bool CheckPlayerWithinSight()
        {
            //check player is within sight and angular range
            bool isInRange = Vector2.Distance(player.transform.position, this.transform.position) < sightRange;
            float sightAngle = Vector2.Angle(player.transform.position - this.transform.position, this.transform.up);

            return isInRange && (sightAngle < angleRange);
        }

        public bool CheckPlayerWithinAttackRange()
        {
            //check player is within attack range
            bool isInRange = Vector2.Distance(player.transform.position, this.transform.position) < attackRange;

            return isInRange;
        }

        #endregion movement

        #region suspicion

        public void SetEyeColour(Color eyeColour)
        {
            //set guard eye colour to show current state
            eyeCircle.color = eyeColour;
        }

        public float GetSuspicion()
        {
            //get suspicion value
            return suspicion;
        }

        public void SetSuspicion(float susValue)
        {
            //set suspicion value clamped between 0 and 1
            suspicion = Mathf.Clamp01(susValue);

            //update suspicion display
            ShowSuspicion();
        }

        public void AddSuspicionDeltaTime(float dTime)
        {
            //increase suspicion by time, clamped between 0 and 1
            suspicion = Mathf.Clamp01(suspicion + susRate * dTime);

            //update suspicion display
            ShowSuspicion();
        }

        public void ReduceSuspicionDeltaTime(float dTime)
        {
            //decrease suspicion by time, clamped between 0 and 1
            suspicion = Mathf.Clamp01(suspicion - susRate * dTime);

            //update suspicion display
            ShowSuspicion();
        }

        private void ShowSuspicion()
        {
            //change circle size to show suspicion value
            suspicionCircle.transform.localScale = new Vector2(suspicion, suspicion);
        }

        #endregion suspicion

        #region attack

        public bool CheckAttacking()
        {
            //check attack timer
            return attackTimer <= attackInterval;
        }

        public void ResetAttackTimer()
        {
            //set attack timer value
            attackTimer = attackInterval;
        }

        public void AddAttackTimer(float dTime)
        {
            //increase attack timer
            attackTimer += dTime;
        }

        public void DoAttack()
        {
            //start attack sequence
            attackTimer = 0;

            AttackStrategy attackStrat;
            attackStrat = gameObject.GetComponent<AttackStrategy>();


            //Task 5a: Do attack
            //Perform the attack assigned to the enemy by calling DoAttack function in the attached AttackStrategy.
            //You should first check if an AttackStrategy has been attached to this gameObject.
            //Task 5a START

            // You should first check if an AttackStrategy has been attached to this gameObject. 
            if (attackStrat != null)
            {
                // Perform the attack assigned to the enemy by calling
                // DoAttack function in the AttackStrategy attached to the Guard game object. 
                attackStrat.DoAttack();
            }

            //Task 5a
        }

        #endregion attack

        #region idle

        public void AddIdleCount(int count)
        {
            idleCount += count;
        }

        public void SetIdleCount(int count)
        {
            idleCount = count;
        }

        public bool CheckIsIdle()
        {
            return idleInterval > 0 && idleCount >= idleInterval;
        }

        #endregion idle

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, attackRange);

            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, sightRange);
        }
    }
}