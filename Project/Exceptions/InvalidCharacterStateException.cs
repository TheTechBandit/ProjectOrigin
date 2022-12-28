using System;

namespace ProjectOrigin
{
    /// <summary>An exception that can be thrown when a user attempts to do and action not allowed by their current character state
    /// (character doesn't exist, character is in the wrong place, etc).</summary>
    [Serializable]
    class InvalidCharacterStateException : Exception
    {
        public InvalidCharacterStateException()
        {

        }

        public InvalidCharacterStateException(string type)
            : base($"{type}")
        {

        }
    }
}