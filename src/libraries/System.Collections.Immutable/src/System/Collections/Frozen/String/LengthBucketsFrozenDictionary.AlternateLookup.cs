﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace System.Collections.Frozen
{
    internal sealed partial class LengthBucketsFrozenDictionary<TValue>
    {
        /// <summary>
        /// Invokes <see cref="GetValueRefOrNullRefCoreAlternate(ReadOnlySpan{char})"/>
        /// on instances known to be of type <see cref="LengthBucketsFrozenDictionary{TValue}"/>.
        /// </summary>
        private static readonly AlternateLookupDelegate<ReadOnlySpan<char>> s_alternateLookup = (dictionary, key)
            => ref ((LengthBucketsFrozenDictionary<TValue>)dictionary).GetValueRefOrNullRefCoreAlternate(key);

        /// <inheritdoc/>
        private protected override AlternateLookupDelegate<TAlternateKey> GetAlternateLookupDelegate<TAlternateKey>()
        {
            Debug.Assert(typeof(TAlternateKey) == typeof(ReadOnlySpan<char>));
            return (AlternateLookupDelegate<TAlternateKey>)(object)s_alternateLookup;
        }

        /// <inheritdoc cref="GetValueRefOrNullRefCore(string)" />
        private ref readonly TValue GetValueRefOrNullRefCoreAlternate(ReadOnlySpan<char> key)
        {
            IAlternateEqualityComparer<ReadOnlySpan<char>, string> comparer = GetAlternateEqualityComparer<ReadOnlySpan<char>>();

            // If the length doesn't have an associated bucket, the key isn't in the dictionary.
            int bucketIndex = (key.Length - _minLength) * LengthBuckets.MaxPerLength;
            int bucketEndIndex = bucketIndex + LengthBuckets.MaxPerLength;
            int[] lengthBuckets = _lengthBuckets;
            if (bucketIndex >= 0 && bucketEndIndex <= lengthBuckets.Length)
            {
                string[] keys = _keys;
                TValue[] values = _values;

                for (; bucketIndex < bucketEndIndex; bucketIndex++)
                {
                    int index = lengthBuckets[bucketIndex];
                    if ((uint)index < (uint)keys.Length)
                    {
                        if (comparer.Equals(key, keys[index]))
                        {
                            return ref values[index];
                        }
                    }
                    else
                    {
                        // -1 is used to indicate a null, when it's casted to unit it becomes > keys.Length
                        break;
                    }
                }
            }

            return ref Unsafe.NullRef<TValue>();
        }
    }
}
