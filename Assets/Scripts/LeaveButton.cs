using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeaveButton : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private Button leaveButton;

    private void Start()
    {
        leaveButton.onClick.AddListener(() =>
        {
            gameManager.LeaveRoom();
        });
    }
}
