using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using FileSizeCounter.Model;
using Winforms = System.Windows.Forms;

namespace FileSizeCounter
{
  /// <summary>
  ///   Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow
  {
    public SizeCounterViewModel ViewModel { get; private set; }
    public MainWindow()
    {
      InitializeComponent();

      ViewModel = new SizeCounterViewModel(this);
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
  }
}