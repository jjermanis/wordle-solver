using System.Collections.Generic;
using System.Linq;

namespace wordle_solver
{
    internal class CommandLineArgs
    {
        private const string HARD_MODE_ARG = "-h";
        private const string INTERACTIVE_ARG = "-p";
        private const string TEST_ARG = "-t";
        private const string ILLEGAL_CHECK_ARG = "-i";
        private const string LEGAL_CHECK_ARG = "-l";
        private const string HELP_ARG = "-?";

        private static readonly IReadOnlyDictionary<string, GameActions> ACTIONS
            = new Dictionary<string, GameActions>()
        {
                { INTERACTIVE_ARG, GameActions.Interactive },
                { TEST_ARG, GameActions.Interactive },
                { ILLEGAL_CHECK_ARG, GameActions.Interactive },
                { LEGAL_CHECK_ARG, GameActions.Interactive },
                { HELP_ARG, GameActions.Help },
        };

        private static readonly IReadOnlyDictionary<string, string> DOCS
            = new Dictionary<string, string>()
        {
            { HARD_MODE_ARG, "Specifies to play in hard mode, all successive guesses must comply with previous hints." },
            { INTERACTIVE_ARG, "Use to help you play Wordle, provides the best guesses for each turn." },
            { TEST_ARG, "Simulates the game many times, providing a performance report." },
            { ILLEGAL_CHECK_ARG, "Checks this programs dictionary to make sure that all its words are acceptable guesses." },
            { LEGAL_CHECK_ARG, "Checks this programs dictionary to make sure that all possible answers are known." }
        };

        public GameActions Action { get; set; }
        public bool IsHardMode { get; set; }
        public string Docs { get; set; }

        public CommandLineArgs(string[] args)
        {
            IsHardMode = false;
            foreach (var arg in args)
                if (arg.Equals(HARD_MODE_ARG))
                    IsHardMode = true;

            GameActions? action = null;
            string actionArg = null;
            foreach (var arg in args)
            {
                if (ACTIONS.ContainsKey(arg))
                {
                    if (actionArg != null)
                    {
                        Action = GameActions.Error;
                        Docs = $"You have specified multiple conflicting actions: {actionArg} and {arg}. Please specify\r\n" +
                            $"only one of these. Use {HELP_ARG} for usage details.";
                        return;
                    }
                    else
                    {
                        action = ACTIONS[arg];
                        actionArg = arg;
                    }
                }
            }
            if (!action.HasValue)
                action = GameActions.Interactive;
            Action = action.Value;

            if (Action == GameActions.Help)
            {
                Docs = "Command line arguments:\r\n";
                Docs += "    note: only one ACTION can be used when running\r\n";
                var docArgs = DOCS.Keys.OrderBy(d => d);
                foreach (var arg in docArgs)
                {
                    var actionText = ACTIONS.ContainsKey(arg) ? "(ACTION)" : "";
                    Docs += $"{arg} : {actionText} {DOCS[arg]}\r\n";
                }
            }
        }
    }
}