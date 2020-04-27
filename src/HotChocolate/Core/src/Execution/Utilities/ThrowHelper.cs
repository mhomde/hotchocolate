using System;
using HotChocolate.Execution.Properties;
using HotChocolate.Language;
using HotChocolate.Types;

namespace HotChocolate.Execution.Utilities
{
    internal static class ThrowHelper
    {
        public static GraphQLException VariableIsNotAnInputType(
            VariableDefinitionNode variableDefinition)
        {
            return new GraphQLException(
                ErrorBuilder.New()
                    .SetMessage(
                        "Variable `{0}` is not an input type.",
                        variableDefinition.Variable.Name.Value)
                    .SetCode(ErrorCodes.Execution.NonNullViolation)
                    .SetExtension("variable", variableDefinition.Variable.Name.Value)
                    .SetExtension("type", variableDefinition.Type.ToString())
                    .AddLocation(variableDefinition)
                    .Build());
        }

        public static GraphQLException NonNullVariableIsNull(
            VariableDefinitionNode variableDefinition)
        {
            return new GraphQLException(
                ErrorBuilder.New()
                    .SetMessage(
                        "Variable `{0}` is required.",
                        variableDefinition.Variable.Name.Value)
                    .SetCode(ErrorCodes.Execution.NonNullViolation)
                    .SetExtension("variable", variableDefinition.Variable.Name.Value)
                    .AddLocation(variableDefinition)
                    .Build());
        }

        public static GraphQLException VariableValueInvalidType(
            VariableDefinitionNode variableDefinition,
            Exception? exception = null)
        {
            IErrorBuilder errorBuilder = ErrorBuilder.New()
                .SetMessage(
                    "Variable `{0}` got an invalid value.",
                    variableDefinition.Variable.Name.Value)
                .SetCode(ErrorCodes.Execution.InvalidType)
                .SetExtension("variable", variableDefinition.Variable.Name.Value)
                .AddLocation(variableDefinition);

            switch (exception)
            {
                case ScalarSerializationException ex:
                    errorBuilder.SetExtension("scalarError", ex.Message);
                    break;
                case InputObjectSerializationException ex:
                    errorBuilder.SetExtension("inputObjectError", ex.Message);
                    break;
                default:
                    errorBuilder.SetException(exception);
                    break;
            }

            return new GraphQLException(errorBuilder.Build());
        }

        public static GraphQLException MissingIfArgument(DirectiveNode directive)
        {
            return new GraphQLException(
                ErrorBuilder.New()
                    .SetMessage(
                        Resources.ThrowHelper_MissingDirectiveIfArgument,
                        directive.Name.Value)
                    .AddLocation(directive)
                    .Build());
        }
    }

    internal static class ErrorHelper
    {
        public static IError ArgumentNonNullError(string responseName, ArgumentNode argument, Path path, )
        {
            return ErrorBuilder.New()
                .SetMessage(
                    CultureInfo.InvariantCulture,
                    TypeResources.ArgumentValueBuilder_NonNull,
                    argument.Name,
                    TypeVisualizer.Visualize(report.Type))
                .AddLocation(fieldInfo.Selection)
                .SetExtension(_argumentProperty, report.Path.ToCollection())
                .SetPath(fieldInfo.Path.AppendOrCreate(fieldInfo.ResponseName))
                .Build();
        }
    }
}
