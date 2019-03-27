using System;

using Android.App;
using Android.Support.V4.App;
using Android.Support.V7.Widget;
using Android.Widget;
using static Android.Support.V7.Widget.RecyclerView;
using static Android.Widget.AbsListView;
using static Bumptech.Glide.ListPreloader;

namespace Bumptech.Glide.Integration.RecyclerView
{
    /// <summary>
    /// Converts android.support.v7.widget.RecyclerView.OnScrollListener events to AbsListView scroll events.
    /// 
    /// Requires that the the recycler view be using a LinearLayoutManager subclass.
    /// </summary>
    public class RecyclerToListViewScrollListener : OnScrollListener
    {
        private readonly IOnScrollListener _scrollListener;
        private int _lastFirstVisible = -1;
        private int _lastVisibleCount = -1;
        private int _lastItemCount = -1;

        public RecyclerToListViewScrollListener(IOnScrollListener scrollListener)
        {
            _scrollListener = scrollListener;
        }

        public override void OnScrollStateChanged(Android.Support.V7.Widget.RecyclerView recyclerView, int newState)
        {
            ScrollState listViewState = ScrollState.Idle;
            switch (newState)
            {
                case ScrollStateDragging:
                    listViewState = ScrollState.TouchScroll;
                    break;
                case ScrollStateIdle:
                    listViewState = ScrollState.Idle;
                    break;
                case ScrollStateSettling:
                    listViewState = ScrollState.Fling;
                    break;
            }

            _scrollListener.OnScrollStateChanged(null, listViewState);
        }

        public override void OnScrolled(Android.Support.V7.Widget.RecyclerView recyclerView, int dx, int dy)
        {
            LinearLayoutManager layoutManager = (LinearLayoutManager)recyclerView.GetLayoutManager();

            int firstVisible = layoutManager.FindFirstVisibleItemPosition();
            int visibleCount = Math.Abs(firstVisible - layoutManager.FindLastCompletelyVisibleItemPosition());
            int itemCount = recyclerView.GetAdapter().ItemCount;

            if (firstVisible != _lastFirstVisible || visibleCount != _lastVisibleCount || itemCount != _lastItemCount)
            {
                _scrollListener.OnScroll(null, firstVisible, visibleCount, itemCount);
                _lastVisibleCount = firstVisible;
                _lastVisibleCount = visibleCount;
                _lastItemCount = itemCount;
            }
        }
    }

    /// <summary>
    /// Loads a few resources ahead in the direction of scrolling in any RecyclerView so that
    /// images are in the memory cache just before the corresponding view in created in the list. Gives
    /// the appearance of an infinitely large image cache, depending on scrolling speed, cpu speed, and
    /// cache size.
    /// 
    /// Must be added as a listener to the RecycleView using RecyclerView.AddOnScrollListener(...),
    /// or have it's corresponding methods called from another RecyclerView.OnScrollListener to function.
    /// 
    /// This class only works with android.support.v7.widget.LinearLayoutManager and subclasses of android.support.v7.widget.LinearLayoutManager.
    /// </summary>
    /// <typeparam name="T">The type of the model being displayed in the RecyclerView</typeparam>
    public class RecyclerViewPreloader<T> : OnScrollListener
    {
        private readonly RecyclerToListViewScrollListener _recyclerScrollListener;

        /// <summary>
        /// Constructor that accepts interfaces for providing the dimensions of images to preload, the list
        /// of models to preload for a given position, and the request to use to load images.
        /// </summary>
        /// <param name="requestManager"></param>
        /// <param name="preloadModelProvider">Provides models to load and requests capable of loading them.</param>
        /// <param name="preloadDimensionProvider">Provides the dimensions of images to load.</param>
        /// <param name="maxPreload">Maximum number of items to preload.</param>
        public RecyclerViewPreloader(RequestManager requestManager, IPreloadModelProvider preloadModelProvider, IPreloadSizeProvider preloadDimensionProvider, int maxPreload)
        {
            ListPreloader listPreloader = new ListPreloader(requestManager, preloadModelProvider, preloadDimensionProvider, maxPreload);
            _recyclerScrollListener = new RecyclerToListViewScrollListener(listPreloader);
        }

        public override void OnScrolled(Android.Support.V7.Widget.RecyclerView recyclerView, int dx, int dy)
        {
            _recyclerScrollListener.OnScrolled(recyclerView, dx, dy);
        }

        /// <summary>
        /// Helper constructor that accepts an Activity.
        /// </summary>
        /// <param name="activity"></param>
        /// <param name="preloadModelProvider"></param>
        /// <param name="preloadDimensionProvider"></param>
        /// <param name="maxPreload"></param>
        public RecyclerViewPreloader(Activity activity, IPreloadModelProvider preloadModelProvider, IPreloadSizeProvider preloadDimensionProvider, int maxPreload)
            : this(Glide.With(activity), preloadModelProvider, preloadDimensionProvider, maxPreload)
        {
        }

        /// <summary>
        /// Helper constructor that accepts an FragmentActivity.
        /// </summary>
        /// <param name="fragmentActivity"></param>
        /// <param name="preloadModelProvider"></param>
        /// <param name="preloadDimensionProvider"></param>
        /// <param name="maxPreload"></param>
        public RecyclerViewPreloader(FragmentActivity fragmentActivity, IPreloadModelProvider preloadModelProvider, IPreloadSizeProvider preloadDimensionProvider, int maxPreload)
            : this(Glide.With(fragmentActivity), preloadModelProvider, preloadDimensionProvider, maxPreload)
        {
        }

        /// <summary>
        /// Helper constructor that accepts an Fragment.
        /// </summary>
        /// <param name="fragment"></param>
        /// <param name="preloadModelProvider"></param>
        /// <param name="preloadDimensionProvider"></param>
        /// <param name="maxPreload"></param>
        public RecyclerViewPreloader(Android.App.Fragment fragment, IPreloadModelProvider preloadModelProvider, IPreloadSizeProvider preloadDimensionProvider, int maxPreload)
            : this(Glide.With(fragment), preloadModelProvider, preloadDimensionProvider, maxPreload)
        {
        }
    }
}