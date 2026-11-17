using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PopUpController : MonoBehaviour
{
    // This is the pop up window that we will 
    public GameObject popupWindow;     // Reference to the PopupWindow panel
    public Button editButton;          // Reference to the EditButton
    public Button followupButton;      // Reference to the followupButton
    public GameObject followupWindow;  // Reference to the FollowupWindow panel

    void Start()
    {
        // Initialize the popup as hidden
        popupWindow.SetActive(false);

        followupWindow.SetActive(false);

        editButton.onClick.AddListener(ShowPopup);

        followupButton.onClick.AddListener(ShowFollowup);
    }

    void ShowFollowup()
    {
        popupWindow.SetActive(!popupWindow.activeSelf);
    }

    void ShowPopup()
    {
        popupWindow.SetActive(!popupWindow.activeSelf);
    }
}
