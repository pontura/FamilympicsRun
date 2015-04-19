using UnityEngine;
using System.Collections;

public class InputManager : MonoBehaviour {

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Events.OnKeyDown("space");
        }

        //RaycastHit hit;
        //Ray ray;
        //if (Input.GetMouseButtonDown(0))
        //{
        //    ray = UICamera.ScreenPointToRay(Input.mousePosition);
        //    if (Physics.Raycast(ray, out hit))
        //    {
        //        if (hit.collider != null)
        //        {
        //            Events.OnUIClicked(hit.collider.gameObject);
        //            return;
        //        }
        //    }
        //    ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //    if (Physics.Raycast(ray, out hit))
        //    {
        //        if (hit.collider.tag == "Hero")
        //        { }
        //        }

        //}
    }
}
