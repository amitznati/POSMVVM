using POS.Data.Repository;
using POS_WPF.Events;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace POS_WPF.ViewModels
{
    public class EntityViewModelBase<TEntity> : BindableViewModelBase, IEntityViewModelBase where TEntity : class
    {
        IEventAggregator _eventAggregator;
        public EntityViewModelBase()
        {

            _eventAggregator = Globals.get().EventAggregator;
            IsInEditMode = Globals.get().IsInEditMode;
            IsInNewMode = Globals.get().IsInNewMode;
            IsToShowEditButtons = IsInNewMode || IsInEditMode;
            InitiatEntity();
        }

         
        public TEntity Entity { get { return GetValue(() => Entity); } set { SetValue(() => Entity, value); } }
        public bool IsInEditMode { get; set; }
        public bool IsInNewMode { get; set; }
        public bool IsToShowEditButtons { get; set; }
        public virtual void InitiatEntity()
        {
            if (Globals.get().ActiveType != typeof(TEntity)) return;
            if (IsInEditMode)
                Entity = ((TEntity)Globals.get().CurrentTypeForEdit);
            else
                Globals.get().CurrentTypeForEdit = Entity;
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
            _eventAggregator.GetEvent<SaveDialogComplete>().Publish(entity);
        }

    }
}
