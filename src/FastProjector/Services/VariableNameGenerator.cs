namespace FastProjector.Services
{
    public class VariableNameGenerator : IVariableNameGenerator
    {
        private int _sequence = 0x0;

        public string GetNew()
        {
            _sequence++;
            return "s" + _sequence.ToString("X");
        }
    }
}