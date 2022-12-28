namespace ProjectOrigin
{
    /// <summary>A static class for converting a mon's name to a mon of that name.</summary>
    public static class MonRegister
    {
        static MonRegister()
        {

        }

        /// <summary>Converts a string to a mon based on species name.</summary>
        /// <param name="str">The species name of the mon.</param>
        /// <returns>Returns a mon.</returns>
        public static BasicMon StringToMonRegister(string str)
        {
            BasicMon mon;
            str = str.ToLower();

            switch (str)
            {
                case "suki":
                    mon = new Suki(true);
                    break;
                case "snoril":
                    mon = new Snoril(true);
                    break;
                case "smoledge":
                    mon = new Smoledge(true);
                    break;
                case "grasipup":
                    mon = new Grasipup(true);
                    break;
                case "arness":
                    mon = new Arness(true);
                    break;
                case "meliosa":
                    mon = new Meliosa(true);
                    break;
                case "ritala":
                    mon = new Ritala(true);
                    break;
                case "elesoak":
                    mon = new Elesoak(true);
                    break;
                case "elecute":
                    mon = new Elecute(true);
                    break;
                case "psygoat":
                    mon = new Psygoat(true);
                    break;
                case "ook":
                    mon = new Ook(true);
                    break;
                case "stebble":
                    mon = new Stebble(true);
                    break;
                case "sedimo":
                    mon = new Sedimo(true);
                    break;
                default:
                    mon = new Snoril(true);
                    break;
            }

            return mon;
        }

    }
}