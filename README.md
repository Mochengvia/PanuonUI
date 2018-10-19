# PanuonUI(v0.0.5 alpha)
一个好看精致，不限制个人或商业使用的WPF控件库。<br/>
本库是一个正在开发的项目，如果遇到问题或有更好的建议，请发送邮件至bonjour@panuon.com，或在我的知乎账户上私信我(@末城via)。<br/>
请勿将本控件库或本控件库的一部分作为一个新的控件库发布。否则将追究相关法律责任。
## 目录
[Window / MessageBox 窗体控件](#Window)<br/>
[Button 窗体控件](#Button)<br/>

### Window 窗体控件
PUWindow是一个继承自Window的控件，但尚不支持边角拖动缩放。<br/>
图中演示了如何使用不同的动画效果打开PUMessageBox，该控件是一个继承自PUWindow的窗体，可以提供消息显示。<br/>
![](https://github-1252047526.cos.ap-chengdu.myqcloud.com/window.gif)<br/>

| 依赖属性  | 类型 | 含义 |
| --- | --- | ---|
| Header | Object | 通常情况下，Title属性会同时设置窗体的左上角标题和任务栏标题。如果你期望使用不同的值，可以单独设置Header属性来改变左上角的标题内容。如果设置为Null，左上角标题将默认使用Title属性的内容。默认值为Null。  |
| Icon | Object | 显示在右上角标题之前的图标。默认值为Null。  |
| AnimationStyle | AnimationStyles枚举 | 启动/关闭时使用的动画样式。默认值为Scale（其余可选项为Gradual、Fade）。  |
| AnimateOut | Boolean | 关闭窗体时是否使用动画。默认值为True。  |
| AnimateIn | Boolean | 打开窗体时是否使用动画。默认值为True。  |
| NavButtonVisibilty | Visibility | 设置控制条右侧三个按钮的显示状态。默认值为Visible。  |
| IsCoverMaskShow | Boolean | 是否显示窗体的遮罩层。默认值为False。  |
| AllowShowDelay  | Boolean | 是否允许延迟显示窗体内容。在页面较为复杂时，将此属性设置为True有助于减少动画卡顿。  |
| NavbarBackground | Brush | 控制栏的背景色。默认值为White（白色）。  |
| NavbarHeight | Double | 控制栏的高度。默认值为30。  |
| NavButtonHeight | Double | 控制栏按钮的高度。默认值为30。  |
| NavButtonWidth | Double | 控制栏按钮的宽度。默认值为30。  |



PUWindow包含以下一个方法。<br/>

| 方法 | 含义 |
| ----- | ----- |
| CloseWindow | 若要关闭窗体，请使用该方法。否则关闭动画可能不会如期执行。 |

扩展：PUMessageBox
该控件继承自PUWindow，因而可以使用上面任意一个属性来配置它。
你在项目的任意地方调用PUMessageBox，它将自动打开父窗体的遮罩层。
```
//像下面这样调用，来显示一段提示
PUMessageBox.ShowDialog($"操作成功。");
//或显示一个询问对话框
PUMessageBox.ShowConfirm($"确定吗？");
```
（文档更新中）
