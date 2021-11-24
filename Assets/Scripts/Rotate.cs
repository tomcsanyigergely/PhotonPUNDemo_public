using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            transform.localRotation = Quaternion.Euler(Time.time*180.0f, Time.time*180.0f, Time.time*180.0f);
        }
    }
}
