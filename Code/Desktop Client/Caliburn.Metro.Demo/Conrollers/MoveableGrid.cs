using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Caliburn.Metro.Demo.Conrollers
{
    public partial class MenuSetView : PropertyChangedBase
    {
        public int itemid { get; set; }
        public int id { get; set; }
        public int locationX { get; set; }
        public int locationY { get; set; }
        public int indexRow { get; set; }
        public int indexCell { get; set; }
        public int numOfRows { get; set; }
        public int numOfCells { get; set; }
        public string type { get; set; }
        public Nullable<int> color { get; set; }
        public string image { get; set; }
        public Nullable<int> foreColor { get; set; }
        public string fontString { get; set; }
        public string Name { get { return _name; } set { } }
        private string _name;
    }
    public  class MoveableGrid : UserControl
    {
        public MoveableGrid()
        {

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
                _movingItem.indexCell = x;
                _movingItem.indexRow = y;
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
            _movingItem = ((MenuSetView)((Button)sender).DataContext);
            maxRow = 25 - (int)_movingItem.numOfRows;
            maxCol = 25 - (int)_movingItem.numOfCells;
            mouseOffset = Mouse.GetPosition(this);
            _viewModel.FreeItemPosition(_movingItem);
            isPress = true;
        }

        public Point mouseOffset { get; set; }

        private void Button_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            isPress = false;
            _movingItem.indexCell = lastCol;
            _movingItem.indexRow = lastRow;
            _viewModel.UpdateItem();

        }

        public bool isPress { get; set; }

        private int maxRow, maxCol;

        private MoveableGridModel _viewModel = null;

        private MenuSetView _movingItem = null;
        public void getPosition(FrameworkElement element, out int col, out int row)
        {
            double actualHeight = element.ActualHeight / (int)_movingItem.numOfRows;
            double actualWidth = element.ActualWidth / (int)_movingItem.numOfCells;
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
    }
}
