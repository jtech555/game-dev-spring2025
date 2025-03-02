using UnityEngine;

public class CoinCollection : MonoBehaviour
{
    private int Coin = 0;
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(Coin);
        if (other.transform.tag == "coin")
        {
            Coin++;
            Destroy(other.gameObject);
        }
    }
}
