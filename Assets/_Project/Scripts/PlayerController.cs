using UnityEngine;

namespace spintap
{
    public class PlayerController : MonoBehaviour
    {
        private BoxCollider2D pointerCollider;
        [SerializeField] private GameObject gameController;
        [SerializeField] private GameObject pivotObject;
        private GameController controllerScript;
        private GameObject coin;

        private bool hit = false;
        private bool inTrigger = false;

        private float direction = 1f;
        public float rotationSpeed;
        private bool isRotatingRight = false;

        private float TRIGGER_GRACE_BUFFER = .05f;
        private float triggerGraceTimer;
        private float triggerExitGraceTimer;
        private bool inGracePeriod = false;
        private bool inExitGracePeriod = false;
        private bool playerInput = false;

        private int PERFECT_HIT_BONUS = 20;

        void Awake()
    	{
            pointerCollider = GetComponent<BoxCollider2D>();
            controllerScript = gameController.GetComponent<GameController>();
    	}

        void Update()
        {            
            // Checks for player input.
            if(Input.GetKeyDown(KeyCode.B))
            {
                playerInput = true;
            }

            #region GRACE PERIOD MANAGEMENT
            if(inExitGracePeriod && !inGracePeriod)
            {
                triggerExitGraceTimer -= Time.deltaTime;

                if(playerInput)
                {
                    hit = true;
                    updateGame();
                }
                else if(triggerExitGraceTimer < 0f)
                {
                    inExitGracePeriod = false;
                    controllerScript.endGame();
                }
            }

            if(inGracePeriod && !inExitGracePeriod) // controls the grace period timer. Checks for collision whenever the timer is > 0.
            {
                triggerGraceTimer -= Time.deltaTime;

                if(triggerGraceTimer > 0f)
                {
                    checkCollision();
                }
                else
                {
                    inGracePeriod = false;
                    controllerScript.endGame();
                }
            }
            #endregion

            // If the player made an input execute
            if(playerInput)
            {
                checkCollision(); // This only runs once for an immediate check if the pointer is in the trigger.
            }

            setRotation();
        }

        // Checks if the player's pointer is in the trigger. 
        // If so, reset values and continue the game.
        // If not, and the player is not in a grace period, start the timer for the grace period.
        private void checkCollision()
        {
            if(inTrigger)
            {
                hit = true;
                updateGame();
            }
            else if(!inGracePeriod)
            {
                inGracePeriod = true;
                playerInput = false;
                triggerGraceTimer = TRIGGER_GRACE_BUFFER;
            }
        }

        private void updateGame()
        {
            addCoin();
            changeDirection();

            if(!inExitGracePeriod && !inGracePeriod) // If no grace period was used, grant player a bonus for a perfect hit
                controllerScript.updateScore(PERFECT_HIT_BONUS);
            else // If the player was in a grace period, grant no bonus.
                controllerScript.updateScore(0);

            // resets values
            hit = false;
            playerInput = false;
            inGracePeriod = false;
            inExitGracePeriod = false;
            triggerGraceTimer = -1f;
            triggerExitGraceTimer = -1f;
        }

        // Rotates the pointer around the pivot point.
        private void setRotation()
        {
            transform.RotateAround(pivotObject.transform.position, new Vector3(0, 0, direction), rotationSpeed * Time.deltaTime);
        }

        // Changes the direction of rotation
        private void changeDirection()
        {
            if(!isRotatingRight)
            {
                direction = -1f;
            }
            else
            {
                direction = 1f;
            }
            isRotatingRight = !isRotatingRight;

            rotationSpeed += 5f;
        }

        // Destroys the current coin and adds another one.
        private void addCoin()
        {
            coin.GetComponent<CoinController>().destroyCoin();
            controllerScript.spawnCoin();
        }

        void OnTriggerEnter2D(Collider2D col)
        {
            coin = col?.gameObject;
            inTrigger = true;
        }

        void OnTriggerExit2D(Collider2D col)
        {
            inTrigger = false;

            if(!hit)
            {
                inExitGracePeriod = true;
                triggerExitGraceTimer = TRIGGER_GRACE_BUFFER;
            }
        }
    }
}