using System;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviourPunCallbacks
{
    public static GameManager Instance;

    [SerializeField] private GameObject playerPrefab;
    
    void Start()
    {
        Instance = this;
        
        if (playerPrefab == null)
        {
            Debug.LogError("<Color=Red><a>Missing</a></Color> playerPrefab Reference. Please set it up in GameObject 'Game Manager'",this);
        }
        else
        {
            if (PlayerManager.LocalPlayerInstance == null)
            {
                Debug.LogFormat("We are Instantiating LocalPlayer from {0}", SceneManagerHelper.ActiveSceneName);
                // we're in a room. spawn a character for the local player. it gets synced by using PhotonNetwork.Instantiate
                PhotonNetwork.Instantiate(this.playerPrefab.name, new Vector3(0f, 5f, 0f), Quaternion.identity, 0);
            }
            else
            {
                Debug.LogFormat("Ignoring scene load for {0}", SceneManagerHelper.ActiveSceneName);
            }
        }

        byte[] enabledInterestGroups = new byte[127]; // size is 128-1 = 127 because interest group 0 is always enabled for all players
        byte nextEnabledInterestGroupIndex = 0;
        
        byte[] disabledInterestGroups = new byte[128];
        byte nextDisabledInterestGroupIndex = 0;
        
        Debug.Log("Local player actor number is: " + PhotonNetwork.LocalPlayer.ActorNumber);

        byte disabledInterestGroupMask = Convert.ToByte(2 << PhotonNetwork.LocalPlayer.ActorNumber); // todo: replace actor number with player number < 8

        // skipping interestGroupNumber 0 because it is the broadcast group, it is always enabled for all players
        for (int interestGroupNumber = 1; interestGroupNumber < 256; interestGroupNumber++)
        {
            if ((interestGroupNumber & disabledInterestGroupMask) == disabledInterestGroupMask)
            {
                disabledInterestGroups[nextDisabledInterestGroupIndex] = Convert.ToByte(interestGroupNumber);
                nextDisabledInterestGroupIndex++;
            }
            else
            {
                enabledInterestGroups[nextEnabledInterestGroupIndex] = Convert.ToByte(interestGroupNumber);
                nextEnabledInterestGroupIndex++;
            }
        }
        
        PhotonNetwork.SetInterestGroups(disabledInterestGroups, enabledInterestGroups);
    }
    
    public override void OnLeftRoom()
    {
        Debug.Log("Room left.");
        
        SceneManager.LoadScene("MainMenu");
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    private void LoadArena()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            Debug.LogFormat("PhotonNetwork : Loading Level : {0}", PhotonNetwork.CurrentRoom.PlayerCount);
            PhotonNetwork.LoadLevel("Room for " + PhotonNetwork.CurrentRoom.PlayerCount);
        }
        else
        {
            Debug.LogError("PhotonNetwork : Trying to Load a level but we are not the master Client");
        }
    }
    
    public override void OnPlayerEnteredRoom(Player other)
    {
        Debug.LogFormat("OnPlayerEnteredRoom() {0}", other.NickName); // not seen if you're the player connecting


        if (PhotonNetwork.IsMasterClient)
        {
            Debug.LogFormat("OnPlayerEnteredRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient); // called before OnPlayerLeftRoom


            //LoadArena();
        }
    }
    
    public override void OnPlayerLeftRoom(Player other)
    {
        Debug.LogFormat("OnPlayerLeftRoom() {0}", other.NickName); // seen when other disconnects


        if (PhotonNetwork.IsMasterClient)
        {
            Debug.LogFormat("OnPlayerLeftRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient); // called before OnPlayerLeftRoom


            //LoadArena();
        }
    }
}

