using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearEffect : MonoBehaviour
{
    [SerializeField] ParticleSystem particleSystemL;
    [SerializeField] ParticleSystem particleSystemR;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void StartClearEffect()
    {
        particleSystemL.Play();
        particleSystemR.Play();
    }
}
