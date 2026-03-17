using TMPro;
using UnityEngine;

public class SimpleName : MonoBehaviour
{
    public TMP_InputField angleInputField;
    public GameObject spherePrfab;

    public Transform firePoint;
    public float force = 15;

    public void Launch()
    {

        float angle = float.Parse(angleInputField.text);
        float rad = angle * Mathf.Deg2Rad;

        Vector3 dir = new Vector3(Mathf.Cos(rad), 0f, Mathf.Sin(rad));

        GameObject sphere = Instantiate(spherePrfab, firePoint.position, Quaternion.identity);
        Rigidbody rb = sphere.GetComponent<Rigidbody>();

        rb.AddForce((dir + Vector3.up * .3f).normalized * force, ForceMode.Impulse); ;
    }
}
    

