

using UnityEngine;

public class Ball : MonoBehaviour
{
    //足球是否進入球門
    public static bool complete;

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "進球感應區")
        {
            complete = true;
        }
    }
   
}
