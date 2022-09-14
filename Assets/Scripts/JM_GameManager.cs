using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class JM_GameManager : MonoBehaviour
{
    public List<Transform> spawnPosList = new List<Transform>();

    void Start()
    {
        PhotonNetwork.SerializationRate = 60;

        PhotonNetwork.SendRate = 60;

        int randomNum = Random.Range(0, 3);

        PhotonNetwork.Instantiate("Crew", spawnPosList[randomNum].position, Quaternion.identity);
    }

    void Update()
    {
        
    }
}
