using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace Projects.Tool.Util
{
    public sealed class InterfaceValidatorAttribute : ConfigurationValidatorAttribute
    {
        Type interfaceType;

        public InterfaceValidatorAttribute(Type type)
        {
            if (!type.IsInterface)
                throw new ArgumentException(type + " must be an interface");

            this.interfaceType = type;
        }

        public override ConfigurationValidatorBase ValidatorInstance
        {
            get
            {
                return new InterfaceValidator(this.interfaceType);
            }
        }
    }


    public class InterfaceValidator : ConfigurationValidatorBase
    {
        Type interfaceType;

        public InterfaceValidator(Type type)
        {
            if (!type.IsInterface)
                throw new ArgumentException(type + " must be an interface");

            this.interfaceType = type;
        }

        public override bool CanValidate(Type type)
        {
            return ((type == typeof(Type)) || base.CanValidate(type));
        }

        public override void Validate(object value)
        {
            if (value != null)
            {
                CheckForInterface((Type)value, this.interfaceType);
            }
        }

        static void CheckForInterface(Type type, Type interfaceType)
        {
            if (((type != null) && (interfaceType != null)) && (Array.IndexOf<Type>(type.GetInterfaces(), interfaceType) == -1))
            {
                throw new ConfigurationErrorsException("The type " + type.AssemblyQualifiedName + " must implement " + interfaceType.AssemblyQualifiedName);
            }
        }
    }
}
