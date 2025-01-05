using UnityEngine;

public class DotProductTest : MonoBehaviour
{
    [SerializeField] private Vector3 vectorA;
    [SerializeField] private Vector3 vectorB;

    [SerializeField] private float dotProduct;

    private void Update()
    {
        dotProduct = Vector3.Dot(vectorA.normalized, vectorB.normalized);
    }
}
