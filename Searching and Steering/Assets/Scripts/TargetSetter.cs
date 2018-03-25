using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetSetter : MonoBehaviour {

void Update () {
		if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;

            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 1000))
            {
                transform.position = new Vector3(hit.point.x, transform.position.y, hit.point.z);
            }
        }
	}
}
