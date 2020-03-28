using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using VSCC.DataType;

namespace VSCC.Controls
{
    /// <summary>
    /// Interaction logic for InventoryItemPanel.xaml
    /// </summary>
    public partial class InventoryItemPanel : UserControl
    {
        public InventoryItemPanel() => this.InitializeComponent();

        public void SetDataContext(InventoryItem context) => this.DataContext = this.Picture.DataContext = this.SName.DataContext = context;

        private void UserControl_PreviewDrop(object sender, DragEventArgs e)
        {
            if (e.Data != null && e.Data.GetDataPresent("InventoryItem"))
            {
            }
        }

        private void UserControl_Drop(object sender, DragEventArgs e)
        {
            if (e.Data != null && e.Data.GetDataPresent("InventoryItem"))
            {
                this.SetDataContext((InventoryItem)e.Data.GetData("InventoryItem"));
                e.Handled = true;
            }
        }

        private void UserControl_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (this.DataContext != null)
            {
                DataObject dragData = new DataObject("InventoryItem", this.DataContext);
                DragDrop.DoDragDrop(this, dragData, DragDropEffects.Move);
                this.SetDataContext(null);
            }
        }
    }
}
