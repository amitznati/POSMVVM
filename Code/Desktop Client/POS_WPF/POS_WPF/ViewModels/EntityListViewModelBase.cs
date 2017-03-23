using POS.Data.Repository;
using POS_WPF.Events;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace POS_WPF.ViewModels
{
    public class EntityListViewModelBase<TEntity> : BindableViewModelBase,IEntityListViewModelBase where TEntity : class
    {
        IEventAggregator _eventAggregator;
        Repository<TEntity> Repository;
        public EntityListViewModelBase()
        {
            _eventAggregator = Globals.get().EventAggregator;
            Repository = new Repository<TEntity>(UnitOfWork.get().Context);
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
            EntityList = new ListCollectionView(Repository.GetAll().ToList<TEntity>());
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
                Globals.get().CurrentTypeForEdit = SelectedEntity;
                EntityViewModel.Entity = SelectedEntity;
                _eventAggregator.GetEvent<SelectedEntityChanged>().Publish(SelectedEntity);
            }
        }



        public void SaveNewEntity(object entity)
        {
            if (!Globals.get().IsInEditMode)
            {
                POS.Data.Repository.Repository<TEntity> Repository = new Repository<TEntity>(UnitOfWork.get().Context);
                Repository.Add((TEntity)Globals.get().CurrentTypeForEdit);
            }
            try
            {
                UnitOfWork.get().Complete();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            initEntityList();
            _eventAggregator.GetEvent<SaveDialogComplete>().Publish(entity);
        }

        public virtual FrameworkElement getEntityView()
        {
            throw new NotImplementedException();
        }
    }
}
