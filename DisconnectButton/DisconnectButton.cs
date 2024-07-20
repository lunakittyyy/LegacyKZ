namespace DisconnectButton
{
    internal class DisconnectButton : GorillaTriggerBox
    {
        public override void OnBoxTriggered()
        {
            base.OnBoxTriggered();
            CustomMaps.CustomMaps.instance.Disconnect();
        }
    }
}
