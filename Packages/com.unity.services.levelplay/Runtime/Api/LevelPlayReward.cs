namespace Unity.Services.LevelPlay
{
    /// <summary>
    /// Represents the reward from LevelPlay Rewarded Ad, including a name and the amount
    /// </summary>
    public sealed class LevelPlayReward
    {
        internal static readonly LevelPlayReward Default = new(string.Empty, 0);

        /// <summary>
        /// The name of the reward.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// The amount of the reward.
        /// </summary>
        public int Amount { get; }

        internal LevelPlayReward(string name, int amount)
        {
            Name = name;
            Amount = amount;
        }
    }
}
