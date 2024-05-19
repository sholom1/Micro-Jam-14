using UnityEngine;

public class PictureExplosion : MonoBehaviour
{
    [SerializeField]
    private GameObject prefab;

    public void smash()
    {
        GameObject debris = Instantiate(prefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
