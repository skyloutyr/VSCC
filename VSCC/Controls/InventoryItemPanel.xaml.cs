namespace VSCC.Controls
{
    using System;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using VSCC.Controls.Tabs;
    using VSCC.DataType;
    using VSCC.State;

    /// <summary>
    /// Interaction logic for InventoryItemPanel.xaml
    /// </summary>
    public partial class InventoryItemPanel : UserControl
    {
        public InventoryItemPanel() => this.InitializeComponent();

        public void SetDataContext(InventoryItem context)
        {
            if (context != null && context.ImageList == null)
            {
                context.ImageList = AppState.Current.TInventory.Images;
                if (!string.IsNullOrEmpty(context.ImageIndex) && context.ImageIndex[0] != '\\') // Old Item
                {
                    context.ImageIndex = AppState.Current.TInventory.Images.TryFindName(context.ImageIndex);
                }
            }

            this.DataContext = this.Picture.DataContext = this.SName.DataContext = context;
        }

        private void UserControl_PreviewDrop(object sender, DragEventArgs e)
        {
            if (e.Data != null && e.Data.GetDataPresent("InventoryItem"))
            {
                e.Effects = DragDropEffects.Move;
            }
        }

        private void UserControl_Drop(object sender, DragEventArgs e)
        {
            if (e.Data != null && e.Data.GetDataPresent("InventoryItem"))
            {
                Tuple<object, object, ICollectionView> data = (Tuple<object, object, ICollectionView>)e.Data.GetData("InventoryItem");
                this.SetDataContext((InventoryItem)data.Item1);
                e.Handled = true;
            }
        }

        private void UserControl_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (this.DataContext != null && !Keyboard.Modifiers.HasFlag(ModifierKeys.Control))
            {
                this.SetDataContext(null);
            }
        }

        private void UserControl_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (Keyboard.Modifiers.HasFlag(ModifierKeys.Control) && this.DataContext is InventoryItem ii)
            {
                InventoryTab.RunItemsRoll20Integration(ii);
                e.Handled = true;
            }
        }
    }
}
