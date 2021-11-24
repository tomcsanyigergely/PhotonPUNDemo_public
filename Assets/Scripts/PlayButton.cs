using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using WebSocketSharp;

public class PlayButton : MonoBehaviour
{
    [SerializeField] private Launcher launcher;
    [SerializeField] private Button playButton;
    [SerializeField] private TMP_InputField playerNameInputField;

    private void Start()
    {
        playButton.onClick.AddListener(() =>
        {
            string playerName = playerNameInputField.text;
            if (!playerName.IsNullOrEmpty())
            {
                PhotonNetwork.NickName = playerName;
                launcher.Connect();
            }
            else
            {
                Debug.Log("Player name is empty or null");
            }
        });
    }
}
