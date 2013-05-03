using System;
using System.Collections.Generic;
using System.Reflection;

namespace NUnit.Framework.Internal
{
    /// <summary>
    /// This is a wrapper class for Random. 
    ///     Additionaly functionality is built into this class to handle the following:
    ///	        Random Int
    ///	        Random Short
    ///	        Random Bool
    ///     	Random Float
    ///	        Random Double
    ///	        Random Enum
    /// </summary>
    public class RandomGenerator
    {
        #region Members & Constructor
        /// <summary>
        /// Seed for the wrapped Random
        /// </summary>
        public readonly int seed;

        private Random random;

        /// <summary>
        /// Lazy-loaded Random built on the readonly Seed
        /// </summary>
        private Random Rand
        {
            get
            {
                random = random == null ? new Random(seed) : random;
                return random;
            }
        }

        /// <summary>
        /// Constructor requires Seed value in order to store it for use in Random creation
        /// </summary>
        /// <param name="seed"></param>
        public RandomGenerator(int seed)
        {
            this.seed = seed;
        }
        #endregion

        #region Ints
        /// <summary>
        /// Get Next Integer from Random 
        /// </summary>
        /// <returns> int </returns>
        public int GetInt()
        {
            return Rand.Next();
        }
        /// <summary>
        /// Get Next Integer within the specified min & max from Random 
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns> int </returns>
        public int GetInt(int min, int max)
        {
            return Rand.Next(min, max);
        }
        #endregion

        #region Shorts
        /// <summary>
        /// Get Next Short from Random
        /// </summary>
        /// <returns> short </returns>
        public short GetShort()
        {
            return (short)Rand.Next(short.MinValue, short.MaxValue);
        }
        /// <summary>
        /// Get Next Short within the specified min & max from Random 
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns> short </returns>
        public short GetShort(short min, short max)
        {
            return (short)Rand.Next(min, max);
        }
        #endregion

        #region Bytes
        /// <summary>
        /// Get Next Byte from Random
        /// </summary>
        /// <returns> byte </returns>
        public byte GetByte()
        {
            return (byte)Rand.Next(Byte.MinValue, Byte.MaxValue);
        }
        /// <summary>
        /// Get Next Byte within the specified min & max from Random
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns> byte </returns>
        public byte GetByte(byte min, byte max)
        {
            return (byte)Rand.Next(min, max);
        }
        #endregion

        #region Bools
        /// <summary>
        /// Get Random Boolean value
        /// </summary>
        /// <returns> bool </returns>
        public bool GetBool()
        {
            return Rand.Next(0, 1) == 0;
        }
        /// <summary>
        /// Get Random Boolean value based on the probability of that value being true
        /// </summary>
        /// <param name="probability"></param>
        /// <returns> bool </returns>
        public bool GetBool(double probability)
        {
            return Rand.NextDouble() < Math.Abs(probability % 1.0);
        }
        #endregion

        #region Double & Float
        /// <summary>
        /// Get Next Double from Random
        /// </summary>
        /// <returns></returns>
        public double GetDouble()
        {
            return Rand.NextDouble();
        }
        /// <summary>
        /// Get Next Float from Random
        /// </summary>
        /// <returns></returns>
        public float GetFloat()
        {
            return (float)Rand.NextDouble();
        }
        #endregion

        #region Enums
#if SILVERLIGHT || NETCF
        /// <summary>
        /// Return a random enum value from the specified type
        /// </summary>
        /// <param name="enumType"></param>
        /// <returns> object </returns>
        public object GetEnum(Type enumType)
        {
            List<string> enumNames = new List<string>();
            foreach (FieldInfo fi in enumType.GetType().GetFields(BindingFlags.Static | BindingFlags.Public)){
                enumNames.Add(fi.Name);
            }
            string enumeration = enumNames[Rand.Next(0, enumNames.Count)];
            return Enum.Parse(enumType, enumeration, true);
        }

#else
        /// <summary>
        /// Return a random enum value representation of the specified Type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns> T </returns>
        public T GetEnum<T>()
        {
            string[] items = Enum.GetNames(typeof(T));
            string enumeration = items[Rand.Next(0, items.Length)];
            return (T)Enum.Parse(typeof(T), enumeration, true);
        }
#endif
        #endregion

    }
}
