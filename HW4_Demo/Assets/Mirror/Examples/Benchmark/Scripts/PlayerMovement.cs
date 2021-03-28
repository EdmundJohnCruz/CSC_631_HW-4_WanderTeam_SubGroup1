using UnityEngine;

namespace Mirror.Examples.Benchmark
{
    public class PlayerMovement : NetworkBehaviour
    {
        public float speed = 5;

        [Client]
        private void Update()
        {

            if (!hasAuthority) { return; }
            if (!isLocalPlayer) return;

            CmdMove();
        }
        [Command]
        private void CmdMove()
        {
            RpcMove();
        }


        [ClientRpc]
        private void RpcMove()
        {
            float h = Input.GetAxisRaw("Horizontal");
            float v = Input.GetAxisRaw("Vertical");

            Vector3 dir = new Vector3(h, 0, v);
            transform.position += dir.normalized * (Time.deltaTime * speed);
        }
    }
}
