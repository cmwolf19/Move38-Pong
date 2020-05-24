/******************************************
 * File Name: BallBehaviour.cs
 * Author: Connor Wolf
 * Date: 5/23/2020
 * Desc:
 *      Controls the ball, including calling point functions.
 ******************************************/
using UnityEngine;

public class BallBehaviour : MonoBehaviour
{
    #region Variables
        private Rigidbody2D self;
        private AudioSource audSource;

        /// <summary>
        /// Speed at which the ball launches at. Assigned in inspector.
        /// </summary>
        [Tooltip("Initial speed of the ball.")]
        public float launchSpeed;

        /// <summary>
        /// Maximum speed of the ball. Assigned in inspector.
        /// </summary>
        [Tooltip("Maximum speed of the ball.")]
        public float topSpeed;

        /// <summary>
        /// Multiplier to apply to "bank shots". Assigned in inspector.
        /// </summary>
        [Tooltip("Bank Shot Multiplier.")]
        [Range(1,4)]
        public float wobbleMultiplier;

        /// <summary>
        /// Vertical boundaries of the ball. Assigned in inspector.
        /// </summary>
        [Tooltip("Vertical boundaries of the ball.")]
        public float verticalBounds;
    #endregion

    #region Unity Callbacks
        void Awake()
        {
            self = GetComponent<Rigidbody2D>();
            audSource = GetComponent<AudioSource>();
        }
    
        void FixedUpdate() //FixedUpdate used to ensure consistent movement at different framerates.
        {
            //Max Velocity
            if (self.velocity.magnitude > topSpeed) self.velocity = self.velocity.normalized * topSpeed; 

            //Vertical Boundaries
            if (Mathf.Abs(transform.position.y) > verticalBounds) 
            {
                transform.position = new Vector3(transform.position.x, Mathf.Sign(transform.position.y) * verticalBounds, transform.position.z);
                self.velocity = new Vector2(self.velocity.x, -self.velocity.y);
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            self.velocity = new Vector2(self.velocity.x, (transform.position.y - collision.transform.position.y) * wobbleMultiplier);
            self.velocity = self.velocity.normalized * (self.velocity.magnitude + 1);
            audSource.Play();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (transform.position.x < 0) GameManager.instance.GivePoint("RIGHT");
            else GameManager.instance.GivePoint("LEFT");
        }
    #endregion

    #region Functions
        /// <summary>
        /// Resets the ball back to its initial position.
        /// </summary>
        public void Respawn()
            {
            self.velocity = Vector3.zero;
            transform.position = Vector3.zero;
            transform.rotation = Quaternion.identity;
        }

        /// <summary>
        /// Shoots the ball in the direction of the winning player (or random in a tie).
        /// </summary> 
        public void Launch()
        {
            int launchDir = GameManager.instance.CheckScore();
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, 90 - 90 * launchDir)); //This bit of weird math saves a lot of time.
            self.velocity = transform.right * launchSpeed;
        }
    #endregion
}
