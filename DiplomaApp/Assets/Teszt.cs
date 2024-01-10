using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace TesztNamespace{
    public class Teszt : MonoBehaviour
    {
        // Start is called before the first frame update
        [SerializeField] GameObject plcHmi;
        [SerializeField] private GameObject input, output;
        void Start()
        {
            var hmi = Instantiate(plcHmi, Vector3.zero, Quaternion.identity);
            Vector3 buttonStartPos = new Vector3(-0.0292f,0.0042f,0.0273f);
            Vector3 lightStartPos = new Vector3(0.0081f,0.0042f,0.0273f);
            for(int i=0; i< 5; i++)
            {
                var offset = new Vector3(0f ,0f , .011f) * i;
                if(i%2 == 0) // 0: bemenet
                {
                    var inputInstance = Instantiate(input, buttonStartPos - offset, Quaternion.identity, hmi.transform);
                    // inputInstance.transform.localScale = 0.5f * Vector3.one;
                    inputInstance.name += "_"+i; 
                }
                else{   //1: kimenet
                    var outputInstance = Instantiate(output, lightStartPos - offset, Quaternion.identity, hmi.transform);
                    // inputInstance.transform.localSc
                    outputInstance.name += "_"+i;;
                }
            }
        }

        // Update is called once per frame
        void Update()
        {
            
        }
    }
}
