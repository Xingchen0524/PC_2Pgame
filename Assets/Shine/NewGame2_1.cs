using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class NewGame2_1 : MonoBehaviour
{
    public Vector3 spawnPosition;
    // Start is called before the first frame update
    void Start()
    {
        if (PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("Role"))
        {
            string role = (string)PhotonNetwork.LocalPlayer.CustomProperties["Role"];
            if (role == "©j©j")
            {
                PhotonNetwork.Instantiate("PlayerPrefab", spawnPosition, Quaternion.identity);
            }
        }
    }
   
    // Update is called once per frame
    void Update()
    {
        
    }
}
