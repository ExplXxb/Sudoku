using System;
using System.Collections.Generic;
using UnityEngine;

namespace Unity.Services.LevelPlay
{
    /// <summary>
    /// Represents an initialization error for the LevelPlay SDK.
    /// </summary>
    public sealed class LevelPlayInitError
    {
        public int ErrorCode {get;}
        public string ErrorMessage {get;}

        internal LevelPlayInitError(string json)
        {
            if (string.IsNullOrEmpty(json))
            {
                return;
            }

            try
            {
                Dictionary<string, object> jsonDic =
                    LevelPlayJson.Deserialize(json) as Dictionary<string, object>;
                if (jsonDic.TryGetValue("errorCode", out var obj) && obj != null)
                {
                    ErrorCode = Int32.Parse(obj.ToString());
                }
                if (jsonDic.TryGetValue("errorMessage", out obj) && obj != null)
                {
                    ErrorMessage = obj.ToString();
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError("Failed to parse LevelPlayInitError: " + e.Message);
            }
        }

        /// <summary>
        /// Returns a string that represents the current <see cref="LevelPlayInitError"/>.
        /// </summary>
        /// <returns>A string that contains the error code and message.</returns>
        public override string ToString()
        {
            return $"LevelPlayInitError: {ErrorCode}, {ErrorMessage}";
        }
    }
}
