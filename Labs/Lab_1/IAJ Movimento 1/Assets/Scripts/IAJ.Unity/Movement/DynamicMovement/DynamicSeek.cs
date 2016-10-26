namespace Assets.Scripts.IAJ.Unity.Movement.DynamicMovement
{
    public class DynamicSeek : DynamicMovement
    {
        public DynamicSeek()
        {
            this.MaxAcceleration = 20.0f;
        }

        public override string Name
        {
            get { return "Seek"; }
        }

        public override KinematicData Target { get; set; }

        public override MovementOutput GetMovement()
        {
            var output = new MovementOutput();

            output.linear = this.Target.position - this.Character.position;

            if (output.linear.sqrMagnitude > 0)
            {
                output.linear.Normalize();
                output.linear *= this.MaxAcceleration;
            }

            return output;
        }
    }
}
