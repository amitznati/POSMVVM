using POS.DesktopClient.ViewModels.MainPos;
using POSEntities.Entities;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace POS.DesktopClient.Views.MainPos
{
    /// <summary>
    /// Interaction logic for ItemsGrigForEdit.xaml
    /// </summary>
    public partial class ItemsGrigForEdit : UserControl
    {
        private int GridNumOfRows = 20;
        private int GridNumOfColumns = 20;
        public ItemsGrigForEdit()
        {
            InitializeComponent();
            _viewModel = new ItemsGridForEditViewModel();
            this.DataContext = _viewModel;
        }

        private int lastRow, lastCol;
        private void Button_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (isPress)
            {

                Point mouseDelta = Mouse.GetPosition(this);
                mouseDelta.Offset(-mouseOffset.X, -mouseOffset.Y);
                Thickness margin = new Thickness(
                    Margin.Left + mouseDelta.X,
                    Margin.Top + mouseDelta.Y,
                    Margin.Right - mouseDelta.X,
                    Margin.Bottom - mouseDelta.Y);
                ((Button)sender).Margin = margin;

                int x, y;
                getPosition(((FrameworkElement)sender), out x, out y);
                _movingItem.IndexColumn = x;
                _movingItem.IndexRow = y;
                if (_viewModel.isPositionFree(_movingItem))
                {
                    lastCol = x;
                    lastRow = y;
                    ((Button)sender).Opacity = 1.0;
                    Console.WriteLine("x:" + x + " y:" + y);
                }
                else
                    ((Button)sender).Opacity = 0.5;
            }
        }

        private void Button_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _movingItem = ((ItemDisplayInfo)((Button)sender).DataContext);
            maxRow = GridNumOfRows - (int)_movingItem.NumberOfRows;
            maxCol = GridNumOfColumns - (int)_movingItem.NumberOfColumns;
            mouseOffset = Mouse.GetPosition(this);
            _viewModel.FreeItemPosition(_movingItem);
            isPress = true;
        }

        public Point mouseOffset { get; set; }

        private void Button_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            isPress = false;
            _movingItem.IndexColumn = lastCol;
            _movingItem.IndexRow = lastRow;
            _viewModel.UpdateItem();

        }

        public bool isPress { get; set; }

        private int maxRow, maxCol;

        private ItemsGridForEditViewModel _viewModel = null;

        private ItemDisplayInfo _movingItem = null;
        public void getPosition(FrameworkElement element, out int col, out int row)
        {
            double actualHeight = element.ActualHeight / (int)_movingItem.NumberOfRows;
            double actualWidth = element.ActualWidth / (int)_movingItem.NumberOfColumns;
            var Ppoint = Mouse.GetPosition(this);//minus mouseposition on button;
            var Ipoint = Mouse.GetPosition(element);
            Point point = new Point(Ppoint.X - Ipoint.X + 3, Ppoint.Y - Ipoint.Y + 3);
            row = 0;
            col = 0;
            double accumulatedHeight = 0.0;
            double accumulatedWidth = 0.0;
            // calc row mouse was over
            for (int i = 0; i < maxRow; i++)
            {
                accumulatedHeight += actualHeight;
                if (accumulatedHeight >= point.Y)
                    break;
                row++;
            }

            // calc col mouse was over
            for (int i = 0; i < maxCol; i++)
            {
                accumulatedWidth += actualWidth;
                if (accumulatedWidth >= point.X)
                    break;
                col++;
            }
        }

        private void EnterToMenu_Click(object sender, RoutedEventArgs e)
        {
            var item = getItem(sender);
            _viewModel.EnterToMenu(item);
        }

        private void EditItem_Click(object sender, RoutedEventArgs e)
        {
            var item = getItem(sender);
            _viewModel.EditItem(item);

        }

        private void DeleteItem_Click(object sender, RoutedEventArgs e)
        {
            var item = getItem(sender);
            _viewModel.DeleteItem(item);
        }

        private ItemDisplayInfo getItem(object sender)
        {
            MenuItem menuItem = sender as MenuItem;
            if (menuItem != null)
            {
                ContextMenu parentContextMenu = menuItem.CommandParameter as ContextMenu;
                if (parentContextMenu != null)
                {
                    Button button = parentContextMenu.PlacementTarget as Button;
                    if (button != null)
                    {
                        ItemDisplayInfo item = button.DataContext as ItemDisplayInfo;
                        return item;
                    }
                }
            }
            return null;
        }
    }
}
