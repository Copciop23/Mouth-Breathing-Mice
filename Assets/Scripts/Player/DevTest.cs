using UnityEngine;
using System.Collections;
using TMPro;

public class DevTest : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI speeds;
    [SerializeField] private Rigidbody2D rb;

    void Update()
    {
        speeds.text = " x: "+rb.position.x.ToString()+"\n y: "+ rb.position.y.ToString()+"\n x vel: "+rb.linearVelocity.x.ToString() + "\n y vel: " + rb.linearVelocity.y.ToString();
    }
}
