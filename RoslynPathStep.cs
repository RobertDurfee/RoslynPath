using System.Text.RegularExpressions;

namespace RoslynPath
{
    class RoslynPathStep
    {
        public enum StepTypes
        {
            Invalid,
            Root,
            //Current,
            Regex,
            Index,
            //Filter
        }

        public StepTypes StepType { get; set; } = StepTypes.Invalid;

        public enum ScanTypes
        {
            Invalid,
            Children,
            Descendants
        }

        public ScanTypes ScanType { get; set; } = ScanTypes.Invalid;

        public Regex Pattern { get; set; }

        public int? Index { get; set; }

        //public RoslynPathFilter Filter { get; set; }
    }
}
