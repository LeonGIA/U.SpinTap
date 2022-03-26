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

        private float TRIGGER_GRACE_BUFFER = 1f;
        private float triggerGraceTimer;
        private float triggerExitTime;
        private bool inGracePeriod = false;

        void Awake()
    	{
            pointerCollider = GetComponent<BoxCollider2D>();
            controllerScript = gameController.GetComponent<GameController>();
    	}

        void Update()
        {            
            // if(Input.anyKeyDown && !inGracePeriod)
            // {
            //     if(inTrigger)
            //     {
            //         hit = true;
            //         if(coin != null)
            //         {
            //             addCoin();
            //         }
            //     }
            //     else {
            //         triggerGraceTimer = TRIGGER_GRACE_BUFFER;
            //         inGracePeriod = true;
            //     }
            // }
            // else if(triggerGraceTimer > 0f)
            // {
            //     triggerGraceTimer -= Time.deltaTime;
            // }

            // if(inTrigger && inGracePeriod && triggerGraceTimer > 0f)
            // {
            //     hit = true;
            //     if(coin != null)
            //     {
            //         addCoin();
            //     }
            // }
            // else
            // {
            //     Time.timeScale = 0f;
            // }

            if(inTrigger)
            {
                if(Input.anyKeyDown || inGracePeriod)
                {
                    hit = true;
                    if(coin != null)
                    {
                        addCoin();
                        controllerScript.updateScore();
                    }
                    inGracePeriod = false;
                }
            }
            else if(Input.anyKeyDown)
            {
                controllerScript.endGame();
            }

            setRotation();
        }

        private void setRotation()
        {
            transform.RotateAround(pivotObject.transform.position, new Vector3(0, 0, direction), rotationSpeed * Time.deltaTime);
        }

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

        private void addCoin()
        {
            coin.GetComponent<CoinController>().destroyCoin();
            controllerScript.spawnCoin();
            
            changeDirection();
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
                controllerScript.endGame();
            }
            hit = false;
        }
    }
}
