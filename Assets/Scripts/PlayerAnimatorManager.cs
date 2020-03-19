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
        private float distToGround;
        private CapsuleCollider collider;
        private Vector3 Movement;
        private PlayerManager playerManager;
        Animator animator;

        Transform transform;
        #endregion
        #region Public Fields
        public float speed;
        public bool jumping = false;
        public bool falling = false;
        public float fallspeed = 0.0f;
        public float fallForwardSpeed = 0.3f;

        public int WeaponState = 0;

        #endregion

        #region MonoBehaviour CallBacks

        /// <summary>
        /// MonoBehaviour method called on GameObject by Unity during initialization phase.
        /// </summary>
        void Start()
        {
            playerManager = GetComponent<PlayerManager>();

            animator = GetComponent<Animator>();
            collider = GetComponent<CapsuleCollider>();
            distToGround = 0.03f;

            animator.SetInteger("WeaponState", WeaponState);
            animator.SetBool("Idling", true);//stop moving
            transform = GetComponent<Transform>();
        }

        /// <summary>
        /// MonoBehaviour method called on GameObject by Unity on every frame.
        /// </summary>

        void Update()
        {
            DetectGround();
            // deal with Jumping
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

            // only allow jumping if we are running.
            Movement = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
			Movement = Quaternion.Euler(0, playerManager.globalRotationY, 0) * Movement;
            // Debug.LogError("Horizonatal Input: " + Input.GetAxis("Horizontal"));
            // Debug.LogError("Vertical Input: " + Input.GetAxis("Vertical"));
            if (Movement.x != 0 || Movement.z != 0)
            {
                animator.SetBool("isRunning", true);
                transform.rotation = Quaternion.LookRotation(Movement);
            }
            else
            {
                animator.SetBool("isRunning", false);

            }
            if (!DetectCollision(Movement))
            {

                transform.position += Movement * speed * Time.deltaTime;
            }
            else
            {
                // transform.position += -Movement * speed * Time.deltaTime;

            }
            if (Input.GetKey("space"))
            {
                animator.SetTrigger("New Trigger");
            }
        }
        void LateUpdate()
        {
            animator.SetInteger("WeaponState", WeaponState);

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



        }


        #endregion
        #region Private Methods
        void DetectGround()
        {
            //////////////////////////////

            //GROUND DETECTION

            //////////////////////////////
            // ray.direction = transform.up * -1;//make a downward ray, negative "up" is "down"
            // ray.origin = transform.position + transform.up;//one meter up from our feet
            // bool foundOne = false;
            // float closest = 210.0f;
            // RaycastHit target = new RaycastHit();
            // for (int i = 0; i < game.allZones.Length; i++)
            // {
            //     for (int f = 0; f < game.allZones[i].floors.Count; f++)
            //     {
            //         //RUNGY, change from ALLZONES to zone[currentZone].zones, should only be checking zone connected data
            //         //If a floor is the closest...
            //         if (game.allZones[i].floors[f].Raycast(ray, out hit, 500))
            //         {
            //             float dist = Vector3.Distance(hit.point, ray.origin);
            //             if (!foundOne)
            //             {
            //                 //we are going to take the first floor we check so that we at least have one
            //                 target = hit;
            //                 floorPlane = game.allZones[i].floors[f];
            //                 closest = dist;
            //                 foundOne = true;
            //             }

            //             if (dist <= closest)
            //             {
            //                 closest = dist;
            //                 target = hit;
            //                 floorPlane = game.allZones[i].floors[f];
            //                 //RUNGY...need to check for PLATFORMS HERE BADLY
            //                 //AND SETPLATFORM
            //             }
            //         }
            //     }
            // }


            //use two ground checks...one higher...the other lower
            // if (floorPlane.Raycast(ray, out hit, 1.0f))
            // {
            //     //always hit if we are going up
            //     if (hit.point.y > (transform.position.y + 0.02f))
            //     {
            //         transform.position = hit.point;
            //         foundFloor = true;//RE-using foundFloor bool, to represent "ON FLOOR" status
            //     }
            //     //angler.up=hit.normal;
            // }
            // else if (floorPlane.Raycast(ray, out hit, 1.2f))
            // {
            //     if (!jumping)
            //     {
            //         // lower hit check for going down ramps
            //         if (hit.point.y < (transform.position.y - 0.02f))
            //         {
            //             transform.position = hit.point;
            //             foundFloor = true;//RE-using foundFloor bool, to represent "ON FLOOR" status
            //         }
            //         //angler.up=hit.normal;
            //     }
            // }
            // else 
            if (!jumping && !IsGrounded())//we have no ground contact, and if we're not jumping, then we are falling
            {
                //Falling
                if (falling == false)
                {
                    animator.SetBool("Falling", true);//start falling animation
                    fallForwardSpeed = 0.03f;
                }
                animator.SetBool("Idling", true);
                falling = true;
                transform.parent = null;
                fallspeed += 0.3f;
                Vector3 v = new Vector3(0.0f, fallspeed * Time.deltaTime, 0.0f);
                fallForwardSpeed = fallForwardSpeed * (1.0f - (0.9f * Time.deltaTime)); //this will diminish the forward speed, but never negate it
                                                                                        //v += -(transform.forward * fallForwardSpeed);
                transform.position -= v;
                transform.position += transform.forward * fallForwardSpeed;

                // lookAtPos = transform.position + transform.forward * 0.3f;
                // movementTargetPosition = transform.position + transform.forward * 0.3f;
            }

            if (IsGrounded() == true)
            {
                fallspeed = 0.0f;
                if (falling == true)//we currently have ground contact, turn off falling if we were falling
                {
                    animator.SetBool("Falling", false);//start falling animation
                    falling = false;
                }
                // if (floorPlane.name.ToLower().Contains("platform"))
                // {
                //     transform.parent = floorPlane.transform;
                //     //movementTarget.transform.parent=floorPlane.transform;
                // }
                // else
                // {
                //     transform.parent = null;
                //     //movementTarget.transform.parent=null;
                // }
            }

            if (transform.position.y < 0)
            {
                transform.position = new Vector3(transform.position.x, .2f, transform.position.z);
            }
        }

        bool IsGrounded()
        {
            bool isGrounded = Physics.Raycast(transform.position, -Vector3.up, distToGround);
            return isGrounded;
        }

        bool DetectCollision(Vector3 movement)
        {
            bool isColliding = Physics.Raycast(transform.position, movement, 1f);
            return isColliding;
        }

        #endregion


    }
}