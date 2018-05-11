# PanuonUI
一个好看精致，不限制个人或商业使用的WPF控件库。<br/>
本库是一个正在开发的项目，只会把已经验证过没有问题的控件放出来（每周不定时更新）。<br/>
最近更新日期：2018/5/11（重大更新，新增Window和MessageBox）。最新的版本共有7个控件。<br/>

有关自定义依赖属性的详细解释，请在我的CSDN博客上查看。<br/>
我的博客：https://blog.csdn.net/qq_36663276/article/details/80209684<br/>
本项目（Panuon.UI）使用了FontAwesome字体；示例程序（Panuon UIBrowser）中使用了Caliburn.Micro开源框架，请知悉。<br/>

#### 许可说明：
不限制在任何情景下使用本控件库，你可以将一些控件拷贝到你的项目中并重命名，这是被允许的。<br/>
但是，不允许通过抄袭、售卖本库或本库中的一部分来获取利益（例如剽窃源码后改名成了其他的控件库，甚至用于出售盈利），否则追究相关责任。<br/>
本项目是完全开放使用的控件库，请尊重作者劳动成果，共同维护开源社区成长。:)<br/>
如果遇到任何问题，或者有更好的建议，请发送至我的邮箱bonjour@panuon.com，或是在知乎上私信我（末城via）。<br/>

### 所有控件组
目前已拥有的控件：ScrollViewer、Window、Button、TextBox、CheckBox、RadioButton。
特殊控件：MessageBox

##### ScrollViewer控件
一个滚动视图控件，具体样式请参考其他图片（TextBox中的滚动视图就是此样式）。鼠标移入和移出时，透明度会发生改变。
要使用该控件，只需要在你的项目中引用Generic.xaml资源字典，它将覆盖默认的ScrollViewer样式。
```
//将下列资源字典加入你的MergedDictionaries
<ResourceDictionary Source="pack://application:,,,/Panuon.UI;component/Themes/Generic.xaml" />
//在窗体或控件中直接使用即可
<ScrollViewer>
  ...
</ScrollViewer>
```
##### PUWindow控件
这是一个融合了遮罩层、动画渐入渐出的窗体控件。 
UI Browser和PUMessageBox控件都是使用了PUWindow的窗体。 
![Alt text](https://img-blog.csdn.net/20180510213434344?watermark/2/text/aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L3FxXzM2NjYzMjc2/font/5a6L5L2T/fontsize/400/fill/I0JBQkFCMA==/dissolve/70)

##### PUButton按钮控件
![Alt text](https://img-blog.csdn.net/20180510213810302?watermark/2/text/aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L3FxXzM2NjYzMjc2/font/5a6L5L2T/fontsize/400/fill/I0JBQkFCMA==/dissolve/70)
当你的鼠标移入和点击时，按钮会出现不同的变化。

##### PUTextBox输入框控件
![Alt text](https://img-blog.csdn.net/2018051021383478?watermark/2/text/aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L3FxXzM2NjYzMjc2/font/5a6L5L2T/fontsize/400/fill/I0JBQkFCMA==/dissolve/70)
支持水印效果（WaterMark属性），当输入框被激活时，控件会出现阴影效果（颜色可以自定义，ShadowColor属性）。

##### PUCheckBox选择框控件
![Alt text](https://img-blog.csdn.net/20180510214602422?watermark/2/text/aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L3FxXzM2NjYzMjc2/font/5a6L5L2T/fontsize/400/fill/I0JBQkFCMA==/dissolve/70)
包含一组标准样式、Switch开关样式和枝杈样式。

##### PURadioButton单选按钮控件
![Alt text](https://img-blog.csdn.net/20180510214620422?watermark/2/text/aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L3FxXzM2NjYzMjc2/font/5a6L5L2T/fontsize/400/fill/I0JBQkFCMA==/dissolve/70)
包含一组标准样式、Switch开关样式和枝杈样式（可以用作侧边栏）。

###版本更新日志
### V0.0.3测试中 版本新增
2018.5.10 <br/>
新增PUWindow控件，它支持三种启动/关闭动画并融合了遮罩层；新增PUMessageBox特殊控件。

### V0.0.2测试中 版本新增
2018.5.8 <br/>
移除PUScrollViewer控件，只需添加资源字典并使用ScrollViewer控件即可覆盖默认样式。新增PUCheckBox、PURadioButton两组控件。

### V0.0.1测试中 版本新增
2018.5.5 <br/>
新增PUScrollViewer、PUButton、PUTextBox三组控件。
