﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Text;
using DataGridFilterLibrary.Support;
using System.Collections;
using System.Windows.Data;
using System.ComponentModel;
using System.Threading;
using System.Diagnostics;
using System.Windows.Threading;

namespace DataGridFilterLibrary.Querying
{
    public class QueryController
    {
        public FilterData  ColumnFilterData { get; set; }
        public IEnumerable ItemsSource { get; set; }

        private readonly Dictionary<string, FilterData> filtersForColumns;

        Query query;

        public Dispatcher       CallingThreadDispatcher { get; set; }
        public bool             UseBackgroundWorker { get; set; }
        private readonly object lockObject;

        public QueryController()
        {
            lockObject = new object();

            filtersForColumns = new Dictionary<string, FilterData>();
            query = new Query();
        }

        public void DoQuery()
        {
            DoQuery(false);
        }

        public void DoQuery(bool force)
        {
            ColumnFilterData.IsSearchPerformed = false;

            if (!filtersForColumns.ContainsKey(ColumnFilterData.ValuePropertyBindingPath))
            {
                filtersForColumns.Add(ColumnFilterData.ValuePropertyBindingPath, ColumnFilterData);
            }
            else
            {
                filtersForColumns[ColumnFilterData.ValuePropertyBindingPath] = ColumnFilterData;
            }

            if (isRefresh)
            {
                if (filtersForColumns.ElementAt(filtersForColumns.Count - 1).Value.ValuePropertyBindingPath
                    == ColumnFilterData.ValuePropertyBindingPath)
                {
                    runFiltering(force);
                }
            }
            else if (filteringNeeded)
            {
                runFiltering(force);
            }

            ColumnFilterData.IsSearchPerformed = true;
            ColumnFilterData.IsRefresh = false;
        }

        public bool IsCurentControlFirstControl
        {
            get
            {
                return filtersForColumns.Count > 0
                    ? filtersForColumns.ElementAt(0).Value.ValuePropertyBindingPath == ColumnFilterData.ValuePropertyBindingPath : false;
            }
        }

        public void ClearFilter()
        {
            int count = filtersForColumns.Count;
            for(int i = 0; i < count; i++)
            {
                FilterData data = filtersForColumns.ElementAt(i).Value;

                data.ClearData();
            }

            DoQuery();
        }

        #region Internal

        private bool isRefresh
        {
            get { return (from f in filtersForColumns where f.Value.IsRefresh == true select f).Count() > 0; }
        }

        private bool filteringNeeded
        {
            get { return (from f in filtersForColumns where f.Value.IsSearchPerformed == false select f).Count() == 1; }
        }

        private void runFiltering(bool force)
        {
            bool filterChanged;

            createFilterExpressionsAndFilteredCollection(out filterChanged, force);

            if (filterChanged || force)
            {
                OnFilteringStarted(this, EventArgs.Empty);

                applayFilter();
            }
        }

        private void createFilterExpressionsAndFilteredCollection(out bool filterChanged, bool force)
        {
            QueryCreator queryCreator = new QueryCreator(filtersForColumns);

            queryCreator.CreateFilter(ref query);

            filterChanged = (query.IsQueryChanged || (query.FilterString != String.Empty && isRefresh));

            if ((force && query.FilterString != String.Empty) || (query.FilterString != String.Empty && filterChanged))
            {
                IEnumerable collection = ItemsSource as IEnumerable;

                if (ItemsSource is ListCollectionView)
                {
                    collection = (ItemsSource as ListCollectionView).SourceCollection as IEnumerable;
                }

                var observable = ItemsSource as System.Collections.Specialized.INotifyCollectionChanged;
                if (observable != null)
                {
                    observable.CollectionChanged -= new System.Collections.Specialized.NotifyCollectionChangedEventHandler(observable_CollectionChanged);
                    observable.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(observable_CollectionChanged);

                }

                #region Debug
                #if DEBUG
                System.Diagnostics.Debug.WriteLine("QUERY STATEMENT: " + query.FilterString);

                string debugParameters = String.Empty;
                query.QueryParameters.ForEach(p =>
                {
                    if (debugParameters.Length > 0) debugParameters += ",";
                    debugParameters += p.ToString();
                });

                System.Diagnostics.Debug.WriteLine("QUERY PARAMETRS: " + debugParameters);
                #endif
                #endregion

                if (query.FilterString != String.Empty)
                {
                    var result = collection.AsQueryable().Where(query.FilterString, query.QueryParameters.ToArray<object>());

                    filteredCollection = result.Cast<object>().ToList();
                }
            }
            else
            {
                filteredCollection = null;
            }

            query.StoreLastUsedValues();
        }

        private void observable_CollectionChanged(
            object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (isToRunQuery)
            {
                DoQuery(true);
                isToRunQuery = false;
            }
        }
        private bool isToRunQuery { get; set; }
        #region Internal Filtering

        private IList filteredCollection;
        HashSet<object> filteredCollectionHashSet;

        void applayFilter()
        {
            ICollectionView view = CollectionViewSource.GetDefaultView(ItemsSource);

            if (filteredCollection != null)
            {
                executeFilterAction(
                    new Action(() =>
                    {
                        filteredCollectionHashSet = initLookupDictionary(filteredCollection);
 
                        view.Filter = new Predicate<object>(itemPassesFilter);

                        OnFilteringFinished(this, EventArgs.Empty);
                    })
                );
            }
            else
            {
                executeFilterAction(
                    new Action(() =>
                    {
                        if (view.Filter != null)
                        {
                            view.Filter = null;
                        }

                        OnFilteringFinished(this, EventArgs.Empty);
                    })
                );
            }
        }

        private void executeFilterAction(Action action)
        {
            if (UseBackgroundWorker)
            {
                BackgroundWorker worker = new BackgroundWorker();

                worker.DoWork += delegate(object sender, DoWorkEventArgs e)
                {
                    lock (lockObject)
                    {
                        executeActionUsingDispatcher(action);
                    }
                };

                worker.RunWorkerCompleted += delegate(object sender, RunWorkerCompletedEventArgs e)
                {
                    if (e.Error != null)
                    {
                        OnFilteringError(this, new FilteringEventArgs(e.Error));
                    }
                };

                worker.RunWorkerAsync();
            }
            else
            {
                try
                {
                    executeActionUsingDispatcher(action);
                }
                catch (Exception e)
                {
                    OnFilteringError(this, new FilteringEventArgs(e));
                }
            }
        }

        private void executeActionUsingDispatcher(Action action)
        {
            if (this.CallingThreadDispatcher != null && !this.CallingThreadDispatcher.CheckAccess())
            {
                this.CallingThreadDispatcher.Invoke
                    (
                        new Action(() =>
                        {
                            invoke(action);
                        })
                    );
            }
            else
            {
                invoke(action);
            }
        }

        private static void invoke(Action action)
        {
            System.Diagnostics.Trace.WriteLine("------------------ START APPLAY FILTER ------------------------------");
            Stopwatch sw = Stopwatch.StartNew();

            action.Invoke();

            sw.Stop();
            System.Diagnostics.Trace.WriteLine("TIME: " + sw.ElapsedMilliseconds);
            System.Diagnostics.Trace.WriteLine("------------------ STOP APPLAY FILTER ------------------------------");
        }

        private bool itemPassesFilter(object item)
        {
            return filteredCollectionHashSet.Contains(item);
        }

        #region Helpers
        private HashSet<object> initLookupDictionary(IList collection)
        {
            HashSet<object> dictionary;

            if (collection != null)
            {
                dictionary = new HashSet<object>(collection.Cast<object>()/*.ToList()*/);
            }
            else
            {
                dictionary = new HashSet<object>();
            }

            return dictionary;
        }
        #endregion

        #endregion
        #endregion

        #region Progress Notification
        public event EventHandler<EventArgs> FilteringStarted;
        public event EventHandler<EventArgs> FilteringFinished;
        public event EventHandler<FilteringEventArgs> FilteringError;

        private void OnFilteringStarted(object sender, EventArgs e)
        {
            EventHandler<EventArgs> localEvent = FilteringStarted;

            if (localEvent != null) localEvent(sender, e);
        }

        private void OnFilteringFinished(object sender, EventArgs e)
        {
            EventHandler<EventArgs> localEvent = FilteringFinished;

            if (localEvent != null) localEvent(sender, e);
        }

        private void OnFilteringError(object sender, FilteringEventArgs e)
        {
            EventHandler<FilteringEventArgs> localEvent = FilteringError;

            if (localEvent != null) localEvent(sender, e);
        }
        #endregion
    }
}
