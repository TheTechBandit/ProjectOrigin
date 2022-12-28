using System;
using System.Collections.Generic;

namespace ProjectOrigin
{
    /*
     * -TODO- Since Types storing Types is very inefficient and causes a lot of issues with data storage-
     * replace the list of types in Advantages, Disadvantages, and Immunities with a list of strings and add
     * a fucntion to parse strings into Types.
     */
    /// <summary>Framework for Types and calculating Type Effectiveness.</summary>
    public class BasicType
    {
        /// <summary>The name of the Type</summary>
        public virtual string Type { get; }
        /// <summary>The list of types this type has Advantages against.</summary>
        public virtual List<BasicType> Advantages { get; }
        /// <summary>The list of types this type has Disadvantages against.</summary>
        public virtual List<BasicType> Disadvantages { get; }
        /// <summary>The list of types this type has Immunities against.</summary>
        public virtual List<BasicType> Immunities { get; }
        /// <summary>A description of the type.</summary>
        public virtual string Description { get; }

        /// <summary>Empty constructor for deserialization.</summary>
        public BasicType()
        {

        }

        public BasicType(bool newtype)
        {

        }

        /// <summary>Determine how effective this type is against another type.</summary>
        /// <param name="def">The list of defending types to compare against.</param>
        /// <returns>Returns a double that represents damage multiplication based off type effectiveness.</returns>
        public double ParseEffectiveness(List<BasicType> def)
        {
            var effect = 1.0;

            string defstr = $"{def[0].Type}";
            if (def.Count > 1)
                defstr += $"/{def[1].Type}";
            Console.WriteLine($"{Type} vs. {defstr}");
            Console.WriteLine($"{this.GetType().ToString()}");

            foreach (BasicType ty in def)
            {
                foreach (BasicType adv in Advantages)
                {
                    if (ty.GetType() == adv.GetType())
                    {
                        Console.WriteLine($"{Type} is advantagous against {ty.Type}");
                        effect *= 2.0;
                    }
                }

                foreach (BasicType dis in Disadvantages)
                {
                    if (ty.GetType() == dis.GetType())
                    {
                        Console.WriteLine($"{Type} is disadvantagous against {ty.Type}");
                        effect *= 0.5;
                    }
                }
            }

            return effect;
        }

        public override string ToString()
        {
            return $"{Type}";
        }
    }
}