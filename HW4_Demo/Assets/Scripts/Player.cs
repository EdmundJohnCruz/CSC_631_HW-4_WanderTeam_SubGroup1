using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Mirror
{

    public class Player : NetworkBehaviour
    {
        [SerializeField] private Vector3 movement = new Vector3();


        // Update is called once per frame
        private void Update()
        {
            if (!hasAuthority) { return; }
            if (!Input.GetKeyDown(KeyCode.Space)) { return; }

            transform.Translate(movement);
        }
    }
}
