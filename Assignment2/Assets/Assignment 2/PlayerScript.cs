using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assignment2
{
    public class PlayerScript : MonoBehaviour
    {
        private LevelController levelController;

        public Transform initialPosition;
        public float speed = 6f;
        public int initHealth = 5;
        public Transform healthBar;

        private int currHealth;

        public void MovePlayer(Vector2 moveDir)
        {
            //set player direction
            if (moveDir.magnitude > 0) this.transform.up = moveDir.normalized;

            //move player position
            this.transform.position += (Vector3)moveDir * speed;
        }

        public void Initialize(LevelController aController)
        {
            levelController = aController;

            //set to initial position
            this.transform.position = initialPosition.transform.position;

            currHealth = initHealth;

            UpdateHealthDisplay();
        }

        public void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.GetComponent<DamagerScript>() != null)
            {
                TakeDamage(1);
                collision.gameObject.GetComponent<DamagerScript>().DoOnHit();
            }

            //detect collectible
            if (collision.gameObject.GetComponent<CollectibleScript>() != null && !levelController.CheckGameOver() && levelController.CheckIsStarted())
            {
                levelController.AddCollectedCount(1);
                collision.gameObject.GetComponent<CollectibleScript>().DoCollect();
            }

            //detect end point
            if (collision.gameObject.GetComponent<EndPointScript>() != null && !levelController.CheckGameOver())
                levelController.SetGameOver(true, true);
        }

        private void UpdateHealthDisplay()
        {
            float healthPercent = (float)currHealth / (float)initHealth;
            healthBar.localScale = new Vector3(healthPercent, healthPercent, 1f);
        }

        private void TakeDamage (int dmg)
        {
            currHealth -= dmg;
            UpdateHealthDisplay();

            if (currHealth <= 0)
            {
                levelController.SetGameOver(true, false);
            }
        }
    }
}