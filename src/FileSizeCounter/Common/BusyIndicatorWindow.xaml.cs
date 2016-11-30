using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace FileSizeCounter.Common
{
    /// <summary>
    ///   A long time execution host window to:
    ///   - Execute the time consuming operation in another thread (MTA)
    ///   - Show the executing status
    ///   - Return the execution result
    /// </summary>
    public partial class BusyIndicatorWindow : IBusyIndicatorWindow
    {
        private readonly Dispatcher _CurrentDispatcher = Dispatcher.CurrentDispatcher;
        private object _ExecutionResult;
        private Window OwnerWindow { get; set; }

        /// <summary>
        ///   Ctor
        /// </summary>
        /// <param name="ownerWindow">The parent window.</param>
        public BusyIndicatorWindow(Window ownerWindow)
        {
            InitializeComponent();

            OwnerWindow = ownerWindow;
        }

        /// <summary>
        ///   Gets if there is any execution exception.
        ///   null: there is no exception.
        /// </summary>
        public Exception ExecutionException { get; private set; }

        /// <summary>
        ///   Gets if the function is successfully executed without any exceptions.
        /// </summary>
        public bool? IsSuccessfullyExecuted { get; private set; }

        /// <summary>
        /// Show the processing element prompt message
        /// </summary>
        /// <param name="elementName"></param>
        public void ShowCurrentInspectingElement(string elementName)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                ProgressingTextBlock.Text = elementName;
            });
        }

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
        public TReturnType ExecuteAndWait<TReturnType>(string operationDescription,
          Func<TReturnType> functionToRunInAnotherThread)
        {
            Debug.Assert(OwnerWindow != null);

            PromptMessageTextBlock.Text = operationDescription;

            ExecuteAsync(functionToRunInAnotherThread, OnFinishCallback);

            Owner = OwnerWindow;
            ShowDialog();

            return (TReturnType)_ExecutionResult;
        }

        private void OnFinishCallback<TReturnType>(TReturnType executeResult)
        {
            Action<TReturnType> callback =
              resultIn =>
              {
                  _ExecutionResult = resultIn;
                  Hide();
              };

            _CurrentDispatcher.BeginInvoke(callback, DispatcherPriority.Input, executeResult);
        }

        private void ExecuteAsync<TReturnType>(Func<TReturnType> functionToRunInAnotherThread,
          Action<TReturnType> onFinishCallback)
        {
            new TaskFactory().StartNew(() => Execute(functionToRunInAnotherThread, onFinishCallback));
        }

        private void Execute<TReturnType>(Func<TReturnType> functionToRunInAnotherThread,
          Action<TReturnType> onFinishCallback)
        {
            var result = default(TReturnType);
            try
            {
                result = functionToRunInAnotherThread();
            }
            catch (Exception ex)
            {
                ExecutionException = ex;
                IsSuccessfullyExecuted = false;
            }
            finally
            {
                if (!IsSuccessfullyExecuted.HasValue)
                {
                    IsSuccessfullyExecuted = true;
                }

                onFinishCallback(result);
            }
        }
    }
}