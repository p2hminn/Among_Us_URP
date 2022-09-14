using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SH_RoomInfo : MonoBehaviour
{
    public Text roomInfo;
    public void SetInfo(string roomName, int currPlayer, byte maxPlayer)
    {
        //roomInfo.text = $"{roomName} / {nickName} / imposter = {imposterNum} ( {currPlayer} / {maxPlayer} ) ";
        roomInfo.text = $"{roomName} ( {currPlayer} / {maxPlayer} ) ";
    }
}
