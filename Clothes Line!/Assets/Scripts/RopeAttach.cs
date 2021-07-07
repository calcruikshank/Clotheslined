using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Obi;

public class RopeAttach : MonoBehaviour
{
    [SerializeField] ObiParticleAttachment attachment1, attachment2, attachment3, attachment4;
    [SerializeField] Transform startRope;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Attach(int playerID, Transform player)
    {
        if (playerID == 0)
        {
            attachment1.target = player;
        }
        if (playerID == 1)
        {
            attachment2.target = player;
        }
    }
    public void Detach()
    {
        attachment1.enabled = false;
        attachment2.enabled = false;
    }
}
