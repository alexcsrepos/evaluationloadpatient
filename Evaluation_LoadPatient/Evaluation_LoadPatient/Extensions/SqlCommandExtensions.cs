using System.Data.SqlClient;

namespace Evaluation_LoadPatient.Extensions
{
    internal static class SqlCommandExtensions
    {
        /// <summary>
        /// This will add an array of parameters to a SqlCommand. This is used for an IN statement.
        /// Use the returned value for the IN part of your SQL call. (i.e. SELECT * FROM table WHERE field IN (returnValue))
        /// </summary>
        /// <param name="sqlCommand">The SqlCommand object to add parameters to.</param>
        /// <param name="array">The array of strings that need to be added as parameters.</param>
        /// <param name="paramName">What the parameter should be named.</param>
        public static string AddArrayParameters(this SqlCommand sqlCommand, string[] array, string paramName)
        {
            var parameters = new string[array.Length];
            for (int i = 0; i < array.Length; i++)
            {
                parameters[i] = string.Format("@{0}{1}", paramName, i);
                sqlCommand.Parameters.AddWithValue(parameters[i], array[i]);
            }

            return string.Join(", ", parameters);
        }
    }
}
