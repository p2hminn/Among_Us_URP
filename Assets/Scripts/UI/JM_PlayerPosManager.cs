using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JM_PlayerPosManager : MonoBehaviour
{
    public static JM_PlayerPosManager instance;
    public GameObject player;
    public Image playerImage;
    public Camera cam;


    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        playerImage.transform.position = cam.WorldToScreenPoint(playerImage.transform.position);
    }
}
