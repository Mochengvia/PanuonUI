# PanuonUI(v0.1.2 beta)
一个好看精致，不限制个人或商业使用的WPF控件库。<br/>
本库是一个正在开发的项目，如果遇到问题或有更好的建议，请发送邮件至bonjour@panuon.com，或在我的知乎账户上私信我(@末城via)，QQ亦可（969954321，请务必备注来意）。<br/>

你可以在任何地方使用该库，包括移植到你自己的项目中。
#### 目前该版本已完成一轮Review，已发现的问题皆已修复。此版本即将新建分支并封存，新代码将升级为1.0版本。

## 使用方式
1.添加对Panuon.UI.dll的引用<br/>
2.在你的项目中添加fontawesome.ttf字体文件，并在App.xaml中添加以下资源字典。<br/>
```
<ResourceDictionary Source="pack://application:,,,/Panuon.UI;component/Themes/Control.xaml" />
```
3.在需要使用PanuonUI的xaml文件中添加引用<br/>
```
xmlns:pu="clr-namespace:Panuon.UI;assembly=Panuon.UI"
```

## 当前的所有控件
基础控件<br/>
[Window / MessageBox 窗体](#window-窗体)<br/>
[Button / RepeatButton 按钮](#button-按钮)<br/>
[TextBox 输入框](#textbox-输入框)<br/>
[PasswordBox 密码框](#passwordbox-密码框)<br/>
[ComboBox / ComboBoxItem 下拉框](#combobox-下拉框)<br/>
[CheckBox 复选框](#checkbox-复选框)<br/>
[RadioButton 单选按钮](#radiobutton-单选按钮)<br/>
[TreeView / TreeViewItem 树视图](#treeview-树视图)<br/>
[ProgressBar 进度条](#progressbar-进度条)<br/>
[TabControl / TabItem 选项卡](#tabcontrol-选项卡)<br/>
[ListBox / ListBoxItem 列表](#listbox-列表)<br/>
[Slider 滑块](#slider-滑块控件)<br/>
特殊控件<br/>
[ResizeGrid 可调大小容器](#resizegrid-可调大小容器)<br/>
[Loading 等待控件](#loading-等待控件)<br/>
[SlideShow 轮播控件](#slideshow-轮播控件)<br/>
[ImageCuter 图片裁剪器](#imagecuter-图片裁剪器)<br/>
[DatePicker 日期时间选择器](#datepicker-日期时间选择器)<br/>
[PagingNav 分页器](#pagingnav-分页器)<br/>
图表<br/>
[LineCharts 折线图](#linchart-折线图)<br/>


### Window 窗体
PUWindow是一个继承自Window的控件，支持边角拖动缩放。<br/>
通过设置IsCoverMaskShow和IsAwaitShow属性，可以快速打开一个遮罩层，或同时打开遮罩层和等待控件。<br/>
<br/>
PUWindow在创建时总是尝试将<b>排在最前面的活动窗口</b>设置为自己的Owner（但如果你在Show或ShowDialog前手动指定了它的Owner，则将以你的为准），以便于使用WindowStartupLocation属性和AllowAutoCoverMask属性（当此属性为True，且Owner是PUWindow类型时，该窗体打开时将自动把其Owner窗体的遮罩层打开，并在关闭时将其遮罩层关闭）。但这可能在某些情况下对你造成困扰。当你在一个窗体中尝试Show出多个PUWindow子窗体时，必须全部指定这些子窗体的Owner属性为当前窗体，否则可能会出现预料不到的问题。<br/>
图中演示了使用Gradual动画效果打开PUMessageBox，该控件是一个继承自PUWindow的窗体，可以提供一段消息显示，一个询问对话框，或一个可以取消的等待框。<br/>
![](https://github-1252047526.cos.ap-chengdu.myqcloud.com/window.png)<br/>
你可以使用PUMessageBox.ShowAwait(string content)来打开一个等待对话框，并用PUMessageBox.CloseAwait()方法来将其关闭。但如果你希望在CloseAwait之后立即打开一个新的PUWindow窗体（PUMessageBox的所有Show方法亦在此列），你必须指定新窗体的Owner为当前的主窗体，或者使用另一个重载方法PUMessageBox.CloseAwait(EventHandle closedCallback)关闭等待窗口，并将打开窗体的方法放入此事件处理中。否则新打开的窗体将被立即关闭。
示例：
```

//这种代码会导致弹框刚显示就被关闭
PUMessageBox.ShowAwait("正在执行...");
PUMessageBox.CloseAwait();
PUMessageBox.ShowDialog("任务已完成");

//必须像下面这样
PUMessageBox.ShowAwait("正在执行...");
PUMessageBox.CloseAwait(delegate
{
  PUMessageBox.ShowDialog("任务已完成");
});

//或这样
PUMessageBox.ShowAwait("正在执行...");
PUMessageBox.CloseAwait();
await Task.Delay(400);
PUMessageBox.ShowDialog("任务已完成");

//或者向下面这样
PUMessageBox.CloseAwait();
var testWindow = new TestWindow();
testWindow.Owner = this;
testWindow.ShowDialog();



```

| 依赖属性  | 类型 | 含义 |
| --- | --- | ---|
| Header | Object | 通常情况下，Title属性会同时设置窗体的左上角标题和任务栏标题。如果你期望使用不同的值，可以单独设置Header属性来改变左上角的标题内容。如果设置为Null，左上角标题将默认使用Title属性的内容。默认值为Null。  |
| Icon | Object | 显示在左上角标题之前的图标。默认值为Null。  |
| AnimationStyle | AnimationStyles枚举 | 启动/关闭时使用的动画样式。默认值为Scale（其余可选项为Gradual、Fade）。  |
| AnimateIn | Boolean | 打开窗体时是否使用动画。默认值为True。  |
| AnimateOut | Boolean | 关闭窗体时是否使用动画。默认值为True。  |
| NavButtonVisibilty | Visibility | 设置控制条右侧三个按钮的显示状态。默认值为Visible。  |
| IsCoverMaskShow | Boolean | 是否显示窗体的遮罩层。默认值为False。  |
| IsAwaitShow | Boolean | 是否打开窗体的遮罩层，并显示等待控件。默认值为False。  |
| AllowShowDelay  | Boolean | 是否允许在启动时延迟显示窗体内容。在页面较为复杂时，将此属性设置为True有助于减少启动动画卡顿。  |
| NavbarBackground | Brush | 控制栏的背景色。默认值为White（白色）。  |
| NavbarHeight | Double | 控制栏的高度。默认值为30。  |
| NavButtonHeight | Double | 控制栏按钮的高度。默认值为30。  |
| NavButtonWidth | Double | 控制栏按钮的宽度。默认值为40。  |
| BorderCornerRadius | CornerRadius | 窗体圆角大小。默认值为0。  |
| AllowAutoCoverMask* | Boolean | 获取或设置是否允许在调用Show或ShowDialog方法时自动打开父窗体的遮罩层，并在Close时将其关闭。默认值为False。  |
| AllowForcingClose | Boolean | 获取或设置是否允许用户使用Alt+F4组合键强制关闭窗体（在窗体Load事件之后设置此属性无效）。默认值为True。  |

| 方法  | 含义 |
| --- | --- |
| AppendNavButton(object content, RoutedEventHandler clickHandler) | 向标题栏右侧控制按钮组中添加一个新的按钮，该按钮将被添加在按钮组的最左侧。 |

注意：上一个版本的CloseWindow()方法将不再使用，直接使用Close()方法即可触发关闭动画（若AnimateOut为True）。<br/>
扩展：PUMessageBox<br/>
该控件继承自PUWindow，因而可以使用上面任意一个属性来配置它。<br/>
你可以在项目的任意地方调用PUMessageBox，它将自动打开父窗体的遮罩层。<br/>
```
//像下面这样调用，来显示一段提示
PUMessageBox.ShowDialog("操作成功。");
//或显示一个询问对话框
PUMessageBox.ShowConfirm("确定吗？");
```

### Button 按钮
PUButton是一个继承自Button的控件，目前共有四种样式。<br/>
PURepeatButton和PUButton的样式、属性、方法完全一致。<br/>
![](https://github-1252047526.cos.ap-chengdu.myqcloud.com/button.png)<br/>


| 依赖属性  | 类型 | 含义 |
| --- | --- | ---|
| ButtonStyle | ButtonStyles枚举 | 按钮的基本样式。默认值为General（其他可选项为Hollow、Outline、Link）。  |
| ClickStyle | ClickStyles枚举 | 鼠标点击时按钮的效果。默认值为Classic（其他可选项为Sink）。  |
| BorderCornerRadius | CornerRadius | 按钮圆角大小。默认值为0。  |
| CoverBrush | AnimationStyles枚举 | 鼠标悬浮时遮罩层的背景颜色（Outline和Link样式下为前景色）。默认值为白色（在Outline和Link样式下为灰色）  |


### TextBox 输入框
PUTextBox是一个继承自TextBox的控件，目前共有两种样式。<br/>
![](https://github-1252047526.cos.ap-chengdu.myqcloud.com/textbox.png)<br/>

| 依赖属性  | 类型 | 含义 |
| --- | --- | ---|
| TextBoxStyle | TextBoxStyles枚举 | 输入框的基本样式。默认值为General（其他可选项为IconGroup）。  |
| Watermark | String | 水印。默认值为空。  |
| Icon | Object | 放置在输入框前的图标，仅在IconGroup样式下有效。默认值为空。  |
| IconWidth | Double | 图标的宽度。默认值为30。  |
| ShadowColor | Color | 输入框获得焦点时阴影的颜色。默认值为#888888。  |
| BorderCornerRadius | CornerRadius | 输入框圆角大小。默认值为0。  |

### PasswordBox 密码框
#### PUPasswordBox继承自TextBox。恶意程序可能会通过内存读取用户输入的密码，请勿在较高安全要求环境中使用。<br/>
不要对Text属性进行赋值，可能会导致意外的错误。按原生PasswordBox的方式使用即可。
![](https://github-1252047526.cos.ap-chengdu.myqcloud.com/passwordbox.png)<br/>

| 依赖属性  | 类型 | 含义 |
| --- | --- | ---|
| PasswordBoxStyle | PasswordBoxStyles枚举 | 密码框的基本样式。默认值为General（其他可选项为IconGroup）。  |
| Watermark | String | 水印。默认值为空。  |
| Icon | Object | 放置在输入框前的图标，仅在IconGroup样式下有效。默认值为空。  |
| IconWidth | Double | 图标的宽度，仅在IconGroup样式下有效。默认值为30。  |
| ShadowColor | Color | 密码框获得焦点时阴影的颜色。默认值为#888888。  |
| BorderCornerRadius | CornerRadius | 密码框圆角大小。默认值为0。  |

### ComboBox 下拉框
![](https://github-1252047526.cos.ap-chengdu.myqcloud.com/combobox.png)<br/>
#### PUComboBox依赖属性
| 依赖属性  | 类型 | 含义 |
| --- | --- | ---|
| ShadowColor | Color | 获下拉框激活时阴影的颜色。默认值为#88888。  |
| CoverBrush | String | 鼠标悬浮在选择项时的背景颜色。默认值为#EEEEEE。  |
| DeleteMode | DeleteModes枚举 | 当用户点击下拉框子项的删除按钮时，应该执行的操作。默认值为DeleteAndRouted（其他可选项为EventOnly）。  |
| BindingItems | IList<PUComboBoxItemModel>集合 | 绑定对象模板，通过该属性可以将内容绑定到Model类集合，而不是UI对象集合。默认值为空。  |
| SelectedValuePath | SelectedValuePaths枚举 | 当子项目被选中时，SelectedValue应呈现子项PUComboBoxItem的哪一个值。默认值为Header（Value）。  |
| SelectedValue | Object | 子项PUComboBoxItem的Header或Value属性（取决于SelectedValuePath的值），或通过设置该值来选中指定的子项目。默认值为空。  |
| BorderCornerRadius | CornerRadius | 下拉框和输入框的圆角大小。默认值为0。  |

PUComboBox包含以下一个额外的路由事件。<br/>

| 路由事件 | 含义 |
| ----- | ----- |
| DeleteItem | 当用户点击子项PUComboBoxItem的删除按钮时，触发此路由事件。 |

#### PUComboBoxItem依赖属性
| 依赖属性  | 类型 | 含义 |
| --- | --- | ---|
| DeleteButtonVisibility | Visibility | 下拉选择项后侧删除按钮的显示状态。默认值为Collpased。  |
| Value | Object | 下拉选择项可以携带的值，仅用于标记该子项的实际内容，不会对前端显示造成影响。默认值为Null。  |

PUComboBoxItem包含以下一个额外的类。<br/>

| 类 | 含义 |
| ----- | ----- |
| PUComboBoxItemModel | 用于绑定到PUComboBox子项集合的模型类，参见PUComboBox的BindingItems属性。 |
  
（文档更新中）
