using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

[RequireComponent(typeof(PhotonView))]
public class PhotonCullingView : MonoBehaviour, IPunObservable
{
    [SerializeField] private Rotate rotate;
    
    private PhotonView photonView;
    private PhotonTransformView photonTransformView;
    
    private byte dummyProperty = 0; // Does nothing. We use it only to trigger a synchronization on the game object when we change it in Synchronize().

    private ViewSynchronization defaultViewSynchronization;
    
    public void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            photonView = GetComponent<PhotonView>();
            photonView.Group = 255;

            defaultViewSynchronization = photonView.Synchronization;
            photonView.Synchronization = ViewSynchronization.Off;

            photonTransformView = GetComponent<PhotonTransformView>();
            if (photonTransformView)
            {
                photonTransformView.enabled = false;
            }

            rotate.enabled = false;
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(dummyProperty);
        }
        else
        {
            dummyProperty = (byte)stream.ReceiveNext();
        }
    }

    private void TriggerSynchronization()
    {
        dummyProperty++;
    }

    public void SetSynchronization(int playerNumber, bool synchronize)
    {
        if (synchronize)
        {
            photonView.Group = Convert.ToByte(photonView.Group & ~(2 << playerNumber));
            
            photonView.Synchronization = defaultViewSynchronization;
            if (photonTransformView)
            {
                photonTransformView.enabled = true;
            }

            rotate.enabled = true;

            GetComponent<Renderer>().material.color = Color.green;
            
            TriggerSynchronization();
        }
        else
        {
            photonView.Group = Convert.ToByte(photonView.Group | (2 << playerNumber));
            if (photonView.Group == 255)
            {
                photonView.Synchronization = ViewSynchronization.Off;
                if (photonTransformView)
                {
                    photonTransformView.enabled = false;
                }

                rotate.enabled = false;
                
                GetComponent<Renderer>().material.color = Color.white;
            }
        }
    }
}
