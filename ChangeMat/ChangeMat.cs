using Photon.Pun;

namespace ChangeMat
{
    public class ChangeMat : GorillaTriggerBox
    {
        public int matID;

        public override void OnBoxTriggered()
        {
            base.OnBoxTriggered();
            FindObjectOfType<GorillaPlaySpace>().myVRRig.GetComponentInParent<PhotonView>().RPC("ChangeMaterial", RpcTarget.All, matID);
        }
    }
}
