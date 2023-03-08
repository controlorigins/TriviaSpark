namespace TriviaSpark.Core.Match
{
    /// <summary>
    /// Represents a scorecard for a quiz or trivia game.
    /// </summary>
    public class ScoreCardModel
    {
        /// <summary>
        /// Gets or sets the adjusted score for the scorecard.
        /// </summary>
        public double AdjustedScore { get; set; }

        /// <summary>
        /// Gets or sets the number of correct answers for the scorecard.
        /// </summary>
        public int NumCorrect { get; set; }

        /// <summary>
        /// Gets or sets the number of incorrect answers for the scorecard.
        /// </summary>
        public int NumIncorrect { get; set; }

        /// <summary>
        /// Gets or sets the total number of questions for the scorecard.
        /// </summary>
        public int NumQuestions { get; set; }

        /// <summary>
        /// Gets the percentage of correct answers for the scorecard.
        /// </summary>
        public double PercentCorrect { get; set; }
    }
}

