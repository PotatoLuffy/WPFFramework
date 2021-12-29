using EIPMonitor.IDomainService;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace EIPMonitor.ViewModel
{
    public class BaseViewDialog<TView>: IModelDialog where TView: new()
    {

        public TView Tview;
        /// <summary>
        /// 绑定数据上下文(主动)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="viewModel"></param>
        public virtual void BindViewModel<T>(T viewModel)
        {
            var dialog = GetDialog();
            (dialog as UserControl).DataContext = viewModel;
        }
        /// <summary>
        /// 获取视图页
        /// </summary>
        /// <returns></returns>
        public virtual object GetDialog()
        {
            if (Tview == null)
            {
                Tview = new TView();
                this.BindDefaultViewModel();
            }
            return Tview;
        }
        public async virtual Task<bool> ShowDialog(DialogOpenedEventHandler openedEventHandler = null, DialogClosingEventHandler closingEventHandler = null)
        {
            var dialog = GetDialog();
            object taskResult = await DialogHost.Show(dialog, "AppDialog", openedEventHandler, closingEventHandler);//位于等级窗口
            return (bool)taskResult;
        }
        /// <summary>
        /// close the win or user control
        /// </summary>
        public virtual void Close() { }
        /// <summary>
        /// 注册窗口默认事件
        /// </summary>
        public virtual void RegisterDefaultEvent()
        {
            
        }
        /// <summary>
        /// 绑定默认视图
        /// </summary>
        public virtual void BindDefaultViewModel()
        {
            
        }
    }
}
