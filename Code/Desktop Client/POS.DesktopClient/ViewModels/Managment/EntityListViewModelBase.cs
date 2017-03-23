using POS.Data.Repository;
using POS.Data.Repository.Core.Repository;
using POS.Data.Repository.Repositories;
using POS.DesktopClient.Events.ManagmentEvents;
using POS.Windows;
using POSEntities;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace POS.DesktopClient.ViewModels.Management
{
    public class EntityListViewModelBase<TEntity> : BindableViewModelBase,IEntityListViewModelBase where TEntity : class,IUnDeleteableEntity
    {
        IEventAggregator _eventAggregator;
        IUnDeleteableRepository<TEntity> Repository;
        public EntityListViewModelBase()
        {
            _eventAggregator = Globals.get().EventAggregator;
            Repository = new UnDeleteableRepository<TEntity>(UnitOfWork.get().Context);
            initEntityList();
            _eventAggregator.GetEvent<SaveDialogComplete>().Subscribe(SaveDialogCompleteExecute);
            
        }

        public virtual Type MyType()
        {
            return typeof(TEntity);
        }
        private void SaveDialogCompleteExecute(object entity)
        {
            if (MyType() == Globals.get().ActiveType)
                initEntityList();
        }
        protected virtual void initEntityList()
        {
            EntityList = new ListCollectionView(Repository.GetAll().ToList());
        }
        public ListCollectionView EntityList
        {
            get { return GetValue(() => EntityList); }
            set { SetValue(() => EntityList, value); }
        }

        public EntityViewModelBase<TEntity> EntityViewModel
        {
            get { return GetValue(() => EntityViewModel); }
            set { SetValue(() => EntityViewModel, value); }
        }
        public TEntity SelectedEntity
        {
            get { return GetValue(() => SelectedEntity); }
            set 
            { 
                SetValue(() => SelectedEntity, value);
                if (MyType() == Globals.get().ActiveType)
                {
                    Globals.get().CurrentTypeForEdit = SelectedEntity;
                    EntityViewModel.Entity = SelectedEntity;
                    _eventAggregator.GetEvent<SelectedEntityChanged>().Publish(SelectedEntity);
                }
            }
        }



        public void SaveNewEntity(object entity)
        {
            if (!Globals.get().IsInEditMode)
            {
                Repository.Add((TEntity)Globals.get().CurrentTypeForEdit);
            }
            SaveCahnges();
            _eventAggregator.GetEvent<SaveDialogComplete>().Publish(entity);
        }

        public virtual FrameworkElement getEntityView()
        {
            throw new NotImplementedException();
        }


        public void RemoveEntity(object entity)
        {
            ((IUnDeleteableEntity)entity).Mode = false;
            SaveCahnges();
        }

        private void SaveCahnges()
        {
            UnitOfWork.get().Complete();
            initEntityList();
        }
    }
}
