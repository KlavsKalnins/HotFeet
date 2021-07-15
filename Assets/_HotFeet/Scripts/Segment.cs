using System.Collections;
using UnityEngine;

public class Segment : MonoBehaviour
{
    [SerializeField] private BoxCollider col;
    private void OnEnable()
    {
        StartCoroutine(Init());
    }

    IEnumerator Init()
    {
        yield return new WaitForSeconds(0.5f);
        col.enabled = true;
    }

    private void OnDisable() => col.enabled = false;
}