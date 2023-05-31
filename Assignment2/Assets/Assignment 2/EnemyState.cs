using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assignment2
{
    public abstract class EnemyState
    {
        protected EnemyScript enemyScript;

        public EnemyState(EnemyScript enemyScript)
        {
            //store all fields
            this.enemyScript = enemyScript;
        }

        public abstract void DoActionUpdate(float dTime);

        public abstract void ReachTarget();
    }
    public class EnemyStatePatrol : EnemyState
    {
        public EnemyStatePatrol(EnemyScript enemyScript) : base(enemyScript)
        {
            //enter patrol state with no suspicion 
            enemyScript.SetSuspicion(0);

            //set appearance
            enemyScript.SetEyeColour(Color.blue);
        }

        public override void DoActionUpdate(float dTime)
        {
            //move to next waypoint
            enemyScript.MoveTowards(dTime, enemyScript.GetNextWaypoint(), enemyScript.moveSpeed);

            //Task 4a: Patrol State Transitions 
            //Check if the player is within the enemy's sight, using CheckPlayerWithinSight in enemyScript.
            //If player is within range, set current state to EnemyStateAlert.
            //If the player is NOT within range, check if the guard is due for idle state 
            //using CheckIsIdle in enemyScript and change to EnemyStateIdle state if true. 
            //When changing states, you can pass enemyScript as an input parameter.
            //Task 4a START

            //Task 4a END
        }

        public override void ReachTarget()
        {
            //reached target position
            enemyScript.SetToNextWaypoint();

            enemyScript.AddIdleCount(1);
        }
    }

    public class EnemyStateAlert : EnemyState
    {
        public EnemyStateAlert(EnemyScript enemyScript) : base(enemyScript)
        {
            //enter alert state with full suspicion 
            enemyScript.SetSuspicion(1f);

            //set appearance
            enemyScript.SetEyeColour(Color.yellow);
        }

        public override void DoActionUpdate(float dTime)
        {
            //check if player is within sight
            if (enemyScript.CheckPlayerWithinSight())
            {
                Vector2 targetPosition = enemyScript.GetPlayerPosition();

                //set player last seen position
                enemyScript.SetPlayerLastSeenPos(targetPosition);

                //if player still within range, move towards player
                enemyScript.MoveTowards(dTime, targetPosition, enemyScript.chaseSpeed);
            }
            else
            {
                //else move towards last seen position
                enemyScript.MoveTowards(dTime, enemyScript.GetPlayerLastSeenPos(), enemyScript.chaseSpeed);
            }

            //Task 4b: Alert State Transitions
            //When the distance between the target position and the guard's position is within farRange, stay in the current state.
            //Otherwise, transition to EnemyStateSuspicious state.
            //Task 4b START

            //Task 4b END
        }

        public override void ReachTarget()
        {
            //reached last seen position, go to suspicious state
            enemyScript.SetCurrentState(new EnemyStateSuspicious(enemyScript));
        }
    }

    public class EnemyStateAttack : EnemyState
    {
        public EnemyStateAttack(EnemyScript enemyScript) : base(enemyScript)
        {
            //enter alert state with full suspicion 
            enemyScript.SetSuspicion(1f);

            //set appearance
            enemyScript.SetEyeColour(Color.red);
        }

        public override void DoActionUpdate(float dTime)
        {
            //check if enemy is currently attacking
            bool isAttacking = enemyScript.CheckAttacking();
            if (!isAttacking)
            {
                //when enemy is not attacking, check if player is within attack range
                if (!enemyScript.CheckPlayerWithinAttackRange())
                {
                    //if not in range, chase
                    enemyScript.SetCurrentState(new EnemyStateAlert(enemyScript));
                }
                else
                {
                    //if in range, attack
                    Vector2 targetPosition = enemyScript.GetPlayerPosition();

                    //set player last seen position
                    enemyScript.SetPlayerLastSeenPos(targetPosition);

                    //face the player
                    enemyScript.FaceTowards(targetPosition);

                    //do attack
                    enemyScript.DoAttack();
                }
            }
        }

        public override void ReachTarget()
        {

        }
    }

    public class EnemyStateSuspicious : EnemyState
    {
        float lookDir = 0;

        public EnemyStateSuspicious(EnemyScript enemyScript) : base(enemyScript)
        {
            //enter suspicious state set appearance
            enemyScript.SetEyeColour(Color.magenta);

            lookDir = 0;
        }

        public override void DoActionUpdate(float dTime)
        {
            //Task 4c: Suspicious State Transitions
            //Check if the player is within sight or attack range, 
            //using CheckPlayerWithinSight and CheckPlayerWithinAttackRange in enemyScript.
            //If true, go to Alert state.
            //Otherwise, the enemy should rotate in a random direction on the spot
            //until suspicion reaches or goes below 0, at which point it returns to Patrol state.
            //Current suspicion value can be obtained using GetSuspicion function in enemyScript.
            //Task 4c START

            //randomly select look direction
            if (lookDir == 0) lookDir = (Random.Range(0, 1f) > 0.5f) ? 1f : -1f;

            //rotate on the spot
            enemyScript.Rotate(dTime * enemyScript.lookAroundSpeed * lookDir);

            //reduce suspicion
            enemyScript.ReduceSuspicionDeltaTime(dTime);

            //Task 4c END
        }

        public override void ReachTarget()
        {

        }
    }

    //Task 3: Make EnemyStateIdle state
    //Make an Idle state for the Guards. Eye colour should be set to black and
    //idle count in enemyScript will need to be reset to 0 when entering the state.
    //Guards will stay in idle state until a timer reaches or exceeds the specified idleTime in enemyScript, 
    //after which they will change to Patrol state.
    //Task 3 START

    //Task 3 END
}