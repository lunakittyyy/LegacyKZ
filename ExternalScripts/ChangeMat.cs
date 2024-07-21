using Photon.Pun;
using UnityEngine;

namespace CustomMaps
{
    public class ChangeMat : GorillaTriggerBox
    {
        public int matID;

        public override void OnBoxTriggered()
        {
            base.OnBoxTriggered();
            Object.FindObjectOfType<GorillaPlaySpace>().myVRRig.GetComponentInParent<PhotonView>().RPC("ChangeMaterial", RpcTarget.All, matID);
        }
    }
}
