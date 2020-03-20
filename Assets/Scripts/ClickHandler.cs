using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Pun.Demo.PunBasics;
public class ClickHandler : MonoBehaviourPunCallbacks
{
    GameObject player;
    TargetingController targetingController;

    // Start is called before the first frame update
    void Start()
    {
        if (photonView.IsMine)
        {
            player = gameObject;
            targetingController = player.GetComponent<TargetingController>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        checkForClicks();
    }

    void checkForClicks()
    {
        if (Input.GetMouseButtonDown(0))
        {
            LeftClick();
        }
        // If the right mouse button is clicked anywhere...
        else if (Input.GetMouseButtonDown(1))
        {
            Debug.Log("Right Clicked");
        }
    }
    void LeftClick()
    {
        Debug.Log("Left Clicked");

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit[] hits = Physics.RaycastAll(ray);
        if (hits.Length != 0)
        {
            Debug.Log("Target hits found");

            ChooseTargetWithClick(hits);
        }

    }

    public Vector2 getMousePosition()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
    void ChooseTargetWithClick(RaycastHit[] hits)
    {
        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.gameObject.GetComponent<PlayerManager>() != null)
            {
                targetingController.target = hit.collider.gameObject;
                Debug.Log("Target set");

            };

        }
    }
}
