namespace CustomMaps
{
    internal class DisconnectButton : GorillaTriggerBox
    {
        public override void OnBoxTriggered()
        {
            base.OnBoxTriggered();
            CustomMaps.instance.Disconnect();
        }
    }
}
