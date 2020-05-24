/******************************************
 * File Name: PaddleBehaviour.cs
 * Author: Connor Wolf
 * Date: 5/23/2020
 * Desc:
 *      Controls the paddles within the game.
 * 
 ******************************************/ 
using UnityEngine;

public class PaddleBehaviour : MonoBehaviour
{
    #region Variables
    /// <summary>
    /// Name of the axis to use for movement. Assigned in inspector. Accessible in Project Settings.
    /// </summary>
    [Tooltip("Axis used for movement.")]
    public string axisName;

    /// <summary>
    /// Speed at which the paddle travels. Assigned in inspector.
    /// </summary>
    [Tooltip("Speed at which the paddle travels.")]
    public float paddleSpeed;

    /// <summary>
    /// Vertical boundaries of the paddle. Assigned in inspector.
    /// </summary>
    [Tooltip("Vertical boundaries of the paddle.")]
    public float verticalBounds;
    #endregion

    #region Unity Callbacks  
    void FixedUpdate() //FixedUpdate used to ensure consistent movement at different framerates.
    {
        transform.Translate(Vector3.down * Input.GetAxis(axisName) * paddleSpeed);

        //Checks the boundaries of the paddle and enforces them.
        if (Mathf.Abs(transform.position.y) > verticalBounds)
        {
            float newY = Mathf.Sign(transform.position.y) * verticalBounds;
            transform.position = new Vector3(transform.position.x, newY, transform.position.z);
        }
    }
    #endregion
}
