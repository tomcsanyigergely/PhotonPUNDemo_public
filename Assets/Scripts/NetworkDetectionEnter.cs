using System;
using Photon.Pun;
using UnityEngine;
using Random = System.Random;

public class NetworkDetectionEnter : MonoBehaviour
{
    [SerializeField] private PhotonView photonView;
    
    private void OnTriggerEnter(Collider other)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonCullingView otherCullingView = other.gameObject.GetComponent<PhotonCullingView>();
            if (otherCullingView && other.GetComponent<PhotonView>() != photonView)
            {
                otherCullingView.SetSynchronization(photonView.Owner.ActorNumber, true);
            }
        }
    }
}
