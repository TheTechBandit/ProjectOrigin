﻿using System.Collections.Generic;

namespace ProjectOrigin
{
    public class AirType : BasicType
    {
        public override string Type { get; } = "Air";
        public override List<BasicType> Advantages { get; }
        public override List<BasicType> Disadvantages { get; }
        public override List<BasicType> Immunities { get; }
        public override string Description { get; } = "";

        public AirType() : base()
        {

        }

        public AirType(bool newtype) : base(newtype)
        {
            Advantages = new List<BasicType>()
            {
                new FireType(),
                new PrimalType()
            };
            Disadvantages = new List<BasicType>()
            {
                new ElectricType(),
                new MetalType(),
                new ColdType()
            };
            Immunities = new List<BasicType>()
            {

            };
        }

    }
}