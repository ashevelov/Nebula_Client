namespace Nebula
{
    /// <summary>
    /// The game state.
    /// </summary>
    public enum GameState
    {
        /// <summary>
        /// The connected.
        /// </summary>
        Connected,

        /// <summary>
        /// The disconnected.
        /// </summary>
        Disconnected,

        /// <summary>
        /// The wait for connect.
        /// </summary>
        WaitForConnect,

        /// <summary>
        /// The world entered.
        /// </summary>
        WorldEntered,

        Login,

        /// <summary>
        /// We are waiting for change world
        /// </summary>
        WaitingForChangeWorld,

        Workshop,

        SelectCharacter
    }
}