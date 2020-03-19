// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PlayerAnimatorManager.cs" company="Exit Games GmbH">
//   Part of: Photon Unity Networking Demos
// </copyright>
// <summary>
//  Used in PUN Basics Tutorial to deal with the networked player Animator Component controls.
// </summary>
// <author>developer@exitgames.com</author>
// --------------------------------------------------------------------------------------------------------------------

using UnityEngine;

namespace Photon.Pun.Demo.PunBasics
{
    public class PlayerAnimatorManager : MonoBehaviourPun
    {
        #region Private Fields

        [SerializeField]
        private float directionDampTime = 0.25f;
        Animator animator;

		Transform transform;
        #endregion
        #region Public Fields

        public int WeaponState = 0;


        #endregion

        #region MonoBehaviour CallBacks

        /// <summary>
        /// MonoBehaviour method called on GameObject by Unity during initialization phase.
        /// </summary>
        void Start()
        {
            animator = GetComponent<Animator>();

            animator.SetInteger("WeaponState", WeaponState);
            animator.SetBool("Idling", true);//stop moving
			transform = GetComponent<Transform>();
        }

        /// <summary>
        /// MonoBehaviour method called on GameObject by Unity on every frame.
        /// </summary>
        void LateUpdate()
        {

            // Prevent control is connected to Photon and represent the localPlayer
            if (photonView.IsMine == false && PhotonNetwork.IsConnected == true)
            {
                return;
            }

            // failSafe is missing Animator component on GameObject
            if (!animator)
            {
                return;
            }

            // deal with Jumping
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

            // only allow jumping if we are running.
            Vector3 Movement = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            // Debug.LogError("Horizonatal Input: " + Input.GetAxis("Horizontal"));
            // Debug.LogError("Vertical Input: " + Input.GetAxis("Vertical"));

            transform.position += Movement * 10 * Time.deltaTime;
            Debug.LogError("transform.position: " + transform.position.ToString());
        }

        #endregion

    }
}