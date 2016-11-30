using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using FileSizeCounter.Common;
using FileSizeCounter.Model;
using Winforms = System.Windows.Forms;

namespace FileSizeCounter
{
  /// <summary>
  ///   Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow
  {
    private SizeCounterViewModel ViewModel { get; set; }
    private IBusyIndicatorWindow BusyIndicatorWindow { get; set; }

    public MainWindow()
    {
      InitializeComponent();

      BusyIndicatorWindow = new BusyIndicatorWindow(this);
      ViewModel = new SizeCounterViewModel(BusyIndicatorWindow);
      DataContext = ViewModel;
    }

    private void BrowseButton_OnClick(object sender, RoutedEventArgs e)
    {
      var folderBrowseDialog = new Winforms.FolderBrowserDialog
      {
        Description = Res.Resources.MainWindow_Info_SelectTargetDirectory
      };
      var result = folderBrowseDialog.ShowDialog();
      switch (result)
      {
        case Winforms.DialogResult.OK:
          InspectDirectoryTextBox.Text = folderBrowseDialog.SelectedPath;
          break;
      }
    }

    private void TreeView_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
    {
      var item = sender as TreeViewItem;
      if (item != null)
      {
        item.Focus();
        e.Handled = true;
      }
    }

    // Since the SeletedItem on TreeView is read-only and does not support do data-binding, so use the event
    private void ResultTreeView_OnSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
    {
      var selectedItem = e.NewValue as IElement;
      ViewModel.SelectedElement = selectedItem;
    }

    // Only allow input numeric text, refer to 
    // http://stackoverflow.com/questions/1268552/how-do-i-get-a-textbox-to-only-accept-numeric-input-in-wpf#
    private void ThresholdValueTextBox_OnPreviewTextInput(object sender, TextCompositionEventArgs e)
    {
      var textBlock = sender as TextBox;
      string originalText = string.Empty;
      if (textBlock != null)
        originalText = textBlock.Text;

      string previewFullText = originalText + e.Text;

      e.Handled = !Helper.IsValidNumeric(previewFullText);
    }
    
    // Suppress the Cut/copy/paste command and the short-cut keys
    private void ThresholdValueTextBox_OnPreviewExecuted(object sender, ExecutedRoutedEventArgs e)
    {
      if (e.Command == ApplicationCommands.Copy ||
         e.Command == ApplicationCommands.Cut ||
         e.Command == ApplicationCommands.Paste)
      {
        e.Handled = true;
      }
    }
  }
}