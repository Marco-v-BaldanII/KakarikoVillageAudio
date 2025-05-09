using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraToNavi : MonoBehaviour
{
    public Navi navi;

    public ThirdPersonOrbitCamBasic cameraScript;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.Z))
        {
            cameraScript.enabled = false;
            Vector3 v = navi.transform.position - transform.position;

                Quaternion targetRotation = Quaternion.LookRotation( v );
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 250 * Time.deltaTime);

        }
        else
        {
            cameraScript.enabled = true;
        }
    }
}
