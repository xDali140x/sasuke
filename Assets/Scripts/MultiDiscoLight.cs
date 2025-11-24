using UnityEngine;

public class MultiDiscoLightRandom : MonoBehaviour
{
    public Light[] luces;
    public float speed = 2f;

    void Update()
    {
        for (int i = 0; i < luces.Length; i++)
        {
            if (luces[i] != null)
            {
                float offset = i * 0.3f;  // desincroniza cada luz
                float t = Mathf.PingPong((Time.time + offset) * speed, 1f);
                luces[i].color = Color.HSVToRGB(t, 1f, 1f);
            }
        }
    }
}
