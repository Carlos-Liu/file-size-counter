using System;

namespace FileSizeCounter.Common
{
    /// <summary>
    /// The interface for the busy indicator window.
    /// </summary>
    public interface IBusyIndicatorWindow
    {
        /// <summary>
        ///   Gets if the function is successfully executed without any exceptions.
        /// </summary>
        bool? IsSuccessfullyExecuted { get; }

        /// <summary>
        ///   Gets if there is any execution exception.
        ///   null: there is no exception.
        /// </summary>
        Exception ExecutionException { get; }

        /// <summary>
        /// Show the processing element prompt message
        /// </summary>
        /// <param name="elementName"></param>
        void ShowCurrentInspectingElement(string elementName);

        /// <summary>
        ///   Start to execute the <paramref name="functionToRunInAnotherThread" />, and
        ///   show the waiting dialog.
        /// </summary>
        /// <typeparam name="TReturnType">The return type.</typeparam>
        /// <param name="operationDescription">The prompt string displayed on UI.</param>
        /// <param name="functionToRunInAnotherThread"></param>
        /// <returns>
        ///   the execution result if there is no exception occurred,
        ///   otherwise the new <typeparamref name="TReturnType" />.
        /// </returns>
        /// <remarks>
        ///   The window will be showed by ShowDialog().
        ///   Refer to <see cref="ExecutionException" /> to check if there is any exception occurred.
        /// </remarks>
        TReturnType ExecuteAndWait<TReturnType>(string operationDescription, Func<TReturnType> functionToRunInAnotherThread);
    }
}
