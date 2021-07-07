using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootRaycast : MonoBehaviour
{
    PlayerController otherPlayer;
    GameManager gameManager;
    Vector3 collision = Vector3.zero;
    GameObject lastHit;
    ZombieCollision zombie;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        foreach(PlayerController player in gameManager.GetPlayersList())
        {
            if (this.gameObject.GetComponent<PlayerController>() != player)
            {
                otherPlayer = player;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.gameOver)
        {
            return;
        }
        if (otherPlayer == null)
        {
            foreach (PlayerController player in gameManager.GetPlayersList())
            {
                if (this.gameObject.GetComponent<PlayerController>() != player)
                {
                    otherPlayer = player;
                }
            }
            return;
        }
        Vector3 targetDirection = otherPlayer.transform.position - this.transform.position;
        var ray = new Ray(this.transform.position, targetDirection);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, targetDirection.magnitude))
        {
            lastHit = hit.transform.gameObject;
            collision = hit.point;

            //Debug.Log(lastHit);
        }

        RaycastHit[] rayHits = Physics.RaycastAll(this.transform.position, targetDirection, (this.transform.position - otherPlayer.transform.position).magnitude);
        foreach (RaycastHit rayHit in rayHits)
        {
            zombie = rayHit.transform.gameObject.GetComponent<ZombieCollision>();
            if (zombie != null)
            {
                zombie.ZombieDeath();
            }
        }
    }

}
