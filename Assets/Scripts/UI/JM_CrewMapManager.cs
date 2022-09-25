using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class JM_CrewMapManager : MonoBehaviourPun
{
    public static JM_CrewMapManager instance;

    public GameObject crewMapUI;

    public GameObject unlockManifold;
    public GameObject startReactor;
    public GameObject swipeCard;
    public GameObject cleanO2Filter;
    public GameObject calibrateDistributor;
    public GameObject fuelst1;
    public GameObject fuelst2_lower;
    public GameObject fuelst2_upper;
    public GameObject primeShield;
    public GameObject upload_cafeteria;
    public GameObject upload_weapons;
    public GameObject upload_navigation;
    public GameObject upload_communications;
    public GameObject upload_electrics;
    public GameObject upload_admin;
    public GameObject uploadst2_admin;

    private void Awake()
    {
        instance = this;
    }

    public void UnlockManifold()
    {
        photonView.RPC("RPC_UnlockManifold", RpcTarget.All);
    }

    [PunRPC]
    void RPC_UnlockManifold()
    {
        unlockManifold.SetActive(false);
    }

    public void StartReactor()
    {
        photonView.RPC("RPC_StartReactor", RpcTarget.All);
    }

    [PunRPC]
    void RPC_StartReactor()
    {
        startReactor.SetActive(false);
    }

    public void SwipeCard()
    {
        photonView.RPC("RPC_SwipeCard", RpcTarget.All);
    }

    [PunRPC]
    void RPC_SwipeCard()
    {
        swipeCard.SetActive(false);
    }

    public void CleanO2Filter()
    {
        photonView.RPC("RPC_CleanO2Filter", RpcTarget.All);
    }

    [PunRPC]
    void RPC_CleanO2Filter()
    {
        cleanO2Filter.SetActive(false);
    }

    public void CalibrateDistributor()
    {
        photonView.RPC("RPc_CalibrateDistributor", RpcTarget.All);
    }


    [PunRPC]
    void RPc_CalibrateDistributor()
    {
        calibrateDistributor.SetActive(false);
    }

    public void FuelSt1()
    {
        photonView.RPC("RPC_FuelSt1", RpcTarget.All);
    }

    [PunRPC]
    void RPC_FuelSt1()
    {
        fuelst1.SetActive(false);
    }

    public void FuelSt2Lower()
    {
        photonView.RPC("RPC_FuelSt2Lower", RpcTarget.All);
    }

    [PunRPC]
    void RPC_FuelSt2Lower()
    {
        fuelst2_lower.SetActive(false);
    }

    public void FuelSt2Upper()
    {
        photonView.RPC("RPC_FuelSt2Upper", RpcTarget.All);
    }

    [PunRPC]
    void RPC_FuelSt2Upper()
    {
        fuelst2_upper.SetActive(false);
    }

    public void PrimeShield()
    {
        photonView.RPC("RPC_PrimeShield", RpcTarget.All);
    }
    
    [PunRPC]
    void RPC_PrimeShield()
    {
        primeShield.SetActive(false);
    }

    public void UploadCafeteria()
    {
        photonView.RPC("RPC_UploadCafeteria", RpcTarget.All);
    }

    [PunRPC]
    void RPC_UploadCafeteria()
    {
        upload_cafeteria.SetActive(false);
    }

    public void UploadWeapons()
    {
        photonView.RPC("RPC_UploadWeapons", RpcTarget.All);
    }

    [PunRPC]
    void RPC_UploadWeapons()
    {
        upload_weapons.SetActive(false);
    }

    public void UploadNavigation()
    {
        photonView.RPC("RPC_UploadNavigation", RpcTarget.All);
    }

    [PunRPC]
    void RPC_UploadNavigation()
    {
        upload_navigation.SetActive(false);
    }

    public void UploadCommunications()
    {
        photonView.RPC("RPC_UploadCommunications", RpcTarget.All);
    }

    [PunRPC]
    void RPC_UploadCommunications()
    {
        upload_communications.SetActive(false);
    }

    public void UploadElectrics()
    {
        photonView.RPC("RPC_UploadElectrics", RpcTarget.All);
    }

    [PunRPC]
    void RPC_UploadElectrics()
    {
        upload_electrics.SetActive(false);
    }

    public void UploadAdmin()
    {
        photonView.RPC("RPC_UploadAdmin", RpcTarget.All);
    }

    [PunRPC]
    void RPC_UploadAdmin()
    {
        upload_admin.SetActive(false);
    }

    public void UploadSt2Admin()
    {
        photonView.RPC("RPC_UploadSt2Admin", RpcTarget.All);
    }

    [PunRPC]
    void RPC_UploadSt2Admin()
    {
        uploadst2_admin.SetActive(false);
    }

    public void OnClickCancel()
    {
        crewMapUI.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
