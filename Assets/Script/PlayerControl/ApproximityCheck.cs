using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApproximityCheck : MonoBehaviour
{
    public static ApproximityCheck P1Instance;
    public static ApproximityCheck P2Instance;

    Player For;
    private void Awake()
    {
        if (transform.parent.name == "Player1")
        {
            For = Player.First;
            if (P1Instance is null)
                P1Instance = this;
            else
                Destroy(gameObject);
        }
        else if (transform.parent.name == "Player2")
        {
            For = Player.Second;
            if (P2Instance is null)
                P2Instance = this;
            else
                Destroy(gameObject);
        }
    }

    public bool WithinRange;
    private void OnTriggerEnter(Collider other)
    {
        WithinRange = (For == Player.First && other.name == "Player2") ||
            (For == Player.Second) && other.name == "Player1";
    }

    private void OnTriggerExit(Collider other)
    {
        if (For == Player.First && other.name == "Player2")
            WithinRange = false;
        if (For == Player.Second && other.name == "Player1")
            WithinRange = false;
    }
}
