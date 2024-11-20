using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ChangeInput : MonoBehaviour
{
    EventSystem system;
    public Selectable firstInput;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        system = EventSystem.current;
        firstInput.Select();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && Input.GetKey(KeyCode.LeftShift)) {
            Selectable next = system.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnUp();
            if (next != null) {
                next.Select();
            }
        }
        else if (Input.GetKeyDown(KeyCode.Tab)) {
            Selectable next = system.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnDown();
            if (next != null) {
                next.Select();
            }
        }
    }
}
