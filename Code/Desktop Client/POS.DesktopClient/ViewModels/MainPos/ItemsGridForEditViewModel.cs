using POS.Data.Repository;
using POS.DesktopClient.Events.MainPosEvents;
using POS.Windows;
using POSEntities.Entities;
using Prism.Events;
using System.Linq;
using System.Windows.Data;

namespace POS.DesktopClient.ViewModels.MainPos
{
    public class ItemsGridForEditViewModel :BindableViewModelBase
    {
        public ItemsGridForEditViewModel()
        {

            eventAggregator = Globals.get().EventAggregator;
            eventAggregator.GetEvent<AddItemEvent>().Subscribe(AddItem);
            CurrentMenu = UnitOfWork.get().Menus.Get(1);
            LockPositions();
        }
        public void EnterToMenu(ItemDisplayInfo item)
        {
            CurrentMenu = item.Menus.ToList<Menu>().FirstOrDefault();
            UpdateItem();
        }
        public void EditItem(ItemDisplayInfo item)
        {
            CurrentMenu.ContainItemDisplayInfoes.Remove(item);
            UpdateItem();
            eventAggregator.GetEvent<DisplayItemEditClick>().Publish(item);
        }
        public void DeleteItem(ItemDisplayInfo item)
        {
            CurrentMenu.ContainItemDisplayInfoes.Remove(item);
            if (item.IsMenu)
                UnitOfWork.get().Menus.Get(item.ItemID).ItemDisplayInfoes.Remove(item);
            else
                UnitOfWork.get().Products.Get(item.ItemID).ItemDisplayInfoes.Remove(item);
            UnitOfWork.get().ItemDisplayInfoes.Remove(item);
            UpdateItem();
        }
        public Menu CurrentMenu
        {
            get { return GetValue(() => CurrentMenu); }
            set { SetValue(() => CurrentMenu, value); }
        }
        private bool[,] FreePositions { get; set; }

        private void LockPositions()
        {
            FreePositions = new bool[25, 25];
            for (int row = 0; row < 25; row++)
                for (int col = 0; col < 25; col++)
                    FreePositions[row, col] = true;
            foreach (ItemDisplayInfo item in CurrentMenu.ContainItemDisplayInfoes)
            {
                LockItemPosition(item);
            }
        }

        internal void UpdateItem()
        {
            LockPositions();
            CollectionViewSource.GetDefaultView(CurrentMenu.ContainItemDisplayInfoes).Refresh();
        }

        public bool isPositionFree(ItemDisplayInfo item)
        {
            for (int row = item.IndexRow; row < item.IndexRow + item.NumberOfRows; row++)
                for (int col = item.IndexColumn; col < item.IndexColumn + item.NumberOfColumns; col++)
                    if (!FreePositions[row, col]) return false;
            return true;
        }

        internal void FreeItemPosition(ItemDisplayInfo item)
        {
            for (int row = item.IndexRow; row < item.IndexRow + item.NumberOfRows; row++)
                for (int col = item.IndexColumn; col < item.IndexColumn + item.NumberOfColumns; col++)
                    FreePositions[row, col] = true;
        }

        internal void LockItemPosition(ItemDisplayInfo item)
        {
            for (int row = item.IndexRow; row < item.IndexRow + item.NumberOfRows; row++)
                for (int col = item.IndexColumn; col < item.IndexColumn + item.NumberOfColumns; col++)
                    FreePositions[row, col] = false;
        }

        public void AddItem(ItemDisplayInfo item)
        {
            item.MenuID = CurrentMenu.ID;
            CurrentMenu.ContainItemDisplayInfoes.Add(item);
            UpdateItem();
        }

        public IEventAggregator eventAggregator { get; set; }


        
    }
}
