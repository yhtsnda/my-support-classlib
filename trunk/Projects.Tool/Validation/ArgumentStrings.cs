using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projects.Tool
{
    internal static class ArgumentStrings
    {
        public const string Argument_EmptyArray = "'{0}' must contain at least one element.";

        public const string Argument_EmptyString = "'{0}' cannot be an empty string or start with the null character.";

        public const string Argument_NullElement = "'{0}' cannot contain a null element.";

        public const string Argument_Whitespace = "The parameter '{0}' cannot consist entirely of white space characters.";
    }
}
