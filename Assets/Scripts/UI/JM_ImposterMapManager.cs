using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JM_ImposterMapManager : MonoBehaviour
{
    public static JM_ImposterMapManager instance;

    public GameObject map;

    public Image playerImg;
    public GameObject player;
    public Color playerImgColor;

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
        playerImg.GetComponent<Image>().material.SetColor("_PlayerColor", playerImgColor);
    }

    public void OnClickCancel()
    {
        map.SetActive(false);
    }

    public void SetPlayerPos()
    {
        playerImg.GetComponent<RectTransform>().anchoredPosition = new Vector2(player.transform.position.x * 30, player.transform.position.y * 26);
    }

}
