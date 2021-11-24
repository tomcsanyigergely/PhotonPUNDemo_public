using Photon.Pun;
using UnityEngine;

public class NetworkDetectionExit : MonoBehaviour
{
    [SerializeField] private PhotonView photonView;

    private void OnTriggerExit(Collider other)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonCullingView otherCullingView = other.gameObject.GetComponent<PhotonCullingView>();
            if (otherCullingView && other.GetComponent<PhotonView>() != photonView)
            {
                otherCullingView.SetSynchronization(photonView.Owner.ActorNumber, false);
            }
        }
    }
}
