using UnityEngine;
using UnityEngine.UIElements;

public class ButtonHandler : MonoBehaviour
{
    [SerializeField] GameObject popUp;
    public void OpenPopUp()
    {
        popUp.SetActive(true);
    }

    public void closePopUp()
    {
        popUp.SetActive(false);
    }
}
