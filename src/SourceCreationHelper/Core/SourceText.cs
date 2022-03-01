namespace SourceCreationHelper.Core
{   
    internal class SourceText: SourceTextBase
    {
        private readonly string text;
        public SourceText(string text)
        {
            this.text = text;
        }
        protected override string BuildSource()
        {
            return text;
        }
    }
}