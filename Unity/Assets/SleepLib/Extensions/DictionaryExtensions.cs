using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System;

namespace DictionaryExtension
{
	public static class DictionaryExtensions 
	{
		public static bool ChangeKey<TKey, TValue>(this Dictionary<TKey, TValue> dict, 	TKey oldKey, TKey newKey)
		{
		    TValue value;
		    if (!dict.TryGetValue(oldKey, out value))
		        return false;
		
		    dict.Remove(oldKey); 
		    dict[newKey] = value;
		    return true;
		}
	}
}
