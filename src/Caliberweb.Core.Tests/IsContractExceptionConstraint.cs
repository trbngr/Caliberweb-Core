using System;

using NUnit.Framework.Constraints;

namespace Caliberweb.Core
{
    class IsContractExceptionConstraint : Constraint
    {
        public override bool Matches(object instance)
        {
            if (!(instance is Exception))
                return false;

            string name = instance.GetType().FullName;
            return name == "System.Diagnostics.Contracts.__ContractsRuntime+ContractException";
        }

        public override void WriteDescriptionTo(MessageWriter writer)
        {
            writer.Write("exception");
        }
    }
}