using System;
using System.Collections.Generic;

namespace OpenTibia.Game.Common.ServerObjects
{
    public interface IValues : IDisposable
    {
        void Start();

        object GetValue(string key);


        bool GetBoolean(string key);

        ushort GetUInt16(string key);

        int GetInt32(string key);

        long GetInt64(string key);

        string GetString(string key);

        bool[] GetBooleanArray(string key);

        ushort[] GetUInt16Array(string key);

        int[] GetInt32Array(string key);

        long[] GetInt64Array(string key);

        string[] GetStringArray(string key);

        HashSet<ushort> GetUInt16HashSet(string key);

        Dictionary<ushort, ushort> GetUInt16IUnt16Dictionary(string key);
    }
}