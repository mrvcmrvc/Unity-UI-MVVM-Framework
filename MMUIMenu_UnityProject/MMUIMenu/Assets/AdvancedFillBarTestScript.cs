using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdvancedFillBarTestScript : MonoBehaviour
{
    public AdvancedFillBarScript Bar;

    private void Update()
    {
        if (Input.GetKey(KeyCode.Space))
            Bar.UpdateBar(75);

        if (Input.GetKey(KeyCode.LeftControl))
            Bar.UpdateBar(100);
    }
}
