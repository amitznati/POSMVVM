using POS_WPF.Events;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using POS.Data.Repository;
using POS.Data;
using MenuDesign.Models;

namespace POS_WPF.ViewModels
{
    class MoveableGridViewModel : BindableBase
    {

        public MoveableGridViewModel(IEventAggregator eventAggregator)
        {
            _productSet = new ObservableCollection<MenuSetView>();
            fillItems();
            eventAggregator.GetEvent<AddItemEvent>().Subscribe(AddItem);
        }
        private ObservableCollection<MenuSetView> _productSet;
        public ObservableCollection<MenuSetView> ProductSet
        {
            get
            {
                if (_productSet == null)
                {
                    fillItems();
                }
                return _productSet;
            }
            set
            {
                SetProperty(ref _productSet, value);
            }
        }

        private bool[,] FreePositions { get; set; }
        private void fillItems()
        {

            MenuSetView msv = new MenuSetView();
            msv.indexCell = 0;
            msv.indexRow = 0;
            msv.numOfCells = 1;
            msv.numOfRows = 1;
            msv.itemid = 1;
            msv.Name = string.Format("msv.indexCell = {0}, msv.indexRow = {1}", msv.indexCell, msv.indexRow);
            _productSet.Add(msv);
            msv = new MenuSetView();
            msv.indexCell = 2;
            msv.indexRow = 2;
            msv.numOfCells = 5;
            msv.numOfRows = 5;
            msv.itemid = 2;
            msv.Name = string.Format("msv.indexCell = {0}, msv.indexRow = {1}", msv.indexCell, msv.indexRow);
            _productSet.Add(msv);
            msv = new MenuSetView();
            msv.indexCell = 5;
            msv.indexRow = 5;
            msv.numOfCells = 5;
            msv.numOfRows = 5;
            msv.itemid = msv.indexRow;
            msv.Name = string.Format("msv.indexCell = {0}, msv.indexRow = {1}", msv.indexCell, msv.indexRow);
            _productSet.Add(msv);
            LockPositions();
        }

        private void LockPositions()
        {
            FreePositions = new bool[25, 25];
            for (int row = 0; row < 25; row++)
                for (int col = 0; col < 25; col++)
                    FreePositions[row, col] = true;
            foreach (MenuSetView item in ProductSet)
            {
                LockItemPosition(item);
            }
        }

        internal void UpdateItem()
        {
            LockPositions();
            CollectionViewSource.GetDefaultView(ProductSet).Refresh();
        }

        public bool isPositionFree(MenuSetView item)
        {
            for (int row = item.indexRow; row < item.indexRow + item.numOfRows; row++)
                for (int col = item.indexCell; col < item.indexCell + item.numOfCells; col++)
                    if (!FreePositions[row, col]) return false;
            return true;
        }

        internal void FreeItemPosition(MenuSetView item)
        {
            for (int row = item.indexRow; row < item.indexRow + item.numOfRows; row++)
                for (int col = item.indexCell; col < item.indexCell + item.numOfCells; col++)
                    FreePositions[row, col] = true;
        }

        internal void LockItemPosition(MenuSetView item)
        {
            for (int row = item.indexRow; row < item.indexRow + item.numOfRows; row++)
                for (int col = item.indexCell; col < item.indexCell + item.numOfCells; col++)
                    FreePositions[row, col] = false;
        }

        public void AddItem(MenuSetView item)
        {
            ProductSet.Add(item);
            //  UpdateItem();
        }
    }
}
