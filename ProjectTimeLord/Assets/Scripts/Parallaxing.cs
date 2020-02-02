using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallaxing : MonoBehaviour
{
    [SerializeField] private Transform[] allBackgrounds;
    [SerializeField] private float[] parallaxScales;

    private Transform camRef;
    private Vector3 prevCamPos;
    //private float[] parallaxScales;
    public float smoothing = 1f;

    void Awake()
    {
        camRef = Camera.main.transform;
    }

    void Start()
    {
        prevCamPos = camRef.position;
        //parallaxScales = new float[allBackgrounds.Length];

        //for (int i = 0; i < allBackgrounds.Length; i++)
        //{
        //    parallaxScales[i] = allBackgrounds[i].position.z * -1;
        //}
    }

    void Update()
    {
        for (int i = 0; i < allBackgrounds.Length; i++)
        {
            float parallax = (prevCamPos.x - camRef.position.x) * parallaxScales[i];
            float targXPosBackground = allBackgrounds[i].position.x + parallax;
            Vector3 targBackgroundPos = new Vector3(targXPosBackground, allBackgrounds[i].position.y, allBackgrounds[i].position.z);
            allBackgrounds[i].position = Vector3.Lerp(allBackgrounds[i].position, targBackgroundPos, smoothing * Time.deltaTime);
        }

        prevCamPos = camRef.position;
    }
}
