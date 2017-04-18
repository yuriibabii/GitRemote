using System;
using DLToolkit.Forms.Controls;
using Prism.Commands;
using Xamarin.Forms;

namespace GitRemote.Behaviors
{
    public class FlowListViewPaginating : Behavior<FlowListView>
    {
        public FlowListView AssosiatedObject { get; private set; }

        public static readonly BindableProperty CommandProperty =
            BindableProperty.Create(nameof(Command), typeof(DelegateCommand<object>), typeof(FlowListViewPaginating));

        public static readonly BindableProperty InputConverterProperty =
            BindableProperty.Create(nameof(Converter), typeof(IValueConverter), typeof(FlowListViewPaginating));

        public DelegateCommand<object> Command
        {
            get { return (DelegateCommand<object>)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        public IValueConverter Converter
        {
            get { return (IValueConverter)GetValue(InputConverterProperty); }
            set { SetValue(InputConverterProperty, value); }
        }

        protected override void OnAttachedTo(FlowListView bindable)
        {
            base.OnAttachedTo(bindable);
            AssosiatedObject = bindable;
            bindable.BindingContextChanged += OnBindingContextChanged;
            bindable.FlowItemAppearing += OnItemAppearing;
        }

        protected override void OnDetachingFrom(FlowListView bindable)
        {
            base.OnDetachingFrom(bindable);

            bindable.BindingContextChanged -= OnBindingContextChanged;
            bindable.FlowItemAppearing -= OnItemAppearing;
            AssosiatedObject = null;
        }

        private void OnItemAppearing(object sender, ItemVisibilityEventArgs e)
        {
            var flowListView = (FlowListView)sender;
            if (flowListView.IsRefreshing) return;
            if (Command == null) return;

            var parameter = Converter.Convert(e, typeof(object), null, null);
            if (Command.CanExecute(parameter))
                Command.Execute(parameter);
        }

        protected void OnBindingContextChanged(object sender, EventArgs e)
        {
            OnBindingContextChanged();
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            BindingContext = AssosiatedObject.BindingContext;
        }
    }
}
