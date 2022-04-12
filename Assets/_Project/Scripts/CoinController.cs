using UnityEngine;

namespace spintap
{
    public class CoinController : MonoBehaviour
    {
        private CircleCollider2D coinCollider;

        void Awake()
        {
            coinCollider = GetComponent<CircleCollider2D>();
        }

        public void destroyCoin()
        {
            Destroy(this.gameObject);
        }
    }
}
